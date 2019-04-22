using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GAB2019.Inception.Functions
{
    public static class LauncherFunction
    {
        [FunctionName("LauncherFunction")]
        public static void Run([BlobTrigger("inception-files/{name}",
            Connection = "StorageSettings:BlobContainerConnection")]Stream myBlob,
            [Blob("inception-files/processed/thumb-{name}", FileAccess.Write)] Stream imageSmall,
            string name,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }

        //[FunctionName("BlobTriggerCSharp")]
        //public static void Run([BlobTrigger("inception-files/{name}")] Stream myBlob, string name, ILogger log)
        //{
        //    log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        //}
    }
}
