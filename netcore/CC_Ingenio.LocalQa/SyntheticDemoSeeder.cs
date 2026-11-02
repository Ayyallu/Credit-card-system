using Microsoft.EntityFrameworkCore;
using CC_Ingenio.Data;
using OrganizationEntity = CC_Ingenio.Core.MasterRepo.MasterAggregate.Organization;
using TransactionEntity = CC_Ingenio.Core.MasterRepo.MasterAggregate.Transaction;

namespace CC_Ingenio.LocalQa;

/// <summary>
/// Adds a small, deterministic set of fictional reference records.
/// It deliberately does not create customers, cards, balances, or payment activity.
/// </summary>
internal static class SyntheticDemoSeeder
{
    private const string OrganizationCode = "DEMO_ORG_001";
    private const string Actor = "SYNTHETIC_SEEDER";
    private static readonly DateTime SeedTimestamp = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

    private static readonly TransactionEntity[] Transactions =
    [
        CreateTransaction("DEMO_PURCHASE", "Synthetic demo purchase", installmentRequired: false),
        CreateTransaction("DEMO_CASH", "Synthetic demo cash advance", installmentRequired: false),
        CreateTransaction("DEMO_INSTALL", "Synthetic demo installment purchase", installmentRequired: true)
    ];

    public static async Task SeedAsync(UnitOfWork db, CancellationToken cancellationToken = default)
    {
        if (!await db.Database.CanConnectAsync(cancellationToken))
            throw new InvalidOperationException("Cannot connect to the local Ingenio_QA database.");

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        var organization = await db.Organization
            .SingleOrDefaultAsync(x => x.OrganizationCode == OrganizationCode, cancellationToken);

        if (organization is null)
        {
            db.Organization.Add(CreateOrganization());
        }
        else
        {
            EnsureOrganizationMatches(organization);
        }

        foreach (var expected in Transactions)
        {
            var existing = await db.Transaction.SingleOrDefaultAsync(
                x => x.OrganizationCode == OrganizationCode &&
                     x.TransactionCode == expected.TransactionCode,
                cancellationToken);

            if (existing is null)
            {
                db.Transaction.Add(CloneTransaction(expected));
            }
            else
            {
                EnsureTransactionMatches(existing, expected);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public static async Task ValidateAsync(UnitOfWork db, CancellationToken cancellationToken = default)
    {
        var organization = await db.Organization.AsNoTracking()
            .SingleOrDefaultAsync(x => x.OrganizationCode == OrganizationCode, cancellationToken);

        if (organization is null)
            throw new InvalidOperationException($"Missing synthetic organization {OrganizationCode}.");

        EnsureOrganizationMatches(organization);

        foreach (var expected in Transactions)
        {
            var actual = await db.Transaction.AsNoTracking().SingleOrDefaultAsync(
                x => x.OrganizationCode == OrganizationCode &&
                     x.TransactionCode == expected.TransactionCode,
                cancellationToken);

            if (actual is null)
                throw new InvalidOperationException($"Missing synthetic transaction {expected.TransactionCode}.");

            EnsureTransactionMatches(actual, expected);
        }
    }

    private static OrganizationEntity CreateOrganization() => new()
    {
        OrganizationCode = OrganizationCode,
        OrganizationDescription = "Synthetic QA organization - not a real financial institution",
        OrganizationShortName = "DEMO QA",
        OrganizationTypeCode = "DEMO",
        OrganizationLogo = string.Empty,
        EffectiveFromDate = new DateTime(2020, 1, 1),
        EffectiveToDate = new DateTime(2099, 12, 31),
        CreatedBy = Actor,
        CreatedOn = SeedTimestamp,
        ApprovedBy = Actor,
        ApprovedOn = SeedTimestamp,
        ApprovalStatus = "A",
        LastUpdatedBy = Actor,
        LastUpdatedOn = SeedTimestamp
    };

    private static TransactionEntity CreateTransaction(
        string code,
        string description,
        bool installmentRequired) => new()
    {
        OrganizationCode = OrganizationCode,
        TransactionCode = code,
        Description = description,
        InstallmentRequired = installmentRequired,
        InterestWith = false,
        DeferredPayment = false,
        FeesRequired = false,
        TaxesRequired = false,
        InstallmentRelated = installmentRequired,
        CapitalizeInterest = false,
        ApprovalStatus = "A",
        CreatedBy = Actor,
        CreatedOn = SeedTimestamp,
        ApprovedBy = Actor,
        ApprovedOn = SeedTimestamp,
        LastUpdatedBy = Actor,
        LastUpdatedOn = SeedTimestamp
    };

    private static TransactionEntity CloneTransaction(TransactionEntity source) => new()
    {
        OrganizationCode = source.OrganizationCode,
        TransactionCode = source.TransactionCode,
        Description = source.Description,
        CreditSegment = source.CreditSegment,
        InstallmentRequired = source.InstallmentRequired,
        InterestWith = source.InterestWith,
        DeferredPayment = source.DeferredPayment,
        FeesRequired = source.FeesRequired,
        TaxesRequired = source.TaxesRequired,
        InstallmentRelated = source.InstallmentRelated,
        CapitalizeInterest = source.CapitalizeInterest,
        ApprovalStatus = source.ApprovalStatus,
        CreatedBy = source.CreatedBy,
        CreatedOn = source.CreatedOn,
        ApprovedBy = source.ApprovedBy,
        ApprovedOn = source.ApprovedOn,
        LastUpdatedBy = source.LastUpdatedBy,
        LastUpdatedOn = source.LastUpdatedOn
    };

    private static void EnsureOrganizationMatches(OrganizationEntity actual)
    {
        var expected = CreateOrganization();
        if (actual.OrganizationDescription != expected.OrganizationDescription ||
            actual.OrganizationShortName != expected.OrganizationShortName ||
            actual.OrganizationTypeCode != expected.OrganizationTypeCode ||
            actual.CreatedBy != Actor ||
            actual.ApprovalStatus != expected.ApprovalStatus)
        {
            throw new InvalidOperationException(
                $"Refusing to overwrite {OrganizationCode}: an existing row does not match the synthetic fixture.");
        }
    }

    private static void EnsureTransactionMatches(TransactionEntity actual, TransactionEntity expected)
    {
        if (actual.Description != expected.Description ||
            actual.InstallmentRequired != expected.InstallmentRequired ||
            actual.InstallmentRelated != expected.InstallmentRelated ||
            actual.CreatedBy != Actor ||
            actual.ApprovalStatus != expected.ApprovalStatus)
        {
            throw new InvalidOperationException(
                $"Refusing to overwrite transaction {expected.TransactionCode}: an existing row does not match the synthetic fixture.");
        }
    }
}
