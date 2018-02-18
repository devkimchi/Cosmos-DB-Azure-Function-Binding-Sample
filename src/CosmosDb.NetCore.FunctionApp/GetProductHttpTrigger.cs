using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;

using Newtonsoft.Json;

namespace CosmosDb.NetCore.FunctionApp
{
    /// <summary>
    /// This represents the trigger entity for HTTP request.
    /// </summary>
    public static class GetProductHttpTrigger
    {
        /// <summary>
        /// Invokes the trigger.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="id">Product Id.</param>
        /// <param name="collector"><see cref="IAsyncCollector{Message}"/> instance.</param>
        /// <param name="log"><see cref="TraceWriter"/> instance.</param>
        /// <returns>Returns the <see cref="IActionResult"/> instance.</returns>
        [FunctionName("GetProductHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")] HttpRequest req,
            string id,
            [ServiceBus("%ServiceBusTopic%", EntityType = EntityType.Topic)] IAsyncCollector<Message> collector,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var endpoint = new Uri(CosmosDbConnection.AccountEndpoint);
            var accesskey = CosmosDbConnection.AccountKey;
            using (var client = new DocumentClient(endpoint, accesskey))
            {
                var options = new RequestOptions() { PartitionKey = new PartitionKey("Beer") };
                var uri = UriFactory.CreateDocumentUri(CosmosDbConnection.Database, CosmosDbConnection.Collection, id);
                var result = await client.ReadDocumentAsync<Product>(uri, options).ConfigureAwait(false);

                var serialised = JsonConvert.SerializeObject(result.Document);

                var message = new Message(Encoding.UTF8.GetBytes(serialised))
                {
                    ContentType = "application/json",
                    UserProperties = { { "sample", "key" } }
                };

                await collector.AddAsync(message).ConfigureAwait(false);

                return new OkObjectResult(result.Document);
            }
        }
    }
}