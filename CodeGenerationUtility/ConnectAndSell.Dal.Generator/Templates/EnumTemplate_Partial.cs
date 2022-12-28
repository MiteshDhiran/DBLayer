using System;
using System.Collections.Generic;
using System.Text;
using ConnectAndSell.Dal.Generator.Common;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.Dal.Generator.Templates
{
    public partial class EnumTemplate
    {
        public DataContextGeneratorParams DataContextGeneratorParams { get; }

        public EnumInfo EnumDataContractInfo { get; }
        public string EnumName { get; set; }

        public string EnumMemberList { get; set; }

        public string EnumType { get; set; }

        public EnumTemplate(DataContextGeneratorParams dataContextGeneratorParams,string enumName, string enumMemberList, string enumType)
        {
            DataContextGeneratorParams = dataContextGeneratorParams;
            EnumName = enumName;
            EnumMemberList = enumMemberList;
            EnumType = enumType;
        }

    }
}
