using Microsoft.Data.SqlClient;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public class SecureConnectionString
    {
        public SqlCredential SqlCredential { get; set; }

        public string ConnectionString { get; set; }

        public bool IsValid
        {
            get
            {
                try
                {
                    string str = this.ConnectionString.ToLower().Replace(" ", "");
                    return !string.IsNullOrWhiteSpace(this.ConnectionString) && this.SqlCredential != null || !string.IsNullOrWhiteSpace(this.ConnectionString) && (str.Contains("integratedsecurity=true") || str.Contains("integratedsecurity=sspi") || str.Contains("integratedsecurity=yes"));
                }
                catch 
                {
                    return false;
                }
            }
        }

        public SecureConnectionString(string connectionString, SqlCredential sqlCredentials)
        {
            this.ConnectionString = connectionString;
            this.SqlCredential = sqlCredentials;
        }
    }
}
