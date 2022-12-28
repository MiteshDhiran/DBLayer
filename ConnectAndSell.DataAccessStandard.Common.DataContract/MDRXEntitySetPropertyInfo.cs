

using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    /// <summary>
    /// MDRXEntitySetPropertyInfo class
    /// </summary>
    public class MDRXEntitySetPropertyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MDRXEntitySetPropertyInfo"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="entitySetTableType">Type of the entity set table.</param>
        /// <param name="entitySetTableName">Name of the entity set table.</param>
        /// <param name="parentTableName">Name of the parent table.</param>
        /// <param name="parentTableType">Type of the parent table.</param>
        /// <param name="parentTableColumnNames">The parent table column names.</param>
        /// <param name="childTableColumnNames">The child table column names.</param>
        public MDRXEntitySetPropertyInfo(IColumnNetInfo propertyInfo, string propertyName, Type entitySetTableType, string entitySetTableName, string parentTableName, Type parentTableType, List<string> parentTableColumnNames, List<string> childTableColumnNames)
        {
            PropertyInfo = propertyInfo;
            PropertyName = propertyName;
            EntitySetTableType = entitySetTableType;
            EntitySetTableName = entitySetTableName;
            ParentTableName = parentTableName;
            ParentTableType = parentTableType;
            ParentTableColumnNames = parentTableColumnNames;
            ChildTableColumnNames = childTableColumnNames;
        }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <value>
        /// The property information.
        /// </value>
        public IColumnNetInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get;}

        /// <summary>
        /// Gets the type of the entity set table.
        /// </summary>
        /// <value>
        /// The type of the entity set table.
        /// </value>
        public Type EntitySetTableType { get;}

        /// <summary>
        /// Gets the name of the entity set table.
        /// </summary>
        /// <value>
        /// The name of the entity set table.
        /// </value>
        public string EntitySetTableName { get;}

        /// <summary>
        /// Gets the name of the parent table.
        /// </summary>
        /// <value>
        /// The name of the parent table.
        /// </value>
        public string ParentTableName { get;}

        /// <summary>
        /// Gets the type of the parent table.
        /// </summary>
        /// <value>
        /// The type of the parent table.
        /// </value>
        public Type ParentTableType { get;}

        /// <summary>
        /// Gets or sets the parent table column names.
        /// </summary>
        /// <value>
        /// The parent table column names.
        /// </value>
        public List<string> ParentTableColumnNames { get; set; }

        /// <summary>
        /// Gets or sets the child table column names.
        /// </summary>
        /// <value>
        /// The child table column names.
        /// </value>
        public List<string> ChildTableColumnNames { get; set; }
        
        
    }
}

/* P r o p r i e t a r y  N o t i c e */
/*
Confidential and proprietary information of Allscripts Healthcare, LLC and/or its affiliates. Authorized users only.
Notice to U.S. Government Users: This software is "Commercial Computer Software." Subject to full notice set
forth herein.
*/
/* P r o p r i e t a r y  N o t i c e */