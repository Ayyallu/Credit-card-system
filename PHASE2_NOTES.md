# Phase 2 — Dependency Modernization Notes

Tracks findings and work done for [MIGRATION_PLAN.md](MIGRATION_PLAN.md) Phase 2. As with Phase 1, there is no MSBuild/.NET Framework toolchain in this environment, so none of these changes have been build-verified — verify via CI or a local Visual Studio build before merging.

## NuGet version bumps

Updated `CC_Ingenio.Web/packages.config` and the matching `<Reference>`/`HintPath` entries in `CC_Ingenio.Web.csproj` (the only project referencing these packages):

| Package | Before | After |
|---|---|---|
| Newtonsoft.Json | 4.5.6 | 13.0.3 |
| Castle.Core | 3.2.0 | 5.1.1 |
| Castle.Windsor | 3.2.1 | 5.1.2 |

**Risk assessment:** Newtonsoft.Json's `JsonConvert`/serializer API used here has been stable since 4.x — low risk. Castle Windsor's fluent registration API used in `BootStrapContainer.cs` (`Classes.FromThisAssembly()`, `Component.For<T>().ImplementedBy<T>()`, `Types.FromThisAssembly()`) has also been stable across major versions — low-to-moderate risk, but this is the most-used piece of infrastructure in the app (every controller and ~140 repository/manager components are registered through it), so **this must be verified with an actual build and a smoke-test login** before merging.

**Caveat:** I could not run `nuget restore` in this environment to confirm the exact `lib/<tfm>/` folder names inside each new package version. The `HintPath` values I set (`net462` for Castle.*, `net45` for Newtonsoft.Json) are based on those packages' known structure as of their respective releases, but should be confirmed by actually restoring packages before the build is trusted.

## Office Interop Excel — investigated, partially cleaned up

Searched all real usages of `Microsoft.Office.Interop.Excel` across the codebase. Findings:

- **`CC_Ingenio.Web/Controllers/BillingController.cs`**: imported `Excel = Microsoft.Office.Interop.Excel` but **never actually used it** anywhere in the file. **Removed the unused `using` statement** — zero behavior change, safe.
- **`BillingManager.ImportFromExcel()`** (`CC_Ingenio.Manager/BillingManager.cs`): does *not* use COM Interop — it reads the uploaded Excel file via the `Microsoft.ACE.OLEDB.12.0` OLE DB provider and a SQL query against `[Sheet1$]`. No code change needed here for Phase 2, though the ACE OLEDB provider itself is a separate legacy dependency (32-bit/64-bit driver install requirement) worth revisiting during the .NET 8 migration.
- **The report export methods actually used for most reports** (`DataSetToExcel` / `DataTableToExcel` in both `ReportViewerController.cs` and `BillingController.cs`) also do **not** use COM Interop — they generate an HTML table and stream it back with `Content-Type: application/vnd.ms-excel` (a common legacy "fake Excel" trick that Excel happens to open). No interop dependency here either.
- **The one real COM Interop usage** is `ReportViewerController.ExcelImport()` / `ExportExcel()`, used specifically for the "Daily Balance Report" flow. It opens a **macro-enabled template** (`DailyBalanceReport.xlsx`) via `Excel.Application` and calls `RunMacro(oExcel, new Object[] { "Run_Program" })` — i.e., it invokes an embedded VBA macro.

**Not replaced in this pass.** A library like ClosedXML/EPPlus can write cells, formulas, and formatting, but **cannot execute an existing VBA macro** embedded in a template file. Faithfully replacing this path requires knowing what `Run_Program` actually does — and that logic lives inside the binary `.xlsm`/`.xlsx` macro storage, not in any file visible to source control or static analysis here. Replacing it without that knowledge risks silently changing the Daily Balance Report's output, which is a financial report.

**Recommendation:** before touching this path, someone with access to open `DailyBalanceReport.xlsx` in Excel should extract and review the `Run_Program` macro's VBA source, so its logic can be reimplemented in C# (e.g., with ClosedXML) as part of Phase 3, with a side-by-side output comparison against the legacy path.

## Crystal Reports — replacement research (no code changed)

Current usage (`ReportViewerController.cs`, `BillingController.cs`): `CrystalDecisions.CrystalReports.Engine.ReportDocument`, loading `.rpt` files (`StatementGeneration.rpt`, `RenewalCards.rpt`, `SampleReport.rpt`) and populating them with ADO.NET parameters via a custom `CreateReportDocument(...)` helper.

This is the **longest-lead item** in the whole modernization because SAP Crystal Reports for Visual Studio has **no supported runtime on .NET Core/.NET 5+** — it only runs on .NET Framework. Options, roughly in order of migration effort:

| Option | Migration effort | Notes |
|---|---|---|
| **Keep Crystal Reports, isolate it** | Low now, pays off later | Run the existing .NET Framework 4.8 report-generation code as a separate microservice/Windows service behind an internal API, called by the new .NET 8 app. Defers the `.rpt` rewrite indefinitely but adds an extra moving part to operate. |
| **QuestPDF** | Medium-high | Modern, actively maintained, code-first (C# fluent API, no WYSIWYG designer) — every `.rpt` must be manually re-authored as a QuestPDF document. Free for small companies/OSS, commercial license otherwise. |
| **Telerik Reporting / Stimulsoft / other commercial .NET Core reporting suites** | Medium | Closer like-for-like replacement (has a report designer), but it's another paid/proprietary dependency — not actually solving the vendor-lock-in problem, just changing vendors. |
| **Rebuild as Razor views → PDF (e.g., with a HTML-to-PDF library)** | Medium | Reuses existing Razor/HTML skills already in the team; works well for simpler statement-style reports; more effort for complex multi-section reports like `StatementGeneration.rpt`. |

Only 3 `.rpt` files exist today (`SampleReport.rpt` looks like a template/demo, not production), which somewhat limits the blast radius — but `StatementGeneration.rpt` is a core customer-facing billing statement, so its replacement needs careful side-by-side validation regardless of which option is chosen.

**No decision made or code changed** — this needs a stakeholder call (budget for a commercial tool vs. engineering time to hand-rewrite reports) before Phase 3 can proceed on this front, per the open question already logged in [MIGRATION_PLAN.md](MIGRATION_PLAN.md).

## Front-end libraries — plan only, no code changed

Vendored libraries (jQuery 1.8.3/1.9.1, jQuery UI 1.10.0, Knockout 3.1.0, DataTables 1.8.1, Modernizr 2.5.3) were **not bumped** in this pass. Reasoning: there are 236 Razor views and no UI test coverage at all (confirmed in [CURRENT_VERSION.md](CURRENT_VERSION.md)), so a library swap here cannot be safely verified — a jQuery 1.x → 3.x jump in particular has known breaking changes (removed `$.browser`, changed AJAX globalEval behavior, stricter CSS selector handling) that are very likely to silently break specific pages in a 236-view app with no automated or manual regression pass planned yet.

**Recommended approach instead of a blind bump:**
1. Do this only after Phase 1's test coverage has grown enough to include at least smoke tests for the highest-traffic pages (so breakage is caught).
2. Bump one library at a time, starting with the lowest-risk one (Modernizr — feature-detection only, rarely referenced directly in app code) before touching jQuery itself.
3. For jQuery specifically, budget time for a manual regression pass across major modules (Authorization, Billing, Card Booking, Customer Service) since this is the library most views depend on directly.

This has been left as a planning note rather than executed code changes, to avoid introducing untested UI regressions into a PCI-scoped production app.

## CI build blocker — Crystal Reports (discovered after pushing Phase 1/2)

Pushing `phase-1` to trigger the GitHub Actions build revealed a **pre-existing** issue, not caused by anything in Phase 1/2:

1. `CC_Ingenio.Common`, `CC_Ingenio.Core`, `CC_Ingenio.Manager`, and `CC_Ingenio.DataTranfer` all referenced `System.Web.Mvc` with **no HintPath** — relying on it being registered in the GAC by an ASP.NET MVC 4 installer on a dev machine, which a clean CI runner doesn't have. **Fixed**: added `packages.config` entries (and csproj HintPaths) pointing these projects at the `Microsoft.AspNet.Mvc` NuGet package already used in `CC_Ingenio.Web`.
2. **Not fixable from CI alone**: `CC_Ingenio.Common/Utils/Report.cs` directly uses `CrystalDecisions.CrystalReports.Engine.ReportDocument` and friends, and the project's `CrystalDecisions.*` references point at a hardcoded local path (`C:\Program Files\Common Files\managed\...`) that only exists if SAP Crystal Reports for Visual Studio is installed on that machine. Since **every other project in the solution depends on `CC_Ingenio.Common`**, this isn't just a `CC_Ingenio.Web` problem — it blocks the entire solution (including `CC_Ingenio.Tests`) from building on a stock GitHub-hosted runner.

I did not attempt to script an install of Crystal Reports for Visual Studio in CI — it's a licensed, interactively-installed product, and I wasn't confident enough in any specific download URL to use it (a wrong or stale one is worse than none). The workflow now runs the build and test steps with `continue-on-error: true` and a clear `::notice::` explaining the blocker, so CI doesn't silently claim success while also not masking the real cause.

**Options going forward:**
- Set up a **self-hosted GitHub Actions runner** on a machine that already has Crystal Reports for Visual Studio installed (the most direct fix, if such a machine exists or can be provisioned).
- **Isolate the Crystal Reports dependency out of `CC_Ingenio.Common`** so everything that doesn't need it can build and test cleanly in CI even before Crystal Reports itself is replaced.
- Accept that CI only validates syntax/intent via `continue-on-error` for now, and treat a local Visual Studio build (with Crystal Reports for VS installed) as the real gate until one of the above is done.

### Correction: the extraction was bigger than first described

I initially recommended "move `Report.cs` into `CC_Ingenio.Web`" as a mechanical, low-risk fix. That was wrong — checking all callers of `ReportGenerator`/`ReportParam` showed **`CC_Ingenio.Manager`** (`BillingManager.cs`, `CardBookingManager.cs`) also instantiates `ReportGenerator` directly, not just `CC_Ingenio.Web`. Since `Manager` sits below `Web` in the layering (`Web` depends on `Manager`, never the reverse), moving `Report.cs` into `Web` would have broken `Manager`'s build instead of fixing anything — just trading one failure for another.

**What was actually done**: created a new project, **`CC_Ingenio.Reporting`**, containing `Report.cs` (namespace left as `CC_Ingenio.Common.Utils` unchanged, so none of the 6 call sites across `Manager` and `Web` needed any edits) plus the `CrystalDecisions.*` references. `CC_Ingenio.Common` no longer references Crystal Reports at all. `CC_Ingenio.Manager` and `CC_Ingenio.Web` now reference `CC_Ingenio.Reporting` via a `ProjectReference` instead of referencing Crystal Reports directly for this purpose (`CC_Ingenio.Web` still has its own direct `CrystalDecisions.*` references too, since its controllers use `ReportDocument` types directly for other things — those were left as-is).

**Net effect for CI**: `CC_Ingenio.Common`, `CC_Ingenio.Core`, `CC_Ingenio.Data`, `CC_Ingenio.Repository`, `CC_Ingenio.DataTranfer`, `CC_Ingenio_Resources`, and — critically — **`CC_Ingenio.Tests`** can now build and run on a stock GitHub-hosted runner with no Crystal Reports install. `CC_Ingenio.Reporting`, `CC_Ingenio.Manager`, and `CC_Ingenio.Web` remain blocked until Crystal Reports for VS is available on the build machine (self-hosted runner) or is replaced (see the Crystal Reports replacement research above) — that scope is now much smaller and clearly isolated, rather than silently blocking everything.

**Still unverified**: I have no MSBuild toolchain here to confirm this actually compiles — it needs a real CI run or local Visual Studio build to be trusted. The GUIDs used for the new project are freshly generated and not verified against VS's own GUID generator, though any well-formed GUID works for this purpose.

## Summary of what actually changed in this pass

- `CC_Ingenio.Web/packages.config` + `CC_Ingenio.Web.csproj`: Newtonsoft.Json, Castle.Core, Castle.Windsor bumped to current versions (needs build verification)
- `CC_Ingenio.Web/Controllers/BillingController.cs`: removed one unused `using Microsoft.Office.Interop.Excel` import (zero risk)
- `CC_Ingenio.Common`, `CC_Ingenio.Core`, `CC_Ingenio.Manager`, `CC_Ingenio.DataTranfer`: fixed `System.Web.Mvc` references to resolve via the `Microsoft.AspNet.Mvc` NuGet package instead of relying on a GAC install
- New `CC_Ingenio.Reporting` project: isolates the Crystal Reports dependency (previously in `CC_Ingenio.Common`) so it no longer blocks the rest of the solution, including the test suite, from building in CI
- `.github/workflows/build.yml`: splits the build into a real CI gate (everything that doesn't need Crystal Reports, including running the test suite) and a `continue-on-error` attempt at the Crystal-Reports-dependent projects, with a clear notice explaining why
- Office Interop removal (the macro-dependent path), Crystal Reports replacement, and front-end bumps remain documented as follow-up work requiring either more information (macro contents, stakeholder decision) or test coverage that doesn't exist yet.
