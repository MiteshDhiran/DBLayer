

using System;
using System.Reflection;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{

    /// <summary>
    ///  EntityColumnInfo class
    /// </summary>
    public class EntityColumnInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityColumnInfo"/> class.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyClrType">Type of the property color.</param>
        /// <param name="dbType">Type of the database.</param>
        /// <param name="isDbGeneratedOnUpdate">if set to <c>true</c> [is database generated on update].</param>
        /// <param name="isDbGeneratedOnAdd">if set to <c>true</c> [is database generated on add].</param>
        /// <param name="isPrimaryKey">if set to <c>true</c> [is primary key].</param>
        /// <exception cref="ArgumentNullException">
        /// columnName
        /// or
        /// propertyInfo
        /// or
        /// propertyName
        /// or
        /// propertyClrType
        /// or
        /// dbType
        /// </exception>
        public EntityColumnInfo(string columnName, IColumnNetInfo propertyInfo, string propertyName, Type propertyClrType,
            string dbType, bool isDbGeneratedOnUpdate, bool isDbGeneratedOnAdd, bool isPrimaryKey)
        {
            ColumnName = columnName ?? throw new ArgumentNullException(nameof(columnName));
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            PropertyCLRType = propertyClrType ?? throw new ArgumentNullException(nameof(propertyClrType));
            DbType = dbType ?? throw new ArgumentNullException(nameof(dbType));
            IsDbGeneratedOnUpdate = isDbGeneratedOnUpdate;
            IsDbGeneratedOnAdd = isDbGeneratedOnAdd;
            IsPrimaryKey = isPrimaryKey;
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <value>
        /// The name of the column.
        /// </value>
        public string ColumnName { get; }

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
        public string PropertyName { get; }

        /// <summary>
        /// Gets the type of the property color.
        /// </summary>
        /// <value>
        /// The type of the property color.
        /// </value>
        public Type PropertyCLRType { get; }

        /// <summary>
        /// Gets the type of the database.
        /// </summary>
        /// <value>
        /// The type of the database.
        /// </value>
        public string DbType { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is database generated on update.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is database generated on update; otherwise, <c>false</c>.
        /// </value>
        public bool IsDbGeneratedOnUpdate { get;  }

        /// <summary>
        /// Gets a value indicating whether this instance is database generated on add.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is database generated on add; otherwise, <c>false</c>.
        /// </value>
        public bool IsDbGeneratedOnAdd { get;  }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is primary key; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimaryKey { get; set; }
        
    }
}
/* P r o p r i e t a r y  N o t i c e */
/*
Confidential and proprietary information of Allscripts Healthcare, LLC and/or its affiliates. Authorized users only.
Notice to U.S. Government Users: This software is "Commercial Computer Software." Subject to full notice set
forth herein.
*/
/* P r o p r i e t a r y  N o t i c e */