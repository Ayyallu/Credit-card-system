# Local QA setup agent prompt

Copy the prompt below into Claude Code (or another coding agent) running on your Windows development PC.

---

You are setting up a **local-only SQL Server QA environment** for the existing Ingenio credit-card application.

## Repository

- Repository: https://github.com/Ayyallu/Credit-card-system.git
- Required branch: `modernization/local-sql-qa-bootstrap`
- Local working directory: `C:\\Work\\Credit-card-system` (if this directory already exists, inspect it before cloning; do not overwrite user work)
- Database name: `Ingenio_QA`
- SQL Server: local SQL Server Developer Edition, or an already installed local SQL Server/LocalDB instance
- Authentication: Windows Integrated Authentication only

## Non-negotiable safety rules

1. Never connect to production, a shared QA server, a VPN-hosted SQL Server, or any remote database.
2. Never request, read, print, store, or commit production credentials or real cardholder data.
3. Never copy a production backup to this workstation.
4. Never run destructive SQL such as DROP DATABASE, DROP TABLE, TRUNCATE, or destructive migrations.
5. Never overwrite an existing local database. If `Ingenio_QA` already exists, stop and inspect/report; do not delete or recreate it.
6. Do not change the `master`, `phase-1`, or `modernization/production-safe-next-phase` branches.
7. Do not merge or push code changes. This task is local environment setup and verification only.
8. Do not bypass Windows UAC, security controls, endpoint protection, or corporate software-installation policy.
9. Before installing software, changing Windows services, enabling TCP/IP, changing firewall rules, or adding Windows users to SQL Server administrators, explain the action and ask for explicit user approval.
10. Do not claim production schema parity. The current EF Core model is a migration model, not an authoritative production database schema.

## Work to perform

### A. Inspect the workstation first

Run read-only checks in PowerShell:

- Windows version and architecture.
- Available disk space.
- Installed .NET SDKs (`dotnet --list-sdks`).
- SQL Server services (`Get-Service -Name 'MSSQL*','SQLBrowser','SQLAgent*'`).
- Registered SQL Server instances (registry key `HKLM:\\SOFTWARE\\Microsoft\\Microsoft SQL Server\\Instance Names\\SQL`).
- Whether `sqlcmd` and SSMS are available.
- Whether Git is installed.

Also run the repository script:

```powershell
.\\scripts\\Check-LocalSqlServer.ps1
```

Do not assume that SSMS means the SQL Server database engine is installed.

### B. Clone or switch to the correct branch

If the repository is not present, clone only the required branch:

```powershell
git clone --branch modernization/local-sql-qa-bootstrap --single-branch https://github.com/Ayyallu/Credit-card-system.git C:\\Work\\Credit-card-system
```

If it is already present:
- Inspect `git status` first.
- Preserve all user changes.
- Fetch origin and switch to `modernization/local-sql-qa-bootstrap`.
- Pull only if the working tree is safe and clean.
- Confirm `git branch --show-current` returns the required branch.

Never stash, reset, clean, checkout over, or discard uncommitted user work without asking.

### C. Install only what is missing

Preferred local stack:
- SQL Server Developer Edition database engine.
- SSMS (optional GUI).
- .NET 8 SDK (the current migrated projects target net8.0).
- Git for Windows.

Use official Microsoft download pages only:
- SQL Server: https://www.microsoft.com/sql-server/sql-server-downloads
- SSMS: https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms
- .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
- Git: https://git-scm.com/download/win

If SQL Server is missing, guide the user through the official installer. Ask before starting an installer or making administrative changes. Prefer a local default instance; if a local instance already exists, reuse it rather than installing another. Enable Windows Authentication and add the current Windows account as SQL Server administrator only if necessary and only after approval.

.NET 8 support ends November 10, 2026. This setup is only for validating the existing phase-1 code; do not treat .NET 8 as the production target. The production modernization plan must coordinate .NET 10 and EF Core 10.

### D. Create the local empty schema

Read `LOCAL_QA_SETUP.md` and inspect the initializer project before running it.

Use one of these local connection strings (Integrated Security only):

Default instance:
```
Server=localhost;Database=Ingenio_QA;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;
```

SQL Express:
```
Server=localhost\\SQLEXPRESS;Database=Ingenio_QA;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;
```

LocalDB:
```
Server=(localdb)\\MSSQLLocalDB;Database=Ingenio_QA;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;
```

Run from the repository root:

```powershell
dotnet restore .\\netcore\\CC_Ingenio.LocalQa\\CC_Ingenio.LocalQa.csproj
dotnet run --project .\\netcore\\CC_Ingenio.LocalQa\\CC_Ingenio.LocalQa.csproj -- "Server=localhost;Database=Ingenio_QA;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"
```

Replace only the server value if using another local instance. The initializer must reject remote hosts, non-Ingenio_QA database names, and SQL username/password authentication. It uses EF Core EnsureCreated and must never drop or overwrite anything.

If the database already exists, stop. Do not try to repair it automatically.

### E. Verify and report

Verify:
- SQL Server service is running.
- Local connection succeeds with Windows Authentication.
- Database `Ingenio_QA` exists on the local instance.
- EF Core model creation completed.
- The initializer did not seed any records or modify another database.

Report the instance name, SQL Server version, database name, EF Core entity count, command outputs, and any errors. Redact local usernames, machine-specific paths, tokens and connection details before sharing logs.

## Explicitly out of scope

- Importing any production backup or customer data.
- Creating guessed financial transactions or synthetic seed records.
- Claiming the database matches production.
- Running the legacy web application against this database.
- Modifying legacy authentication, business logic, schema mappings or migrations.
- Pushing commits, opening/merging PRs, or deploying anything.

After the empty local schema is created, stop and provide a report. The next development task will be a separately reviewed synthetic-data seeder based on entity constraints and business invariants, followed by schema comparison against an approved legacy schema export if one becomes available.
