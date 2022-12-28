using System.Collections.Generic;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class NonPrimaryUniqueIndexRecordInfo
    {
        public string IndexName { get; }
        public string TableName { get; }
        public int ColumnOrder { get; }
        public string ColumnName { get; }

        public NonPrimaryUniqueIndexRecordInfo(string indexName, string tableName, int columnOrder, string columnName)
        {
            IndexName = indexName;
            TableName = tableName;
            ColumnOrder = columnOrder;
            ColumnName = columnName;
        }
    }

    public class NonPrimaryUniqueIndexInfo
    {
        public string IndexName { get; }
        public string TableName { get; }
        
        public List<string> ColumnNames { get; }

        public NonPrimaryUniqueIndexInfo(string indexName, string tableName, List<string> columnNames)
        {
            IndexName = indexName;
            TableName = tableName;
            ColumnNames = columnNames;
        }
    }

    public class NonPrimaryUniqueIndexDictionary
    {
        public NonPrimaryUniqueIndexDictionary(string tableName, Dictionary<string, List<string>> nonPrimaryIndexDictionary)
        {
            TableName = tableName;
            NonPrimaryIndexDictionary = nonPrimaryIndexDictionary;
        }

        public string  TableName { get; }
        public Dictionary<string,List<string>> NonPrimaryIndexDictionary { get; }
        

    }
}
