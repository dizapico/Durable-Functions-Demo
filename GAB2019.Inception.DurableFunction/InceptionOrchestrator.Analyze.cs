using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;

namespace GAB2019.Inception.DurableFunction
{
    public static partial class InceptionOrchestrator
    {
        [FunctionName("InceptionOrchestrator_AnalyzeImageCognitiveServicesFunction")]
        public static async Task AnalyzeImage([ActivityTrigger]string fileName,
            ILogger log)
        {
            try
            {
                log.LogInformation($" ---- Start analyzing image ----");

                string subscriptionKey = Environment.GetEnvironmentVariable("CognitiveServicesSettings:SubscriptionKey");
                ComputerVisionClient computerVision = new ComputerVisionClient(
                    new ApiKeyServiceClientCredentials(subscriptionKey),
                    new System.Net.Http.DelegatingHandler[] { });
                computerVision.Endpoint = "https://westeurope.api.cognitive.microsoft.com/";

                var imageUrl = $"https://inceptionstg.blob.core.windows.net/inception-input/{fileName}";
                // Specify the features to return
                List<VisualFeatureTypes> features =
                    new List<VisualFeatureTypes>() {
                                                        VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                                                        VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                                                        VisualFeatureTypes.Tags
                                                    };

                if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                {
                    Console.WriteLine(
                        "\nInvalid remoteImageUrl:\n{0} \n", imageUrl);
                    return;
                }

                ImageAnalysis analysis = await computerVision.AnalyzeImageAsync(imageUrl, features);

            }
            catch (Exception e)
            {
                log.LogInformation(e.Message);
                //document = null;
            }
        }

        private static byte[] GetImageAsByteArray(Stream image)
        {
            BinaryReader binaryReader = new BinaryReader(image);
            return binaryReader.ReadBytes((int)image.Length);
        }

    }
}