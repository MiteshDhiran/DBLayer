using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public class SProcMethodInfo
    {
        internal readonly string methodName;
        public readonly SPParameterInfo spParamters;
        internal List<Type> ReturnRecordTypeList;
        
        public IReadOnlyList<DBParameterArgInfo> InputArgs { get; private set; }
        public IReadOnlyList<DBParameterArgInfo> OutputArgs { get; private set; }
        public DBParameterArgInfo[] AllParameterInfoArray;

        internal SProcMethodInfo(MethodInfo methodInfo, string methodName)
        {
            var inputArgsLocal = new List<DBParameterArgInfo>();
            var outputArgsLocal = new List<DBParameterArgInfo>();
            this.methodName = methodName;
            this.spParamters = new SPParameterInfo
            {
                paraInfo = methodInfo.GetParameters()
            };
            var list = new List<ParameterAttribute>();
            for (int index = 0; index < this.spParamters.paraInfo.Length; ++index)
            {
                object[] customAttributes = this.spParamters.paraInfo[index].GetCustomAttributes(typeof(ParameterAttribute), true);
                if (customAttributes != null && Enumerable.Count<object>((IEnumerable<object>)customAttributes) > 0)
                {
                    var parameterAttribute = (ParameterAttribute)customAttributes[0];
                    list.Add(parameterAttribute);
                }
            }
            this.spParamters.paraAttrs = list.ToArray();
            
            ReturnRecordTypeList = Enumerable.ToList<Type>(Enumerable.Select<ResultTypeAttribute, Type>(
                (IEnumerable<ResultTypeAttribute>) Attribute.GetCustomAttributes((MemberInfo) methodInfo,
                    typeof(ResultTypeAttribute)), (Func<ResultTypeAttribute, Type>) (d => d.Type)));
            
            for (int index = 0; index < list.Count; ++index)
            {
                ParameterAttribute parameterAttribute = list[index];

                if (parameterAttribute != null)
                {
                    int? parameterSize = null;
                    byte? parameterPrecision = null;
                    byte? parameterScale = null;
                    SqlDbType? sqlParameterDbType = null;
                    
                    var parameterName = parameterAttribute.Name;
                    if (!this.spParamters.paraInfo[index].IsIn &&
                        parameterAttribute.DbType.IndexOf("(", StringComparison.Ordinal) > 0)
                    {
                        if (parameterAttribute.DbType.IndexOf(",", StringComparison.Ordinal) == -1)
                        {
                            parameterSize = Convert.ToInt32(parameterAttribute.DbType.Substring(
                                parameterAttribute.DbType.IndexOf("(", StringComparison.Ordinal) + 1,
                                parameterAttribute.DbType.IndexOf(")") - 1 - parameterAttribute.DbType.IndexOf("(")));
                        }
                        else
                        {
                            parameterPrecision = Convert.ToByte(parameterAttribute.DbType.Substring(
                                parameterAttribute.DbType.IndexOf("(", StringComparison.Ordinal) + 1,
                                parameterAttribute.DbType.IndexOf(",", StringComparison.Ordinal) - 1 -
                                parameterAttribute.DbType.IndexOf("(")));
                            parameterScale = Convert.ToByte(parameterAttribute.DbType.Substring(
                                parameterAttribute.DbType.IndexOf(",", StringComparison.Ordinal) + 1,
                                parameterAttribute.DbType.IndexOf(")", StringComparison.Ordinal) - 1 -
                                parameterAttribute.DbType.IndexOf(",")));
                        }
                    }

                    sqlParameterDbType = GetSqlParameterDbType(parameterAttribute.DbType);

                    if (this.spParamters.paraInfo[index].IsOut == true)
                    {
                        outputArgsLocal.Add(new DBParameterArgInfo(this.spParamters.paraInfo[index].IsIn,this.spParamters.paraInfo[index].IsOut,this.spParamters.paraInfo[index].ParameterType.IsByRef, index,parameterName,ParameterDirection.Output,parameterSize,parameterPrecision,parameterScale,sqlParameterDbType));
                    }
                    else
                    {
                        inputArgsLocal.Add(new DBParameterArgInfo(this.spParamters.paraInfo[index].IsIn,this.spParamters.paraInfo[index].IsOut,this.spParamters.paraInfo[index].ParameterType.IsByRef, index,parameterName,ParameterDirection.Input,parameterSize,parameterPrecision,parameterScale,sqlParameterDbType));
                    }
                }
            }

            this.InputArgs = inputArgsLocal;
            this.OutputArgs = outputArgsLocal;
            var allParameters = new List<DBParameterArgInfo>();
            allParameters.AddRange(inputArgsLocal);
            allParameters.AddRange(outputArgsLocal);
            this.AllParameterInfoArray = allParameters.ToArray();
        }
        
        private static SqlDbType? GetSqlParameterDbType(string paraType)
        {
            string str2 = paraType.IndexOf("(", StringComparison.Ordinal) <= 0
                ? paraType.Trim()
                : paraType.Substring(0, paraType.IndexOf("(", StringComparison.InvariantCultureIgnoreCase)).Trim();
            if (!DBUtility.SqlTypemapper.ContainsKey(str2.ToLower()))
                return null;
            return DBUtility.SqlTypemapper[str2.ToLower()];
        }

        private static readonly ConcurrentDictionary<string,SProcMethodInfo> ProcMethodInfoCacheDic = new ConcurrentDictionary<string,SProcMethodInfo>();

        public static SProcMethodInfo GetSPMethodInfo(MethodInfo mInfo,string spName)
        {
            SProcMethodInfo spMethodInfo;
            if (ProcMethodInfoCacheDic.TryGetValue(spName,out spMethodInfo) == false)
            {
                spMethodInfo = new SProcMethodInfo(mInfo,spName);
                ProcMethodInfoCacheDic.TryAdd(spName,spMethodInfo);
            }
            return spMethodInfo;
        }
    }
}
