using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace CosmosDb.NetFramework.FunctionApp
{
    /// <summary>
    /// This represents the trigger entity for HTTP request.
    /// </summary>
    public static class UpsertProductHttpTrigger
    {
        /// <summary>
        /// Invokes the trigger.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="collector"><see cref="IAsyncCollector{Product}"/> instance.</param>
        /// <param name="log"><see cref="TraceWriter"/> instance.</param>
        /// <returns>Returns the <see cref="HttpResponseMessage"/> instance.</returns>
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