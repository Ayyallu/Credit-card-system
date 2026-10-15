# CC_Ingenio.Data — .NET 8 / EF Core 8 port notes

Fourth Phase 3 artifact, and the first one that changes *behaviour-bearing* code rather than just dropping dead references: the legacy project is an Entity Framework 5 `DbContext` (`UnitOfWork`, 246 `DbSet`s) plus a generic repository. This port targets **EF Core 8** (`Microsoft.EntityFrameworkCore.SqlServer`, `.Proxies`).

**What is and isn't verified.** There is no database in CI or in this environment, so nothing here has run a query. What *is* verified, on every push, by `CC_Ingenio.Data.Tests`: EF Core builds and validates the entire model (~250 entity types), generates DDL, keeps composite primary keys and composite foreign keys in the legacy column order, and names tables the legacy way. Runtime query/save behaviour against a real SQL Server is **not** verified — see "Behaviour differences to check against a real database".

## What was ported

- `UnitOfWork.cs` → EF Core `DbContext`: all `IDbSet<T>` → `DbSet<T>` (mechanical, scripted), EF5-only APIs replaced (below).
- `GenericRepository.cs`, `Contracts/IQueryableUnitOfWork.cs`, `Contracts/ISql.cs` — same shape, `using Microsoft.EntityFrameworkCore;`.
- New `LegacyEf5ModelConventions.cs`, applied from `OnModelCreating`.
- The 11 `HasPrecision(15, 6)` decimal mappings are carried over unchanged.

**Not ported (all confirmed dead in the legacy build):**
- `ContactDbContext.cs`, `ContactUnitofWork.cs`, `IContactDbContext.cs` — never in the legacy `.csproj`; leftover scaffold in a `Contact.*` namespace.
- `EntityMapping/*` (`AddressMapping`, `ContactAddressMapping`, `ContactMapping`, `CurrencyMapping`, `LookUp/CountryMapping`) — EF5 `EntityTypeConfiguration` classes that the legacy `OnModelCreating` never registered (the registrations are commented out), so porting them would have *added* constraints (e.g. `HasMaxLength(15)`) the legacy app never had.
- `Properties/AssemblyInfo.cs`, plus the dead `using GemCryptor;`/`System.Configuration` imports.

## EF5 → EF Core changes

| Legacy (EF5) | Here (EF Core 8) |
|---|---|
| `base(BuildConnectionString())`; `ProxyCreationEnabled`/`LazyLoadingEnabled = true`; `ObjectContext.CommandTimeout = 1500` | `OnConfiguring`: `UseLazyLoadingProxies().UseSqlServer(Common.Utils.BuildConnectionString())`; `Database.SetCommandTimeout(1500)`. A second constructor takes `DbContextOptions<UnitOfWork>` (used by the tests / future DI). |
| `Database.SqlQuery<T>` (any `T`, untracked) | Mapped entity types → `Set<T>().FromSqlRaw(...).AsNoTracking()`; anything else (DTOs, scalars) → `Database.SqlQueryRaw<T>(...)` (EF Core 8 supports arbitrary unmapped types). |
| `Database.SqlQuery(Type, ...)` (non-generic) | No EF Core equivalent; dispatches to the generic overload by reflection. |
| `Database.ExecuteSqlCommand` | `Database.ExecuteSqlRaw` |
| `PluralizingTableNameConvention` removed | `LegacyEf5ModelConventions` sets table = CLR type name unless `[Table]`. In practice no difference today (the two entities whose `DbSet` name differs from their type both have `[Table]`); it's a safeguard. |
| Composite keys as `[Key, Column(Order = n)]` (≈100 entities) | EF Core **refuses** these annotations. `LegacyEf5ModelConventions` re-declares each via `HasKey(...)` in `Column(Order)` order. |
| Property-level composite `[ForeignKey("Nav")]` on several properties | EF Core **throws** at model build, before `OnModelCreating`. Rewritten in the *ported Core only* (legacy untouched) for the five affected entities — `BillingParameter`, `BlockCodeForCommissionExclusion`, `CreditSubSegment`, `MerchantDiscount`, `Product` — to the supported navigation-level `[ForeignKey("A,B")]`, in the original `Column(Order)` order. Tests assert each order and that no property-level composite FK is reintroduced. |

## Behaviour differences to check against a real database

These could not be tested here and are the real risk in this port:

1. **`System.Data.SqlClient` vs `Microsoft.Data.SqlClient`.** EF Core's SQL Server provider uses `Microsoft.Data.SqlClient`. Callers that build `System.Data.SqlClient.SqlParameter` objects and pass them to `ExecuteQuery`/`ExecuteCommand` will fail at runtime. The Repository port must switch to `Microsoft.Data.SqlClient.SqlParameter`. Output parameters need re-checking.
2. **Connection encryption.** `Microsoft.Data.SqlClient` defaults to `Encrypt=true`. `Common.Utils.BuildConnectionString()` doesn't set `Encrypt`/`TrustServerCertificate`, so connecting to an older SQL Server with a self-signed certificate will fail until the connection string is adjusted. (The legacy project's `EDMX` targeted SQL Server 2008 compatibility.)
3. **`DateTime` column type.** EF5 mapped `DateTime` to `datetime`; EF Core maps it to `datetime2`. Reads/writes against existing `datetime` columns work, but precision/rounding and any generated DDL differ. Don't use `GenerateCreateScript()` output to migrate an existing database.
4. **Lazy loading.** `UseLazyLoadingProxies()` requires virtual navigation properties on every entity; the model validates under it (tested), but lazy-load *runtime* behaviour (N+1, disposed-context access) differs from EF5 and is untested.
5. **Change tracking semantics.** `Add`/`Attach`/`Entry(x).State` on object graphs, `ApplyCurrentValues` → `CurrentValues.SetValues`, and `DbUpdateConcurrencyException` handling were ported literally but behave differently around graphs with pre-set keys. `RollbackChanges()` (set every tracked entry to `Unchanged`) is carried over as-is.
6. **Decimal defaults.** Unspecified `decimal` columns default to `decimal(18,2)` in both, but EF Core warns about it; only the 11 explicit `(15,6)` mappings are tuned.
7. **Cascade delete defaults** for required relationships are the same in principle (cascade) but are re-derived by EF Core's conventions, not copied from the EDMX/EF5 model — worth diffing against the real schema's FK actions.

## Next

- `CC_Ingenio.Repository` (EF repositories + the EDMX) is the next project; it will exercise items 1 and 5 above for real.
- A schema-comparison test against a real (masked) database would be the first check that actually closes items 2–7 — see the still-open "masked/synthetic dataset" item in [PHASE0_NOTES.md](../../PHASE0_NOTES.md).
