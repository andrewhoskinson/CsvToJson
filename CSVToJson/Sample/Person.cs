using CSVToJson.Mapping;

namespace CSVToJson.Sample
{
    /// <summary>
    /// Note the proper case on property names, and the use of .column() in the ClassMap to map these columns to their lowercase
    /// names in the input
    /// </summary>
    public class Person
    {
        public string Name { get; set; }
        public Address Address { get; set; }      
    }

    public class PersonMapping : ClassMap<Person>
    {
        public PersonMapping()
        {
            Map(x => x.Name).Column("name");
            Map(x => x.Address);
        }
    }
}
