

using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    /// <summary>
    /// RootRecordInfo
    /// </summary>
    public class RootRecordInfo
    {
        //public AuditArgInfo AuditArg { get; }        
        /// <summary>
        /// Gets the type of the root class.
        /// </summary>
        /// <value>
        /// The type of the root class.
        /// </value>
        public Type RootClassType { get; }

        /// <summary>
        /// Gets the name of the root table.
        /// </summary>
        /// <value>
        /// The name of the root table.
        /// </value>
        public string RootTableName { get; }

        /// <summary>
        /// Gets the root object.
        /// </summary>
        /// <value>
        /// The root object.
        /// </value>
        public object RootObject { get; }

        //public SFMEntityState RootState { get; private set; }

        /// <summary>
        /// Gets the primary key property infos.
        /// </summary>
        /// <value>
        /// The primary key property infos.
        /// </value>
        public List<IColumnNetInfo> PrimaryKeyPropertyInfos { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootRecordInfo"/> class.
        /// </summary>
        /// <param name="rootClassType">Type of the root class.</param>
        /// <param name="rootTableName">Name of the root table.</param>
        /// <param name="primaryKeyPropertyInfos">The primary key property infos.</param>
        /// <param name="rootObject">The root object.</param>
        /// <exception cref="ArgumentNullException">
        /// rootClassType
        /// or
        /// rootTableName
        /// or
        /// rootObject
        /// or
        /// primaryKeyPropertyInfos
        /// </exception>
        public RootRecordInfo(Type rootClassType, string rootTableName,List<IColumnNetInfo> primaryKeyPropertyInfos, object rootObject)
        {
            //AuditArg = auditArg ?? throw new ArgumentNullException(nameof(auditArg));
            RootClassType = rootClassType ?? throw new ArgumentNullException(nameof(rootClassType));
            RootTableName = rootTableName ?? throw new ArgumentNullException(nameof(rootTableName));
            RootObject = rootObject ?? throw new ArgumentNullException(nameof(rootObject));
            PrimaryKeyPropertyInfos = primaryKeyPropertyInfos ?? throw new ArgumentNullException(nameof(primaryKeyPropertyInfos));
        }
    }

    /// <summary>
    /// ParentRecordInfo
    /// </summary>
    public class ParentRecordInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParentRecordInfo"/> class.
        /// </summary>
        /// <param name="parentClassType">Type of the parent class.</param>
        /// <param name="parentClassName">Name of the parent class.</param>
        /// <param name="parentTableName">Name of the parent table.</param>
        /// <param name="parentPropertyNamePointingToChild">The parent property name pointing to child.</param>
        /// <param name="primaryKeyPropertyInfos">The primary key property infos.</param>
        /// <param name="parentObject">The parent object.</param>
        public ParentRecordInfo(Type parentClassType, string parentClassName, string parentTableName, string parentPropertyNamePointingToChild,List<IColumnNetInfo> primaryKeyPropertyInfos, object parentObject)
        {
            ParentClassType = parentClassType;
            ParentClassName = parentClassName;
            ParentTableName = parentTableName;
            ParentPropertyNamePointingToChild = parentPropertyNamePointingToChild;
            PrimaryKeyPropertyInfos = primaryKeyPropertyInfos;
            ParentObject = parentObject;
        }

        /// <summary>
        /// Gets the type of the parent class.
        /// </summary>
        /// <value>
        /// The type of the parent class.
        /// </value>
        public Type ParentClassType { get; }

        /// <summary>
        /// Gets the name of the parent class.
        /// </summary>
        /// <value>
        /// The name of the parent class.
        /// </value>
        public string ParentClassName { get; }

        /// <summary>
        /// Gets the name of the parent table.
        /// </summary>
        /// <value>
        /// The name of the parent table.
        /// </value>
        public string ParentTableName { get; }

        /// <summary>
        /// Gets the parent property name pointing to child.
        /// </summary>
        /// <value>
        /// The parent property name pointing to child.
        /// </value>
        public string ParentPropertyNamePointingToChild { get; }

        /// <summary>
        /// Gets or sets the primary key property infos.
        /// </summary>
        /// <value>
        /// The primary key property infos.
        /// </value>
        public List<IColumnNetInfo> PrimaryKeyPropertyInfos { get; set; }

        /// <summary>
        /// Gets the parent object.
        /// </summary>
        /// <value>
        /// The parent object.
        /// </value>
        public object ParentObject { get; }
        
    }

    /// <summary>
    /// ChangeTrackingInfo
    /// </summary>
    public class ChangeTrackingInfo
    {
        /// <summary>
        /// Gets the root record information.
        /// </summary>
        /// <value>
        /// The root record information.
        /// </value>
        public RootRecordInfo RootRecordInfo { get; }

        /// <summary>
        /// Gets the type of the current class.
        /// </summary>
        /// <value>
        /// The type of the current class.
        /// </value>
        public Type CurrentClassType { get; }

        /// <summary>
        /// Gets the name of the current table.
        /// </summary>
        /// <value>
        /// The name of the current table.
        /// </value>
        public string CurrentTableName { get; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the old value.
        /// </summary>
        /// <value>
        /// The old value.
        /// </value>
        public object OldValue { get; }

        /// <summary>
        /// Creates new value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        public object NewValue { get; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public DomainEntityState State { get; }

        /// <summary>
        /// Gets the current entity.
        /// </summary>
        /// <value>
        /// The current entity.
        /// </value>
        public object CurrentEntity { get; }

        /// <summary>
        /// Gets all parent information.
        /// </summary>
        /// <value>
        /// All parent information.
        /// </value>
        public Queue<ParentRecordInfo> AllParentInfo { get; }

        /// <summary>
        /// Gets the primary property information list.
        /// </summary>
        /// <value>
        /// The primary property information list.
        /// </value>
        public List<IColumnNetInfo> PrimaryPropertyInfoList { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeTrackingInfo"/> class.
        /// </summary>
        /// <param name="rootRecordInfo">The root record information.</param>
        /// <param name="allParentPath">All parent path.</param>
        /// <param name="currentEntity">The current entity.</param>
        /// <param name="primaryPropertyInfoList">The primary property information list.</param>
        /// <param name="state">The state.</param>
        /// <param name="currentClassType">Type of the current class.</param>
        /// <param name="currentTableName">Name of the current table.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <exception cref="ArgumentNullException">
        /// rootRecordInfo
        /// or
        /// currentEntity
        /// or
        /// currentClassType
        /// or
        /// currentTableName
        /// or
        /// PropertyName
        /// </exception>
        public ChangeTrackingInfo(RootRecordInfo rootRecordInfo,Queue<ParentRecordInfo> allParentPath, object currentEntity,List<IColumnNetInfo> primaryPropertyInfoList , DomainEntityState state, Type currentClassType, string currentTableName, string propertyName, object oldValue, object newValue)
        {
            RootRecordInfo = rootRecordInfo ?? throw new ArgumentNullException(nameof(rootRecordInfo));
            AllParentInfo = allParentPath;
            CurrentEntity = currentEntity ?? throw new ArgumentNullException(nameof(currentEntity));
            PrimaryPropertyInfoList = primaryPropertyInfoList;
            State = state;
            CurrentClassType = currentClassType ?? throw new ArgumentNullException(nameof(currentClassType));
            CurrentTableName = currentTableName ?? throw new ArgumentNullException(nameof(currentTableName));
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(PropertyName));
            OldValue = oldValue;
            NewValue = newValue;
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