using API.Interfaces;
using Azure.Core;
using Azure.Data.Tables;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace API.Services
{
    public class DBFactory : IDBFactory
    {
        private const string TableName = "Students";
        private readonly IConfiguration _configuration;
        

        public DBFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<TableClient> GetTableClient()
        {
            // Get Connection Key from Key Vault
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };
            var client = new SecretClient(new Uri(_configuration["KeyVaultConfig:kvUri"]), new DefaultAzureCredential(), options);
            KeyVaultSecret secret = client.GetSecret(_configuration["KeyVaultConfig:DBConnectionStringName"]);

            // Connection Key Obtained
            var serviceClient = new TableServiceClient(secret.Value);

            var tableClient = serviceClient.GetTableClient(TableName);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }
    }
}
