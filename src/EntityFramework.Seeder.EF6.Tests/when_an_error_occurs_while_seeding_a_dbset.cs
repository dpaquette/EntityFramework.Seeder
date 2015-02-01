using System;
using System.Linq;
using EntityFramework.Seeder.EF6.Tests.Domain;
using Given.Common;
using Given.NUnit;
using NUnit.Framework;

namespace EntityFramework.Seeder.EF6.Tests
{
    [Story(AsA = "developer",
        IWant = "see exception details",
        SoThat = "I can diagnose errors that might happen while seeding a dbset")]
    public class while_an_error_occurs_while_seeding_a_dbset : Scenario
    {
        public static CountryContext _context;

        given an_empty_context = () =>
        {
            _context = new CountryContext();
            _context.DeleteAll();
        };


        [then]
        [ExpectedException(typeof(EfSeederException))]
        public void when_seeding_from_a_file_with_unmapped_columns()
        {
            _context.Countries.SeedFromFile("CountriesFileWithErrors.csv", c => c.Code);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}