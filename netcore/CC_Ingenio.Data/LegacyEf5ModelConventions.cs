using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace CC_Ingenio.Data
{
    /// <summary>
    /// Reproduces the EF5 mapping behaviour the legacy CC_Ingenio.Core
    /// entities were written against, where EF Core's conventions differ
    /// (see PORT_NOTES.md):
    ///  1. Table names: the legacy context removed PluralizingTableNameConvention,
    ///     so every table is named after the CLR type (unless [Table] says
    ///     otherwise). EF Core would instead use the DbSet property name. Today
    ///     that makes no difference (the only two DbSets whose names differ from
    ///     their type, CardReactivateReason and UserGroupPrivilege, have explicit
    ///     [Table] attributes), so this is a safeguard against a future entity
    ///     being added with a differently-named DbSet.
    ///  2. Composite primary keys: ~100 entities declare them as
    ///     [Key, Column(Order = n)]. EF Core refuses composite keys declared
    ///     through data annotations, so they are re-declared here via HasKey in
    ///     the Column(Order) order.
    /// Composite foreign keys are not handled here: the five entities that
    /// used EF5's property-level [ForeignKey("Nav")] form were rewritten in the
    /// ported CC_Ingenio.Core to the navigation-level [ForeignKey("A,B")] form,
    /// which EF Core supports natively.
    /// </summary>
    internal static class LegacyEf5ModelConventions
    {
        public static void Apply(ModelBuilder modelBuilder)
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes().ToList();

            foreach (var entityType in entityTypes)
            {
                var clr = entityType.ClrType;
                if (entityType.BaseType == null && clr.GetCustomAttribute<TableAttribute>() == null)
                {
                    entityType.SetTableName(clr.Name);
                }
            }

            foreach (var entityType in entityTypes)
            {
                var clr = entityType.ClrType;
                var keyProperties = clr
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<KeyAttribute>() != null)
                    .OrderBy(ColumnOrder)
                    .ThenBy(p => p.MetadataToken)
                    .Select(p => p.Name)
                    .ToArray();

                if (keyProperties.Length > 1)
                {
                    modelBuilder.Entity(clr).HasKey(keyProperties);
                }
            }
        }

        // Column(Order) when set, otherwise after everything that has one.
        internal static int ColumnOrder(PropertyInfo property)
        {
            var order = property.GetCustomAttribute<ColumnAttribute>()?.Order ?? -1;
            return order < 0 ? int.MaxValue : order;
        }
    }
}
