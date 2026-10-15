# CC_Ingenio.Core — .NET 8 port notes

First concrete Phase 3 artifact (see [MIGRATION_PLAN.md](../../MIGRATION_PLAN.md)). This is a mechanical port of the legacy `CC_Ingenio.Core` project (domain aggregates + repository interfaces) to a standalone, SDK-style `net8.0` class library.

## What was done

- Copied all 415 `.cs` files that were actually compiled by the old `CC_Ingenio.Core.csproj` (verified against its `<Compile Include>` list, not just what's on disk).
- **Excluded 16 orphaned files** that existed on disk but were never wired into the old `.csproj` (so never actually part of the build): `Test.cs`, `TestFactory.cs`, `ITestRepository.cs`, `CommissionCode.cs`, `CommissionCodeFactory.cs`, `ICommissionCodeRepository.cs`, `CustomerRUT.cs`, `ICountryFactory.cs`, `LoginUser.cs`, `LoginUserFactory.cs`, `ILoginUserRepository.cs`, `UserEntityAction.cs`, `UserEntityActionFactory.cs`, `IUserEntityActionRepository.cs`, `UserEntityFactory.cs`, `UsergroupPrivilegeFactory.cs`. If any of these turn out to be needed, recover them from the original `CC_Ingenio.Core/` folder.
- **Removed 15 dead `using System.Web.Mvc;` lines** — confirmed via grep that no MVC type (`SelectListItem`, `ModelBinder`, etc.) was ever actually used in any of these files; the reference was always vestigial.
- Excluded `Properties/AssemblyInfo.cs` — SDK-style projects auto-generate assembly attributes; the old file's `[ComVisible(false)]` etc. aren't needed.
- Converted to a modern SDK-style `.csproj` targeting `net8.0` with implicit file globbing (no explicit `<Compile Include>` list needed).

## The one real dependency: `BusinessDateShim.cs`

62 `*Factory.cs` files call `CC_Ingenio.Common.Utils.Utils.BusinessDate` (a single static `DateTime` field on the legacy `CC_Ingenio.Common` project's large `Utils` class) — this was the only non-vestigial external dependency found.

`CC_Ingenio.Common` itself has not been assessed or ported to .NET 8 yet (it has real `System.Web`, Enterprise Library 6.0, and other Framework-only baggage per [PHASE2_NOTES.md](../../PHASE2_NOTES.md)/[CURRENT_VERSION.md](../../CURRENT_VERSION.md)), so referencing it directly from this net8 project isn't viable yet.

**Temporary fix**: added `BusinessDateShim.cs`, which declares a `CC_Ingenio.Common.Utils.Utils` class with just the one `BusinessDate` field, in the *same* namespace/class/field name as the legacy one. This means **none of the 62 call sites needed to change** — they compile unchanged against this shim. It is explicitly not wired to the legacy app's actual business-date state.

**This must be revisited** once `CC_Ingenio.Common` is itself assessed/ported — delete `BusinessDateShim.cs` and take a real dependency on the ported `Common` (or whatever replaces `BusinessDate` by then, e.g. a proper `IBusinessDateProvider` service instead of a mutable static field).

## Not yet done / explicitly out of scope for this step

- **Not compiled/verified.** This environment has the .NET runtime (`Microsoft.NETCore.App 10.0.7`) but **no .NET SDK installed** — `dotnet build` is not available here. This port is based on static analysis (grep-verified absence of blocking APIs) and careful mechanical file-level transformation, not an actual successful build. **Run `dotnet build` on a machine with the .NET 8 SDK before trusting this compiles.**
- Not added to any solution file yet (neither the legacy `CC_Ingenio.sln` nor a new one) — it's a standalone project under `netcore/CC_Ingenio.Core/`.
- Not consumed by anything yet — this is scaffolding for the next strangler-fig increment (likely `CC_Ingenio.DataTranfer` next, per the plan's sequencing, or a feasibility pass on `CC_Ingenio.Common` to retire the shim).
- The 16 orphaned files were excluded rather than evaluated for whether they contain logic that's actually needed elsewhere (e.g., via reflection or a build step not visible to a `.csproj` grep) — low risk given they were dead in the original build too, but worth a quick human sanity check before deleting them from the legacy project.
