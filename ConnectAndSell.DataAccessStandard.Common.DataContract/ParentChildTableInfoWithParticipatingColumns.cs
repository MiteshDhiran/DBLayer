using System;
using System.Collections.Generic;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class ParentChildTableInfoWithParticipatingColumns
    {
        public string ParentTableName { get; }
        public string ChildTableName{ get; }
        public List<string> PrimaryTableColumns { get;  }
        public List<string> ChildTableColumns { get;  }
        public string FKName { get; }

        public ParentChildTableInfoWithParticipatingColumns(string parentTableName, string childTableName, List<string> primaryTableColumns, List<string> childTableColumns, string fkName)
        {
            ParentTableName = parentTableName ?? throw new ArgumentNullException($"{nameof(parentTableName)}");
            ChildTableName = childTableName ?? throw new ArgumentNullException($"{nameof(childTableName)}");
            PrimaryTableColumns = primaryTableColumns ?? throw new ArgumentNullException($"{nameof(primaryTableColumns)}");
            ChildTableColumns = childTableColumns ?? throw new ArgumentNullException($"{nameof(childTableColumns)}");
            FKName = fkName ?? throw new ArgumentNullException($"{nameof(fkName)}");
        }
    }
}
