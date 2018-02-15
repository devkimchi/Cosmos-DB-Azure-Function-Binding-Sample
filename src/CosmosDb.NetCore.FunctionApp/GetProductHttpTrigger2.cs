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

using Newtonsoft.Json;

namespace CosmosDb.NetCore.FunctionApp
{
    public static class GetProductHttpTrigger2
    {
        [FunctionName("GetProductHttpTrigger2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products2/{id}")] HttpRequest req,
            string id,
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

                var topic = new TopicClient(ServiceBusConnection.ConnectionString, ServiceBusConnection.Topic);

                var message = new Message(Encoding.UTF8.GetBytes(serialised))
                {
                    ContentType = "application/json",
                    UserProperties = { { "sample", "key" } }
                };

                await topic.SendAsync(message).ConfigureAwait(false);

                return new OkObjectResult(result.Document);
            }
        }
    }
}