# Production Modernization Runbook

Status: active engineering checklist  
Working branch: `modernization/production-safe-next-phase`  
Production branch: `master` (must remain unchanged until an approved release)

This runbook governs the modernization of the existing Ingenio credit-card application. The objective is to preserve business behaviour while replacing unsupported or fragile technology in controlled, reversible increments.

## Non-negotiable safety rules

- Never deploy from this branch directly to production.
- Never commit production connection strings, passwords, API keys, encryption keys, certificates, or exports.
- Never use production cardholder/customer data in developer machines, CI, test fixtures, screenshots, or migration databases.
- Use a formally approved, masked database backup or synthetic dataset. Record the masking owner and validation evidence.
- No automatic schema migrations or destructive DDL against production. Database changes require reviewed, versioned scripts, a backup/restore plan, and a separately approved maintenance window.
- Preserve the legacy application as the behavioural reference until module parity is demonstrated.
- All changes must be additive or isolated until the replacement path has passed regression and operational checks.

## Current branch baseline

The existing `phase-1` work includes a .NET 8 / EF Core 8 port of Core, Resources, DataTransfer, Common, Data, Repository and Manager. Offline model/repository tests do not prove behaviour against SQL Server.

**Runtime support decision:** Microsoft lists .NET 8 end of support as 2026-11-10. The current date is 2026-11-03. .NET 10 is the supported LTS target through 2028-11-14. Therefore .NET 8 is retained temporarily as a migration baseline only; it is not an acceptable production target for a new cutover. Before adding new production-hosted services, upgrade the port as a coordinated change to .NET 10 and compatible EF Core 10 packages, then run the full compatibility suite. Do not perform a blind package/TFM replacement without building and testing all projects.

Reference: https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core

## Phase A — establish a trustworthy baseline

- [ ] Run the legacy solution build on a Windows machine with the required Visual Studio workloads and Crystal Reports runtime/design-time components.
- [ ] Run the existing `CC_Ingenio.Tests` suite and save the build/test logs as a CI artifact.
- [ ] Run all current netcore build and test projects; record exact SDK/package versions and results.
- [ ] Inventory the production SQL Server version, compatibility level, collation, schema, constraints, indexes, triggers, stored procedures, SQL Agent jobs and report dependencies. Store only non-secret metadata in this repository.
- [ ] Identify the business owner and technical owner for Billing, Authorization, Card Booking, Customer Service, Approval Queue, Master Data and Statements.
- [ ] Capture approved baseline examples for critical workflows using synthetic/masked data: login/privileges, authorization, cancellation, billing, EMI, payment posting, adjustment, maker-checker approval, statement and report generation.
- [ ] Agree measurable parity criteria with the business owner (record counts, totals, rounding, status transitions, audit entries and generated document comparisons).

Exit gate: baseline build is reproducible and critical workflows have approved expected outcomes.

## Phase B — security and configuration containment

- [ ] Treat all credentials and cryptographic material previously committed to Git history as potentially exposed. Security/infrastructure owners must determine validity and rotate active credentials through the normal change process.
- [ ] Move environment secrets to the approved secret manager / deployment secret store. Source control should contain names and configuration shape only, never values.
- [ ] Remove obsolete secrets from current tracked files only after consumers have been migrated and validated. History cleanup is a separate coordinated operation; do not rewrite shared Git history without repository-owner approval.
- [ ] Replace reversible password comparison with a modern password-hashing/identity design only after documenting legacy compatibility, password reset/migration strategy, lockout, MFA/SSO and rollback.
- [ ] Set production-safe error handling and disable debug compilation in the eventual hosted ASP.NET Core application. Do not blindly edit the current legacy production config without a tested release plan.
- [ ] Audit authorization boundaries, session handling, CSRF protection, output encoding and all `Html.Raw` call sites for user-controlled content.
- [ ] Confirm PCI scope, data retention, audit logging, encryption at rest/in transit and access controls with the organization's security/compliance owner.

Exit gate: no active secret is required from source control; credential rotation and security ownership are documented; authentication migration has a tested rollback path.

## Phase C — SQL Server compatibility proof

- [ ] Provision a dedicated, isolated SQL Server test database from an approved masked backup or a synthetic schema/data set.
- [ ] Ensure the test identity has only the permissions required by the test suite. Never point CI at production.
- [ ] Keep the schema unchanged initially. Capture schema metadata before testing.
- [ ] Run the .NET 10 / EF Core 10 port against this database with explicit connection encryption settings and certificate validation appropriate to the environment.
- [ ] Validate all 127 DbSets, composite keys/foreign keys, decimal precision, datetime mappings, nullability, indexes, cascade rules and table/column names against the real schema.
- [ ] Exercise raw SQL and stored-procedure calls, including parameter types, output parameters, transactions, isolation levels and timeout behaviour.
- [ ] Compare read/write results with the legacy application for the approved critical workflows.
- [ ] Test rollback, concurrency, duplicate submission/idempotency, deadlocks and retry behaviour. Do not add retries to non-idempotent financial writes without a business-safe design.

Exit gate: documented schema diff is reviewed; critical workflows pass parity checks; transaction and concurrency behaviour is understood; no production connection was used.

## Phase D — request isolation and service lifetimes

- [ ] Before the new Web host is introduced, register each EF Core DbContext/UnitOfWork as scoped (one instance per request/unit of work).
- [ ] Repositories and managers that depend on the DbContext must be scoped; stateless pure helpers may be singleton only after review.
- [ ] Add automated tests proving two concurrent scopes receive different DbContext instances and do not share tracked entities.
- [ ] Replace process-wide mutable user/organization/business-date statics with an explicit per-request current-user/tenant context.
- [ ] Add tests proving two users/organizations cannot observe each other's session or tenant context under parallel requests.
- [ ] Review the 133 legacy TransactionScope usages and document transaction boundaries before moving any flow to a new provider/runtime.

Exit gate: no DbContext or user-specific mutable state is shared across requests; parallel isolation tests pass.

## Phase E — ASP.NET Core Web migration

- [ ] Create a separate ASP.NET Core Web host; do not replace the legacy MVC application in place.
- [ ] Agree the authentication/session design with the identity owner before porting controllers.
- [ ] Define the dropdown/select-list DTO contract once and use it consistently across layers.
- [ ] Choose and prove one grid replacement with paging, sorting, filtering, accessibility and export parity before converting the 100+ WebGrid usages.
- [ ] Migrate infrastructure first (DI, auth, session, culture, error handling, static assets, logging), then small modules, then Master Data last.
- [ ] Review every `Html.Raw` conversion and preserve anti-forgery and authorization checks.
- [ ] Keep Crystal Reports and the DailyBalanceReport macro behind explicit interfaces until vendor/licensing and VBA behaviour are understood.
- [ ] Do not route production traffic to the new host until side-by-side parity, load, security and rollback gates are signed off.

Exit gate: module-specific regression suite passes; authorization and financial totals match; operations can roll back without data divergence.

## Phase F — release readiness

- [ ] Build and test on every push and pull request for the modernization branch.
- [ ] Require review from application, database, security and business owners for changes in their areas.
- [ ] Run SAST/dependency/secret scanning and resolve or formally accept findings.
- [ ] Complete performance/load testing using synthetic data and production-like topology.
- [ ] Verify backup/restore, monitoring, alerting, audit logs, incident runbook and support ownership.
- [ ] Deploy to a non-production environment first; run smoke tests and user acceptance.
- [ ] Approve a staged/canary cutover with a defined rollback trigger, owner and time window.
- [ ] Keep `master` untouched until the release is approved through the organization's normal change process.

## Items that require environment or stakeholder access

The repository alone cannot safely complete these tasks:

- SQL Server instance/schema and approved masked backup.
- Secret manager and credential rotation authority.
- Crystal Reports licensing/runtime and report owner.
- DailyBalanceReport VBA source/functional specification.
- Identity provider and password migration policy.
- PCI/security approval and production deployment/change process.

Do not replace these dependencies with guessed values, production exports, or unverified mock assumptions.
