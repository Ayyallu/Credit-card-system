# CC_Ingenio Modernization — Migration Plan

> Companion to [CURRENT_VERSION.md](CURRENT_VERSION.md). This is a proposed phased plan — sequencing and scope should be confirmed before execution begins.

## Guiding principles

- **Strangler-fig, not big-bang.** ~157K LOC, zero automated tests, PCI-sensitive domain — a full rewrite-and-cutover carries too much risk. Each phase should leave the app shippable.
- **Safety net before surgery.** No framework change should happen before there's a way to detect regressions (tests, or at minimum a tested parallel-run/diffing strategy).
- **No real cardholder data in dev/test/migration tooling.** Masked or synthetic data only.

## Target stack (proposed)

| Layer | Current | Target |
|---|---|---|
| Runtime | .NET Framework 4.8 | .NET 8 (LTS) |
| Web framework | ASP.NET MVC 4 | ASP.NET Core MVC |
| ORM | EF5, Database-First (EDMX) | EF Core, reverse-engineered from existing schema, migrated to Code-First over time |
| DI | Castle Windsor 3.2 | Built-in ASP.NET Core DI (or Windsor on Core, if the team prefers continuity) |
| Auth | DotNetOpenAuth + Forms/SimpleMembership | ASP.NET Core Identity or OIDC (Azure AD / Keycloak, depending on org identity provider) |
| JSON | Newtonsoft.Json 4.5.6 | System.Text.Json (or current Newtonsoft, if full compatibility needed short-term) |
| Cross-cutting | Enterprise Library 6.0 | Microsoft.Extensions.Logging + Polly (retry/resilience) |
| Reporting | Crystal Reports 12 | QuestPDF, or a licensed replacement (e.g., Telerik Reporting) — avoid another proprietary lock-in if possible |
| Excel export | Office Interop | ClosedXML or EPPlus (non-interop, server-safe) |
| Front-end | jQuery 1.8/1.9, Knockout 3.1, DataTables 1.8 | Incrementally replace per-page; no big-bang SPA rewrite required initially |
| Deployment | Manual IIS Web Deploy | CI/CD pipeline (Azure DevOps/GitHub Actions) + containerization (optional, post-migration) |

## Phases

### Phase 0 — Groundwork (prerequisite, not yet started)
- Remove dead/duplicate code identified in the analysis (`BillingController1/2/old`, `HomeController1`, `UserController1`, `MasterManager - Copy.cs`, `Web - Copy.config`) to avoid porting ambiguity.
- Stand up CI (build + lint) for the current .NET Framework 4.8 codebase so regressions are caught immediately, even before any test suite exists.
- Resolve the `GemCryptor.dll` / `KeyGeneration.dll` unknowns — confirm vendor/source or plan a reimplementation; these block any dependency upgrade that touches encryption.
- Establish a masked/synthetic dataset for dev and migration testing; stop referencing real exports (`Credit_Notes_*.csv`, `Payments_*.csv`, etc.) outside production.

### Phase 1 — Safety net
- Introduce automated tests against current behavior: unit tests for Manager/Core business logic (highest value, least UI churn risk), and a thin layer of integration/smoke tests for critical flows (authorization, billing, approvals).
- Fix the config drift already found (`httpRuntime targetFramework` 4.5 vs 4.8 in `Web.config`; mismatched Razor/WebPages versions across projects) so the baseline is internally consistent before touching the runtime.
- Document real production connection strings/config (currently only a dev placeholder exists in source) — needed to validate parity during migration.

### Phase 2 — Dependency modernization (still on .NET Framework 4.8)
- Upgrade in place where possible without a runtime change: Newtonsoft.Json, Castle Windsor, jQuery/jQuery UI/DataTables/Knockout to current EOL-safe versions.
- Replace Office Interop Excel export with ClosedXML/EPPlus.
- Evaluate Crystal Reports replacement options (licensing, report complexity) — this is likely the longest-lead item.

### Phase 3 — Runtime migration to .NET 8
- Port `CC_Ingenio.Core`, `CC_Ingenio.DataTranfer`, `CC_Ingenio.Common` first (lowest framework coupling) to .NET Standard/.NET 8 class libraries.
- Migrate `CC_Ingenio.Data`/`CC_Ingenio.Repository` from EF5/EDMX to EF Core (reverse-engineered scaffold from the existing SQL Server schema, preserving schema compatibility).
- Rebuild `CC_Ingenio.Web` as ASP.NET Core MVC, porting controllers/views module by module (Approval Queue, Authorization, Billing, Card Booking, Contact, Customer Service, Master Data, User/Security) — each module ships independently behind the safety-net tests from Phase 1.
- Replace DotNetOpenAuth/SimpleMembership with ASP.NET Core Identity or OIDC.

### Phase 4 — Deployment & operations
- Build CI/CD pipeline (build, test, deploy) to replace manual Web Deploy.
- Containerize if the org's hosting strategy calls for it; otherwise target IIS on a current Windows Server + .NET 8 hosting bundle.
- Decommission the checked-in deployment ZIP and dev-placeholder connection string from source control.

## Key risks

- **No tests today** — every phase before "Phase 1 — Safety net" is riskier without one; consider front-loading at least smoke tests for the highest-value flows (authorization, billing) even before full Phase 1.
- **Unknown proprietary DLLs** (`GemCryptor.dll`, `KeyGeneration.dll`) could block the EF/encryption-adjacent migration if no source or vendor support exists.
- **Crystal Reports and Office Interop replacements** are likely the longest-lead, highest-effort items — start vendor/tooling evaluation early, in parallel with Phase 0–1.
- **PCI scope**: any new environment (CI, test data, migration tooling) must avoid real cardholder data; this should be a hard gate before Phase 2 begins.

## Open questions for stakeholders

1. Is a cloud/container target in scope, or is on-prem IIS staying for the foreseeable future?
2. Who owns `GemCryptor.dll` / `KeyGeneration.dll` — can source or vendor support be located?
3. What's the organization's identity provider (for the auth replacement decision)?
4. Is there budget/appetite for a Crystal Reports replacement, or should Phase 2 keep Crystal Reports on life support through the .NET 8 migration (it is not compatible with .NET Core/8 without a separate compatibility layer)?
