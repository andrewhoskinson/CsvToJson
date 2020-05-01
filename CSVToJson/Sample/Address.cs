using CSVToJson.Mapping;

namespace CSVToJson.Sample
{
    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
    }

    public class AddressMapping : ClassMap<Address>
    {
        public AddressMapping()
        {
            Prefix("address_");

            Map(x => x.Line1).Column("line1");
            Map(x => x.Line2).Column("line2");
        }
    }
}
