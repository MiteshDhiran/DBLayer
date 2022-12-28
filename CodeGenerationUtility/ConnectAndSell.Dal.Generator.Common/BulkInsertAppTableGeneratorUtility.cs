using System.Linq;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.Dal.Generator.Common
{
    public static class BulkInsertAppTableGeneratorUtility
    {
        public static BulkInsertAppTableGeneratorInfo GetBulkInsertAppTableGeneratorInfo(TableDataContractMetaInfo tableDataContractMetaInfo)
        {
            var columnDefinitionList =  tableDataContractMetaInfo.ColumnPropertiesList.Select(c =>
                $"[{c.ColumnName}] {c.SqlDataTypeForTableType} {GetMaxLengthContraint(c.StringMaxLength)} NULL").ToList();
            columnDefinitionList.Add("[RID] [bigint] NULL");
            columnDefinitionList.Add("[PRID] [bigint] NULL");
            var columnDefinitionText = string.Join("\r\n,", columnDefinitionList);
            var retVal = new BulkInsertAppTableGeneratorInfo($"{tableDataContractMetaInfo.TableName}AppDtTbl",columnDefinitionText);
            return retVal;
        }

        private static string GetMaxLengthContraint(int? argStringMaxLength) => argStringMaxLength.HasValue ? $"({argStringMaxLength.Value})" : "";
    }
}