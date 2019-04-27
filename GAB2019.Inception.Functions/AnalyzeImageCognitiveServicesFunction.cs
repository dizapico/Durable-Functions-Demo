using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GAB2019.Inception.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GAB2019.Inception.Functions
{
    public static class AnalyzeImageCognitiveServicesFunction
    {
        
        [FunctionName("AnalyzeImageCognitiveServicesFunction")]
        public static void Run([ActivityTrigger]Stream image, string name,
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
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                string requestParameters = "visualFeatures=Objects";

                string uri = analyzeServiceURI + "?" + requestParameters;

                HttpResponseMessage response;

                byte[] byteData = GetImageAsByteArray(image);

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

        

        private static bool FindLeo(ImageObject Objects)
        {
            if (Objects.Object.Equals("retriever")
                || Objects.Object.Equals("Golden retriever")
                || Objects.Object.Equals("dog"))
            {
                return true;
            }
            return false;
        }

        private static byte[] GetImageAsByteArray(Stream image)
        {
            BinaryReader binaryReader = new BinaryReader(image);
            return binaryReader.ReadBytes((int)image.Length);
        }
    }

}

