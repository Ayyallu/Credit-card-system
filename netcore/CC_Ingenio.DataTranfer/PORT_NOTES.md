# CC_Ingenio.DataTranfer — .NET 8 port notes

Second Phase 3 artifact, following [CC_Ingenio.Core's port](../CC_Ingenio.Core/PORT_NOTES.md). Mechanical port of the legacy `CC_Ingenio.DataTranfer` project (DTOs) to a standalone, SDK-style `net8.0` class library.

## What was done

- Copied all 166 `.cs` files actually compiled by the old `CC_Ingenio.DataTranfer.csproj` (verified against its `<Compile Include>` list).
- **Excluded 6 orphaned files** never wired into the old `.csproj`: `UpdatedCustomerAddressDTO1.cs`, `CommissionCodeDTO.cs`, `TestDTO.cs`, `UserEntityActionDTO.cs`, `UsergroupPrivilegeDTO.cs`, `UsergroupPrivilegesDTO.cs`. Recoverable from the original `CC_Ingenio.DataTranfer/` folder if needed.
- Removed 1 dead `using System.Web.Mvc;` (`MasterInfo/PaymentAllocationTypeDTO.cs` — confirmed unused).
- Removed 7 dead `using System.ComponentModel.DataAnnotations.Resources;` — this one actually failed CI: `System.ComponentModel.DataAnnotations.Resources` is a .NET-Framework-only namespace that **doesn't exist at all on .NET 8** (unlike the rest of `System.ComponentModel.DataAnnotations`, which does). Confirmed unused (0 references to anything from it) before removing, same as the other dead usings, but this one is worth noting since it wasn't just a style cleanup — it was a real CI-caught compile error (`CS0234`), not something visible from static analysis alone until an actual `dotnet build` ran.
- Excluded `Properties/AssemblyInfo.cs` (SDK auto-generates assembly attributes).
- Also ported **`CC_Ingenio_Resources`** (the localization resx project) to `netcore/CC_Ingenio_Resources/` as a real dependency, not a shim — it had zero framework blockers (no `ProjectReference`s, only BCL assembly references, pure `ResourceManager`/resx code), so it was a trivial, risk-free full port rather than something worth stubbing out.

## Two real (non-dead) legacy dependencies found

Unlike `CC_Ingenio.Core` (which only had one real dependency), this project had two:

### 1. `CC_Ingenio.Common.Utils.Utils` regex constants — shim retired

7 `[RegularExpression(...)]` data annotations reference `const string` fields on `CC_Ingenio.Common.Utils.Utils` (`MSG_ALLOWED_CHARS`, `MSG_ALLOWED_NUMERALS`, `MSG_ALLOWED_NOSPACE`, `MSG_ALLOWED_PERCENTAGE`, `MSG_VALID_PHONENO`, `MSG_VALID_EMAIL`, `MSG_VALID_MERCHANTCODE`). Attribute arguments must be compile-time constants, so the containing type has to resolve even though the value gets inlined.

A temporary `UtilsConstsShim.cs` originally declared `CC_Ingenio.Common.Utils.Utils` with just these 7 consts. **Update**: [`CC_Ingenio.Common` has since been ported](../CC_Ingenio.Common/PORT_NOTES.md) and its real `Utils.cs` includes all 7 of these constants verbatim (they were part of the framework-independent half kept in that port). `UtilsConstsShim.cs` has been **deleted**, and this project now takes a real `ProjectReference` on `CC_Ingenio.Common` instead. No call sites changed.

### 2. `System.Web.Mvc.SelectListItem` in `UserDTO.UserAllowedBranch` — shimmed (per explicit decision)

`UserDTO.UserAllowedBranch` is a `List<System.Web.Mvc.SelectListItem>` — and unlike the other 14 `System.Web.Mvc` usings found across this migration so far, **this one is genuinely used**: populated in the legacy `CC_Ingenio.Web`'s `UserController.cs` and rendered as a multi-select branch picker in `Views/User/UserDetails.cshtml`.

Since `System.Web.Mvc` doesn't exist on .NET 8, this needed a real decision (asked and confirmed): **shim `System.Web.Mvc.SelectListItem` locally** (`SelectListItemShim.cs`, same `Text`/`Value`/`Selected` shape, same namespace) rather than introduce a cleaner DTO-layer type right now. This keeps `UserDTO.cs` unchanged today, consistent with how `CC_Ingenio.Core` was ported, but **must be revisited** once `CC_Ingenio.Web` itself is ported — at that point, decide between a proper `CC_Ingenio.DataTranfer`-owned type (e.g. `SelectOption`) or taking a real dependency on ASP.NET Core's `Microsoft.AspNetCore.Mvc.Rendering.SelectListItem`.

## Not yet done / out of scope for this step

- **Verified by CI, not locally.** Same as `CC_Ingenio.Core`: this environment has no .NET SDK, so this was ported via careful static analysis (grep-verified) rather than a local `dotnet build`. The CI job added for `CC_Ingenio.Core` (`build-netcore` in `.github/workflows/build.yml`) needs to be extended to also build this project and `CC_Ingenio_Resources` before this is actually confirmed to compile.
- Not added to any solution file.
- The 6 orphaned files weren't evaluated for whether they contain logic needed elsewhere — same caveat as `CC_Ingenio.Core`'s orphans.
- `CC_Ingenio.DataTranfer` does **not** depend on `CC_Ingenio.Core` or vice versa (confirmed no cross-references) — they remain independent standalone projects, though both now reference the real ported `CC_Ingenio.Common`.
- The `SelectListItemShim.cs` for `System.Web.Mvc.SelectListItem` (§2 above) is unrelated to `CC_Ingenio.Common` and is **not** retired by this update — it stays until `CC_Ingenio.Web` is ported.
