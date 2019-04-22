using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GAB2019.Inception.Functions
{
    public static class InceptionOrchestrator
    {
        [FunctionName("InceptionOrchestrator")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("InceptionOrchestrator_Hello", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("InceptionOrchestrator_Hello", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("InceptionOrchestrator_Hello", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("InceptionOrchestrator_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName("InceptionOrchestrator_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("InceptionOrchestrator", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        //    [FunctionName("DataOrchestration_QueueStart")]
        //    public static async Task QueueStart([QueueTrigger("tenants", Connection = "Blob:StorageConnection")] string input,
        //[OrchestrationClient]DurableOrchestrationClient starter,
        //ILogger log)
        //    {
        //        var tenant = JsonConvert.DeserializeObject<Tenant>(input);

        //        if (tenant != null)
        //        {
        //            string instanceId = await starter.StartNewAsync("DataOrchestration", tenant);

        //            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        //        }

        //    }
    }
}