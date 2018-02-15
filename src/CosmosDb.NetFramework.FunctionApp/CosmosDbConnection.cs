using System;

namespace CosmosDb.NetFramework.FunctionApp
{

    public static class CosmosDbConnection
    {
        public static string ConnectionString { get; } = Environment.GetEnvironmentVariable("AzureWebJobsDocumentDBConnectionString");

        public static string AccountEndpoint { get; } = GetAccountEndpoint();

        public static string AccountKey { get; } = GetAccountKey();

        public static string Database { get; } = Environment.GetEnvironmentVariable("CosmosDbDdatabaseName");

        public static string Collection { get; } = Environment.GetEnvironmentVariable("CosmosDbCollectionName");

        private static string GetAccountEndpoint()
        {
            var segments = ConnectionString.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var endpoint = segments[0].Replace("AccountEndpoint=", string.Empty);

            return endpoint;
        }

        private static string GetAccountKey()
        {
            var segments = ConnectionString.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var key = segments[1].Replace("AccountKey=", string.Empty);

            return key;
        }
    }
}