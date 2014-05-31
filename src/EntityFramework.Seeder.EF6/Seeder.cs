using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Seeder.EF6
{
    public static class Seeder
    {
        public static void SeedFromStream<T>(this DbSet<T> dbSet, Stream stream, Expression<Func<T, object>> identifierExpression) where T : class
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                CsvReader csvReader = new CsvReader(reader);
                csvReader.Configuration.WillThrowOnMissingField = false;
                var entities = csvReader.GetRecords<T>().ToArray();
                dbSet.AddOrUpdate(identifierExpression, entities);
            }
        }

        public static void SeedFromResource<T>(this DbSet<T> dbSet, string embeddedResourceName, Expression<Func<T, object>> identifierExpression) where T : class
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(embeddedResourceName))
            {
                SeedFromStream(dbSet, stream, identifierExpression);
            }
        }

        public static void SeedFromFile<T>(this DbSet<T> dbSet, string fileName, Expression<Func<T, object>> identifierExpression) where T : class
        {
            using (Stream stream = File.OpenRead(fileName))
            {
                SeedFromStream(dbSet, stream, identifierExpression);
            }
        }


    }
}
