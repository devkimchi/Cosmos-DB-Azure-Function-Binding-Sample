using System;

namespace CosmosDb.NetCore.FunctionApp
{
    public static class ServiceBusConnection
    {
        public static string ConnectionString { get; } = Environment.GetEnvironmentVariable("AzureWebJobsServiceBus");

        public static string Topic { get; } = Environment.GetEnvironmentVariable("ServiceBusTopic");
    }
}