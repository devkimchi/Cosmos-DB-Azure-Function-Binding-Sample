using System.Collections.Generic;
using System.Linq;

using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

using Newtonsoft.Json;

namespace CosmosDb.NetCore.FunctionApp
{
    /// <summary>
    /// This represents the trigger entity for Cosmos DB.
    /// </summary>
    public static class UpsertProductCosmosDbTrigger
    {
        /// <summary>
        /// Invokes the trigger.
        /// </summary>
        /// <param name="input">List of <see cref="Document"/> instances.</param>
        /// <param name="log"><see cref="TraceWriter"/> instance.</param>
        [FunctionName("ProductUpsertCosmosDbTrigger")]
        public static void Run(
            [CosmosDBTrigger(
                // Those names come from the application settings. Those names can come with both preceding % and trailing %.
                databaseName: "CosmosDbDdatabaseName",
                collectionName: "CosmosDbCollectionName",
                LeaseDatabaseName = "CosmosDbDdatabaseName",
                LeaseCollectionName = "CosmosDbLeaseCollectionName")] IReadOnlyList<Document> input,
            TraceWriter log)
        {
            if (input == null || !input.Any())
            {
                log.Info("No document found");
            }

            log.Info($"Count: {input.Count}");

            foreach (var document in input)
            {
                log.Info(JsonConvert.SerializeObject(document));
                log.Info(document.ToString());

                Product product = (dynamic)document;
                log.Info(JsonConvert.SerializeObject(product));
            }
        }
    }
}