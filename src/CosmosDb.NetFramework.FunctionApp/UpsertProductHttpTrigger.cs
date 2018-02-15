using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace CosmosDb.NetFramework.FunctionApp
{
    public static class UpsertProductHttpTrigger
    {
        [FunctionName("ProductUpsertHttpTrigger")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequestMessage req,
            [DocumentDB(
                databaseName: "%CosmosDbDdatabaseName%",
                collectionName: "%CosmosDbCollectionName%")] IAsyncCollector<Product> collector,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var data = await req.Content.ReadAsAsync<Product>().ConfigureAwait(false);

            await collector.AddAsync(data).ConfigureAwait(false);

            return req.CreateResponse(HttpStatusCode.OK, data);
        }
    }
}