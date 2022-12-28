using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectAndSell.Dal.Generator.Templates
{
    public partial class DataContextTableConfiguration
    {
        public string TableClassName { get; set; }
        public string CommaseperatedkeyColumns { get; set; }
        public string PKName { get; set; }
        public string TableName { get; set; }
        public string TablePropertiesConfiguration { get; set; }

        public List<string> IgnorePropertyNames { get; set; }
        
        public string RelationConfiguration { get; set; }
    }
}
