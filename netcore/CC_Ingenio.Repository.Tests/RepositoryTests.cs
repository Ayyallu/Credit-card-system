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

        [TestMethod]
        public void Every_Repository_Constructs_And_Its_Query_Translates_To_Sql()
        {
            using (var context = CreateContext())
            {
                foreach (var type in RepositoryTypes())
                {
                    var repository = Activator.CreateInstance(type, context);
                    var query = (IQueryable)type.GetMethod("GetAll").Invoke(repository, null);
                    var sql = query.ToQueryString();
                    Assert.IsFalse(string.IsNullOrWhiteSpace(sql), "No SQL for " + type.Name);
                }
            }
        }
    }
}
