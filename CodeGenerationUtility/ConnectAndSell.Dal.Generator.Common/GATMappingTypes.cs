namespace ConnectAndSell.Dal.Generator.Common
{
    public class GATMappingTypes
    {
        public string CStype { get; set; }

        public string LINQAttributeType { get; set; }

        public GATMappingTypes(string cStype)
        {
            this.CStype = cStype;
            this.LINQAttributeType = cStype;
        }

        public GATMappingTypes(string cStype, string lINQAttributeType)
        {
            this.CStype = cStype;
            this.LINQAttributeType = lINQAttributeType;
        }
    }
}
