using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        [ExpectedException(typeof(Exception))]
        public void when_seeding_from_a_file_with_unmapped_columns()
        {
            _context.Countries.SeedFromFile("CountriesFileWithErrors.csv", c => c.Code);
        }

        [then]
        public void the_exception_should_be_serializable()
        {
            Exception exception = null;
            try
            {
                _context.Countries.SeedFromFile("CountriesFileWithErrors.csv", c => c.Code);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);

            string exceptionToString = exception.ToString();

            // Round-trip the exception: Serialize and de-serialize with a BinaryFormatter
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                // "Save" object state
                bf.Serialize(ms, exception);

                // Re-use the same stream for de-serialization
                ms.Seek(0, 0);

                // Replace the original exception with de-serialized one
                exception = (Exception)bf.Deserialize(ms);
            }

            // Double-check that the exception message and stack trace (owned by the base Exception) are preserved
            Assert.AreEqual(exceptionToString, exception.ToString(), "ex.ToString()");
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}