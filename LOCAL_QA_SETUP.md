# Local Ingenio QA Database Setup (Windows)

This guide creates a **local-only, empty QA database** from the migrated EF Core model. It does not require production access, a production backup, or real cardholder data.

## Safety boundary

- Use only a local SQL Server Developer instance on your own workstation.
- The database name must be exactly `Ingenio_QA`.
- The initializer rejects remote SQL Server hosts and SQL username/password authentication.
- It uses Windows Integrated Authentication.
- It never drops or overwrites an existing database.
- It does not import production data or create synthetic customer records.
- The generated schema represents the current EF Core model, **not a verified copy of the legacy production schema**. Stored procedures, triggers, SQL Agent jobs, Crystal Reports dependencies and all legacy database objects are not recreated by this first bootstrap.

Do not point this tool at production, shared QA, or any remote database.

## 1. Install prerequisites

Recommended local stack:

1. **SQL Server Developer Edition** (database engine, local instance). Choose the Database Engine Services feature.
2. **SQL Server Management Studio (SSMS)** (optional GUI for inspecting the database).
3. **.NET 8 SDK** (the current migrated data projects target .NET 8).
4. **Git for Windows**.

Use Microsoft's official download pages:
- SQL Server: https://www.microsoft.com/sql-server/sql-server-downloads
- SSMS: https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms
- .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
- Git: https://git-scm.com/download/win

The repository's migrated projects currently target .NET 8. .NET 8 support ends on November 10, 2026; this is a local validation baseline, not the intended production runtime target. The modernization plan targets a coordinated .NET 10 / EF Core 10 upgrade before production cutover.

During SQL Server setup, use the default instance if you do not already have one, enable Windows authentication, and add your Windows account as a SQL Server administrator. If an existing local SQL Server instance is already installed, do not install a second one.

## 2. Clone the repository branch

Open PowerShell:

```powershell
git clone --branch modernization/local-sql-qa-bootstrap --single-branch https://github.com/Ayyallu/Credit-card-system.git
cd Credit-card-system
```

If the repository is already cloned, use:

```powershell
git fetch origin
git switch modernization/local-sql-qa-bootstrap
git pull
```

## 3. Check local prerequisites

Run:

```powershell
.\scripts\Check-LocalSqlServer.ps1
```

The script reports detected SQL Server services, command-line tools and .NET SDK versions. It does not install software or change machine settings.

## 4. Select your local SQL Server connection

Common local instance names:

| SQL Server installation | Server value |
|---|---|
| Default instance | `localhost` |
| SQL Express named instance | `localhost\SQLEXPRESS` |
| LocalDB | `(localdb)\MSSQLLocalDB` |

The initializer requires Windows Integrated Authentication. Example:

```powershell
dotnet run --project .\netcore\CC_Ingenio.LocalQa\CC_Ingenio.LocalQa.csproj -- "Server=localhost;Database=Ingenio_QA;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"
```

For SQL Express, replace `Server=localhost` with `Server=localhost\SQLEXPRESS`.

For LocalDB, use `Server=(localdb)\MSSQLLocalDB`.

The program validates that the server is local and the database name is exactly `Ingenio_QA`. It refuses SQL username/password credentials.

## 5. Verify in SSMS

Connect to the same local instance using Windows Authentication. Expand Databases and confirm `Ingenio_QA` exists. The initializer uses EF Core `EnsureCreated` against the current migrated `UnitOfWork` model.

If the database already exists, the initializer will not alter it. Do not manually delete an existing database to make the command succeed; inspect it first.

## 6. What this first local database is useful for

- Creating the current EF Core model schema locally.
- Checking model creation and basic connectivity.
- Building further integration tests against a real SQL Server engine.
- Developing deterministic synthetic seed data in a later, reviewed change.

It is **not yet suitable for claiming production schema parity or end-to-end business workflow validation**. Before those claims, we need an authoritative legacy schema export (schema-only is sufficient for much of the work) and must compare all tables, columns, keys, indexes, defaults, constraints, views, stored procedures and triggers.

## Troubleshooting

- **Cannot connect:** confirm the SQL Server service is running and the server/instance name is correct.
- **Login failed:** use Windows Authentication and confirm your Windows account was added as a local SQL Server administrator during setup.
- **Cannot find .NET SDK:** install the .NET 8 SDK and open a new PowerShell window.
- **Database already exists:** stop and inspect it in SSMS. The initializer intentionally does not overwrite it.
- **EF model creation fails:** capture the exception text (remove hostnames or any sensitive details) and investigate the model issue before retrying.

## Next planned phase

After the empty schema is created and verified, add a reviewed synthetic-data seeder based on required entity relationships and business invariants. Do not fabricate production financial baselines. Synthetic records must be clearly labelled and must never be sent to real payment, email, SMS or external banking integrations.
