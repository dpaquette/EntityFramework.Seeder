using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Seeder
{
    /// <summary>
    /// Represents an exception that has occurred while seeding a dbset.
    /// Wraps the underlying exception and is serializable so that the exception message can be
    ///   properly displayed in the package manager console in Visual Studio
    /// </summary>
    public class EfSeederException : Exception, ISerializable
    {
        public EfSeederException()
        {
        }

        public EfSeederException(string message) : base(message)
        {
        }

        public EfSeederException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EfSeederException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
