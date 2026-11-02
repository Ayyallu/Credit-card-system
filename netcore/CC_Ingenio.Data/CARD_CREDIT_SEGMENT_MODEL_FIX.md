# Card credit segment SQL Server model correction

## Trigger

The guarded LocalDB initialization of the EF Core 8 model reported:

- SQL Server error 1785 for `FK_MT_CardCreditSegment_MT_Organization_OrganizationCode`: the direct Organization relationship creates a second cascade path because `CardCreditSegment` also reaches `Organization` through `CreditSegment`.
- A 2,700-byte clustered primary key on `MT_CardCreditSegment`.

## Changes in this branch

1. Keep the direct `CardCreditSegment.OrganizationCode` foreign key, but configure its delete action as `NoAction`. This preserves referential integrity and prevents deleting an Organization through this second cascade path. It does not remove the FK or make the relationship optional.
2. Explicitly cap the string key columns in this affected relationship graph at 128 characters. The migrated EF Core model was assigning 450 characters to each string key by convention (3 × nvarchar(450) = 2,700 bytes). The legacy EF5 model's conventional string-key length is 128 characters; the three-column key is therefore 3 × nvarchar(128) = 768 bytes.
3. Add offline model tests for the FK delete behaviour, primary-key order/size and relevant dependent key columns.

## Important validation boundary

This repository does not contain an authoritative Ingenio production schema export for these tables. The 128-character setting restores the legacy EF5 string-key convention; it is not a substitute for comparing the resulting column definitions with a masked schema export before any production migration or cutover.

The tests build EF Core metadata and generate SQL without connecting to a database. A successful LocalDB `EnsureCreated` run is still required to verify SQL Server accepts the complete model. The existing local `Ingenio_QA` database is intentionally not dropped or modified by this branch.
