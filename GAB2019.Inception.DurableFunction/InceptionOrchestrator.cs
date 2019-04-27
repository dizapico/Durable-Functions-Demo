using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GAB2019.Inception.DurableFunction
{
    public static partial class InceptionOrchestrator
    {
        [FunctionName("InceptionOrchestrator")]
        public static async Task  RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context,
            ILogger log)
        {
            log.LogInformation($" **** Started orchestration with ID = {context.InstanceId} ****");

            var fileName = context.GetInput<string>();

            await context.CallActivityAsync("InceptionOrchestrator_AnalyzeImageCognitiveServices", fileName);
            //await context.CallActivityAsync("InceptionOrchestrator_SaveAnalysisInformation", fileName);

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

        [FunctionName("InceptionOrchestrator_BlobLauncher")]
        public static async Task Run([BlobTrigger("inception-input/{name}",
            Connection = "StorageSettings:BlobContainerConnection")]Stream imageSrc,
            [OrchestrationClient]DurableOrchestrationClient starter,
            string name,
            ILogger log)
        {
            string instanceId = await starter.StartNewAsync("InceptionOrchestrator", name);

            log.LogInformation($" **** Orchestration with ID {instanceId} ended ****");
        }

    }
}