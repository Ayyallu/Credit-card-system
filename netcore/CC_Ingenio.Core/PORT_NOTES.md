# CC_Ingenio.Core — .NET 8 port notes

First concrete Phase 3 artifact (see [MIGRATION_PLAN.md](../../MIGRATION_PLAN.md)). This is a mechanical port of the legacy `CC_Ingenio.Core` project (domain aggregates + repository interfaces) to a standalone, SDK-style `net8.0` class library.

## What was done

- Copied all 415 `.cs` files that were actually compiled by the old `CC_Ingenio.Core.csproj` (verified against its `<Compile Include>` list, not just what's on disk).
- **Excluded 16 orphaned files** that existed on disk but were never wired into the old `.csproj` (so never actually part of the build): `Test.cs`, `TestFactory.cs`, `ITestRepository.cs`, `CommissionCode.cs`, `CommissionCodeFactory.cs`, `ICommissionCodeRepository.cs`, `CustomerRUT.cs`, `ICountryFactory.cs`, `LoginUser.cs`, `LoginUserFactory.cs`, `ILoginUserRepository.cs`, `UserEntityAction.cs`, `UserEntityActionFactory.cs`, `IUserEntityActionRepository.cs`, `UserEntityFactory.cs`, `UsergroupPrivilegeFactory.cs`. If any of these turn out to be needed, recover them from the original `CC_Ingenio.Core/` folder.
- **Removed 15 dead `using System.Web.Mvc;` lines** — confirmed via grep that no MVC type (`SelectListItem`, `ModelBinder`, etc.) was ever actually used in any of these files; the reference was always vestigial.
- Excluded `Properties/AssemblyInfo.cs` — SDK-style projects auto-generate assembly attributes; the old file's `[ComVisible(false)]` etc. aren't needed.
- Converted to a modern SDK-style `.csproj` targeting `net8.0` with implicit file globbing (no explicit `<Compile Include>` list needed).

## The one real dependency: `BusinessDate` — shim retired

62 `*Factory.cs` files call `CC_Ingenio.Common.Utils.Utils.BusinessDate` (a single static `DateTime` field on `CC_Ingenio.Common`'s `Utils` class) — this was the only non-vestigial external dependency found.

At the time this project was first ported, `CC_Ingenio.Common` hadn't been assessed for .NET 8 yet, so a temporary `BusinessDateShim.cs` was added (same namespace/class/field name, so none of the 62 call sites needed to change).

**Update**: [`CC_Ingenio.Common` has since been ported](../CC_Ingenio.Common/PORT_NOTES.md), and its `Utils.cs` genuinely includes `BusinessDate` (it was part of the framework-independent half kept in that port). `BusinessDateShim.cs` has been **deleted**, and this project now takes a real `ProjectReference` on `CC_Ingenio.Common` instead. No call sites changed — the real `Utils.BusinessDate` has the identical shape the shim provided.

## Not yet done / explicitly out of scope for this step

- **Not compiled/verified.** This environment has the .NET runtime (`Microsoft.NETCore.App 10.0.7`) but **no .NET SDK installed** — `dotnet build` is not available here. This port is based on static analysis (grep-verified absence of blocking APIs) and careful mechanical file-level transformation, not an actual successful build. **Run `dotnet build` on a machine with the .NET 8 SDK before trusting this compiles.**
- Not added to any solution file yet (neither the legacy `CC_Ingenio.sln` nor a new one) — it's a standalone project under `netcore/CC_Ingenio.Core/`.
- Not consumed by anything yet (no project depends on this one) — `CC_Ingenio.DataTranfer` and `CC_Ingenio.Common` have since been ported alongside it, but `Core` and `DataTranfer` don't reference each other.
- The 16 orphaned files were excluded rather than evaluated for whether they contain logic that's actually needed elsewhere (e.g., via reflection or a build step not visible to a `.csproj` grep) — low risk given they were dead in the original build too, but worth a quick human sanity check before deleting them from the legacy project.
