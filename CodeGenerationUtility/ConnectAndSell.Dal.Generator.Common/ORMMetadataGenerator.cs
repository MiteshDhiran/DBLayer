using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.Dal.Generator.Common
{
    public static class ORMMetadataGenerator
    {
        public static ORMMetadata GetORMMetadata(List<ParentChildTableInfo> parentChildInfo,
            List<TableDataContractMetaInfo> tableDataContractMetaInfo,
            List<AssociatedParentChildTableInfo> associateRootEntities, List<EnumInfo> enumInfoList)
        {
            var dicOfTableInfo = tableDataContractMetaInfo.ToLookup(t => new TypeName(t.TableClassNameSpace, t.TableClassName))
                .ToDictionary(kv => kv.Key, kvg => kvg.First());
            var distinctTableTypeNames = parentChildInfo.Select(t => new TypeName(t.ParentClassNamespace, t.ParentTableName)).Distinct().ToList();
            var distinctChildClassName = parentChildInfo.Where(t => t.ChildTableName != null)
                .Select(t => new TypeName(t.ChildClassNamespace, t.ChildTableName)).Distinct().ToList();
            var rootList = distinctTableTypeNames.Except(distinctChildClassName).ToList();
            var listOfRootTreeNodes = rootList.Select(r => GetTreeNode(dicOfTableInfo, r)).ToList();
            
            var retVal = new ORMMetadata(listOfRootTreeNodes,associateRootEntities,enumInfoList);
            return retVal;
        }

        
        public static ORMMetadata GetORMMetadata(DataContextGeneratorParams dataContextGeneratorParams)
        {
            var parentChildInputList = ParentChildConfigReaderHelper.GetParentChildTableInfos(dataContextGeneratorParams.ConfigFilePath,dataContextGeneratorParams.ProjectDataContractNameSpace);
            var tableDataContractMetaInfoList =  SQLMetadataProcessor.GetTableDataContractMetaInfo(dataContextGeneratorParams, parentChildInputList, dataContextGeneratorParams.ProjectDataContractNameSpace);
            var associateRootEntities = ParentChildConfigReaderHelper.GetAssociatedParentChildTableInfos(dataContextGeneratorParams.ConfigFilePath,
                dataContextGeneratorParams.ProjectDataContractNameSpace);
            return GetORMMetadata(parentChildInputList, tableDataContractMetaInfoList, associateRootEntities, GetEnumDataInfo(dataContextGeneratorParams.ConfigFilePath));
        }
        
        private static List<EnumInfo> GetEnumDataInfo(string parentChildConfigFilePath)
        {
            var rootEnums =
                XElement.Load(parentChildConfigFilePath)
                    .Descendants("Enum").Descendants("Enum").ToList();

            var res = rootEnums.Select(x => new EnumInfo(
                    x.Attribute("Name")?.Value,
                    x.Attribute("EnumType")?.Value,
                    x.Descendants("EnumMember").Select(
                        y => new EnumMemberInfo(
                            y.Attribute("Name")?.Value,
                            y.Attribute("Value")?.Value,
                            y.Parent == x
                        )).Where(y => y.IsDirect).ToList()))
                .ToList();

            return res;
        }
        private static TreeNode<TableDataContractMetaInfo> GetTreeNode(Dictionary<TypeName,TableDataContractMetaInfo> tableInfoDic ,  TypeName rootTypeName)
        {
            TreeNode<TableDataContractMetaInfo> GetTreeNodeInternal(TableDataContractMetaInfo currentContract, TreeNode<TableDataContractMetaInfo> treeNode, TreeNode<TableDataContractMetaInfo> currentNode)
            {
                if (treeNode == null)
                {
                    treeNode = new TreeNode<TableDataContractMetaInfo>(currentContract);
                    currentNode = treeNode;
                }
                else
                {
                    currentNode = currentNode.AddChild(currentContract);
                }
                foreach (var childPropertyMetaInfo in currentContract.ChildEntitySetPropertyInfoList)
                {
                    GetTreeNodeInternal(tableInfoDic[new TypeName(childPropertyMetaInfo.ChildClassNamespace, childPropertyMetaInfo.ChildClassName)], treeNode, currentNode);
                }

                return treeNode;
            }
            return GetTreeNodeInternal(tableInfoDic[rootTypeName],null, null);
        }
    }
}
