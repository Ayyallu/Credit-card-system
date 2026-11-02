# Synthetic Local QA Reference Data

This phase adds a deliberately small, deterministic fixture set to the local Ingenio_QA database.

## Scope

The seeder creates or verifies exactly:

- One organization: DEMO_ORG_001
- Three transaction codes scoped to that organization:
  - DEMO_PURCHASE
  - DEMO_CASH
  - DEMO_INSTALL

Every row is clearly marked with SYNTHETIC_SEEDER in its audit fields and uses a DEMO_ key prefix. Descriptions explicitly identify the organization and transaction records as synthetic.

This is reference/master data only. It does not create customers, card numbers, accounts, balances, statements, authorizations, payment records, addresses, contact details, or real-person information. Those entities have additional lookup and business-rule dependencies that must be mapped before a safe fixture can be designed.

## Safety guards

The console app enforces all of the following for every command:

- Database name must be exactly Ingenio_QA.
- SQL Server must be local (including (localdb)\MSSQLLocalDB).
- Windows Integrated Authentication is mandatory; SQL username/password credentials are rejected.
- The seed command never calls EnsureCreated, drops objects, truncates tables, or deletes records.
- Seed inserts are performed in one database transaction.
- Existing matching fixture rows are treated as already seeded.
- If a fixture key exists but its values do not match the expected synthetic values, the seeder stops rather than updating or replacing it.
- No external payment, email, SMS, banking, or network integration is invoked.

Do not use this utility against production, shared QA, or any remote SQL Server.

## Commands

From the repository root in PowerShell, after the empty schema has been initialized:

    $connection = "Server=(localdb)\MSSQLLocalDB;Database=Ingenio_QA;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"

    dotnet run --project .\netcore\CC_Ingenio.LocalQa\CC_Ingenio.LocalQa.csproj -- seed-synthetic $connection

    dotnet run --project .\netcore\CC_Ingenio.LocalQa\CC_Ingenio.LocalQa.csproj -- validate-synthetic $connection

The original one-argument initializer remains supported:

    dotnet run --project .\netcore\CC_Ingenio.LocalQa\CC_Ingenio.LocalQa.csproj -- $connection

The initializer only creates a missing database; it does not modify an existing database.

## Idempotency and conflict behavior

Running seed-synthetic repeatedly should leave the same four fixture rows in place. It does not create duplicates and does not update existing records. If a key collision is found with different values, the command fails with a conflict message for manual inspection.

validate-synthetic is read-only. It verifies that the expected organization and transaction rows exist and retain the expected synthetic marker and core values.

## Reset

There is intentionally no automated reset command. Do not add broad DELETE or TRUNCATE logic to the seeder.

If a clean database is required, first inspect the exact local (localdb)\MSSQLLocalDB instance and confirm that Ingenio_QA contains only disposable synthetic data. Any database drop/recreation must be a separate, explicitly approved manual operation.

## Verification limits

The seeder is not evidence of production schema parity or end-to-end workflow readiness. The next data phase must map lookup dependencies and business invariants before introducing synthetic customer/card/account records. An authoritative legacy schema export is still required to compare tables, constraints, defaults, indexes, stored procedures, and triggers.
