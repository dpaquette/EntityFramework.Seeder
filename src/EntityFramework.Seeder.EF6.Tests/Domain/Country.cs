using System.Collections.Generic;

namespace EntityFramework.Seeder.EF6.Tests.Domain
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<ProvinceState> ProvinceStates { get; set; }

        public override string ToString()
        {
            return string.Format("Country -> Name: {0}, Code: {1}", Name, Code);
        }
    }
}