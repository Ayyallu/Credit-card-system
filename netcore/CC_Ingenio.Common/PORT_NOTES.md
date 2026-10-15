# CC_Ingenio.Common — .NET 8 port notes

Third Phase 3 artifact. Unlike [CC_Ingenio.Core](../CC_Ingenio.Core/PORT_NOTES.md) and [CC_Ingenio.DataTranfer](../CC_Ingenio.DataTranfer/PORT_NOTES.md), this was **not** a mechanical dead-code-removal port. `CC_Ingenio.Common` (only 9 files, but one of them — `Utils.cs` — is 1406 lines) has three genuine architectural dependencies that needed real decisions, all confirmed before doing this work.

## 1. Utils.cs split: pure logic vs. ASP.NET MVC infrastructure

`Utils.cs` mixed plain business logic with **43 `List<System.Web.Mvc.SelectListItem>` dropdown-population methods** (`GetGender()`, `GetMaritalStatus()`, `GetReportTypes()`, etc. — genuinely used throughout the legacy web layer, not dead code) plus real ASP.NET MVC model-binding infrastructure: `DateTimeBinder`/`DecimalModelBinder` (`IModelBinder`) and `RegularExpressionExAttribute` (`IClientValidatable`). None of this has a .NET 8 equivalent.

**Decision (confirmed): split now.** This port's `Utils.cs` contains only the framework-independent half:

- All constants, static fields, the nested `ApprovalStatus` enum
- `BuildConnectionString()`, `Helper.{Decrypt,Encrypt,EncryptPassword1}` (uses `TwoFishCrypto`, see §2)
- `GetExpression<TEntity>()` (pure LINQ expression-tree building)
- `GetListMCCCode()` / `GetListMerCode()` — these return `List<CodeValue>` (a plain POCO, not `SelectListItem`), so they ported cleanly
- `GetRecordStatus()`, `GetApprovalStatusCode(ApprovalStatus)`, `GetApprovalStatusCode(string)`, `ValidateUserPrivilege()`
- `CheckRangeOverlaps()`, `SplitIntoMultipleTables()`, `ToDataTable<T>()`
- `CodeValue`, `BillGeneration`, `AmountCompareAttribute`, `NotEqualToAttribute` (both just extend the BCL's `ValidationAttribute`, no MVC dependency despite living in the same file as MVC code), `ResourceAccessor`

**Left behind in the legacy `CC_Ingenio.Common/Utils/Utils.cs`, not ported:**

- All 43 `SelectListItem`-returning methods
- `DateTimeBinder`, `DecimalModelBinder`, `RegularExpressionExAttribute`
- `Transaction`, `TransactionEntrySearch`, `Statement` classes — these weren't actually MVC-blocked (just DataAnnotations), but nothing ported so far references them, so they were left out of scope rather than ported speculatively. Pull them in later if something needs them.

This must be revisited once `CC_Ingenio.Web` is ported: design a real replacement (proper option-list types, ASP.NET Core model binding/validation) instead of carrying `SelectListItem`-shaped dropdown logic forward.

`Helpers.cs` and `Functions.cs` had no real MVC dependency (`Helpers.cs`'s `using System.Web.Mvc;` was dead, confirmed unused) and ported as-is.

## 2. GemCryptor.dll — referenced directly, not shimmed

`Utils.Helper.{Decrypt,Encrypt}Password` call `TwoFishCrypto.Encrypt`/`.Decrypt` from `GemCryptor.dll` — used for real password encryption, no source available (confirmed back in [PHASE0_NOTES.md](../../PHASE0_NOTES.md): likely in-house code, embedded PDB path suggests a standalone old VS project, not a licensed vendor product).

**Decision (confirmed): try referencing the raw DLL directly** rather than assuming it needs a rewrite. Copied `GemCryptor.dll` into `netcore/CC_Ingenio.Common/lib/` and added a plain `<Reference>`/`HintPath` to it in the csproj — the same way old-style projects reference arbitrary DLLs. Whether a .NET 8 app can actually load and call into this ~20KB .NET 2.0-era binary is **not yet known** — the CI job added for this project (`Build CC_Ingenio.Common (net8 port)`) is the real test. If it compiles but fails at runtime (this port has no test yet that actually calls `Helper.DecryptPassword`), that's the next thing to find out — not assumed here either way.

## 3. Enterprise Library Logging — shimmed with Microsoft.Extensions.Logging

`HandleLog.cs` genuinely called `Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(...)` — Enterprise Library was discontinued around 2013, no .NET 8 support.

**Decision (confirmed): shim a minimal `ILogger`-compatible wrapper.** `ILogger`, `ILoggerFactory`, `LoggerFactory` (the project's own abstraction) ported unchanged (just removed a dead `using Microsoft.Practices.EnterpriseLibrary.Logging;` from `ILogger.cs`). `HandleLog`/`HandleLogFactory` were reimplemented with the same class names and the same `LogError(message/exception, eventId, priority)` shape, but backed by `Microsoft.Extensions.Logging`'s console provider instead of Enterprise Library. **This is explicitly not a production logging configuration** — once a real ASP.NET Core host exists (after `CC_Ingenio.Web` is ported), whoever owns it should wire `HandleLog` to a logger from the app's actual DI container/configuration instead of the standalone console logger created here.

## Non-issue: `ConfigurationManager`

8 call sites in `Utils.cs` use `ConfigurationManager.AppSettings[...]`. `System.Configuration.ConfigurationManager` (Microsoft's own NuGet package, same API) is referenced in the csproj — this was never actually a blocker, just needed the package reference.

## Not yet done / out of scope

- **Not verified by an actual compile here** — same caveat as every other port in this series: no .NET SDK in this environment. The CI job will give the real answer, especially for the GemCryptor reference.
- No test coverage added for this project yet — in particular, nothing actually exercises `Helper.DecryptPassword`/`EncryptPassword` to confirm `TwoFishCrypto` genuinely works at runtime under .NET 8, only that it compiles (if it does).
- `Transaction`, `TransactionEntrySearch`, `Statement` classes from the legacy `Utils.cs` were left unported (see §1) — not blocked, just unused by anything ported so far.
- `CC_Ingenio.Core`'s `BusinessDateShim.cs` and `CC_Ingenio.DataTranfer`'s `UtilsConstsShim.cs` **have not been retired yet** — this port's `CC_Ingenio.Common` isn't wired up as a dependency of either project. That's the natural next step: point `Core`/`DataTranfer` at this real `CC_Ingenio.Common` and delete the two shims, once this project is confirmed to actually build (and ideally run) correctly.
