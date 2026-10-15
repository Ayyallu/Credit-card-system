# Phase 3 — status, and scope of the remaining work (Manager + Web)

Companion to [MIGRATION_PLAN.md](MIGRATION_PLAN.md). Every number below was measured from the repository (grep/line counts), not estimated. Where something is a judgement call it says so.

## 1. Where Phase 3 stands

Seven projects are ported to .NET 8 under `netcore/`, all building in CI on every push (`build-netcore` job), with no change to the legacy solution:

| Project | Result | Verified by |
|---|---|---|
| `CC_Ingenio_Resources` | Full port (localization resx) | builds |
| `CC_Ingenio.Core` | 415 entity/interface files; 5 composite FKs rewritten to the EF Core form | builds; exercised by Data tests |
| `CC_Ingenio.DataTranfer` | 166 DTOs; one temporary `SelectListItem` shim left | builds |
| `CC_Ingenio.Common` | **Split**: framework-independent half ported; 43 `SelectListItem` helpers + MVC model binders left in legacy | builds (incl. the source-less `GemCryptor.dll`) |
| `CC_Ingenio.Data` | EF5 → **EF Core 8** `DbContext` (127 DbSets) + legacy-mapping conventions | 7 offline model tests |
| `CC_Ingenio.Repository` | 144 thin repositories | 3 offline tests (every query translates to SQL) |
| `CC_Ingenio.Manager` | Business layer, ~24.6K lines, 0 compile errors; `ILoginSession` and `IReportGenerator` seams; Excel import moved off ACE/OLEDB | 15 offline tests |

**What none of this proves:** that anything works against a real SQL Server. There is no database in CI. The offline tests prove the EF Core model is valid and every repository's query translates; they cannot catch the runtime differences listed in [`CC_Ingenio.Data/PORT_NOTES.md`](netcore/CC_Ingenio.Data/PORT_NOTES.md) (`datetime`→`datetime2`, default connection encryption, change-tracking semantics, lazy loading, cascade actions). A masked test database remains the single most valuable missing piece — see the still-open item in [PHASE0_NOTES.md](PHASE0_NOTES.md).

**Remaining:** `CC_Ingenio.Reporting` (Crystal Reports; `Manager` now reaches it only through `IReportGenerator`) and `CC_Ingenio.Web` (UI).

## 2. CC_Ingenio.Manager — ported

**Done** (see [`netcore/CC_Ingenio.Manager/PORT_NOTES.md`](netcore/CC_Ingenio.Manager/PORT_NOTES.md)): the `SqlParameter` swap, the `ILoginSession` and `IReportGenerator` seams, and the Excel import rewrite are all in; it compiles with 0 errors and 15 offline tests pass. What follows is the original measured scope, kept for reference. **Two things the port turned up that bear directly on `Web`:** (a) the legacy Windsor registration gives `UnitOfWork`, every repository and every manager the *default singleton* lifestyle — one `DbContext` for the whole process, which EF Core will not tolerate under concurrent requests, so the Web DI lifetimes must be a deliberate decision; (b) 133 `TransactionScope` blocks (`RequiresNew`, `ReadUncommitted`) and process-wide statics (`Utils.OrganizationCode`, `BusinessDate`) that are overwritten at each login.

Original scope: mostly business logic and DTO mapping, with a small, well-defined set of framework couplings.

| Concern | Measured | What it needs |
|---|---|---|
| `System.Data.SqlClient.SqlParameter` | **876 uses in 8 files** (`MasterManager` 282, `AuthorizationManager` 173, `CustomerServiceManager` 123, `BillingManager` 119, `UserManager` 107, `CardBookingManager` 30, `ApprovalQueueManager` 22, `CommonManager` 20), feeding ~220 `ExecuteQuery`/`ExecuteCommand` calls | Namespace swap to `Microsoft.Data.SqlClient` (EF Core's provider rejects the old type at runtime). Mechanical, but output parameters and `DBNull`/type inference need a real database to confirm. Nothing outside Manager uses `SqlParameter`. |
| Web-session coupling | `CommonManager` takes an `HttpContext` in its constructor and reads `Session["LOGGED_IN_USER"]`; `UserManager.LoadLoginUserDetails` *writes* the session on login; `LookUpManager` reads it (6 `HttpContext` + 4 `Session[` hits) | Introduce a small `ICurrentUser`-style abstraction so the business layer stops depending on the web host. Small code, but it is the seam the whole auth redesign hangs on (§3). |
| `List<SelectListItem>` | 12 uses in 4 files; `ILookUpManager.GetAll<T>()` returns it and **every dropdown in the UI goes through it** | Needs the shared decision on the replacement type (the same open question as the `DataTranfer` shim). |
| Excel import | `BillingManager.ImportFromExcel` reads uploaded workbooks through the **`Microsoft.ACE.OLEDB.12.0`** provider (+ `Server.MapPath`) | ACE is a Windows-only, separately installed driver (and a 32/64-bit trap); replace with an OpenXML/ClosedXML reader. |
| Crystal Reports | `ReportGenerator` instantiated in `BillingManager` and `CardBookingManager` (2 call sites) | Blocked on the Crystal Reports decision (§4). |

Not a concern in Manager: no `ConfigurationManager`, no `FormsAuthentication`, no MVC controllers/results.

## 3. CC_Ingenio.Web — measured inventory

- **Controllers:** 10 files, 25.4K lines, 587 public actions. `MasterController` alone is **15,577 lines** (61% of all controller code). Splitting it is not required for the port but is the obvious moment to do it.
- **Views:** 235 `.cshtml`, 48.4K lines (110 under `Master`, 38 `CustomerService`, 23 `Authorization`, 15 `Billing`, …). **View models:** 87 files.
- **Static assets:** 410 script files (6.4 MB, mostly vendored: jQuery 1.8/1.9, jQuery UI, Knockout 3.1, DataTables 1.8.1) and 78 content files.
- **Startup:** `Global.asax` + 5 `App_Start` classes + a Windsor installer (154 component registrations).
- **Not present (good):** no `ApiController`s (so Web API can be dropped), no child actions (`Html.Action`/`RenderAction` = 0), no Areas, no `[ValidateInput]`.

### Construct-by-construct map

| Legacy construct | Count | ASP.NET Core | Effort / risk |
|---|---|---|---|
| `WebGrid` (`new WebGrid(` / `.GetHtml(`) | **102 in 101 views / 120** | **No equivalent** (`System.Web.Helpers` is gone) | **Largest view-layer item.** Needs a replacement grid approach decided once, then applied ~100 times. |
| `Session[...]` | 397 | Session exists but stores `byte[]`/strings only: `LoginUserDTO` and a `CultureInfo` object are stored directly today | Needs serialization or a move to claims/cookie. Touches auth, culture and most controllers. |
| Auth: custom `AuthorizeUserAttribute : AuthorizeAttribute` (session-based, per-entity privilege check) | 390 attribute uses | `IAuthorizationFilter` / policy-based authorization with `IHttpContextAccessor` | The attribute rewrite is small; the redesign around it is not. |
| `FormsAuthentication` / `WebSecurity` / `Membership` / `OAuthWebSecurity` (SimpleMembership, DotNetOpenAuth) | ~16 | Cookie authentication / ASP.NET Core Identity | Replacement, not port. Forms timeout is set to 100000 minutes today. |
| `TempData` | 416 | Exists; only serializes primitives/JSON | Check what is stored in it. |
| `ViewBag` / `ViewData` | 2056 / 346 | Same | None. |
| `JsonRequestBehavior.AllowGet` | 193 | Enum removed | Mechanical deletion. |
| `ModelState`, `RedirectToAction`, `PartialView` | 179 / 204 / 98 | Same | None. |
| `Html.TextBoxFor` / `DropDownListFor` / `ValidationMessageFor` / `ActionLink` | 666 / 289 / 570 / 441 | Same names (tag-helper rewrite optional) | Low. |
| `Html.Raw(...)` | **900** (e.g. `Html.Raw(Html.ActionLink…)` 111, `Html.Raw(string.Format…)` 138) | Exists, same semantics | Not assessed for injection. **Should be audited** during the port — unescaped output of formatted strings is exactly where user-controlled data bites. |
| Custom `HtmlHelper` extensions (`Helpers/`: 4 files; `Html.BasicButtons` used 71×) | — | Rewrite against `IHtmlContent`/`TagBuilder` | Small. |
| `@Scripts.Render` / `@Styles.Render` + `BundleConfig` (WebGrease) | 45 / 78 lines | `System.Web.Optimization` is gone | Replace with a static-file + bundler setup. |
| `Server.MapPath` / `Response.` direct writes / `Request.` / `HttpPostedFileBase` | 37 / 63 / 12 / 4 | `IWebHostEnvironment` / `FileResult` / `IFormFile` | Mechanical; the 63 `Response.` writes are mostly report/file downloads. |
| Custom `DateTime`/`Decimal` model binders (culture-aware) | 2 registrations | New `IModelBinder` implementations | Small, but the legacy ones stay behind in legacy `Common`. |
| Per-request culture from `Session["Culture"]` (`Application_AcquireRequestState`) | 1 | `UseRequestLocalization` + middleware | Small, but coupled to the session redesign. |
| Windsor DI | 1 installer | Built-in DI (or Windsor on Core) | **Not low.** Only controllers are registered transient; `UnitOfWork`, repositories and managers are default-singleton (see §2). Choosing lifetimes (scoped `DbContext` per request is the safe default) changes tracking/caching behaviour the managers may rely on. |
| Crystal Reports (`ReportDocument` ×4), `Excel.` ×11, HTML-table "Excel" exports | — | See §4 | Blocked on §4. |

### Configuration

`Web.config` keeps database settings as loose `appSettings` keys (server, database, user, an *encrypted* password, a few duplicated and commented-out variants) rather than a connection string, plus `authentication mode="Forms"`, `globalization culture="en-GB"`, and `customErrors mode="Off"`. This becomes `appsettings.json` + a secret store; the encrypted-credentials-in-source-control problem noted in [PHASE1_NOTES.md](PHASE1_NOTES.md) should be fixed as part of it rather than copied over.

## 4. Decisions that are not mine to make

These block or shape the remaining work. Each has a recommendation, but they are judgement calls with real tradeoffs.

1. **Crystal Reports** (blocks `Reporting`, 2 Manager call sites, `ReportViewerController`, 3 `.rpt` files incl. the customer-facing statement). Options and tradeoffs are in [PHASE2_NOTES.md](PHASE2_NOTES.md). Needs a budget/vendor call.
2. **The `DailyBalanceReport` Excel macro** (`Run_Program`, run via COM interop). Cannot be reimplemented without someone extracting the VBA and confirming what it does — it produces a financial report.
3. **Grid replacement for `WebGrid`** — one decision, ~100 applications.
4. **Authentication/session design** — move to cookie auth + claims (recommended, since 397 session reads make "port the session as-is" the more fragile path), or keep a server-side session store. Also decides how the privilege check in `AuthorizeUserAttribute` is fed.
5. **The `SelectListItem` replacement type** (shared by DataTranfer, Manager, Web).
6. **Cutover shape.** A per-controller strangler (reverse proxy routing migrated controllers to the new app) is attractive but the two apps would have to share login state, and that state lives in legacy in-proc session objects. My recommendation is therefore to port `Web` as **one new ASP.NET Core project, module by module, validated side by side against the legacy app**, rather than run both behind a proxy — but that is a bigger-bang cutover and the team may weigh the tradeoff differently.
7. **A real test database.** Without one, Manager and Web can be made to compile and start but their behaviour can't be compared with the legacy app.

## 5. Suggested order

1. Decide §4 items 4 and 5 (they gate Manager *and* Web).
2. Port **Manager** as a new net8 project: the `SqlParameter` swap, the `ICurrentUser` seam, Excel import via OpenXML; leave the 2 `ReportGenerator` call sites behind an interface until §4.1 is decided.
3. Stand up a masked database and run Manager's raw-SQL paths against it (the first runtime proof for the whole data stack).
4. Port **Web**: infrastructure first (startup, auth, session/culture, bundling, DI), then the small modules (`Home`, `User`, `Approval`, `Billing`, `CardBooking`), then `Authorization`/`CustomerService`, and `Master` (110 views, 15.6K controller lines) last, split as it goes.
5. Replace `WebGrid` and the Crystal/Excel report paths as their decisions land.

Sizing in relative terms only: Manager is a *medium* mechanical port; Web is the *large* item of the whole migration, dominated by `MasterController`/`Master` views, `WebGrid`, and the auth/session redesign. I have deliberately not put calendar estimates on it.
