namespace ConnectAndSell.Dal.Generator.Common
{
    public class TableDataTable
    {
        public string TableName { get;}
        public string ColumnName { get;}
        public string SqlDataType { get;}
        public string SqlColumnDefaultValue { get; set; }
        public int? StringMaxLength { get; set; }
        public int? NumericPrecision { get; set; }
        public int? NumericScale { get; set; }
        public int? DateTimePrecision { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPKKey { get; set; }
        public int? PKColumnOrdinalPosition { get; set; }
        public bool IsIdentity { get; set; }
        public TableDataTable(string tableName, string columnName, string sqlDataType, string sqlColumnDefaultValue
            , int? stringMaxLength, int? numericPrecision, int? numericScale, int? dateTimePrecision
            ,bool isNullable, bool isPKKey, int? pkColumnOrdinalPosition, bool isIdentity )
        {
            this.TableName = tableName;
            this.ColumnName = columnName;
            this.SqlDataType = sqlDataType;
            this.SqlColumnDefaultValue = sqlColumnDefaultValue;
            this.StringMaxLength = stringMaxLength;
            this.NumericPrecision = numericPrecision;
            this.NumericScale = numericScale;
            this.DateTimePrecision = dateTimePrecision;
            this.IsNullable = isNullable;
            this.IsPKKey = isPKKey;
            this.PKColumnOrdinalPosition = pkColumnOrdinalPosition;
            this.IsIdentity = isIdentity;
        }
	
    }
}