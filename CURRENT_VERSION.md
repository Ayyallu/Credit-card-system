# CC_Ingenio (CRECCU Web Service) — Current State Analysis

> Prepared as a baseline for a legacy modernization effort. This document describes the **current** architecture, stack, and versions only — no upgrade work has been performed yet.

## 1. Overview

A monolithic **ASP.NET MVC 4** web application (internal name `CC_Ingenio`, deployed as "CRECCU Web service") implementing a credit-card issuing/processing back office: card booking, authorization, billing, approvals, customer service, and master data (commissions, interest, fees, products). Localized for English and Chilean Spanish (`es-CL`), including Chilean RUT (tax ID) handling — indicating a Latin American credit union / card issuer as the target customer.

No CI/CD, no automated tests, and no containerization exist today. Deployment is manual via IIS Web Deploy (`msdeploy`).

**Data sensitivity note:** This repository contains cardholder and transaction data (CSV exports under `CC_Ingenio.Web/Reports/Export/`, a packaged deployment ZIP under `CC_Ingenio.Web/Deploy/`, and customer/RUT/credit-card domain tables). Any migration work should treat this as PCI-DSS-equivalent data and must not copy real production data into test/dev/migration environments without masking.

## 2. Solution Structure

8 projects in one `.sln`, classic layered/N-tier architecture:

| Project | Role |
|---|---|
| `CC_Ingenio.Web` | ASP.NET MVC 4 web app — controllers, Razor views, App_Start, DI bootstrap |
| `CC_Ingenio.Manager` | Business/service layer (`Contracts` interfaces + implementations) |
| `CC_Ingenio.Core` | Domain aggregates + repository interfaces (`IGenericRepository`, `IUnitofWork`) |
| `CC_Ingenio.Repository` | EF Database-First repositories + EDMX model |
| `CC_Ingenio.Data` | DbContext / Unit-of-Work implementation |
| `CC_Ingenio.DataTranfer` | DTOs shuttled between Manager and Web layers |
| `CC_Ingenio.Common` | Logging and shared utilities |
| `CC_Ingenio_Resources` | Localization (en, es, es-CL) |

**Data flow:** Web (Controllers/Views) → Manager (business logic) → Core (domain/interfaces) → Repository (EF) → Data (DbContext).

Business modules: Approval Queue, Authorization (transactions, EMI/installments, reversals), Billing (statements, online payments, refinancing), Card Booking (embossing, card master), Contact/Address, Customer Service (closure, lost/stolen, limit changes), Master Data, User/Security.

## 3. Tech Stack & Versions

| Layer | Technology | Version | Status |
|---|---|---|---|
| Runtime | .NET Framework | **4.8** | Last-ever release of .NET Framework — dead-end runtime, no future versions planned |
| Web framework | ASP.NET MVC | 4.0.20710.0 | EOL |
| Web framework | ASP.NET Web API | 4.0.20710.0 | EOL |
| View engine | Razor | 2.0.20710.0 (Web) / 3.3.0 (Common) | Inconsistent across projects |
| ORM | Entity Framework | 5.0.0, Database-First (EDMX) | EOL; EDMX workflow deprecated by Microsoft |
| DI container | Castle Windsor / Castle.Core | 3.2.1 / 3.2.0 | ~12 years old |
| Auth | DotNetOpenAuth + Forms Auth / SimpleMembership | 4.0.3.12153 | Project discontinued; pre-ASP.NET-Identity pattern |
| JSON | Newtonsoft.Json | 4.5.6 | Very old (current is 13.x) |
| Cross-cutting | Enterprise Library (Common/ExceptionHandling/Logging) | 6.0.1304.0 | Discontinued by Microsoft ~2013 |
| Reporting | SAP Crystal Reports for VS | 12.0.2000.0 | Requires Crystal Reports runtime on server; fragile machine-specific HintPath reference |
| Excel export | Microsoft.Office.Interop.Excel | (COM interop) | Anti-pattern — requires MS Office installed on server |
| Database | SQL Server (via `System.Data.SqlClient`) | EDMX provider manifest targets SQL Server 2008 compatibility | Legacy ADO.NET provider |

### Front-end (vendored under `CC_Ingenio.Web/Scripts`)

| Library | Version | Notes |
|---|---|---|
| jQuery | 1.8.3 / 1.9.1 vendored (2.1.0 in packages.config) | EOL, version drift; known CVEs in this era |
| jQuery UI | 1.10.0 | EOL |
| Knockout.js | 3.1.0 | Old |
| DataTables | 1.8.1 | Extremely old (current major is v2) |
| Modernizr | 2.5.3 | Old |
| Moment.js | 2.5.1 | Upstream project now in maintenance mode |
| WebGrease | 1.1.0 | Deprecated bundling tool |
| PagedList / PagedList.Mvc | 1.17.0.0 / 4.5.0.0 | Old |

### Unidentified proprietary components

`References/GemCryptor.dll`, `References/KeyGeneration.dll` — no visible source or NuGet origin (likely card-data encryption/licensing). **Key migration risk:** nothing to port forward without the vendor/source.

## 4. Database

- SQL Server, EF5 Database-First; EDMX `ProviderManifestToken="2008"` (modeled against SQL Server 2008 compatibility).
- `CC_Ingenio.Repository/App.Config` connection string points to a dev placeholder (`(LocalDb)\v11.0`, catalog `TestMVC.Models.TestMVCDbContext`) — a leftover from project scaffolding, not a real environment string. Real environments presumably use `Web.Debug.config` / `Web.Release.config` transforms or `Deploy/parameters.xml`.
- Core tables: `CONTACTS`, `Addresses`, `ContactAddresses`, plus a large `MasterRepo` domain set (Customer, CustomerCreditCardDetail, CustomerCurrentAccountDetail, CustomerRUT, Billing, Commission, Interest, Transaction, etc.) — 60+ repository classes in `MasterRepo` alone.

## 5. Build & Deployment

- Classic (non-SDK-style) `.csproj` files, built via MSBuild/Visual Studio with the .NET Framework 4.8 targeting pack.
- **No CI/CD** (no YAML pipelines, no Jenkinsfile, no Dockerfile).
- Deployment via IIS Web Deploy (`msdeploy`); `CC_Ingenio.Web/Deploy/` contains `archive.xml`, `parameters.xml`, `systemInfo.xml`, and a built deployment package (`CRECCU Web service.zip`) checked directly into source control.
- `Deploy/systemInfo.xml` shows the original deployment target was **IIS 7.5 on Windows Server 2008 R2** (32-bit, ASP.NET 2.0/4.0 era) — the app was later retrofitted to target .NET 4.8, but `Web.config` still has a vestigial `httpRuntime targetFramework="4.5"` node alongside a 4.8 one, evidence of an incomplete historical upgrade.

## 6. Codebase Size

- ~1,798 files total (excluding `.git`)
- 932 `.cs` files (~108,000 LOC)
- 236 `.cshtml` Razor views (~49,000 LOC)
- Combined estimate: **~157,000 LOC** (C#/Razor only, excludes vendored JS/CSS)

## 7. Legacy / Technical-Debt Indicators

- Entire stack (runtime, ORM, DI container, auth library, JSON serializer, reporting engine, cross-cutting framework, and all major JS libraries) is EOL or discontinued.
- **Zero automated tests** anywhere in the tree (no test projects, no NUnit/xUnit/MSTest references) — no regression safety net for a migration.
- No CI/CD; deployment is manual and artifact-based.
- Duplicate/abandoned code checked into source: `BillingController.cs` / `BillingController1.cs` / `BillingController2.cs` / `BillingControllerold.cs`, `HomeController1.cs`, `UserController1.cs`, `MasterManager.cs` / `MasterManager - Copy.cs`, plus a stray `Web - Copy.config` at the repo root.
- EDMX/data model namespace is still literally `TestMVC.Models.TestMVCDbContextModel` — never renamed from the original scaffold/tutorial.
- Heavy reliance on server-side Office Interop and Crystal Reports — both carry licensing and server-install dependencies that complicate any move to cloud/containers.

## 8. Modernization Considerations (for planning, not yet actioned)

Given the scope (full-stack EOL, no tests, no CI/CD, ~157K LOC, PCI-sensitive domain), an in-place version bump is not realistic. A phased **strangler-fig** approach is recommended once planning begins:

- Target stack candidate: .NET 8+ / ASP.NET Core MVC or Blazor, EF Core (code-first or reverse-engineered from the existing schema), ASP.NET Core Identity or OIDC in place of DotNetOpenAuth/SimpleMembership.
- Replace Crystal Reports and Office Interop with a modern reporting library and a non-interop Excel library (e.g., ClosedXML/EPPlus/OpenXML).
- Resolve `GemCryptor.dll` / `KeyGeneration.dll` — confirm vendor support or reimplement, since no source is available in this repo.
- Introduce automated tests and CI/CD before/alongside the migration to create a regression safety net.
- Clean up duplicate/dead controllers and stray config files before migrating, to avoid porting ambiguity.

*This document reflects the state of the codebase as analyzed on 2026-10-15. It does not include any customer, cardholder, or transaction data — sensitive files were identified by path only and not opened.*
