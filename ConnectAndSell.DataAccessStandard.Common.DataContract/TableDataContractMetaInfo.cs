

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
	/// <summary>
	/// TableDataContractMetaInfo class
	/// </summary>
	[DataContract]
	public class TableDataContractMetaInfo
	{
        /// <summary>
        /// The table name
        /// </summary>
        [DataMember] public readonly string TableName;
        /// <summary>
        /// The table class name space
        /// </summary>
        [DataMember] public readonly string TableClassNameSpace;
        /// <summary>
        /// The table class name
        /// </summary>
        [DataMember] public readonly string TableClassName;
        /// <summary>
        /// The table primary key columns
        /// </summary>
        [DataMember] public readonly List<string> TablePrimaryKeyColumns;
		/// <summary>
		///   The parent property meta information
		///   This Feature is depricated need to use ParentPropertyMetaInfoList
		/// </summary>
		[DataMember] public readonly ParentPropertyMetaInfo ParentPropertyMetaInfo;//Depricated need to use ParentPropertyMetaInfoList		
        /// <summary>
        /// The column properties list
        /// </summary>
        [DataMember] public readonly List<ColumnPropertyMetaInfo> ColumnPropertiesList;
        /// <summary>
        /// The child entity set property information list
        /// </summary>
        [DataMember] public readonly List<ChildPropertyMetaInfo> ChildEntitySetPropertyInfoList;
        /// <summary>
        /// The ignore columns list
        /// </summary>
        [DataMember] public readonly List<string> IgnoreColumnsList;
        /// <summary>
        /// The always database generated
        /// </summary>
        [DataMember] public readonly List<string> AlwaysDBGenerated;
        /// <summary>
        /// The non primary unique constraint
        /// </summary>
        [DataMember] public readonly Dictionary<string,List<string>> NonPrimaryUniqueConstraint;
        /// <summary>
        /// The table primary key with unique key columns
        /// </summary>
        [DataMember] public readonly List<string> TablePrimaryKeyWithUniqueKeyColumns;
        /// <summary>
        /// The resolve look ups
        /// </summary>
        [DataMember] public readonly ResolveLookUps ResolveLookUps;
        /// <summary>
        /// The primary identifier lookup value record information
        /// </summary>
        [DataMember] public readonly PrimaryIDLookupValueRecordInfo PrimaryIDLookupValueRecordInfo;
        /// <summary>
        /// The class name with resolved properties
        /// </summary>
        [DataMember] public readonly string ClassNameWithResolvedProperties;
        /// <summary>
        /// The domain name
        /// </summary>
        [DataMember] public readonly string DomainName;
        [DataMember]private ParentPropertyMetaInfo[] _parentPropertyMetaInfoList;//breaking change

        /// <summary>
        /// Gets the parent property meta information list.
        /// </summary>
        /// <value>
        /// The parent property meta information list.
        /// </value>
        public ParentPropertyMetaInfo[] ParentPropertyMetaInfoList
        {
			get => _parentPropertyMetaInfoList;
			private set => _parentPropertyMetaInfoList = value;
        }

		private List<ColumnPropertyMetaInfo> _primaryKeyColumnProperties;

        /// <summary>
        /// Gets the primary key column properties.
        /// </summary>
        /// <value>
        /// The primary key column properties.
        /// </value>
        public List<ColumnPropertyMetaInfo> PrimaryKeyColumnProperties
		{
			get
			{
				if (_primaryKeyColumnProperties == null)
				{
					_primaryKeyColumnProperties = ColumnPropertiesList.Where(c => TablePrimaryKeyColumns.Contains(c.ColumnName)).ToList();
					return _primaryKeyColumnProperties;
				}
				else
				{
					return _primaryKeyColumnProperties;
				}
			}
		}

		private List<string> _primaryKeyPropertiesHashCodeCSharpText;

		private List<string> PrimaryKeyPropertiesHashCodeCSharpText
		{
			get
			{
				if (_primaryKeyPropertiesHashCodeCSharpText == null)
				{
					var countOfPKs = PrimaryKeyColumnProperties.Count;
					_primaryKeyPropertiesHashCodeCSharpText = PrimaryKeyColumnProperties.Select((c, i) =>
					{
						string hashCodeComputeText;
						if (c.NetDataType != "int")
						{
							hashCodeComputeText = $"{c.ColumnPropertyName}.GetHashCode()";
							if (countOfPKs > 1)
							{
								hashCodeComputeText = $"({hashCodeComputeText} * 397 )";
							}
						}
						else
						{
							hashCodeComputeText = $"{c.ColumnPropertyName}";
							if (countOfPKs > 1 && i == 1)
							{
								hashCodeComputeText = $" ^ {hashCodeComputeText}";
							}
						}
						return hashCodeComputeText;
					}).ToList();
				}

				return _primaryKeyPropertiesHashCodeCSharpText;
			}
		}

        /// <summary>
        /// Gets the name of the resolve class name or class.
        /// </summary>
        /// <value>
        /// The name of the resolve class name or class.
        /// </value>
        public string ResolveClassNameOrClassName => string.IsNullOrEmpty(ClassNameWithResolvedProperties) == false
			? ClassNameWithResolvedProperties
			: TableName;

        /// <summary>
        /// Gets the name of the table primary key class.
        /// </summary>
        /// <value>
        /// The name of the table primary key class.
        /// </value>
        public string TablePrimaryKeyClassName => $"{TableClassName}PrimaryRecordInfo";

        /// <summary>
        /// Gets the table primary key arguments.
        /// </summary>
        /// <value>
        /// The table primary key arguments.
        /// </value>
        public List<string> TablePrimaryKeyArguments =>
			TablePrimaryKeyColumns.Select(c => c.ToLower(CultureInfo.InvariantCulture)).ToList();

        /// <summary>
        /// Gets the table primary key argument comma separated.
        /// </summary>
        /// <value>
        /// The table primary key argument comma separated.
        /// </value>
        public string TablePrimaryKeyArgumentCommaSeparated => string.Join(" , ", ColumnPropertiesList
			.Where(c => c.IsPKColumn)
			.Select(c => $"{c.NetDataType} {c.ColumnPropertyName.ToLower(CultureInfo.InvariantCulture)}" ).ToList());

        /// <summary>
        /// Gets the table primary key property assignment.
        /// </summary>
        /// <value>
        /// The table primary key property assignment.
        /// </value>
        public string TablePrimaryKeyPropertyAssignment => string.Join("\r\n",
			TablePrimaryKeyColumns.Zip(TablePrimaryKeyArguments, (i, o) => $"{i} = {o};"));

        /// <summary>
        /// Gets the primary column properties text.
        /// </summary>
        /// <value>
        /// The primary column properties text.
        /// </value>
        public string PrimaryColumnPropertiesText => string.Join("\r\n",ColumnPropertiesList.Where(c => c.IsPKColumn).Select(c => $"[DataMember]\r\n public {c.NetDataTypeForDataContract} {c.ColumnPropertyName} {{get;set;}}"  ).ToList()) ;

        /// <summary>
        /// Gets the full name of the table class.
        /// </summary>
        /// <value>
        /// The full name of the table class.
        /// </value>
        public string TableClassFullName => $"{TableClassNameSpace}.{TableName}";

        /// <summary>
        /// Gets the name of the table primary key lookup resolve class.
        /// </summary>
        /// <value>
        /// The name of the table primary key lookup resolve class.
        /// </value>
        public string TablePrimaryKeyLookupResolveClassName => $"{TableClassName}PrimaryRecordLookupResolveInfo";

        /// <summary>
        /// Gets the table primary key lookup further dependencies information.
        /// </summary>
        /// <value>
        /// The table primary key lookup further dependencies information.
        /// </value>
        public List<ResolvePropertyDependencyOnAnotherTableInfo> TablePrimaryKeyLookupFurtherDependenciesInfo =>
			PrimaryIDLookupValueRecordInfo?.LookupRecordProperties?.Where(c => string.IsNullOrEmpty(c.ResolveTableName) == false) .Select(c =>
				new ResolvePropertyDependencyOnAnotherTableInfo(c.PropertyName
					, c.ResolveTableName
					, c.ResolveTablePKColumnName.Split(',').ToArray()
					, c.AssociatedColumnName.Split(',').ToArray())).ToList();

        /// <summary>
        /// Gets the table primary lookup properties argument comma separated.
        /// </summary>
        /// <value>
        /// The table primary lookup properties argument comma separated.
        /// </value>
        public string TablePrimaryLookupPropertiesArgumentCommaSeparated => PrimaryIDLookupValueRecordInfo?.GetLookupPropertiesInfo()?.Any() == true 
			? string.Join(" , ", PrimaryIDLookupValueRecordInfo.GetLookupPropertiesInfo()
			.Select(c => $"{c.PropertyTypeName} {c.PropertyName.ToLower(CultureInfo.InvariantCulture)}" ).ToList()) : "";

        /// <summary>
        /// Gets the table primary key lookup resolve property arguments.
        /// </summary>
        /// <value>
        /// The table primary key lookup resolve property arguments.
        /// </value>
        public List<string> TablePrimaryKeyLookupResolvePropertyArguments =>
			PrimaryIDLookupValueRecordInfo?.GetLookupPropertiesInfo()
				?.Select(c => c.PropertyName.ToLower(CultureInfo.InvariantCulture))
				.ToList();

        /// <summary>
        /// Gets the table primary key lookup resolve properties assignment.
        /// </summary>
        /// <value>
        /// The table primary key lookup resolve properties assignment.
        /// </value>
        public string TablePrimaryKeyLookupResolvePropertiesAssignment =>
			PrimaryIDLookupValueRecordInfo?.GetLookupPropertiesInfo().Any() == true
			? string.Join("\r\n", PrimaryIDLookupValueRecordInfo.GetLookupPropertiesInfo().Select(c => c.PropertyName).ToList()
				.Zip(TablePrimaryKeyLookupResolvePropertyArguments, (i,o) =>$"{i} = {o};"))
			: string.Empty;

        /// <summary>
        /// Gets the table primary key lookup resolve properties definition text.
        /// </summary>
        /// <value>
        /// The table primary key lookup resolve properties definition text.
        /// </value>
        public string TablePrimaryKeyLookupResolvePropertiesDefinitionText =>
			PrimaryIDLookupValueRecordInfo?.GetLookupPropertiesInfo()?.Any() == true
				? string.Join("\r\n",
					PrimaryIDLookupValueRecordInfo.GetLookupPropertiesInfo().Select(c =>
						$"[DataMember]\r\npublic {c.PropertyTypeName} {c.PropertyName} {{get;set;}}"
						).ToList())
				: string.Empty;

        /// <summary>
        /// Gets a value indicating whether this instance is composite root table.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is composite root table; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompositeRootTable => ParentPropertyMetaInfo == null;

        /// <summary>
        /// The without lookup column properties
        /// </summary>
        public List<ColumnPropertyMetaInfo> WithoutLookupColumnProperties;

        /// <summary>
        /// Gets the primary key equality check string.
        /// </summary>
        /// <param name="otherObjectVariableName">Name of the other object variable.</param>
        /// <returns></returns>
        public string GetPrimaryKeyEqualityCheckString(string otherObjectVariableName)
		{
			return string.Join( " && ", TablePrimaryKeyColumns.Select(c => $"{c} == {otherObjectVariableName}.{c}").ToList());
		}

        /// <summary>
        /// Gets the hash code compute code text.
        /// </summary>
        /// <returns></returns>
        public string GetHashCodeComputeCodeText()
		{
			var computeText = $"{string.Join(" ", PrimaryKeyPropertiesHashCodeCSharpText)};";
			if (TablePrimaryKeyColumns.Count == 1)
			{
				return $"return {computeText};";
			}
			else
			{
				return $"unchecked\r\n{{\r\n return {computeText} ;\r\n}}";
			}
		}

        private List<ColumnPropertyMetaInfo> _lookupColumnsAlongWithPKColumnsList;

        /// <summary>
        /// Gets the lookup column properties with primary key properties.
        /// </summary>
        /// <returns></returns>
        public List<ColumnPropertyMetaInfo> GetLookupColumnPropertiesWithPrimaryKeyProperties()
		{
			if (_lookupColumnsAlongWithPKColumnsList == null)
			{
				var allLookupColumns = PrimaryIDLookupValueRecordInfo.LookupRecordProperties.Where(c => string.IsNullOrEmpty(c.AssociatedColumnName) == false)
					.Select(c => c.AssociatedColumnName).ToList();
				_lookupColumnsAlongWithPKColumnsList = ColumnPropertiesList.Where(c => c.IsPKColumn || allLookupColumns.Contains(c.ColumnName)).Select(c => c)
					.ToList();    
			}

			return _lookupColumnsAlongWithPKColumnsList;

		}

        /// <summary>
        /// Initializes a new instance of the <see cref="TableDataContractMetaInfo"/> class.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="tableClassNameSpaceName">Name of the table class name space.</param>
        /// <param name="tableClassName">Name of the table class.</param>
        /// <param name="tablePrimaryKeyColumns">The table primary key columns.</param>
        /// <param name="parentPropertyMetaInfo">The parent property meta information.</param>
        /// <param name="columnPropertiesList">The column properties list.</param>
        /// <param name="childEntitySetPropertyInfoList">The child entity set property information list.</param>
        /// <param name="ignoreColumnsList">The ignore columns list.</param>
        /// <param name="alwaysDBGenerated">The always database generated.</param>
        /// <param name="nonPrimaryUniqueConstraint">The non primary unique constraint.</param>
        /// <param name="primaryIDLookupValueRecordInfo">The primary identifier lookup value record information.</param>
        /// <param name="resolveLookUps">The resolve look ups.</param>
        /// <param name="classNameWithResolvedProperties">The class name with resolved properties.</param>
        public TableDataContractMetaInfo(string domainName,string tableName, string tableClassNameSpaceName, string tableClassName
			, List<string> tablePrimaryKeyColumns, ParentPropertyMetaInfo parentPropertyMetaInfo
			, List<ColumnPropertyMetaInfo> columnPropertiesList
			, List<ChildPropertyMetaInfo> childEntitySetPropertyInfoList, List<string> ignoreColumnsList, List<string> alwaysDBGenerated, Dictionary<string,List<string>> nonPrimaryUniqueConstraint, PrimaryIDLookupValueRecordInfo primaryIDLookupValueRecordInfo, ResolveLookUps resolveLookUps, string classNameWithResolvedProperties)
		{
			DomainName = domainName;
			this.TableName = tableName;
			this.TableClassNameSpace = tableClassNameSpaceName;
			this.TableClassName = tableClassName;
			this.TablePrimaryKeyColumns = tablePrimaryKeyColumns;
			this.ParentPropertyMetaInfo = parentPropertyMetaInfo;
			this.ColumnPropertiesList = columnPropertiesList ;
			this.IgnoreColumnsList = ignoreColumnsList ?? new List<string>();
            //IF alwaysDBGenerated column is not present in column list then it should be ignored
            var allColumnNames = ColumnPropertiesList.Select(c => c.ColumnName).ToList();
            this.AlwaysDBGenerated = alwaysDBGenerated?.Where(a => allColumnNames.Contains(a,StringComparer.InvariantCultureIgnoreCase))?.ToList()
                                     ?? new List<string>();
            
			this.ChildEntitySetPropertyInfoList = childEntitySetPropertyInfoList ?? new List<ChildPropertyMetaInfo>();
			this.NonPrimaryUniqueConstraint = nonPrimaryUniqueConstraint ?? new Dictionary<string, List<string>>();
			this.TablePrimaryKeyWithUniqueKeyColumns = TablePrimaryKeyColumns
				.Union(NonPrimaryUniqueConstraint.SelectMany(x => x.Value.Select(xx => xx))).Distinct().ToList();
			PrimaryIDLookupValueRecordInfo = primaryIDLookupValueRecordInfo;
			ResolveLookUps = resolveLookUps ;
			var resolveLookUpColumns = resolveLookUps?.ResolveLookups?.SelectMany(c => c.ResolvableLookupColumnNames.Select(cc => cc)).ToList();
			WithoutLookupColumnProperties = resolveLookUpColumns?.Any() == true
				? columnPropertiesList?.Where(c => resolveLookUpColumns.Contains(c.ColumnName) == false).ToList()
				: columnPropertiesList;
			ClassNameWithResolvedProperties = classNameWithResolvedProperties;

            if (parentPropertyMetaInfo != null)
            {
                ParentPropertyMetaInfoList = new ParentPropertyMetaInfo[] {parentPropertyMetaInfo};
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableDataContractMetaInfo"/> class.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="tableClassNameSpaceName">Name of the table class name space.</param>
        /// <param name="tableClassName">Name of the table class.</param>
        /// <param name="tablePrimaryKeyColumns">The table primary key columns.</param>
        /// <param name="parentPropertyMetaInfoList">The parent property meta information list.</param>
        /// <param name="columnPropertiesList">The column properties list.</param>
        /// <param name="childEntitySetPropertyInfoList">The child entity set property information list.</param>
        /// <param name="ignoreColumnsList">The ignore columns list.</param>
        /// <param name="alwaysDBGenerated">The always database generated.</param>
        /// <param name="nonPrimaryUniqueConstraint">The non primary unique constraint.</param>
        /// <param name="primaryIDLookupValueRecordInfo">The primary identifier lookup value record information.</param>
        /// <param name="resolveLookUps">The resolve look ups.</param>
        /// <param name="classNameWithResolvedProperties">The class name with resolved properties.</param>
        public TableDataContractMetaInfo(string domainName,string tableName, string tableClassNameSpaceName, string tableClassName
			, List<string> tablePrimaryKeyColumns, ParentPropertyMetaInfo[] parentPropertyMetaInfoList
			, List<ColumnPropertyMetaInfo> columnPropertiesList
			, List<ChildPropertyMetaInfo> childEntitySetPropertyInfoList, List<string> ignoreColumnsList, List<string> alwaysDBGenerated, Dictionary<string,List<string>> nonPrimaryUniqueConstraint, PrimaryIDLookupValueRecordInfo primaryIDLookupValueRecordInfo, ResolveLookUps resolveLookUps, string classNameWithResolvedProperties)
		{
			DomainName = domainName;
			this.TableName = tableName;
			this.TableClassNameSpace = tableClassNameSpaceName;
			this.TableClassName = tableClassName;
			this.TablePrimaryKeyColumns = tablePrimaryKeyColumns;
			this.ParentPropertyMetaInfo = parentPropertyMetaInfoList != null && parentPropertyMetaInfoList.Length > 0 ? parentPropertyMetaInfoList.First() : null;
			this.ColumnPropertiesList = columnPropertiesList ;
			this.IgnoreColumnsList = ignoreColumnsList ?? new List<string>();
			this.AlwaysDBGenerated = alwaysDBGenerated ?? new List<string>();
			this.ChildEntitySetPropertyInfoList = childEntitySetPropertyInfoList ?? new List<ChildPropertyMetaInfo>();
			this.NonPrimaryUniqueConstraint = nonPrimaryUniqueConstraint ?? new Dictionary<string, List<string>>();
			this.TablePrimaryKeyWithUniqueKeyColumns = TablePrimaryKeyColumns
				.Union(NonPrimaryUniqueConstraint.SelectMany(x => x.Value.Select(xx => xx))).Distinct().ToList();
			PrimaryIDLookupValueRecordInfo = primaryIDLookupValueRecordInfo;
			ResolveLookUps = resolveLookUps ;
			var resolveLookUpColumns = resolveLookUps?.ResolveLookups?.SelectMany(c => c.ResolvableLookupColumnNames.Select(cc => cc)).ToList();
			WithoutLookupColumnProperties = resolveLookUpColumns?.Any() == true
				? columnPropertiesList?.Where(c => resolveLookUpColumns.Contains(c.ColumnName) == false).ToList()
				: columnPropertiesList;
			ClassNameWithResolvedProperties = classNameWithResolvedProperties;
            ParentPropertyMetaInfoList = parentPropertyMetaInfoList;
        }

		[OnDeserialized()]
		void OnDeserializedMethod(StreamingContext context)
		{
			var resolveLookUpColumns = ResolveLookUps?.ResolveLookups?.SelectMany(c => c.ResolvableLookupColumnNames.Select(cc => cc)).ToList();
			WithoutLookupColumnProperties = resolveLookUpColumns?.Any() == true
				? ColumnPropertiesList.Where(c => resolveLookUpColumns.Contains(c.ColumnName) == false).ToList()
				: ColumnPropertiesList;
            if (ParentPropertyMetaInfo != null && (ParentPropertyMetaInfoList == null || ParentPropertyMetaInfoList.Length == 0 ))
            {
                ParentPropertyMetaInfoList = new ParentPropertyMetaInfo[]{ParentPropertyMetaInfo};
            }

            //For backward compatibility and issue with older ORMMetadata we need to add this - otherwise not needed
            if (AlwaysDBGenerated != null && AlwaysDBGenerated.Any())
            {
                var length = AlwaysDBGenerated.Count;
                var listOfAllColumns = ColumnPropertiesList.Select(c => c.ColumnName).ToList();
                for (var i = length - 1; i >= 0; i--)
                {
                    if (listOfAllColumns.Contains(AlwaysDBGenerated[i],StringComparer.InvariantCultureIgnoreCase) == false)
                    {
                        AlwaysDBGenerated.RemoveAt(i);
                    }
                }
            }

        }

	
	}
}

/* P r o p r i e t a r y  N o t i c e */
/*
Confidential and proprietary information of Allscripts Healthcare, LLC and/or its affiliates. Authorized users only.
Notice to U.S. Government Users: This software is "Commercial Computer Software." Subject to full notice set
forth herein.
*/
/* P r o p r i e t a r y  N o t i c e */