using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace GAB2019.Inception.Functions
{
    public static class FindDiegoFunction
    {
        [FunctionName("FindDiegoFunction")]
        public static void Run(string ImageURL, ILogger log)
        {
            try
            {
                string subscriptionKey = Environment.GetEnvironmentVariable("FaceAPISettings:SubscriptionKey");
                string faceAPIDetectServiceURI = Environment.GetEnvironmentVariable("FaceAPISettings:URIBase") + "/face/v1.0/detect";

                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                string requestParameters = "returnFaceId=true";

                string uri = faceAPIDetectServiceURI + "?" + requestParameters;

                HttpResponseMessage response;
                StringContent stringContent = new StringContent(new JProperty("url",ImageURL).ToString());

                    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    response = client.PostAsync(uri, stringContent).Result;
                

                string contentString = response.Content.ReadAsStringAsync().Result;

               // document = JsonConvert.DeserializeObject<ImageObjects>(contentString);

            }
            catch(Exception e)
            {
                log.LogInformation(e.Message);
            }
        }
    }
}
