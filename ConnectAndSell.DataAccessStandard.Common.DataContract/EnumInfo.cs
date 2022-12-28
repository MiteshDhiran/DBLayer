using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [DataContract]
    public class EnumInfo
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string EnumType { get; set; }

        [DataMember]
        public List<EnumMemberInfo> EnumMemberList { get; set; }

        

        public  EnumInfo(string name,string type, List<EnumMemberInfo> dataMemberList)
        {
            Name = name;
            EnumType = type;
            EnumMemberList = dataMemberList;
        }
    }

    [DataContract]
    public class EnumMemberInfo
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public readonly bool IsDirect;

        public EnumMemberInfo(string mname, string mvalue,bool isDirect)
        {
            Name = mname;
            Value = mvalue;
            IsDirect = isDirect;
        }
    }
}
