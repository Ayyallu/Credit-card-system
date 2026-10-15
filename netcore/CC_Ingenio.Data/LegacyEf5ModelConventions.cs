using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CC_Ingenio.Data
{
    /// <summary>
    /// Reproduces the EF5 mapping behaviour the legacy CC_Ingenio.Core
    /// entities were written against, where EF Core's conventions differ
    /// (see PORT_NOTES.md):
    ///  1. Table names: the legacy context removed PluralizingTableNameConvention,
    ///     so every table is named after the CLR type (unless [Table] says
    ///     otherwise). EF Core would instead use the DbSet property name, which
    ///     differs for two entities (CardReactivateReason, UserGroupPrivilege).
    ///  2. Composite primary keys: ~100 entities declare them as
    ///     [Key, Column(Order = n)]. EF Core refuses composite keys declared
    ///     through data annotations, so they are re-declared here via HasKey in
    ///     the Column(Order) order.
    ///  3. Composite foreign keys declared as property-level
    ///     [ForeignKey("Navigation")] on several properties: EF5 paired them in
    ///     Column(Order) order; EF Core doesn't guarantee an order, so it is
    ///     enforced here.
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

            foreach (var entityType in entityTypes)
            {
                foreach (var foreignKey in entityType.GetDeclaredForeignKeys().ToList())
                {
                    EnforcePropertyLevelForeignKeyOrder(modelBuilder, entityType, foreignKey);
                }
            }
        }

        // Column(Order) when set, otherwise after everything that has one.
        internal static int ColumnOrder(PropertyInfo property)
        {
            var order = property.GetCustomAttribute<ColumnAttribute>()?.Order ?? -1;
            return order < 0 ? int.MaxValue : order;
        }

        private static void EnforcePropertyLevelForeignKeyOrder(
            ModelBuilder modelBuilder, IMutableEntityType entityType, IMutableForeignKey foreignKey)
        {
            if (foreignKey.Properties.Count < 2)
            {
                return;
            }

            // Navigation-level [ForeignKey("A,B")] lists carry an explicit order
            // that EF Core already honours; only property-level declarations
            // are order-ambiguous.
            var propertyInfos = foreignKey.Properties.Select(p => p.PropertyInfo).ToList();
            if (propertyInfos.Any(p => p == null || p.GetCustomAttribute<ForeignKeyAttribute>() == null))
            {
                return;
            }

            var ordered = propertyInfos
                .OrderBy(ColumnOrder)
                .ThenBy(p => p.MetadataToken)
                .Select(p => p.Name)
                .ToArray();

            if (ordered.SequenceEqual(foreignKey.Properties.Select(p => p.Name)))
            {
                return;
            }

            var navigation = foreignKey.DependentToPrincipal;
            if (navigation == null)
            {
                return;
            }

            var principalClr = foreignKey.PrincipalEntityType.ClrType;
            var principalKeyNames = foreignKey.PrincipalKey.Properties.Select(p => p.Name).ToArray();
            var inverseName = foreignKey.PrincipalToDependent?.Name;

            var reference = modelBuilder.Entity(entityType.ClrType).HasOne(principalClr, navigation.Name);
            if (foreignKey.IsUnique)
            {
                reference.WithOne(inverseName)
                    .HasForeignKey(entityType.ClrType, ordered)
                    .HasPrincipalKey(principalClr, principalKeyNames);
            }
            else
            {
                reference.WithMany(inverseName)
                    .HasForeignKey(ordered)
                    .HasPrincipalKey(principalKeyNames);
            }
        }
    }
}
