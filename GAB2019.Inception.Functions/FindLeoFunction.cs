using System;
using System.Collections.Generic;
using GAB2019.Inception.Model;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GAB2019.Inception.Functions
{
    public static class FindLeoFunction
    {
        [FunctionName("FindLeoFunction")]
        public static void Run([CosmosDBTrigger(
            databaseName: "Inception-FunctionsDB",
            collectionName: "CamImageObjectsFound",
            ConnectionStringSetting = "StorageSettings:CosmosDBInception_FunctionsDB")]IReadOnlyList<Document> input, ILogger log)
        {
            try
            {
                bool foundLeo = false;
                ImageObjects imageData = JsonConvert.DeserializeObject<ImageObjects>(input[0].ToString());
                foreach (var obj in imageData.objects)
                {
                    foundLeo = foundLeo || FindLeo(obj);
                }

                if (foundLeo)
                {
                    //Notify Leo
                }
            }catch(Exception e)
            {
                log.LogInformation(e.Message);
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
    }
}
