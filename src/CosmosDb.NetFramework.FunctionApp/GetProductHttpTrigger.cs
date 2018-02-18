using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.ServiceBus.Messaging;

using Newtonsoft.Json;

namespace CosmosDb.NetFramework.FunctionApp
{
    /// <summary>
    /// This represents the trigger entity for HTTP request.
    /// </summary>
    public static class GetProductHttpTrigger
    {
        /// <summary>
        /// Invokes the trigger.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="collector"><see cref="IAsyncCollector{BrokeredMessage}"/> instance.</param>
        /// <param name="log"><see cref="TraceWriter"/> instance.</param>
        /// <returns>Returns the <see cref="HttpResponseMessage"/> instance.</returns>
        [FunctionName("GetProductHttpTrigger")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")] HttpRequestMessage req,
            string id,
            [ServiceBus("%ServiceBusTopic%", EntityType = EntityType.Topic)] IAsyncCollector<BrokeredMessage> collector,
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
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(serialised), false))
                {
                    var message = new BrokeredMessage(stream)
                    {
                        ContentType = "application/json",
                        Properties = { { "sample", "key" } }
                    };

                    await collector.AddAsync(message).ConfigureAwait(false);
                }

                return req.CreateResponse(HttpStatusCode.OK, result.Document);
            }
        }
    }
}