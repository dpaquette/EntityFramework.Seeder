using CsvHelper;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace EntityFramework.Seeder
{
    /// <summary>
    /// A set of helper methods for seeding DbSets from CSV files
    /// </summary>
    public static class Seeder
    {
        /// <summary>
        /// Seeds a DBSet from a CSV file that will be read from the specified stream
        /// </summary>
        /// <typeparam name="T">The type of entity to load</typeparam>
        /// <param name="dbSet">The DbSet to populate</param>
        /// <param name="stream">The stream containing the CSV file contents</param>
        /// <param name="identifierExpression">An expression specifying the properties that should be used when determining whether an Add or Update operation should be performed.</param>
        /// <param name="additionalMapping">Any additonal complex mappings required</param>
        public static void SeedFromStream<T>(this IDbSet<T> dbSet, Stream stream, Expression<Func<T, object>> identifierExpression, params CsvColumnMapping<T>[] additionalMapping) where T : class
        {
            try
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    var map = csvReader.Configuration.AutoMap<T>();
                    map.ReferenceMaps.Clear();
                    csvReader.Configuration.RegisterClassMap(map);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    while (csvReader.Read())
                    {
                        var entity = csvReader.GetRecord<T>();
                        foreach (CsvColumnMapping<T> csvColumnMapping in additionalMapping)
                        {
                            csvColumnMapping.Execute(entity, csvReader.GetField(csvColumnMapping.CsvColumnName));
                        }
                        dbSet.AddOrUpdate(identifierExpression, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Error Seeding DbSet {0}: {1}", dbSet.GetType().Name, ex.Message);
                throw new EfSeederException(message, ex);                
            }
        }

        /// <summary>
        /// Seeds a DBSet from a CSV file that will be read from the specified embedded resource
        /// </summary>
        /// <typeparam name="T">The type of entity to load</typeparam>
        /// <param name="dbSet">The DbSet to populate</param>
        /// <param name="embeddedResourceName">The name of the embedded resource containing the CSV file contents</param>
        /// <param name="identifierExpression">An expression specifying the properties that should be used when determining whether an Add or Update operation should be performed.</param>
        /// <param name="additionalMapping">Any additonal complex mappings required</param>
        public static void SeedFromResource<T>(this IDbSet<T> dbSet, string embeddedResourceName, Expression<Func<T, object>> identifierExpression, params CsvColumnMapping<T>[] additionalMapping) where T : class
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(embeddedResourceName))
            {
                SeedFromStream(dbSet, stream, identifierExpression, additionalMapping);
            }
        }

        /// <summary>
        /// Seeds a DBSet from a CSV file that will be read from the specified file name
        /// </summary>
        /// <typeparam name="T">The type of entity to load</typeparam>
        /// <param name="dbSet">The DbSet to populate</param>
        /// <param name="fileName">The name of the file containing the CSV file contents</param>
        /// <param name="identifierExpression">An expression specifying the properties that should be used when determining whether an Add or Update operation should be performed.</param>
        /// <param name="additionalMapping">Any additonal complex mappings required</param>
        public static void SeedFromFile<T>(this IDbSet<T> dbSet, string fileName, Expression<Func<T, object>> identifierExpression, params CsvColumnMapping<T>[] additionalMapping) where T : class
        {
            using (Stream stream = File.OpenRead(fileName))
            {
                SeedFromStream(dbSet, stream, identifierExpression, additionalMapping);
            }
        }
    }
}
