using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectAndSell.Dal.Generator.Templates
{
    public partial class SPWrapperClassTemplate
    {
        public string WrapperClassSuffix { get; set; }
        public string UsingBlock { get; set; }
        public string  ClassBody { get; set; }

        public string DALHelperName { get; set; }
    }
}
