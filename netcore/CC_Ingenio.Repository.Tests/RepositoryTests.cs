using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using CC_Ingenio.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CC_Ingenio.Repository.Tests
{
    // No database is touched: repositories are constructed over a context that
    // is never opened, and queries are only *translated* to SQL
    // (ToQueryString), which proves every entity behind every repository can be
    // queried through EF Core.
    [TestClass]
    public class RepositoryTests
    {
        private static UnitOfWork CreateContext()
        {
            var options = new DbContextOptionsBuilder<UnitOfWork>()
                .UseLazyLoadingProxies()
                .UseSqlServer("Server=.;Database=ModelOnly;Integrated Security=true;TrustServerCertificate=true")
                .Options;
            return new UnitOfWork(options);
        }

        private static Type[] RepositoryTypes()
        {
            return typeof(CC_Ingenio.Repository.MasterRepo.CurrencyRepository).Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && IsGenericRepository(t))
                .ToArray();
        }

        private static bool IsGenericRepository(Type type)
        {
            for (var t = type.BaseType; t != null; t = t.BaseType)
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(GenericRepository<>))
                {
                    return true;
                }
            }
            return false;
        }

        [TestMethod]
        public void All_Repositories_Are_Discovered()
        {
            Assert.AreEqual(144, RepositoryTypes().Length);
        }

        private static Type EntityTypeOf(Type repositoryType)
        {
            for (var t = repositoryType.BaseType; t != null; t = t.BaseType)
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(GenericRepository<>))
                {
                    return t.GetGenericArguments()[0];
                }
            }
            throw new InvalidOperationException(repositoryType.Name);
        }

        // Repositories whose entity type is not part of the EF model: no DbSet
        // on UnitOfWork and not reachable through any navigation. Legacy EF5 had
        // the same gap (Set<T>() on an unmapped type throws there too), so set
        // operations on these were already unusable; they are only meaningful
        // through UnitOfWork.ExecuteQuery<T> (stored-procedure/raw-SQL result
        // shapes). Kept as an explicit list so a new gap fails this test.
        private static readonly string[] UnmappedEntityRepositories = new string[0];

        [TestMethod]
        public void Repositories_Over_Unmapped_Entities_Are_The_Known_Set()
        {
            using (var context = CreateContext())
            {
                var unmapped = RepositoryTypes()
                    .Where(t => context.Model.FindEntityType(EntityTypeOf(t)) == null)
                    .Select(t => t.Name)
                    .OrderBy(n => n)
                    .ToArray();

                CollectionAssert.AreEqual(
                    UnmappedEntityRepositories.OrderBy(n => n).ToArray(),
                    unmapped,
                    "Unmapped-entity repositories: " + string.Join(", ", unmapped));
            }
        }

        [TestMethod]
        public void Every_Mapped_Repository_Constructs_And_Its_Query_Translates_To_Sql()
        {
            using (var context = CreateContext())
            {
                var failures = new System.Collections.Generic.List<string>();
                foreach (var type in RepositoryTypes())
                {
                    if (context.Model.FindEntityType(EntityTypeOf(type)) == null)
                    {
                        continue;
                    }

                    try
                    {
                        var repository = Activator.CreateInstance(type, context);
                        var query = (IQueryable)type.GetMethod("GetAll").Invoke(repository, null);
                        if (string.IsNullOrWhiteSpace(query.ToQueryString()))
                        {
                            failures.Add(type.Name + ": empty SQL");
                        }
                    }
                    catch (Exception ex)
                    {
                        failures.Add(type.Name + ": " + (ex.InnerException ?? ex).Message);
                    }
                }

                Assert.AreEqual(0, failures.Count, string.Join(Environment.NewLine, failures));
            }
        }
    }
}
