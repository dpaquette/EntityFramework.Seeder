using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Seeder.EF6.Tests
{
    [TestFixture]
    public class LoadFromEmbeddedResourceTest
    {
        [Test]
        public void Test()
        {
            using (CountryContext context = new CountryContext())
            {
                context.Countries.SeedFromResource("EntityFramework.Seeder.EF6.Tests.CountriesResource.csv", c => c.Code);

                Assert.AreEqual(4, context.Countries.Local.Count);
            }
                
        }
    }
}
