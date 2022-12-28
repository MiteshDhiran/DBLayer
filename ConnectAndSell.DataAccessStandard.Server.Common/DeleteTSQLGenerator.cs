using System;
using System.Linq;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public static class DeleteTSQLGenerator
    {
        
        internal static string GetDeleteSQLStatementForType(ORMModelMetaInfo modelInfo, Type type)
        {
            var nestedChildTypeHierarchyList = modelInfo.GetNestedChildTableTypeWithEntitySetInfo(type);
            var list = 
                nestedChildTypeHierarchyList.Where(t => t.EntitySetPropertyInfo != null).Select(t => new { 
                TableDataContractMetaInfoWithType = modelInfo.GetTableDataContractMetaInfoOfType(t.ChildType) 
                ,  TempTableName = t.ChildTempTableNameWithParentName
                , thisTableColumnsUsedForJoining = t.EntitySetPropertyInfo.ChildTableColumnNames
                , otherTableNameToJoinWith = t.ParentTempTableNameWithParentName
                , otherTableColumnNames =t.EntitySetPropertyInfo.ParentTableColumnNames
                , CreateTempTableStatement = modelInfo.GetTableDataContractMetaInfoOfType(t.ChildType)
                        .GetCreatePrimaryKeyWithUniqueKeyTempTableDefinition(t.ChildTempTableNameWithParentName)
                , InsertStatement = 
                    modelInfo.GetTableDataContractMetaInfoOfType(t.ChildType)
                        .CreateInsertStatementOfPrimaryKeyWithUniqueKeyValuesJoiningWithOtherTable(
                            t.ChildTempTableNameWithParentName
                            ,t.EntitySetPropertyInfo.ChildTableColumnNames
                            ,t.ParentTempTableNameWithParentName
                            ,t.EntitySetPropertyInfo.ParentTableColumnNames
                         )
                , DeleteStatement =
                    modelInfo.GetTableDataContractMetaInfoOfType(t.ChildType)
                        .GetDeleteTSQLStatementBasedOnPrimaryKeyFromTempTable(t.ChildTempTableNameWithParentName)
                }).ToList();
            
            var rootTempTableNameWithoutBracket = $"#{modelInfo.GetTableDataContractMetaInfoOfType(type).TableDataContractMetaInfo.TableName}";
            var rootTempTableName =
                new TempTableVariable($"[#{modelInfo.GetTableDataContractMetaInfoOfType(type).TableDataContractMetaInfo.TableName}]");
            var rootTempTableCreationScript = modelInfo.GetTableDataContractMetaInfoOfType(type)
                .GetCreatePrimaryKeyTempTableDefinition(rootTempTableName);

            
            var insertIntoMainTempTableWithXMLArgValue = modelInfo
                .GetTableDataContractMetaInfoOfType(type)
                .GetInsertPrimaryKeyValuesFromXMLIntoTempTable(rootTempTableName, "@inputArg");
            
                    
            var allCreateTempTableStatement = string.Join("\r\n", list.Select(t => t.CreateTempTableStatement));
            var allInsertStatement= string.Join("\r\n", list.Select(t => t.InsertStatement));

            var allDeleteStatement = string.Join("\r\n", list.Select(t => t.DeleteStatement + ";").Reverse());

            var deleteStatementOfRootTable = $@"{
                    modelInfo.GetTableDataContractMetaInfoOfType(type)
                        .GetDeleteTSQLStatementBasedOnPrimaryKeyFromTempTable(rootTempTableNameWithoutBracket)
                }";

            var sql = $@"
                         {rootTempTableCreationScript}
                         {insertIntoMainTempTableWithXMLArgValue}
                         {allCreateTempTableStatement}
                         {allInsertStatement}
                         {allDeleteStatement}
                         {deleteStatementOfRootTable}   
                        ";

            return sql;
        }
    }
}
