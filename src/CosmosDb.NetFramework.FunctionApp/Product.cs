using Newtonsoft.Json;

namespace CosmosDb.NetFramework.FunctionApp
{
    /// <summary>
    /// This represents the entity for product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the product Id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the product category.
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product price.
        /// </summary>
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}