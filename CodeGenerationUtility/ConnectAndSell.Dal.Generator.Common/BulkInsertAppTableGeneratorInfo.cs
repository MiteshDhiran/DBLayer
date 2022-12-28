namespace ConnectAndSell.Dal.Generator.Common
{
    public class BulkInsertAppTableGeneratorInfo
    {
        public string TableUDTName { get; }
        public string ColumnWithDataType { get; }

        public BulkInsertAppTableGeneratorInfo(string tableUdtName, string columnWithDataType)
        {
            TableUDTName = tableUdtName;
            ColumnWithDataType = columnWithDataType;
        }
    }
}
