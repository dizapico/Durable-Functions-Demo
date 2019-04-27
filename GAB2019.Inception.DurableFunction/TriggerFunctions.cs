using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GAB2019.Inception.DurableFunction
{
    public static class TriggerFunctions
    {
        [FunctionName("TriggerFunctions")]
        public static void Run([BlobTrigger("inception-input/{name}",
            Connection = "StorageSettings:BlobContainerConnection")]Stream imageSrc,
            [Blob("inception-thumbnails/sm-{name}", FileAccess.Write)] Stream imageSmall,
            string name,
            ILogger log)
        {
            imageSrc.CopyTo(imageSmall);

            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {imageSrc.Length} Bytes");
        }
    }
}
