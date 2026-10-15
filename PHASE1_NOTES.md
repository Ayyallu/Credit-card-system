# Phase 1 — Safety Net Notes

Tracks findings and work done for [MIGRATION_PLAN.md](MIGRATION_PLAN.md) Phase 1.

## Config/version drift

- **Fixed**: `CC_Ingenio.Web/Web.config` had two conflicting `httpRuntime targetFramework` values (`4.8` and a vestigial `4.5`). The `4.5` node has been updated to `4.8` to match the project's actual `TargetFrameworkVersion`.
- **Investigated, not changed**: `CC_Ingenio.Common/packages.config` references `Microsoft.AspNet.Razor`/`Microsoft.AspNet.WebPages` 3.3.0, while `CC_Ingenio.Web` uses 2.0.20710.0 (paired with ASP.NET MVC 4.0.20710.0). No source file in `CC_Ingenio.Common` actually uses Razor/WebPages APIs — these appear to be unused leftover references. They were **not removed** in this pass because there is no MSBuild/.NET Framework toolchain available in this environment to verify the build still succeeds afterward (only `dotnet` CLI is present, which cannot build this classic non-SDK MVC4 solution). Recommend removing the unused references in a change that can be build-verified on a Windows dev machine or via the new CI workflow (see below) before merging.

## Initial unit test project

Added `CC_Ingenio.Tests` (MSTest, .NET Framework 4.8), wired into `CC_Ingenio.sln`, with a `ProjectReference` to `CC_Ingenio.Common`. First tests target pure, dependency-free logic in `CC_Ingenio.Common/Utils/Utils.cs`:

- `CheckRangeOverlaps` — 4 cases (overlapping, adjacent, disjoint, nested ranges)
- `GetRecordStatus` — unknown code / known code cases

These were chosen deliberately as the lowest-risk, highest-value starting point: no database, no HTTP context, no UI dependency — just business logic that can be safely refactored against once these tests exist.

**Not yet verified by an actual build** — this environment has no MSBuild/.NET Framework SDK to compile a classic MVC4 solution. The CI workflow added in Phase 0 (`.github/workflows/build.yml`) now also restores and runs these tests on `windows-latest`; the first real signal on whether this compiles will come from that pipeline (or a local Visual Studio build) once pushed.

## Connection string documentation

Added [CONNECTION_STRINGS.md](CONNECTION_STRINGS.md) — a checklist of what's needed per environment (Dev/QA/Staging/Production), with explicit guidance not to commit real hostnames/credentials to source control. No real connection strings were written anywhere; this is a template awaiting input from whoever owns the infrastructure.

**Note on existing config**: while fixing the `httpRuntime` drift, `CC_Ingenio.Web/Web.config` was observed to already contain **encrypted** password values and a commented-out database name in `appSettings`. These were not modified, not copied elsewhere, and are not reproduced in this document — flagging only that this file should be treated as containing sensitive configuration and reviewed for whether it belongs in source control at all (a secrets manager is the better long-term home).

## Next

- Verify `CC_Ingenio.Tests` actually builds and the new tests pass (via CI or a local Visual Studio build) — this is unverified until then.
- Decide whether to remove the unused Razor/WebPages references in `CC_Ingenio.Common` now (low risk) or defer to Phase 2/3.
- Fill in [CONNECTION_STRINGS.md](CONNECTION_STRINGS.md) with real environment details, stored via a secrets manager rather than this repo.
