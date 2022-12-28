using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ConnectAndSell.Dal.Generator.Common
{
    public class SPConfigInfo
    {
        public string SPName { get; }
        public List<ResultSetConfigInfo> ResultSetInfoList { get; }
        public SPConfigInfo(string spName, List<ResultSetConfigInfo> resultSetInfoList)
        {
            this.SPName = spName;
            this.ResultSetInfoList = resultSetInfoList;
        }
    }
    
    public class ResultSetConfigInfo 
    {
        public string ClassName { get; set; }
        public string Namespace { get; set; }
        public ResultSetConfigInfo(string className,string namespaceName)
        {
            this.ClassName = className ?? throw new ArgumentNullException($"{nameof(className)}");
            this.Namespace = namespaceName ?? throw new ArgumentNullException($"{nameof(namespaceName)}");
        }
    }

    public static class SPConfigReaderHelper
    {
        public static List<SPConfigInfo> GetSPConfigData(string filePath) =>
            XElement.Load(filePath)
                .Descendants("SP")
                .Select(xe => new SPConfigInfo(xe.Attribute("Name")?.Value, xe.Descendants("ResultSet")
                    .Select(x => new ResultSetConfigInfo(x.Attribute("ClassName")?.Value,x.Attribute("NameSpace")?.Value) ).ToList() )).ToList();
    }
}
