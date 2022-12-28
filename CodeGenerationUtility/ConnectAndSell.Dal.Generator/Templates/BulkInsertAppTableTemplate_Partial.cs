using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectAndSell.Dal.Generator.Templates
{
    public partial class BulkInsertAppTableTemplate
    {
        public string TableUDTName { get; }
        public string ColumnWithDataType { get; }

        public BulkInsertAppTableTemplate(string tableUdtName, string columnWithDataType)
        {
            TableUDTName = tableUdtName;
            ColumnWithDataType = columnWithDataType;
        }
    }
}
