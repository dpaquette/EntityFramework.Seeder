namespace EntityFramework.Seeder.EF6.Tests.Domain
{
    public class ProvinceState
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual Country Country { get; set; }

        public override string ToString()
        {
            return string.Format("Province -> Name: {0}, Code: {1}", Name, Code);
        }
    }
}
