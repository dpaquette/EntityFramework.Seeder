using System.Data.Entity.Migrations;
using System.Linq;
using EntityFramework.Seeder.EF6.Tests.Domain;
using Given.Common;
using Given.NUnit;
using NUnit.Framework;

namespace EntityFramework.Seeder.EF6.Tests
{
    [Story(AsA = "developer",
        IWant = "to load a list of provinces from an embedded resource",
        SoThat = "I do not need to enter it manually")]
    public class when_seeding_a_dbset_with_related_entites : Scenario
    {
        public static CountryContext _context;

        given a_context_with_existing_entities = () =>
        {
            _context = new CountryContext();     
            _context.DeleteAll();
            _context.Countries.AddOrUpdate(c => c.Code, 
                    new Country {Code = "AT", Name = "Austria"},
                    new Country { Code = "AU", Name = "Australia" },
                    new Country { Code = "CA", Name = "Canada" },
                    new Country { Code = "US", Name = "United States" });
            _context.SaveChanges();
        };

        when seeding_provinces_from_a_resource = () =>
        {
            _context.ProvinceStates.SeedFromResource(
                "EntityFramework.Seeder.EF6.Tests.ProvinceStatesResource.csv",
                p => p.Code,
                new CsvColumnMapping<ProvinceState>("CountryCode", (province, countryCode) =>
                    {
                        province.Country = _context.Countries.Single(c => c.Code == countryCode);
                    })
                );
                 
            _context.SaveChanges();
        };

        [then]
        public void the_db_set_should_contain_5_provinces()
        {            
            _context.ProvinceStates.Count().ShouldEqual(5);
        }
        
        [then]
        public void the_db_set_should_contain_the_expected_provinces_with_related_countries()
        {
            _context.ProvinceStates.ShouldContain(c => c.Code == "SK" && c.Name == "Saskatchewan" && c.Country != null && c.Country.Code == "CA");
            _context.ProvinceStates.ShouldContain(c => c.Code == "AB" && c.Name == "Alberta" && c.Country != null && c.Country.Code == "CA");
            _context.ProvinceStates.ShouldContain(c => c.Code == "AZ" && c.Name == "Arizona" && c.Country != null && c.Country.Code == "US");
            _context.ProvinceStates.ShouldContain(c => c.Code == "AR" && c.Name == "Arkansas" && c.Country != null && c.Country.Code == "US");
            _context.ProvinceStates.ShouldContain(c => c.Code == "CA" && c.Name == "California" && c.Country != null && c.Country.Code == "US");
        }

        [then]
        public void no_additional_countries_should_have_been_added()
        {
            _context.Countries.Count().ShouldEqual(4);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}