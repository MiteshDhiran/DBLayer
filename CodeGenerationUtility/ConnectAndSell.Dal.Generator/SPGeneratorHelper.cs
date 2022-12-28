using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using ConnectAndSell.Dal.Generator.Common;

namespace ConnectAndSell.Dal.Generator
{
    public static class SPGeneratorHelper
    {
        public static StringBuilder GenereateCode(SPMetaInfoForCodeGeneration sp)
        {
            var mainSPStringBuilder = new StringBuilder();
            GenerateAttributes(sp,mainSPStringBuilder);
            var paramInfo = GenerateMethodSignature(sp,mainSPStringBuilder);
            mainSPStringBuilder.AppendLine("\t\t{");
            mainSPStringBuilder.AppendLine("\t\t\tSystem.Reflection.MethodInfo mInfo = (System.Reflection.MethodInfo)System.Reflection.MethodInfo.GetCurrentMethod();");
            mainSPStringBuilder.AppendLine("\t\t\t// Call DAL Method and pass specific methodInfo and get ResultSet");
            if (paramInfo.ParametersString.Length > 0)
            {
                if (sp.ResultSetTypes != null && sp.ResultSetTypes.Count == 1)
                    mainSPStringBuilder.AppendLine($"\t\t\tResultSet retVal = {sp.DALHelperName}.ExecuteStoredProcedure<{sp.ResultSetTypes[0].ClassName}>(mInfo, {paramInfo.ParametersString});");
                else
                    mainSPStringBuilder.AppendLine($"\t\t\tResultSet retVal = {sp.DALHelperName}.ExecuteStoredProcedure(mInfo, {paramInfo.ParametersString});");
            }
            else if (sp.ResultSetTypes != null && sp.ResultSetTypes.Count == 1)
                mainSPStringBuilder.AppendLine($"\t\t\tResultSet retVal = {sp.DALHelperName}.ExecuteStoredProcedure<{sp.ResultSetTypes[0].ClassName}>(mInfo);" );
            else
                mainSPStringBuilder.AppendLine("\t\t\tResultSet retVal = {sp.DALHelperName}.ExecuteStoredProcedure(mInfo);");
            if (paramInfo.RefParameters != null && paramInfo.RefParameters.Count > 0)
            {
                mainSPStringBuilder.AppendLine("\t\t\tif (retVal != null)");
                mainSPStringBuilder.AppendLine("\t\t\t{");
                int index = 0;
                foreach (string refParameter in paramInfo.RefParameters)
                {
                    mainSPStringBuilder.AppendLine("\t\t\t\t// Assign Output or ref parameters");
                    if (paramInfo.RefDBParameters[index].Contains("NULLVALUE"))
                    {
                        paramInfo.RefDBParameters[index] = paramInfo.RefDBParameters[index].Remove(paramInfo.RefDBParameters[index].IndexOf("NULLVALUE"));
                        mainSPStringBuilder.AppendLine($"\t\t\t\t{paramInfo.RefParametersType[index]}temp{index};");
                        mainSPStringBuilder.AppendLine($"\t\t\t\tif (retVal.GetOutputPara(\"{paramInfo.RefDBParameters[index]}\") != null && {paramInfo.RefParametersType[index]}.TryParse(retVal.GetOutputPara(\"{paramInfo.RefDBParameters[index]}\").ToString(), out temp{index}))");
                        mainSPStringBuilder.AppendLine($"\t\t\t\t\t{refParameter} = temp{index};");
                    }
                    else if (paramInfo.RefDBParameters[index].Contains("NULLREF"))
                    {
                        paramInfo.RefDBParameters[index] = paramInfo.RefDBParameters[index].Remove(paramInfo.RefDBParameters[index].IndexOf("NULLREF"));
                        mainSPStringBuilder.AppendLine($"\t\t\t\tif (retVal.GetOutputPara(\"{paramInfo.RefDBParameters[index]}\") != null && retVal.GetOutputPara(\"{paramInfo.RefDBParameters[index]}\") != System.DBNull.Value)");
                        mainSPStringBuilder.AppendLine($"\t\t\t\t\t{refParameter} = ({paramInfo.RefParametersType[index]})retVal.GetOutputPara(\"{paramInfo.RefDBParameters[index]}\");");
                    }
                    else
                        mainSPStringBuilder.AppendLine($"\t\t\t\t{refParameter} = ({paramInfo.RefParametersType[index]})retVal.GetOutputPara(\"{paramInfo.RefDBParameters[index]}\");");
                    ++index;
                }
                mainSPStringBuilder.AppendLine("\t\t\t}");
            }
            mainSPStringBuilder.AppendLine("\t\t\t// return IMultipleResults");
            mainSPStringBuilder.AppendLine("\t\t\treturn retVal;");
            mainSPStringBuilder.AppendLine("\t\t}");
            return mainSPStringBuilder;
        }
        
        private static void GenerateAttributes(SPMetaInfoForCodeGeneration sp,StringBuilder writer)
        {
            if (sp.ResultSetTypes != null && sp.ResultSetTypes.Count > 1)
            {
                foreach (var resultSetType in sp.ResultSetTypes)
                {
                    writer.AppendLine($"\t\t[ResultType(typeof({resultSetType.Namespace + "." + resultSetType.ClassName}))]");
                }
            }
            writer.AppendLine("\t\t" + $"[Function(Name = \"{sp.SPName}\")]");
        }

        private static SPGeneratorParameterInfo GenerateMethodSignature(SPMetaInfoForCodeGeneration sp, StringBuilder writer)
        {
            var spGeneratorParamterInfo = new SPGeneratorParameterInfo();
            foreach (DataRow row in (InternalDataCollectionBase)sp.StoredProcInfo.Rows)
            {
                if (!string.IsNullOrEmpty(row["paramName"].ToString()))
                {
                    var flag = TypeMapper.DoesTypeRequiresSize(row["dbtype"].ToString());
                    var dbType = row["dbtype"].ToString();
                    var empty = string.Empty;
                    string str;
                    if (flag)
                    {
                        if (dbType == "numeric" || dbType == "decimal")
                            str =
                                $"[Parameter(Name=\"{row["paramName"]}\", DbType=\"{(object)TypeMapper.GetTypeMap(dbType).LINQAttributeType}({(object)row["precision"].ToString()},{(object)row["scale"].ToString()})\")] ";
                        else
                            str =
                                $"[Parameter(Name=\"{row["paramName"]}\", DbType=\"{(object)TypeMapper.GetTypeMap(dbType).LINQAttributeType}({(object)row["length"].ToString()})\")] ";
                    }
                    else
                        str =
                            $"[Parameter(Name=\"{row["paramName"]}\", DbType=\"{(object)TypeMapper.GetTypeMap(dbType).LINQAttributeType}\")] ";
                    spGeneratorParamterInfo.MethodStringBuilder.Append(str);
                    var camelCasing = TypeMapper.ConvertToCamelCasing(TrimAtSynmbol(row["paramName"].ToString()));
                    if (row["isoutparam"].ToString() == "1")
                    {
                        spGeneratorParamterInfo.MethodStringBuilder.Append(" ref ");
                        spGeneratorParamterInfo.RefParameters.Add(camelCasing);
                        if (row["isnullable"].ToString() == "1")
                        {
                            if (TypeMapper.IsValueType(TypeMapper.GetTypeMap(row["dbtype"].ToString()).CStype))
                                spGeneratorParamterInfo.RefDBParameters.Add(row["paramName"] + "NULLVALUE");
                            else
                                spGeneratorParamterInfo.RefDBParameters.Add(row["paramName"] + "NULLREF");
                        }
                        else
                            spGeneratorParamterInfo.RefDBParameters.Add(row["paramName"].ToString());
                        spGeneratorParamterInfo.RefParametersType.Add(TypeMapper.GetTypeMap(row["dbtype"].ToString()).CStype);
                    }
                    if (row["isnullable"].ToString() == "1" && TypeMapper.IsValueType(TypeMapper.GetTypeMap(row["dbtype"].ToString()).CStype))
                        spGeneratorParamterInfo.MethodStringBuilder.AppendFormat(" System.Nullable<{0}> {1},", TypeMapper.GetTypeMap(row["dbtype"].ToString()).CStype, camelCasing);
                    else
                        spGeneratorParamterInfo.MethodStringBuilder.AppendFormat(" {0} {1},", TypeMapper.GetTypeMap(row["dbtype"].ToString()).CStype, camelCasing);
                    spGeneratorParamterInfo.ParametersString.Append(camelCasing + ",");
                }
            }
            if (spGeneratorParamterInfo.MethodStringBuilder.Length > 0)
            {
                spGeneratorParamterInfo.MethodStringBuilder.Remove(spGeneratorParamterInfo.MethodStringBuilder.Length - 1, 1);
                spGeneratorParamterInfo.ParametersString.Remove(spGeneratorParamterInfo.ParametersString.Length - 1, 1);
            }
            var strArray = sp.SPName.Split('.');
            writer.AppendLine("\t\t" +
                             $"public static ConnectAndSell.DataAccessCore.Server.Common.ResultSet {(object)strArray[strArray.Length - 1]} ({spGeneratorParamterInfo.MethodStringBuilder})");
            return spGeneratorParamterInfo;
        }
        public static string TrimAtSynmbol(string literal)
        {
            if (!literal.StartsWith("@"))
                return literal;
            return literal.TrimStart('@');
        }
    }

    internal class SPGeneratorParameterInfo
    {
        internal StringBuilder MethodStringBuilder { get; }
        internal List<string> RefParameters { get; } //var refParameters = new List<string>();
        internal List<string> RefDBParameters { get; }
        
        internal List<string> RefParametersType { get; }
        internal StringBuilder ParametersString { get; }

        internal SPGeneratorParameterInfo()
        {
            MethodStringBuilder = new StringBuilder();
            RefParameters = new List<string>();
            RefDBParameters = new List<string>();
            RefParametersType = new List<string>();
            ParametersString = new StringBuilder();
        }
    }
}
