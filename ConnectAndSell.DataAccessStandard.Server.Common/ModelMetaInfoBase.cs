/*
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using MDRX.DataAccess.DataContract.Common;

namespace MDRX.DataAccessCore.Server.Common
{
    public abstract class ModelMetaInfoBase : IModelMetaInfo
    {
        private Func<Type,List<string>> GetAllChildPathsFunc { get; }
        private Func<Type,List<string>> GetImmediateChildPropertyNamesFunc { get; }

        private Func<Type,List<MDRXColumnInfo>> GetAllTableColumnPropertiesFunc { get; }
        private Func<Type,List<MDRXEntitySetPropertyInfo>> GetAllTableEntitySetPropertiesFunc { get; }
        
        private Func<Type,PropertyInfo> GetEntityRefPropertyInfoFunc { get; }
        
        private Func<Tuple<Type,Type>,PropertyInfo> GetChildPropertyInfoFunc { get; }
        
        private Func<Type, string> GetTableNameByTypeFunc { get; }
        
        private Func<Type, List<PropertyInfo>> GetPrimaryKeyPropertyInfoListFunc { get; }
        
        private Func<Type, string> GetTSQLByXMLForDataContractFunc { get; }
        
        private Func<Type,List<Tuple<Type,Type>>> GetNestedChildTableTypeFunc { get; }

        protected ModelMetaInfoBase()
        {
            GetAllChildPathsFunc = DBUtility.Memonize<Type,List<string>>((_, type) => GetAllChildPathsInternal(type));
            GetImmediateChildPropertyNamesFunc = DBUtility.Memonize<Type,List<string>>((_, type) => GetImmediateChildPropertyNamesInternal(type));
            GetAllTableColumnPropertiesFunc = DBUtility.Memonize<Type,List<MDRXColumnInfo>>((_, type) => GetAllTableColumnPropertiesInternal(type));
            GetAllTableEntitySetPropertiesFunc = DBUtility.Memonize<Type,List<MDRXEntitySetPropertyInfo>>((_, type) => GetAllTableEntitySetPropertiesInternal(type));
            GetEntityRefPropertyInfoFunc =
                DBUtility.Memonize<Type, PropertyInfo>((_, type) => GetEntityRefPropertyInfoInternal(type));
            GetChildPropertyInfoFunc =
                DBUtility.Memonize<Tuple<Type, Type>, PropertyInfo>((_, tuple) => GetChildPropertyInfoInternal(tuple.Item1, tuple.Item2));
            GetTableNameByTypeFunc = DBUtility.Memonize<Type, string>((_, type) => GetTableNameByTypeInternal(type));
            GetPrimaryKeyPropertyInfoListFunc =
                DBUtility.Memonize<Type, List<PropertyInfo>>((_, type) => GetPrimaryKeyPropertyInfoListInternal(type));
            GetTSQLByXMLForDataContractFunc = DBUtility.Memonize<Type, string>((_, type) => GetTSQLByXMLForDataContractInternal(type));
            GetNestedChildTableTypeFunc =
                DBUtility.Memonize<Type, List<Tuple<Type, Type>>>((_, type) => GetNestedChildTableTypeInternal(type));
        }

        protected abstract List<Tuple<Type, Type>> GetNestedChildTableTypeInternal(Type type);

        protected abstract string GetTSQLByXMLForDataContractInternal(Type type);


        protected abstract List<string> GetAllChildPathsInternal(Type type);
        protected abstract List<string> GetImmediateChildPropertyNamesInternal(Type type);

        protected abstract List<MDRXColumnInfo> GetAllTableColumnPropertiesInternal(Type type);

        protected abstract List<MDRXEntitySetPropertyInfo> GetAllTableEntitySetPropertiesInternal(Type type);

        protected abstract PropertyInfo GetEntityRefPropertyInfoInternal(Type type);

        protected abstract PropertyInfo GetChildPropertyInfoInternal(Type type, Type childType);

        protected abstract string GetTableNameByTypeInternal(Type type);

        protected abstract List<PropertyInfo> GetPrimaryKeyPropertyInfoListInternal(Type type);
        
        
        public List<string> GetAllChildPaths(Type type) => GetAllChildPathsFunc(type);

        public List<string> GetImmediateChildPropertyNames(Type type) => GetImmediateChildPropertyNamesFunc(type);


        public abstract ConcurrentDictionary<MDRXSystemDefinedColumn, string>
            GetSystemDefinedColumnTypeNameDictionary();


        public List<MDRXColumnInfo> GetAllTableColumnProperties(Type type) => GetAllTableColumnPropertiesFunc(type);


        public List<MDRXEntitySetPropertyInfo> GetAllTableEntitySetProperties(Type type) => GetAllTableEntitySetPropertiesFunc(type);

        public PropertyInfo GetEntityRefPropertyInfo(Type type) => GetEntityRefPropertyInfoFunc(type);


        public PropertyInfo GetChildPropertyInfo(Type type, Type childType) =>
            GetChildPropertyInfoFunc(new Tuple<Type, Type>(type, childType));

        public string GetTableNameByType(Type type) => GetTableNameByTypeFunc(type);


        public List<PropertyInfo> GetPrimaryKeyPropertyInfoList(Type type) => GetPrimaryKeyPropertyInfoListFunc(type);

        public string GetTSQLByXMLForDataContract(Type type) => GetTSQLByXMLForDataContractFunc(type);
        public List<Tuple<Type, Type>> GetNestedChildTableType(Type type) => GetNestedChildTableTypeFunc(type);

    }
}
*/