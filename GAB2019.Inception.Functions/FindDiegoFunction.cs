using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GAB2019.Inception.Functions
{
    public static class FindDiegoFunction
    {
        [FunctionName("FindDiegoFunction")]

        public static bool FindDiego([ActivityTrigger]string fileName, ILogger log)

        {
            try
            {
                bool foundDiego = false;
                string subscriptionKey = Environment.GetEnvironmentVariable("FaceAPISettings:SubscriptionKey");
                string faceAPIDetectServiceURI = Environment.GetEnvironmentVariable("FaceAPISettings:URIBase") + "/detect";

                string imageUrl = $"https://inceptionstg.blob.core.windows.net/inception-input/{fileName}";
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                string requestParameters = "returnFaceId=true";

                string uri = faceAPIDetectServiceURI + "?" + requestParameters;

                HttpResponseMessage response;
                StringContent stringContent = new StringContent(new JObject(new JProperty("url", imageUrl)).ToString());

                    stringContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                    response = client.PostAsync(uri, stringContent).Result;
                

                string contentString = response.Content.ReadAsStringAsync().Result;

                 FaceId[] faceIds = JsonConvert.DeserializeObject<FaceId[]>(contentString);
                FaceId faceId;
                if (faceIds.Length > 0)
                {
                    faceId = faceIds[0];

                    client = new HttpClient();

                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);


                    uri = Environment.GetEnvironmentVariable("FaceAPISettings:URIBase") + "/findsimilars";
                    JObject body = new JObject(new JProperty("faceId", faceId.faceId));
                    body.Add(new JProperty("faceListId", "diegofacelist"));
                    stringContent = new StringContent(body.ToString());
                    stringContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json"); 


                    response = client.PostAsync(uri, stringContent).Result;


                    contentString = response.Content.ReadAsStringAsync().Result;

                    Similars[] similars = JsonConvert.DeserializeObject<Similars[]>(contentString);
                    
                    foreach(var similar in similars)
                    {
                        foundDiego = foundDiego || similar.confidence >= 0.75;
                    }

                    

                }
                return foundDiego;
            }
            catch(Exception e)
            {
                log.LogInformation(e.Message);
                return false;
            }
        }
    }

    public class FaceId
    {
        public string faceId { get; set; }
    }

    public class Similars
    {
        public string persistedFaceId { get; set; }
        public double confidence { get; set; }
    }
}
