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
using System.Linq;
using PostConferenceDAL.Repository;
using Microsoft.EntityFrameworkCore;
using PostConferenceDAL.PostConferenceDbContext;

namespace PostConferenceFunctions
{
    public static class DiplomaGenerationFunction
    {
        [FunctionName("DiplomaGenerationFunction")]
        public static async Task Run(
            [QueueTrigger("queue-attendees", Connection = "AzureWebJobsStorage")] string attendeeQueueItem,
            ILogger log,
            [Blob("diplomas/{queueTrigger}|{rand-guid}.jpg  ", FileAccess.ReadWrite, Connection = "AzureWebJobsStorage")]
            CloudBlockBlob outputBlob)
        {
            log.LogInformation("Processing new attende");


            var connectionstring = Environment.GetEnvironmentVariable("PostConferenceConnectionString");
            var optionsBuilder = new DbContextOptionsBuilder<PostConferenceDatabaseContext>();
            optionsBuilder.UseSqlServer(connectionstring);

            var attendeRepository = new AttendeeRepository(new(optionsBuilder.Options));
            var webinarRepository = new WebinarRepository(new(optionsBuilder.Options));

            try
            {

                var data = attendeeQueueItem.Split('|');

                var webinarId = int.Parse(data[0]);
                var attendeId = int.Parse(data[1]);

                var webinar = await webinarRepository.GetAsync(webinarId);
                var attendees = await attendeRepository.GetAsync(attendeId);

                AttendeProperties attendeProperties = new()
                {
                    Email = attendees.Email,
                    FullName = attendees.FullName
                };

                DiplomaProperties diplomaProperties = new DiplomaProperties
                {
                    CourseDate = webinar.StartDateTime.ToShortDateString(),
                    CourseName = webinar.WebinarName,
                    DescriptionLine1 = "Has participated in webinar",
                    DescriptionLine2 = webinar.WebinarName,
                    DiplomaTemplateUrl = webinar.DiplomaTemplateUrl //"https://santatrackapistorage.blob.core.windows.net/certificates/Certificado.jpg?sv=2019-12-12&st=2021-01-27T05%3A32%3A33Z&se=2021-12-29T05%3A32%3A00Z&sr=b&sp=r&sig=ljNYU9K30nqe4UBQDPLcnJJn8HP3U1xgdV11cAL8cls%3D"
                };

                DiplomaGenerator diplomaGenerator = new();

                var diplomaImage = await diplomaGenerator.GetDiploma(diplomaProperties, attendeProperties);

                outputBlob.Metadata.Add("Name", attendeProperties.FullName);
                outputBlob.Metadata.Add("Mail", attendeProperties.Email);
                outputBlob.Metadata.Add("AttendeeId", attendees.AttendeeId.ToString()) ;
                outputBlob.Metadata.Add("WebinarId", attendees.WebinarId.ToString());
                await outputBlob.UploadFromByteArrayAsync(diplomaImage, 0, diplomaImage.Length);

                log.LogInformation($"BlobInput processed blob\n Queue:{attendeeQueueItem} \n Blob Name: {outputBlob.Name} bytes");

            }
            catch (Exception ex)
            {
                var c = ex;
            }
            //var blobUri = await CertificateImageGenerator.Helpers.ImageUploadHelper.UploadCertificate($"{Guid.NewGuid().ToString()}.jpg", certificateImage, storageConnectionString,
            //    "generated-certificates");


            //var signature = outputBlob.GetSharedAccessSignature(new SharedAccessBlobPolicy() { SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddDays(2), Permissions= SharedAccessBlobPermissions.Read });
            //return new OkObjectResult($"{outputBlob.Uri}{signature}");
        }
    }
}
