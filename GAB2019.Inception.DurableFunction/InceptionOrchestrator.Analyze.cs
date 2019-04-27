using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GAB2019.Inception.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GAB2019.Inception.DurableFunction
{
    public static partial class InceptionOrchestrator
    {
        [FunctionName("InceptionOrchestrator_AnalyzeImageCognitiveServicesFunction")]
        public static void AnalyzeImage([ActivityTrigger]Stream image,
            [CosmosDB(
                databaseName: "Inception-FunctionsDB",
                collectionName: "CamImageObjectsFound",
                ConnectionStringSetting = "StorageSettings:CosmosDBInception_FunctionsDB")]out ImageObjects document,
            ILogger log)
        {
            try
            {
                string subscriptionKey = Environment.GetEnvironmentVariable("CognitiveServicesSettings:SubscriptionKey");
                string analyzeServiceURI = Environment.GetEnvironmentVariable("CognitiveServicesSettings:URIBase") + "vision/v2.0/analyze";
                string requestParameters = "visualFeatures=Objects";
                string uri = analyzeServiceURI + "?" + requestParameters;
                byte[] byteData = GetImageAsByteArray(image);
                HttpClient client = new HttpClient();
                HttpResponseMessage response;

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = client.PostAsync(uri, content).Result;
                }

                string contentString = response.Content.ReadAsStringAsync().Result;

                document = JsonConvert.DeserializeObject<ImageObjects>(contentString);
            }
            catch (Exception e)
            {
                log.LogInformation(e.Message);
                document = null;
            }
        }

        private static byte[] GetImageAsByteArray(Stream image)
        {
            BinaryReader binaryReader = new BinaryReader(image);
            return binaryReader.ReadBytes((int)image.Length);
        }

    }
}