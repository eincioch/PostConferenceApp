using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CertificateCommon;
using CertificateImageGenerator;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PostConferenceFunctions
{
    public static class CertificateGenerationFunction
    {
        [FunctionName("CertificateGenerationFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            [Blob("certificates/{rand-guid}.jpg  ", FileAccess.ReadWrite, Connection = "AzureWebJobsStorage")]
            CloudBlockBlob outputBlob)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

           
            AttendeProperties attendeProperties = new AttendeProperties
            {
                Email = "humberto@humbertojaimes.net",
                FullName = $"Humberto Jaimes"
            };


            string description = "Por haber participado en la sesión \"Desarrollo de APIs con .NET Core\"";
            string description2 = "con una duración de 2 horas.";

            string webinarDate = "13-08-2021";
            CertificateProperties certificateProperties = new CertificateProperties
            {
                CourseDate = webinarDate,
                CourseName = "Desarrollo de APIs con .NET",
                DescriptionLine1 = description,
                DescriptionLine2 = description2,
                CertificateTemplateUrl = "https://santatrackapistorage.blob.core.windows.net/certificates/Certificado.jpg?sv=2019-12-12&st=2021-01-27T05%3A32%3A33Z&se=2021-12-29T05%3A32%3A00Z&sr=b&sp=r&sig=ljNYU9K30nqe4UBQDPLcnJJn8HP3U1xgdV11cAL8cls%3D"
            };

            DiplomaGenerator certificateImageGenerator = new DiplomaGenerator();

            try
            {
                var certificateImage = await certificateImageGenerator.GetCertificate(certificateProperties, attendeProperties);
                await outputBlob.UploadFromByteArrayAsync(certificateImage, 0, certificateImage.Length);

            }
            catch (Exception ex)
            {
                var c = ex;
            }
            //var blobUri = await CertificateImageGenerator.Helpers.ImageUploadHelper.UploadCertificate($"{Guid.NewGuid().ToString()}.jpg", certificateImage, storageConnectionString,
            //    "generated-certificates");

            

            return new OkObjectResult(outputBlob.Uri);
        }
    }
}
