# CC_Ingenio.Repository — .NET 8 port notes

Fifth Phase 3 artifact. This one was the easy half of the data layer: all the EF-specific work lives in [`CC_Ingenio.Data`](../CC_Ingenio.Data/PORT_NOTES.md); this project is **144 thin repositories**, each a one-line subclass `XRepository : GenericRepository<X>, IXRepository` taking a `UnitOfWork`. The 144 files were ported verbatim — no code changes — and reference only `CC_Ingenio.Data`, `CC_Ingenio.Core` and the BCL.

## What was ported / left behind

- Ported: exactly the 144 files in the legacy `.csproj`'s compile list (checked file-by-file, none missing).
- **Not compiled in the legacy project, therefore not ported:** the EDMX model (`Contact.edmx`, `.tt` templates, `Contact.Context.cs`, `Contact.Designer.cs`, `Contact.edmx.sql`), the root-level scaffold classes (`Contact.cs`, `Contact1.cs`, `CONTACTS.cs`, `Addresses.cs`, `ContactAddresses.cs`), the root `GenericRepository.cs` (a `Contact.Repository` namespace leftover that clashes in name with the real one), and the `Repositories/` folder. This also means **EF5's EDMX/Database-First tooling is gone from the migration entirely** — it was never part of what actually ran. (`App.Config`'s `(LocalDb)\v11.0` placeholder connection string went with it.)
- `Properties/AssemblyInfo.cs` (SDK projects generate it).

## What is verified (`CC_Ingenio.Repository.Tests`, no database)

- Exactly 144 repository types are discovered (guards against a file silently dropping out of the port).
- Each of the 143 repositories over a mapped entity constructs over an unopened `UnitOfWork` and its `GetAll()` query **translates to SQL** via EF Core's `ToQueryString()` — so every one of those entities is queryable through EF Core with the lazy-loading proxy configuration on.
- The set of repositories over entity types that are *not* in the EF model is pinned to an explicit list, so a new gap fails the build.

## The one repository over an unmapped type

`CustomerServiceDetailsRepository` wraps `CustomerServiceDetails`, which has no `DbSet` on `UnitOfWork` and isn't reachable through any navigation: it's a flat read model (service id, customer, card and address columns), not a table. In legacy EF5 `Set<T>()` on such a type throws as well, so set operations on this repository were never usable; its data comes through `ExecuteQuery<CustomerServiceDetails>(sql, …)`. `UnitOfWork.ExecuteQuery<T>` handles this case — unmapped types go through EF Core 8's `Database.SqlQueryRaw<T>`. Nothing to change, but whoever ports the Manager layer should call `ExecuteQuery` for it and not `GetAll()`.

## Not verified

Same limit as the Data port: nothing here has talked to a SQL Server. See the "Behaviour differences to check against a real database" list in the Data notes — in particular `System.Data.SqlClient.SqlParameter` vs `Microsoft.Data.SqlClient.SqlParameter` becomes a real, immediate problem the moment the **Manager** layer is ported: it is the only project that builds raw-SQL parameters (**876 `SqlParameter` references across 8 files** — `MasterManager` 282, `AuthorizationManager` 173, `CustomerServiceManager` 123, `BillingManager` 119, `UserManager` 107, `CardBookingManager` 30, `ApprovalQueueManager` 22, `CommonManager` 20 — feeding ~220 `ExecuteQuery`/`ExecuteCommand` calls). Nothing in Web, Common, Core or Data touches `SqlParameter`. The namespace swap itself is mechanical, but output parameters and `DBNull`/type inference need runtime checking against a real database.
