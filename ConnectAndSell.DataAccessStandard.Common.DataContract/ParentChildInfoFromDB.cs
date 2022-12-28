namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class ParentChildInfoFromDB
    {
        public string ParentTableName { get; }
        public string ChildTableName { get; }

        public string FKName { get; }
        public string ParentTableFKColumnNames { get; }
        public string ChildTableFKColumnNames { get; }
        public string EntitySetPropertyName { get; }
        public string EntityRefPropertyName { get; }

        public ParentChildInfoFromDB(string parentTableName, string childTableName, string fkName, string parentTableFkColumnNames, string childTableFkColumnNames, string entitySetPropertyName, string entityRefPropertyName)
        {
            ParentTableName = parentTableName;
            ChildTableName = childTableName;
            FKName = fkName;
            ParentTableFKColumnNames = parentTableFkColumnNames;
            ChildTableFKColumnNames = childTableFkColumnNames;
            EntitySetPropertyName = entitySetPropertyName;
            EntityRefPropertyName = entityRefPropertyName;
        }
    }
}
