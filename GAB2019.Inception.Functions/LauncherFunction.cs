using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace GAB2019.Inception.Functions
{
    public static class LauncherFunction
    {
        [FunctionName("LauncherFunction")]
        //[StorageAccount("FunctionLevelStorageAppSetting")]
        public static void Run([BlobTrigger("inception-input/{name}",
            Connection = "StorageSettings:BlobContainerConnection")]Stream imageSrc,
            [Blob("inception-thumbnails/sm-{name}", FileAccess.Write)] Stream imageSmall,
            string name,
            ILogger log)
        {
            imageSrc.CopyTo(imageSmall);

            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {imageSrc.Length} Bytes");
        }

        //[FunctionName("BlobTriggerCSharp")]
        //public static void Run([BlobTrigger("inception-files/{name}")] Stream myBlob, string name, ILogger log)
        //{
        //    log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        //}


    }
}
