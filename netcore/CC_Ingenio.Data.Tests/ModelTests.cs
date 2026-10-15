using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using CC_Ingenio.Core.LookUpRepo;
using CC_Ingenio.Core.MasterRepo.MasterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CC_Ingenio.Data.Tests
{
    // These tests never open a database connection: EF Core builds and
    // validates the model, and generates DDL, entirely offline. They exist to
    // prove the legacy EF5 entity mappings (composite keys declared with
    // [Key, Column(Order)], property-level composite [ForeignKey], table
    // naming, lazy-loading proxies) still produce a valid EF Core model.
    [TestClass]
    public class ModelTests
    {
        private static UnitOfWork CreateContext()
        {
            var options = new DbContextOptionsBuilder<UnitOfWork>()
                .UseLazyLoadingProxies()
                .UseSqlServer("Server=.;Database=ModelOnly;Integrated Security=true;TrustServerCertificate=true")
                .Options;
            return new UnitOfWork(options);
        }

        [TestMethod]
        public void Model_Builds_And_Validates()
        {
            using (var context = CreateContext())
            {
                Assert.IsTrue(context.Model.GetEntityTypes().Count() > 100);
            }
        }

        [TestMethod]
        public void Model_Generates_Create_Script()
        {
            using (var context = CreateContext())
            {
                var script = context.Database.GenerateCreateScript();
                Assert.IsFalse(string.IsNullOrWhiteSpace(script));
            }
        }

        [TestMethod]
        public void TableNames_Follow_Legacy_Convention_Not_DbSet_Property_Names()
        {
            using (var context = CreateContext())
            {
                // DbSet property names differ from these two types' names.
                Assert.AreEqual("CardReactivateReason",
                    context.Model.FindEntityType(typeof(CardReactivateReason)).GetTableName());
                Assert.AreEqual("UserGroupPrivilege",
                    context.Model.FindEntityType(typeof(CC_Ingenio.Core.UserRepo.UserAggregate.UserGroupPrivilege)).GetTableName());

                // Explicit [Table] still wins.
                Assert.AreEqual("MT_BillingParameter",
                    context.Model.FindEntityType(typeof(BillingParameter)).GetTableName());
            }
        }

        [TestMethod]
        public void Composite_Keys_Follow_Column_Order()
        {
            using (var context = CreateContext())
            {
                var key = context.Model.FindEntityType(typeof(BillingParameter)).FindPrimaryKey();
                CollectionAssert.AreEqual(
                    new[] { "OrganizationCode", "ProductCode" },
                    key.Properties.Select(p => p.Name).ToArray());
            }
        }

        [TestMethod]
        public void Every_Entity_With_Key_Attributes_Has_Matching_Primary_Key()
        {
            using (var context = CreateContext())
            {
                foreach (var entityType in context.Model.GetEntityTypes())
                {
                    var expected = entityType.ClrType
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => p.GetCustomAttribute<KeyAttribute>() != null)
                        .OrderBy(ColumnOrder)
                        .ThenBy(p => p.MetadataToken)
                        .Select(p => p.Name)
                        .ToArray();

                    if (expected.Length == 0)
                    {
                        continue;
                    }

                    CollectionAssert.AreEqual(
                        expected,
                        entityType.FindPrimaryKey().Properties.Select(p => p.Name).ToArray(),
                        "Primary key mismatch on " + entityType.ClrType.Name);
                }
            }
        }

        // The five entities that used EF5's property-level composite
        // [ForeignKey("Nav")] were rewritten to navigation-level lists; the
        // order must match the legacy Column(Order) pairing exactly, because a
        // swapped pair of same-typed columns would silently join wrongly.
        [TestMethod]
        public void Converted_Composite_Foreign_Keys_Keep_Legacy_Column_Order()
        {
            AssertForeignKey(typeof(BillingParameter), "Product", "OrganizationCode", "ProductCode");
            AssertForeignKey(typeof(BlockCodeForCommissionExclusion), "Block", "BlockCode", "OrganizationCode");
            AssertForeignKey(typeof(CreditSubSegment), "objCreditSegment", "CreditSegmentCode", "OrganizationCode");
            AssertForeignKey(typeof(MerchantDiscount), "ObjMerchant", "MerchantCode", "OrganizationCode");
            AssertForeignKey(typeof(Product), "Currency", "OrganizationCode", "CurrencyCode");
        }

        // EF Core rejects property-level composite [ForeignKey("Nav")] outright;
        // this guards against one being reintroduced.
        [TestMethod]
        public void No_Entity_Uses_Property_Level_Composite_ForeignKey()
        {
            using (var context = CreateContext())
            {
                foreach (var entityType in context.Model.GetEntityTypes())
                {
                    var groups = entityType.ClrType
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(p => p.GetCustomAttribute<ForeignKeyAttribute>())
                        .Where(a => a != null && !a.Name.Contains(","))
                        .GroupBy(a => a.Name)
                        .Where(g => g.Count() > 1);
                    Assert.IsFalse(groups.Any(), "Property-level composite FK on " + entityType.ClrType.Name);
                }
            }
        }

        private static void AssertForeignKey(Type dependent, string navigation, params string[] expectedProperties)
        {
            using (var context = CreateContext())
            {
                var foreignKey = context.Model.FindEntityType(dependent)
                    .GetDeclaredForeignKeys()
                    .Single(fk => fk.DependentToPrincipal != null && fk.DependentToPrincipal.Name == navigation);
                CollectionAssert.AreEqual(
                    expectedProperties,
                    foreignKey.Properties.Select(p => p.Name).ToArray(),
                    "FK order on " + dependent.Name + "." + navigation);
            }
        }

        private static int ColumnOrder(PropertyInfo property)
        {
            var order = property.GetCustomAttribute<ColumnAttribute>()?.Order ?? -1;
            return order < 0 ? int.MaxValue : order;
        }
    }
}
