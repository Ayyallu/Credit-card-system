using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CC_Ingenio.Data;

if (args.Length != 1 || string.IsNullOrWhiteSpace(args[0]))
{
    Console.Error.WriteLine("Usage:");
    Console.Error.WriteLine("  dotnet run --project netcore/CC_Ingenio.LocalQa -- \"<local integrated-security connection string>\"");
    return 2;
}

SqlConnectionStringBuilder connection;
try
{
    connection = new SqlConnectionStringBuilder(args[0]);
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
Console.WriteLine("This operation creates the current EF Core model schema only. It does not seed data, drop objects, or import production data.");

var options = new DbContextOptionsBuilder<UnitOfWork>()
    .UseLazyLoadingProxies()
    .UseSqlServer(connection.ConnectionString)
    .Options;

try
{
    await using var db = new UnitOfWork(options);
    var created = await db.Database.EnsureCreatedAsync();

    if (created)
    {
        Console.WriteLine("SUCCESS: Ingenio_QA was created from the current EF Core model.");
    }
    else
    {
        Console.WriteLine("No changes made: the database already exists. EnsureCreated does not update an existing database.");
        Console.WriteLine("Inspect the existing database in SSMS before taking any further action.");
    }

    Console.WriteLine($"EF Core model entity types: {db.Model.GetEntityTypes().Count()}");
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine("Local QA database initialization failed:");
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
