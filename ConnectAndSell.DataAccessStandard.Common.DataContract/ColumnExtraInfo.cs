using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [DataContract]
    public class ColumnExtraInfo
    {
        [DataMember] public readonly string ColumnName;
        [DataMember] public readonly string EnumName;

        public ColumnExtraInfo(string columnName, string enumName)
        {
            this.ColumnName = columnName;
            this.EnumName = enumName;
        }
    }
}
