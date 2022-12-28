using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectAndSell.Dal.Generator.Templates
{
    public partial class DataContextClassTemplate
    {
        public string ProjectDataContractUsingStatement { get; set; }
        public string ProjectDataContextNameSpace { get; set; }
        public string DataContextClassName { get; set; }
        public string ModelBuilderConfigurationBody { get; set; }
    }
}
