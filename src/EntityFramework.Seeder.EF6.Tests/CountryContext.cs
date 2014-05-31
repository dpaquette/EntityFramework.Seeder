using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Seeder.EF6.Tests
{
    public class CountryContext : DbContext
    {
        static CountryContext()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<CountryContext>());
        }
        public DbSet<Country> Countries { get; set; }
    }
}
