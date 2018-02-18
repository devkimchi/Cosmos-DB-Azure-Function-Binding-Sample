using System;

namespace CosmosDb.NetCore.FunctionApp
{
    /// <summary>
    /// This represents the entity for Cosmos DB connection.
    /// </summary>
    public static class CosmosDbConnection
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        public static string ConnectionString { get; } = Environment.GetEnvironmentVariable("AzureWebJobsCosmosDBConnectionString");

        /// <summary>
        /// Gets the account endpoint.
        /// </summary>
        public static string AccountEndpoint { get; } = GetAccountEndpoint();

        /// <summary>
        /// Gets the account key.
        /// </summary>
        public static string AccountKey { get; } = GetAccountKey();

        /// <summary>
        /// Gets the database name.
        /// </summary>
        public static string Database { get; } = Environment.GetEnvironmentVariable("CosmosDbDdatabaseName");

        /// <summary>
        /// Gets the collection name.
        /// </summary>
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