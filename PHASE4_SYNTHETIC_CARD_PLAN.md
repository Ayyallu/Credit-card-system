# Phase 4 plan — synthetic customer → card end-to-end workflow

Status: **design only**. No seeder code is included in this document. This plan
extends the existing guarded local-QA seeder (`netcore/CC_Ingenio.LocalQa`,
`SyntheticDemoSeeder.cs`) added in the synthetic-local-qa-data work. It targets
the local `Ingenio_QA` database only.

## Safety boundary (unchanged from prior phases)

- Local SQL Server / LocalDB only, Windows Integrated Authentication, database
  name exactly `Ingenio_QA`. Remote/production/shared QA connections are rejected.
- Synthetic, clearly-labelled, deterministic records only. No real customers,
  cards, PAN/cardholder data, balances, payments, or PII.
- Idempotent: insert-if-missing, validate, and refuse to overwrite a row that
  does not match the synthetic fixture. All rows stamped `SYNTHETIC_SEEDER`.
- No pushes/merges without review. No destructive SQL. Does not claim production
  schema parity.

## Key framing finding

Both `CC_Ingenio.Core` and `CC_Ingenio.Data` compile with
`<Nullable>disable</Nullable>`. As a result **every `string` column is nullable**,
so every string-based foreign key is an **optional** relationship that the
database does **not** enforce, and there are **no `[Required]` attributes** in the
Customer/Card/Master graph. The schema therefore does not force an
Organization → Product → Customer → Card chain.

The only things that actually block an `INSERT` are:

1. the primary key,
2. non-nullable value-type columns (`int` / `decimal` / `bool` / `DateTime`;
   dates must be real values, not `0001-01-01`), and
3. non-nullable value-type FK columns (only these create a hard parent-row
   requirement).

The ordered "chain" below is an application/data-integrity ordering, not a set of
DB FK constraints — except where a non-nullable value-type FK is noted.

## Central entities

| Concept | File | Table | Primary key |
|---|---|---|---|
| Customer | `netcore/CC_Ingenio.Core/MasterRepo/MasterAggregate/Customer.cs` | `MT_Customer` | `CustomerId` (int, Identity) |
| Card | `netcore/CC_Ingenio.Core/CardBookingRepo/CardBookingAggregate/CardMaster.cs` | `MT_CardMaster` | `CardNumber` (string, manual) |
| Account | `netcore/CC_Ingenio.Core/MasterRepo/MasterAggregate/CustomerCurrentAccountDetail.cs` | `MT_CustomerCurrentAccountDetail` | `AccountId` (int, Identity) |

There is **no dedicated card-account table**. `CardMaster.AccountNo` is a bare
string with no foreign key. `CustomerCurrentAccountDetail` is the closest real
account entity (a child collection of `Customer`); its `CustomerId` (int) binds to
`MT_Customer` by convention, so it is the one place in this subgraph that requires
a parent Customer row to exist first.

## Per-entity INSERT-blocking fields

### `MT_Customer`
- PK `CustomerId` is Identity — do not set.
- All FK columns are optional (nullable strings): `OrganizationCode`,
  `ProductCode`, `Nationality` (→ `Country`), `BranchCode`,
  `ProductMemberTypeCode`, `ProductTypeCode`, `ProductCategoryCode`.
  `ParentCustomerId` is a nullable self-FK — leave NULL.
- Must supply values: `CreditScore` (decimal), the bool flags
  (`IsActive`, `IsBlackListed`, `IsCardGenerated`), `CreatedOn`,
  `LastUpdatedOn` (real dates).
- **A Customer row can be inserted with no parent rows.**

### `MT_CardMaster`
- PK `CardNumber` (string) — set manually.
- Optional (nullable-string FK) relationships: `objOrganization`
  (`OrganizationCode` → `Organization`), `objProduct` (convention against
  Product composite PK), `objCustomerSegment`
  (`[ForeignKey("CustomerSegmentCode,OrganizationCode")]`),
  `objCustomerSubSegment` (`[ForeignKey("CustomerSubSegmentCode,OrganizationCode")]`).
- `CustomerId` (int) is **not** a configured FK (no nav, no `[ForeignKey]`); it
  is non-nullable so it needs a value, but the DB does not enforce the link.
- Must supply values: `ValidFromDate`, `CreatedOn`, `LastUpdatedOn` (real dates);
  decimals/bools (limits, OTB fields, flags) default acceptably.
- **A CardMaster row can be inserted with no parent rows** if the optional code
  columns are left NULL.

### `MT_CustomerCurrentAccountDetail`
- PK `AccountId` is Identity.
- `CustomerId` (int) + nav `Customer` → convention-bound FK to `MT_Customer`.
  **Requires a Customer row first.** Other columns are optional strings.

## Insertion order

### (A) DB-enforced minimum
1. `MT_Customer` — dates, bools, `CreditScore`. No parents.
2. `MT_CardMaster` — `CardNumber`, `CustomerId` (from step 1), `ValidFromDate`,
   `CreatedOn`, `LastUpdatedOn`; code FK columns NULL.

### (B) Logically-complete chain (realistic)
```
Organization -> SystemParameter -> Product -> CustomerSegment -> CustomerSubSegment -> Customer -> CardMaster  (-> CustomerCurrentAccountDetail)
```
- `MT_Product` carries the one hard value-type FK: `BillCycleSysParamId`
  (non-nullable `int`, `[ForeignKey("BillingCycle")]` → `MT_SystemParameter`), so
  **a Product cannot be inserted without a SystemParameter row**.
- `MT_Product` composite PK `(OrganizationCode, ProductCode)`; `CustomerSegment`
  `(SegmentCode, OrganizationCode)`; `CustomerSubSegment`
  `(SubSegmentCode, OrganizationCode)`.
- The existing seed (1 Organization + 3 transaction codes) already covers the
  Organization root. Transaction codes are not referenced by any FK in this graph.

## Design flags

- `CardMaster.OrganizationCode` participates in four relationships at once
  (Organization, Product, CustomerSegment, CustomerSubSegment). If set, every
  populated code must resolve under the **same** OrganizationCode.
- `CardMaster.objProduct` has no `[ForeignKey]` and relies on EF Core convention
  against Product's composite PK; EF may synthesize shadow FK columns. **Verify
  against the generated model before populating it.**
- Segment / SubSegment carry an `IsDefault` bool — a minimal seed should set
  exactly one `IsDefault = true` per Organization to avoid "default segment"
  ambiguity in later queries.
- `CardMaster.CustomerId` is a logical-but-unenforced link — a synthetic card
  must copy the real generated `Customer.CustomerId`.
- Only `CardCreditSegment.objOrganization` is pinned to `DeleteBehavior.NoAction`
  (the Phase-3 fix); no cascade overrides apply to Customer/CardMaster insert order.

## Phased delivery

### Tier 1 — minimal (first reviewed PR)
New command `seed-synthetic-card` that reuses the existing `DEMO_ORG_001`:
- Insert 1 `Customer` (synthetic flags/name, real `CreatedOn`/`LastUpdatedOn`,
  `OrganizationCode = DEMO_ORG_001`, all other FK codes NULL).
- Insert 1 `CardMaster` (`CardNumber = "DEMO0000000000001"`, `CustomerId` =
  generated id, `OrganizationCode = DEMO_ORG_001`, segment/product codes NULL,
  `ValidFromDate`/dates set).
- A matching `validate-synthetic-card` check and an idempotency re-run.
- No new master tables; smallest reviewable surface.

### Tier 2 — realistic (follow-up PR)
Add, before the card: `SystemParameter` → `Product` (`DEMO_PROD_001`) →
`CustomerSegment` / `CustomerSubSegment` (one `IsDefault = true` each), then
populate the card's `ProductCode` / segment codes under `DEMO_ORG_001`. Verify the
`objProduct` composite-FK binding against the generated model first.

## Validation plan (both tiers)

- Build, then `seed-synthetic-card`, then `validate-synthetic-card`, then
  `seed-synthetic-card` again (idempotency) — mirroring the existing
  seed → validate → re-seed gate.
- Row-count assertion: exactly the expected synthetic rows, no duplicates,
  all stamped `SYNTHETIC_SEEDER`.

## Out of scope

- Any production backup, real customer data, or synthetic financial baselines.
- Running the legacy web app against this database.
- Claiming production schema parity.
- Pushing/merging — this document is for review first.
