using System.Data.Entity;
using System.Linq;

namespace EntityFramework.Seeder.EF6.Tests.Domain
{
    public class CountryContext : DbContext
    {
        static CountryContext()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<CountryContext>());
        }
        public void DeleteAll()
        {
            foreach (var country in Countries.ToArray())
            {
                Countries.Remove(country);
            }

            foreach (var province in ProvinceStates.ToArray())
            {
                ProvinceStates.Remove(province);
            }

            SaveChanges();

        }
        public DbSet<Country> Countries { get; set; }

        public DbSet<ProvinceState> ProvinceStates { get; set; }
    }
}
