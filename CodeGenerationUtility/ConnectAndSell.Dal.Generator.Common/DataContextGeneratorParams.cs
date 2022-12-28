using System.Collections.Generic;

namespace ConnectAndSell.Dal.Generator.Common
{
    public class DataContextGeneratorParams
    {
        public string DataContextClassName { get; }
        public string ProjectDataContextNameSpace { get; }
        public string ProjectDataContractNameSpace { get;}

        public string BaseDataContractClassName { get; }

        public string ModelDir { get; set; }
        public string DataContextDir { get; set; }

        public string ConfigFilePath { get; set; }

        public string ConnectionString { get; set; }

        public string SPConfigFilePath { get; set; }

        public string SPExecutorWrapperFolderPath { get; set; }

        public string DALHelperName { get; set; }

        public string IgnoreFields { get; }
        public string AlwaysDBGeneratedFields { get; set; }
        
        public List<string> AlwaysDBGeneratedFieldList { get; }

        public DataContextGeneratorParams(string dataContextClassName, string projectDataContextNameSpace, string projectDataContractNameSpace, string baseDataContractClassName, string modelDir, string dataContextDir, string configFilePath, string connectionString, string spConfigFilePath, string spExecutorWrapperFolderPath, string dalHelperName, string ignoreFields, string alwaysDBGeneratedFields)
        {
            this.DataContextClassName = dataContextClassName;
            this.ProjectDataContextNameSpace = projectDataContextNameSpace;
            this.ProjectDataContractNameSpace = projectDataContractNameSpace;
            this.BaseDataContractClassName = baseDataContractClassName;
            this.ModelDir = modelDir;
            this.DataContextDir = dataContextDir;
            this.ConfigFilePath = configFilePath;
            this.ConnectionString = connectionString;
            this.SPConfigFilePath = spConfigFilePath;
            this.SPExecutorWrapperFolderPath = spExecutorWrapperFolderPath;
            this.DALHelperName = dalHelperName;
            IgnoreFields = ignoreFields;
            AlwaysDBGeneratedFields = alwaysDBGeneratedFields;
            AlwaysDBGeneratedFieldList = string.IsNullOrEmpty(alwaysDBGeneratedFields) == false
                ? new List<string>(alwaysDBGeneratedFields.Split(','))
                : new List<string>();
        }
    }
}
