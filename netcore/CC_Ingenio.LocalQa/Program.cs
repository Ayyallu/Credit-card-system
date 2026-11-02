using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CC_Ingenio.Data;
using CC_Ingenio.LocalQa;

var command = args.Length == 1 ? "init" : args.Length == 2 ? args[0] : string.Empty;
var connectionString = args.Length == 1 ? args[0] : args.Length == 2 ? args[1] : string.Empty;

if (string.IsNullOrWhiteSpace(command) || string.IsNullOrWhiteSpace(connectionString) ||
    command is not ("init" or "seed-synthetic" or "validate-synthetic" or "seed-synthetic-card" or "validate-synthetic-card"))
{
    Console.Error.WriteLine("Usage:");
    Console.Error.WriteLine("  dotnet run --project netcore/CC_Ingenio.LocalQa -- \"<local integrated-security connection string>\"");
    Console.Error.WriteLine("  dotnet run --project netcore/CC_Ingenio.LocalQa -- seed-synthetic \"<local integrated-security connection string>\"");
    Console.Error.WriteLine("  dotnet run --project netcore/CC_Ingenio.LocalQa -- validate-synthetic \"<local integrated-security connection string>\"");
    Console.Error.WriteLine("  dotnet run --project netcore/CC_Ingenio.LocalQa -- seed-synthetic-card \"<local integrated-security connection string>\"");
    Console.Error.WriteLine("  dotnet run --project netcore/CC_Ingenio.LocalQa -- validate-synthetic-card \"<local integrated-security connection string>\"");
    return 2;
}

SqlConnectionStringBuilder connection;
try
{
    connection = new SqlConnectionStringBuilder(connectionString);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Invalid SQL Server connection string: {ex.Message}");
    return 2;
}

if (!string.Equals(connection.InitialCatalog, "Ingenio_QA", StringComparison.OrdinalIgnoreCase))
{
    Console.Error.WriteLine("Refusing to continue: database name must be exactly Ingenio_QA.");
    return 3;
}

if (!connection.IntegratedSecurity || !string.IsNullOrWhiteSpace(connection.UserID) ||
    !string.IsNullOrWhiteSpace(connection.Password))
{
    Console.Error.WriteLine("Refusing to continue: Windows Integrated Authentication is required. SQL credentials are not accepted.");
    return 3;
}

if (!IsLocalServer(connection.DataSource))
{
    Console.Error.WriteLine($"Refusing to connect to non-local SQL Server host '{connection.DataSource}'.");
    return 3;
}

Console.WriteLine($"Target: local SQL Server '{connection.DataSource}', database '{connection.InitialCatalog}'.");
Console.WriteLine($"Operation: {command}. Production, shared QA, and remote SQL Server connections are rejected.");

var options = new DbContextOptionsBuilder<UnitOfWork>()
    .UseLazyLoadingProxies()
    .UseSqlServer(connection.ConnectionString)
    .Options;

try
{
    await using var db = new UnitOfWork(options);

    switch (command)
    {
        case "init":
        {
            Console.WriteLine("Creating the current EF Core model schema only. No data is seeded, dropped, or imported.");
            var created = await db.Database.EnsureCreatedAsync();

            Console.WriteLine(created
                ? "SUCCESS: Ingenio_QA was created from the current EF Core model."
                : "No changes made: the database already exists. EnsureCreated does not update an existing database.");

            Console.WriteLine($"EF Core model entity types: {db.Model.GetEntityTypes().Count()}");
            break;
        }
        case "seed-synthetic":
            if (!await db.Database.CanConnectAsync())
                throw new InvalidOperationException("Ingenio_QA is not reachable. Run the schema initializer first.");

            await SyntheticDemoSeeder.SeedAsync(db);
            await SyntheticDemoSeeder.ValidateAsync(db);
            Console.WriteLine("SUCCESS: synthetic reference fixtures are present and validated.");
            Console.WriteLine("Created/verified: 1 synthetic organization and 3 synthetic transaction codes.");
            Console.WriteLine("No customer, card, account, balance, payment, or real-person records were created.");
            break;

        case "validate-synthetic":
            if (!await db.Database.CanConnectAsync())
                throw new InvalidOperationException("Ingenio_QA is not reachable.");

            await SyntheticDemoSeeder.ValidateAsync(db);
            Console.WriteLine("SUCCESS: all synthetic reference fixtures match the expected deterministic values.");
            break;

        case "seed-synthetic-card":
            if (!await db.Database.CanConnectAsync())
                throw new InvalidOperationException("Ingenio_QA is not reachable. Run the schema initializer first.");

            await SyntheticCardSeeder.SeedAsync(db);
            await SyntheticCardSeeder.ValidateAsync(db);
            Console.WriteLine("SUCCESS: synthetic customer and card fixtures are present and validated.");
            Console.WriteLine("Created/verified: 1 synthetic customer and 1 synthetic card (reusing DEMO_ORG_001).");
            Console.WriteLine("No real customer PII, real card numbers, balances, or payment records were created.");
            break;

        case "validate-synthetic-card":
            if (!await db.Database.CanConnectAsync())
                throw new InvalidOperationException("Ingenio_QA is not reachable.");

            await SyntheticCardSeeder.ValidateAsync(db);
            Console.WriteLine("SUCCESS: synthetic customer and card fixtures match the expected deterministic values.");
            break;
    }

    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Local QA operation '{command}' failed:");
    Console.Error.WriteLine(ex);
    return 1;
}

static bool IsLocalServer(string dataSource)
{
    var value = dataSource.Trim();

    if (value.StartsWith("(localdb)\\", StringComparison.OrdinalIgnoreCase))
        return true;

    var host = value;
    var slash = host.IndexOf('\\');
    if (slash >= 0)
        host = host[..slash];

    return host is "." or "(local)" or "localhost" or "127.0.0.1" or "::1"
        || string.Equals(host, Environment.MachineName, StringComparison.OrdinalIgnoreCase);
}
