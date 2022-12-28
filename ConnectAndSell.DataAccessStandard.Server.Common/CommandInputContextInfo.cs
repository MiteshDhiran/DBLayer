using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public class CommandInputContextInfo
    {
        public string CommandName { get; private set; }
        public dynamic ParameterObject { get; private set; }
        public List<Type> ReturnRecordTypes { get; private set; }
        public IReadOnlyList<DBParameterArgInfo> InputArgs { get; private set; }
        public IReadOnlyList<DBParameterArgInfo> OutputArgs { get; private set; }

        public IReadOnlyDictionary<DBParameterArgInfo,object> InputArgumentValue { get; private set; }

        public CommandInputContextInfo(string commandName, dynamic parameterObject, List<Type> returnRecordTypes, IReadOnlyList<DBParameterArgInfo> inputArgs, IReadOnlyList<DBParameterArgInfo> outputArgs, Dictionary<DBParameterArgInfo, object> inputArgumentValue)
        {
            CommandName = commandName ?? throw new ArgumentNullException(nameof(commandName));
            ParameterObject = parameterObject;
            ReturnRecordTypes = returnRecordTypes;
            InputArgs = inputArgs ?? new List<DBParameterArgInfo>();
            OutputArgs = outputArgs ?? new List<DBParameterArgInfo>();
            InputArgumentValue = inputArgumentValue ?? new Dictionary<DBParameterArgInfo, object>();
        }
        
    }

    public class DBParameterArgInfo
    {
        public int ArgIndex { get; }
        public string ParameterName { get; }
        
        public string ParameterNameWithoutAtSign { get; }
        
        public int? Size { get; }
        public byte? Precision { get; }
        public byte? Scale { get; }

        public System.Data.SqlDbType? SqlDbType { get; }
        
        public System.Data.DbType? DbType { get; }
        
        public ParameterDirection ParameterDirection { get; }

        public bool IsIn { get; }
        public bool IsOut { get; }
        public bool IsRef { get; }
        public DBParameterArgInfo(bool isIn,bool isOut,bool isRef ,int argIndex, string parameterName, ParameterDirection parameterDirection, int? size, byte? precision, byte? scale,System.Data.SqlDbType? sqlDbType)
        {
            this.IsIn = isIn;
            this.IsOut = isOut;
            this.IsRef = isRef;
            ArgIndex = argIndex;
            ParameterName = parameterName;
            ParameterDirection = parameterDirection;
            Size = size;
            Precision = precision;
            Scale = scale;
            SqlDbType = sqlDbType;
            DbType = GetDbTypeFromSqlDbType(this.SqlDbType);
            ParameterNameWithoutAtSign = parameterName.Replace("@", "");
        }

        private DbType? GetDbTypeFromSqlDbType(SqlDbType? sqlDbType)
        {
            return sqlDbType.HasValue == false
                ? null
                : DBUtility.SqlDbTypeToDbType.TryGetValue(sqlDbType.Value, out var retVal)
                    ? retVal
                    : (DbType?) null;
        }
    }
}
