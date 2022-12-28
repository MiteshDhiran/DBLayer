using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [DataContract]
    public class PrimaryIDLookupValueRecordInfo
    {
        public PrimaryIDLookupValueRecordInfo(string resolveSPName,
            List<PrimaryIDLookupValueRecordProperty> lookupRecordProperties, string lookupClassName)
        {
            if(string.IsNullOrEmpty(resolveSPName)) throw new ArgumentNullException($"{nameof(resolveSPName)}");
            if(lookupRecordProperties == null || lookupRecordProperties?.Any() == false) throw new ArgumentException($"{nameof(lookupRecordProperties)} is null ORMMetadata empty");
            if(string.IsNullOrEmpty(lookupClassName)) throw new ArgumentNullException($"{nameof(lookupClassName)}");
            
            ResolveSPName = resolveSPName ?? throw new ArgumentNullException(nameof(resolveSPName));
            LookupRecordProperties =
                lookupRecordProperties ?? throw new ArgumentNullException(nameof(lookupRecordProperties));
            LookupClassName = lookupClassName;
        }

        [DataMember] public readonly string ResolveSPName;
        [DataMember] public readonly List<PrimaryIDLookupValueRecordProperty> LookupRecordProperties;
        [DataMember] public readonly string LookupClassName;

        private List<PropertyTypeNameAndPropertyName> _propertyTypeAndPropertyNames;
        public List<PropertyTypeNameAndPropertyName> GetLookupPropertiesInfo()
        {
            if (_propertyTypeAndPropertyNames == null)
            {
                var simpleProperties = LookupRecordProperties
                    .Where(c => string.IsNullOrEmpty(c.ResolveTableName) == true)
                    .Select(c => new PropertyTypeNameAndPropertyName(c.NetDataType, c.PropertyName,false)).ToList();

                var complexProperties = LookupRecordProperties
                    .Where(c => string.IsNullOrEmpty(c.ResolveTableName) == false)
                    .Select(c =>
                        new PropertyTypeNameAndPropertyName($"{c.ResolveTableName}PrimaryRecordLookupResolveInfo",
                            c.PropertyName,true)).ToList();

                var additionalPropertyNeededForFurtherLookup = LookupRecordProperties
                    .Where(c => string.IsNullOrEmpty(c.ResolveTableName) == false)
                    .Select(c => new PropertyTypeNameAndPropertyName(c.NetDataType, c.AssociatedColumnName, false)).ToList();
                
                var allProperties = new List<PropertyTypeNameAndPropertyName>();
                allProperties.AddRange(simpleProperties);
                allProperties.AddRange(complexProperties);
                allProperties.AddRange(additionalPropertyNeededForFurtherLookup);
                if (ResolveSPName == "SXARCMResolveLookup")
                {
                    Debug.WriteLine($"Properties count {allProperties.Count}");
                }
                _propertyTypeAndPropertyNames = allProperties;
            }

            return _propertyTypeAndPropertyNames;
        }
        
        

        public static bool TryParse(string tableName, XElement element, out PrimaryIDLookupValueRecordInfo primaryIDLookupValueRecordInfo)
        {
            if (element == null)
            {
                primaryIDLookupValueRecordInfo = null;
                return false;
            }
            var spName = element.Attribute("ResolveSPName")?.Value;
            var lookupRecordProperties =
                PrimaryIDLookupValueRecordProperty.TryParse(
                    element.Descendants("Properties")?.FirstOrDefault()?.Descendants("Property")?.ToList(),
                    out var primaryIDLookupValueRecordProperties)
                    ? primaryIDLookupValueRecordProperties
                    : null;
            var lookupClassName = element.Attribute("LookupClassName")?.Value;
            lookupClassName = string.IsNullOrEmpty(lookupClassName) == false ? lookupClassName : $"{tableName}PrimaryRecordInfo";
            if (string.IsNullOrEmpty(spName) == false && lookupRecordProperties?.Count > 0)
            {
                primaryIDLookupValueRecordInfo = new PrimaryIDLookupValueRecordInfo(spName,lookupRecordProperties,lookupClassName);
                return true;
            }
            else
            {
                primaryIDLookupValueRecordInfo = null;
                return false;
            }
        }
    }

    [DataContract]
    public class PrimaryIDLookupValueRecordProperty
    {
        public PrimaryIDLookupValueRecordProperty(string propertyName,string netDataType, string associatedColumnName, string resolveTableName, string resolveTablePKColumnName)
        {
            if(string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException($"{nameof(propertyName)}");
            if ((string.IsNullOrEmpty(resolveTableName) == false &&
                 string.IsNullOrEmpty(resolveTablePKColumnName) == true) ||
                (string.IsNullOrEmpty(resolveTableName) == true &&
                 string.IsNullOrEmpty(resolveTablePKColumnName) == false))
            {
                throw new ArgumentException($"Only one of the argument: {nameof(resolveTableName)} {nameof(resolveTablePKColumnName)} is having value. When one argument is having value, other argument should also have value");
            }

            if (string.IsNullOrEmpty(associatedColumnName) == true)
            {
                throw new ArgumentNullException($"Argument {nameof(associatedColumnName)} is null or empty");
            }
            PropertyName = propertyName;
            NetDataType = netDataType;
            AssociatedColumnName = associatedColumnName;
            ResolveTableName = resolveTableName;
            ResolveTablePKColumnName = resolveTablePKColumnName;
        }

        [DataMember] public readonly string PropertyName;
        [DataMember] public readonly string NetDataType;
        [DataMember] public readonly string AssociatedColumnName;
        [DataMember] public readonly string ResolveTableName;
        [DataMember] public readonly string ResolveTablePKColumnName;

        public static bool TryParse(XElement element,
            out PrimaryIDLookupValueRecordProperty primaryIDLookupValueRecordProperty)
        {
            var propertyName = element.Attribute("Name")?.Value;
            var netDataType = element.Attribute("NetDataType")?.Value;
            var associatedColumnName = element.Attribute("AssociatedColumnName")?.Value;
            var resolveTableName = element.Attribute("ResolveTableName")?.Value;
            var resolveTablePKColumnName = element.Attribute("ResolveTablePKColumnName")?.Value;
            
            if (string.IsNullOrEmpty(propertyName) == false)
            {
                primaryIDLookupValueRecordProperty = new PrimaryIDLookupValueRecordProperty(propertyName,netDataType,associatedColumnName, resolveTableName,resolveTablePKColumnName);
                return true;
            }
            else
            {
                primaryIDLookupValueRecordProperty = null;
                return false;
            }
        }

        public static bool TryParse(List<XElement> element,
            out List<PrimaryIDLookupValueRecordProperty> primaryIDLookupValueRecordProperty)
        {
            var parsedPrimaryIDLookupValueRecordProperty = element.Select(x => TryParse(x, out var parsed) ? parsed : null).Where(c => c != null)?.ToList();
            if (element.Count == parsedPrimaryIDLookupValueRecordProperty.Count)
            {
                primaryIDLookupValueRecordProperty = parsedPrimaryIDLookupValueRecordProperty;
                return true;
            }
            else
            {
                primaryIDLookupValueRecordProperty = null;
                return false;
            }
        }
        
    }
}
