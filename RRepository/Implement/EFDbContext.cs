using RDomain.Entity;
using RRepository.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RRepository.Implement
{
    public class EFDbContext : DbContext
    {
        static EFDbContext()
        {
            Database.SetInitializer<EFDbContext>(null);
        }
        public EFDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EFDbContext, Configuration>("DefaultConnection"));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //解决EF动态建库数据库表名变为复数问题  
            // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            var typesRegister = Assembly.GetExecutingAssembly().GetTypes()
              .Where(type => !(string.IsNullOrEmpty(type.Namespace))).Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

        }

        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Income> Incomes { get; set; }
    }
}
