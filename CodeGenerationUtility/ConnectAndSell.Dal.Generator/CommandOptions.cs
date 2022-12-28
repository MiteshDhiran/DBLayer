using CommandLine;

namespace ConnectAndSell.Dal.Generator
{
    public class CommandOptions
    {
        [Option("projectName", Required = true, HelpText = "Project Name")]
        public string ProjectName { get; set; }
        [Option("projectNameSpace", Required = true, HelpText = "Project Namespace")]
        public string ProjectNameSpace { get; set; }
        [Option("outputFolderPath", Required = true, HelpText = "Output folder path where artifacts like data contracts, data context and SP wrapper classes will be generated")]
        public string OutputFolderPath { get; set; }
        [Option("configFolderPath", Required = true, HelpText = "Folder path containing the table (TableConfig.xml) and sp (SPConfig.xml) configuration file")]
        public string  ConfigFolderPath { get; set; }
        [Option("connectionString", Required = true, HelpText = "Database Connection string")]
        public string ConnectionString { get; set; }
        
    }
}
