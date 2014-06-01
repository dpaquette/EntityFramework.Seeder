using System;

namespace EntityFramework.Seeder
{
    /// <summary>
    /// Defines a custom mapping action for a particular column in a CSV file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CsvColumnMapping<T>
    {
        private readonly string _csvColumnName;
        private readonly Action<T, object> _action;

        /// <summary>
        /// Create new custom mapping action for the specified CSV column name
        /// </summary>
        /// <param name="csvColumnName">The name of the column in the CSV file</param>
        /// <param name="action">The action to execute for each row in the CSV file</param>
        public CsvColumnMapping(string csvColumnName, Action<T, object> action)
        {
            _csvColumnName = csvColumnName;
            _action = action;
        }

        public void Execute(T entity, object csvColumnValue)
        {
            _action(entity, csvColumnValue);
        }

        /// <summary>
        /// The name of the csv column
        /// </summary>
        public string CsvColumnName
        {
            get { return _csvColumnName; }
        }
    }
}