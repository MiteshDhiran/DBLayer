using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.Dal.Generator.Common
{
    public static class ParentChildConfigReaderHelper
    {
        public static List<ParentChildTableInfo> GetParentChildTableInfos(string parentChildConfigFilePath,
            string projectDataContractNameSpace)
        {
            var rawList = XElement.Load(parentChildConfigFilePath).Descendants("Table").Select(x => new { 
                 ParentName = x.Attribute("Name")?.Value
                ,IgnoreFields = x.Attribute("IgnoreFields")?.Value
                ,AlwaysDBGeneratedFields = x.Attribute("IgnoreFields")?.Value
                ,ChildrenInfo = x.Descendants("Table").Select(
                    c => new { ChildName = c.Attribute("Name")?.Value , 
                                FKName = c.Attribute("FKName")?.Value, 
                                ChildCollectionPropertyName = c.Attribute("ChildCollectionPropertyName")?.Value, 
                                ParentRefPropertyName = c.Attribute("ParentRefPropertyName")?.Value
                                //,IgnoreFields = c.Attribute("IgnoreFields")?.Value
                                //,AlwaysDBGeneratedFields = c.Attribute("IgnoreFields")?.Value
                                ,IsDirect = c.Parent == x
                             }).Where(c => c.IsDirect).ToList()
                ,PrimaryIDLookupValueRecordInfo = PrimaryIDLookupValueRecordInfo.TryParse(x.Attribute("Name")?.Value,
                                            x.Descendants("PrimaryIDLookupValueRecordInfo").FirstOrDefault()
                                            , out var primaryIDLookupRecordInfoElement) 
                    ?  primaryIDLookupRecordInfoElement 
                    : null   
                ,ResolveLookUp = ResolveLookUps.TryParseResolveLookUps(x.Descendants("ResolveLookups").FirstOrDefault(), out var resolveLookUps)? resolveLookUps : null
                ,ClassNameWithResolvedProperties = x.Attribute("ResolvedClassName")?.Value
                ,DomainName = RecursivelyTraverseForAttributeValue(x,"DomainName", string.Empty)
                ,ColumnExtraInfoList = x.Descendants("ColumnExtraInfoList").Descendants("ColumnExtraInfo").ToList().Select(d=> new ColumnExtraInfo(
                                        d.Attribute("ColumnName")?.Value,
                                        d.Attribute("EnumName")?.Value)).ToList()
                }).ToList();

            var finalResult = rawList.SelectMany(a => a.ChildrenInfo.Count == 0 ? new List<ParentChildTableInfo>()
            {
                new ParentChildTableInfo(projectDataContractNameSpace,a.DomainName ,a.ParentName,null ,null, null, null, null,a.IgnoreFields, a.AlwaysDBGeneratedFields,a.PrimaryIDLookupValueRecordInfo,a.ResolveLookUp, a.ClassNameWithResolvedProperties
                  ,a.ColumnExtraInfoList)
            } : a.ChildrenInfo.Select(ci => 
                new ParentChildTableInfo(projectDataContractNameSpace,a.DomainName ,a.ParentName, projectDataContractNameSpace, ci.ChildName, ci.FKName, ci.ChildCollectionPropertyName, ci.ParentRefPropertyName,a.IgnoreFields, a.AlwaysDBGeneratedFields, a.PrimaryIDLookupValueRecordInfo, a.ResolveLookUp, a.ClassNameWithResolvedProperties,a.ColumnExtraInfoList)).ToList()
                ).ToList();
            return finalResult;
        }

        public static string RecursivelyTraverseForAttributeValue(XElement e, string attributeName, string defaultValue)
        {
            while (true)
            {
                if (e == null) return defaultValue;
                var retVal = e.Attribute(attributeName)?.Value;
                if (string.IsNullOrEmpty(retVal) == false)
                {
                    return retVal;
                }
                if (e.Parent == null)
                {
                    return defaultValue;
                }
                e = e.Parent;
            }
        }

        public static List<AssociatedParentChildTableInfo> GetAssociatedParentChildTableInfos(
            string parentChildConfigFilePath, string projectDataContractNameSpace)
        {
            var rootAssociateTables =
                XElement.Load(parentChildConfigFilePath)
                    .Descendants("AssociateTable")
                    .Where(p => p.Descendants("AssociateTable").Any()
                    ).ToList();
            
            return rootAssociateTables.Select(x => new AssociatedParentChildTableInfo
            (        
                    RecursivelyTraverseForAttributeValue(x,"DomainName",string.Empty)
                    ,  projectDataContractNameSpace  
                    , x.Attribute("TableName")?.Value
                    , x.Attribute("ClassName")?.Value ?? x.Attribute("TableName")?.Value
                    , x.Descendants("AssociateTable").Select(
                        c => new AssociateChildInfo( c.Attribute("PropertyName")?.Value
                            ,c.Attribute("TableName")?.Value
                            ,c.Attribute("JOIN")?.Value
                            ,c.Attribute("FKName")?.Value
                            ,c.Attribute("InterTable")?.Value
                            ,c.Attribute("InterParentJoin")?.Value
                            ,c.Attribute("InterParentJoinFKName")?.Value
                            ,c.Attribute("InterChildJoin")?.Value
                            ,c.Attribute("InterChildJoinFKName")?.Value
                            ,c.Parent == x
                    )).Where(c => c.IsDirect).ToList()
                )).ToList();

            
        }
    }
}
