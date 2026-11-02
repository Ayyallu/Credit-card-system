using Microsoft.EntityFrameworkCore;
using CC_Ingenio.Data;
using CustomerEntity = CC_Ingenio.Core.MasterRepo.MasterAggregate.Customer;
using CardEntity = CC_Ingenio.Core.CardBookingRepo.CardBookingAggregate.CardMaster;

namespace CC_Ingenio.LocalQa;

/// <summary>
/// Tier 1 synthetic end-to-end fixture: one fictional Customer and one fictional
/// Card, reusing the existing synthetic organization (DEMO_ORG_001). It creates
/// no product, segment, account, balance, payment, or real-person data. All
/// optional master foreign-key code columns on the card are left NULL.
///
/// Identifiers are deliberately non-numeric and clearly synthetic so they cannot
/// resemble a real customer identifier or a real card number / PAN.
/// </summary>
internal static class SyntheticCardSeeder
{
    // Reuses the organization created by SyntheticDemoSeeder.
    private const string OrganizationCode = "DEMO_ORG_001";
    private const string Actor = "SYNTHETIC_SEEDER";

    // Natural keys used for idempotent lookup. ApplicationReferenceNo is an
    // otherwise-unused nullable string that gives the identity-keyed Customer a
    // stable synthetic handle. The card number is alphabetic + hyphenated on
    // purpose: it is not a valid PAN and cannot be mistaken for a real card.
    private const string CustomerReference = "SYNTH-CUST-0001";
    private const string CardNumber = "SYNTH-DEMO-CARD-0001";

    private const string CustomerName = "SYNTHETIC QA CUSTOMER";
    private const string CardHolderName = "SYNTHETIC QA CARDHOLDER";

    private static readonly DateTime SeedTimestamp = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
    private static readonly DateTime CardValidFrom = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

    public static async Task SeedAsync(UnitOfWork db, CancellationToken cancellationToken = default)
    {
        if (!await db.Database.CanConnectAsync(cancellationToken))
            throw new InvalidOperationException("Cannot connect to the local Ingenio_QA database.");

        // Tier 1 reuses, but never creates or modifies, the base organization.
        var orgExists = await db.Organization.AsNoTracking()
            .AnyAsync(x => x.OrganizationCode == OrganizationCode, cancellationToken);
        if (!orgExists)
        {
            throw new InvalidOperationException(
                $"Synthetic organization {OrganizationCode} is missing. Run 'seed-synthetic' first; " +
                "this command reuses the existing organization fixture and does not create it.");
        }

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        var customer = await db.Customer
            .SingleOrDefaultAsync(x => x.ApplicationReferenceNo == CustomerReference, cancellationToken);

        if (customer is null)
        {
            customer = CreateCustomer();
            db.Customer.Add(customer);
            // Flush so the identity-generated CustomerId is available for the card.
            await db.SaveChangesAsync(cancellationToken);
        }
        else
        {
            EnsureCustomerMatches(customer);
        }

        var card = await db.CardMaster
            .SingleOrDefaultAsync(x => x.CardNumber == CardNumber, cancellationToken);

        if (card is null)
        {
            db.CardMaster.Add(CreateCard(customer.CustomerId));
            await db.SaveChangesAsync(cancellationToken);
        }
        else
        {
            EnsureCardMatches(card, customer.CustomerId);
        }

        await transaction.CommitAsync(cancellationToken);
    }

    public static async Task ValidateAsync(UnitOfWork db, CancellationToken cancellationToken = default)
    {
        var customer = await db.Customer.AsNoTracking()
            .SingleOrDefaultAsync(x => x.ApplicationReferenceNo == CustomerReference, cancellationToken);
        if (customer is null)
            throw new InvalidOperationException($"Missing synthetic customer {CustomerReference}.");

        EnsureCustomerMatches(customer);

        var card = await db.CardMaster.AsNoTracking()
            .SingleOrDefaultAsync(x => x.CardNumber == CardNumber, cancellationToken);
        if (card is null)
            throw new InvalidOperationException($"Missing synthetic card {CardNumber}.");

        EnsureCardMatches(card, customer.CustomerId);
    }

    private static CustomerEntity CreateCustomer() => new()
    {
        ApplicationReferenceNo = CustomerReference,
        OrganizationCode = OrganizationCode,
        Name = CustomerName,
        PaternalSurname = "SYNTHETIC",
        MaternalSurname = "QA",
        CustomerType = "DEMO",
        CreditScore = 0m,
        IsActive = true,
        IsBlackListed = false,
        IsCardGenerated = true,
        ApprovalStatus = "A",
        CreatedBy = Actor,
        CreatedOn = SeedTimestamp,
        ApprovedBy = Actor,
        ApprovedOn = SeedTimestamp,
        LastUpdatedBy = Actor,
        LastUpdatedOn = SeedTimestamp
        // EmailID, LegacyCardNumber, DateOfBirth, RUT and all other PII-shaped
        // fields are deliberately left NULL.
    };

    private static CardEntity CreateCard(int customerId) => new()
    {
        CardNumber = CardNumber,
        OrganizationCode = OrganizationCode,
        CustomerId = customerId,
        CustomerCardName = CardHolderName,
        CardStatusCode = "DEMO",
        ValidFromDate = CardValidFrom,
        ApprovalStatus = "A",
        CreatedBy = Actor,
        CreatedOn = SeedTimestamp,
        ApprovedBy = Actor,
        ApprovedOn = SeedTimestamp,
        LastUpdatedBy = Actor,
        LastUpdatedOn = SeedTimestamp
        // ProductCode, CustomerSegmentCode, CustomerSubSegmentCode and the
        // objProduct shadow columns are left NULL: all are optional, enforced
        // FKs that are only checked when populated. No limits/balances are set
        // (all default to 0).
    };

    private static void EnsureCustomerMatches(CustomerEntity actual)
    {
        if (actual.Name != CustomerName ||
            actual.OrganizationCode != OrganizationCode ||
            actual.CreatedBy != Actor ||
            actual.ApprovalStatus != "A")
        {
            throw new InvalidOperationException(
                $"Refusing to overwrite customer {CustomerReference}: an existing row does not match the synthetic fixture.");
        }
    }

    private static void EnsureCardMatches(CardEntity actual, int expectedCustomerId)
    {
        if (actual.OrganizationCode != OrganizationCode ||
            actual.CustomerId != expectedCustomerId ||
            actual.CustomerCardName != CardHolderName ||
            actual.CreatedBy != Actor ||
            actual.ApprovalStatus != "A")
        {
            throw new InvalidOperationException(
                $"Refusing to overwrite card {CardNumber}: an existing row does not match the synthetic fixture.");
        }
    }
}
