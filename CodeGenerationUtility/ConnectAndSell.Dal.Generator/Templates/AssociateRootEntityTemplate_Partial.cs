using System;
using System.Collections.Generic;
using System.Text;
using ConnectAndSell.Dal.Generator.Common;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.Dal.Generator.Templates
{
    public partial class AssociateRootEntityTemplate
    {
        public AssociateRootEntityTemplate(DataContextGeneratorParams dataContextGeneratorParams, AssociatedParentChildTableInfo associatedParentChildTableInfo, string associateChildrenPropertiesText, string associateChildrenPropertiesInitializationText, string associateRootColumnsText)
        {
            DataContextGeneratorParams = dataContextGeneratorParams;
            AssociatedParentChildTableInfo = associatedParentChildTableInfo;
            AssociateChildrenPropertiesText = associateChildrenPropertiesText;
            AssociateChildrenPropertiesInitializationText = associateChildrenPropertiesInitializationText;
            AssociateRootColumnsText = associateRootColumnsText ?? string.Empty;
        }

        public DataContextGeneratorParams DataContextGeneratorParams { get; }
        public AssociatedParentChildTableInfo AssociatedParentChildTableInfo { get; }
        public string AssociateChildrenPropertiesText { get; }
        public string AssociateChildrenPropertiesInitializationText { get; }
        public string AssociateRootColumnsText { get; }
        
        
        
    }
}
