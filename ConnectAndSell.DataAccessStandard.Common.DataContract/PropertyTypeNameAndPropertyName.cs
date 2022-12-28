namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class PropertyTypeNameAndPropertyName
    {
        public string PropertyTypeName { get; }
        public string PropertyName { get; }

        public bool IsComplexType { get; }


        public PropertyTypeNameAndPropertyName(string propertyTypeName, string propertyName, bool isComplexType)
        {
            PropertyTypeName = propertyTypeName;
            PropertyName = propertyName;
            IsComplexType = isComplexType;
        }
    }
}
