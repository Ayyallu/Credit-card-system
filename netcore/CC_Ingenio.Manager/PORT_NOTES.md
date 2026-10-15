# CC_Ingenio.Manager — .NET 8 port notes

Sixth Phase 3 artifact: the business layer (20 files, ~24.6K lines — `MasterManager` alone is ~12K). It compiles on .NET 8 with **0 errors** and ships with 15 offline tests. The code is carried over verbatim apart from the changes below; no business logic was rewritten.

## What changed, and why

| Change | Where | Why |
|---|---|---|
| `System.Data.SqlClient` → `Microsoft.Data.SqlClient` | `using` in 8 files (876 `SqlParameter` references now resolve to the new type unchanged — same constructors, same property names) | EF Core's SQL Server provider rejects the old parameter type at runtime. |
| **`ILoginSession` seam** (new) | `CommonManager` takes it instead of `HttpContext`; `LookUpManager` gets it as a new last constructor argument; `UserManager.LoadLoginUserDetails(…)`/`IUserManager` take it instead of `HttpContext` | The business layer read and wrote `HttpContext.Session["LOGGED_IN_USER"]` directly. It now sees a one-property interface (`LoginUserDTO LoggedInUser {get;set;}`); the web host decides how to back it (session, claims…). This keeps the auth/session decision open. |
| **`IReportGenerator` seam** (new) | `BillingManager` and `CardBookingManager` take it as a new last constructor argument; their `GenerateReport` now calls it | The legacy `ReportGenerator` is Crystal Reports. The interface mirrors the one overload that is actually called, so the Crystal decision stays open. |
| **Excel import rewritten** | `BillingManager.ImportFromExcel` → new `TransactionPullingWorkbookReader` (ClosedXML) | The legacy code read uploads through the `Microsoft.ACE.OLEDB.12.0` driver: Windows-only, separately installed, 32/64-bit sensitive. |
| `[NonAction]`, `using System.Web`, dead `Server.MapPath`, `using System.Data.OleDb` | `CommonManager`, `BillingManager` | Meaningless/unused outside MVC. |

`SelectListItem` needed no new decision: Manager uses the `System.Web.Mvc.SelectListItem` shim already living in `CC_Ingenio.DataTranfer`. It remains a temporary stand-in (see the DataTranfer notes).

### The Excel import: deliberate behaviour differences

`TransactionPullingWorkbookReader` keeps the contract (sheet `Sheet1`, first row = header, the same 11 columns in the same order because the bulk copy maps by ordinal, native cell types) but it is **not byte-for-byte what ACE did**:

- It reads the workbook **before** emptying `[TR_TransactionPulling]`. The legacy code deleted the rows first, so an unreadable upload left the table empty.
- Columns are found by header name, case- and whitespace-insensitively (ACE's `select` was effectively the same, but a missing column now produces a clear error naming it).
- Fully blank rows are skipped.
- **Not compared against ACE.** There is no ACE driver here, so type inference on messy real-world sheets (mixed-type columns, text-formatted numbers, the old `IMEX=2` behaviour) has not been checked. Run a real pulling file through both before trusting it.

## Verified (15 tests, `CC_Ingenio.Manager.Tests`, no database)

- Workbook reader: column order and native types, header matching (order/extra columns/case/whitespace), blank-row skipping, header-only sheet, and clear errors for a missing column or missing `Sheet1` (7 tests, using real `.xlsx` files).
- `CommonManager`, as characterization tests of what the legacy code did: session user name (and `""` when nobody is logged in), the privilege **bit mask** decoding (1=View, 2=Authorize, 4=Delete, 8=Update, 16=Add, read from the lowest bit), per-entity lookup, attachment MIME mapping, and that the approval-queue procedure parameters are `Microsoft.Data.SqlClient` types (7 tests).
- `BillingManager.GenerateReport` delegates to `IReportGenerator` with exactly the legacy arguments (1 test).

## Not verified — needs a real database

Nothing here has executed SQL. Specifically unchecked: the ~220 `ExecuteQuery`/`ExecuteCommand` calls against real stored procedures; **table-valued parameters** (`SqlDbType.Structured` + `DataTable`, used e.g. by card embossing and payment-allocation saves); **output parameters** (`GetBusinessDate` reads one back after `ExecuteCommand`); `ExecuteQuery<T>` for both mapped entities and unmapped result shapes; and the transaction behaviour below.

## New findings (all pre-existing, none fixed here)

1. **`UnitOfWork` is effectively a singleton in the legacy app.** `BootStrapContainer` registers it with no lifestyle, and Castle Windsor's default lifestyle is *singleton* — so one `DbContext` is shared by every repository, manager, request and user for the life of the process (only the controllers are registered transient; the managers and repositories are singletons too). A `DbContext` is not thread-safe, and EF Core actively detects concurrent use and throws. It also explains the many `RefreshDataInModel`/`ReloadAllDataInContext` calls (a long-lived context caches stale entities). **This must be a deliberate decision when the Web DI is ported:** replicating it on EF Core would fail under concurrency; moving to scoped-per-request is the right default but changes caching/tracking behaviour that the managers may rely on.
2. **133 `TransactionScope` blocks**, all `RequiresNew` with `IsolationLevel.ReadUncommitted`. They work with a single connection; behaviour with `Microsoft.Data.SqlClient` and a context that outlives requests should be checked against a real database. `ReadUncommitted` means dirty reads are accepted by design today.
3. **Process-wide mutable state.** `UserManager.LoadLoginUserDetails` assigns the static `Utils.OrganizationCode` on every login, and `Utils.BusinessDate`/`CurrentUFRate` are statics too — with several users, the last login wins for everyone. Moving these into per-user/per-request state belongs with the auth/session redesign.
4. **`throw ex;`** is used throughout (the bulk of the build's 219 warnings, CA2200) and discards stack traces. It is behaviour-neutral apart from diagnostics; left untouched to keep this port a pure move.
5. The 20 `catch (SqlException)` blocks all just rethrow; nothing depends on error numbers, so the exception-type change is harmless.

## Next

`CC_Ingenio.Web`. See [PHASE3_STATUS_AND_WEB_SCOPE.md](../../PHASE3_STATUS_AND_WEB_SCOPE.md) for the measured scope and the decisions it needs (auth/session, the grid replacement, the Crystal Reports path behind `IReportGenerator`).
