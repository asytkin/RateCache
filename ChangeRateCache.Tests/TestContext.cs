using DBLayer;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRateCache.Tests
{
    public class TestContext : DbContext
    {
        public TestContext()
            : base("Name=TestContext")
        {

        }
        public TestContext(bool enableLazyLoading, bool enableProxyCreation)
            : base("Name=TestContext")
        {
            Configuration.ProxyCreationEnabled = enableProxyCreation;
            Configuration.LazyLoadingEnabled = enableLazyLoading;
        }
        public TestContext(DbConnection connection)
            : base(connection, true)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<RateFrom> RateFrom { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<TestContext>(new AlwaysCreateInitializer());
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public void Seed(TestContext Context)
        {
            var listRates = new List<RateFrom>() {
           new RateFrom() { Id = 1, From = "USD", CreatingDate=DateTime.Now,To=new List<RateTo>{ new RateTo { Id=1,To="EUR", Rate=2m}, new RateTo { Id = 1, To = "ALL", Rate = 1.6m } } },
           new RateFrom() { Id = 2, From = "EUR", CreatingDate=DateTime.Now,To=new List<RateTo>{ new RateTo { Id=1,To= "USD", Rate=0.5m}, new RateTo { Id = 1, To = "ALL", Rate = 0.2m } } },
           new RateFrom() { Id = 3, From = "ALL", CreatingDate=DateTime.Now,To=new List<RateTo>{ new RateTo { Id=1,To= "USD", Rate=0.625m}, new RateTo { Id = 1, To = "EUR", Rate = 5m } } },
          };
            Context.RateFrom.AddRange(listRates);
            Context.SaveChanges();
        }

        public class DropCreateIfChangeInitializer : DropCreateDatabaseIfModelChanges<TestContext>
        {
            protected override void Seed(TestContext context)
            {
                context.Seed(context);
                base.Seed(context);
            }
        }

        public class CreateInitializer : CreateDatabaseIfNotExists<TestContext>
        {
            protected override void Seed(TestContext context)
            {
                context.Seed(context);
                base.Seed(context);
            }
        }

        public class AlwaysCreateInitializer : DropCreateDatabaseAlways<TestContext>
        {
            protected override void Seed(TestContext context)
            {
                context.Seed(context);
                base.Seed(context);
            }
        }
    }
}
