using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using PostConferenceDAL.PostConferenceDbContext;
using PostConferenceDAL.Repository;
using System.Linq;
using CertificateCommon;
using Microsoft.WindowsAzure.Storage.Blob;
using CertificateImageGenerator;

namespace PostConferenceFunctions
{
    public static class TestFunction
    {
        [FunctionName("TestFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, [Blob("certificates/{rand-guid}.jpg  ", FileAccess.ReadWrite, Connection = "AzureWebJobsStorage")]
            CloudBlockBlob outputBlob)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            DiplomaGenerator diplomaGenerator = new();

            


            try
            {
                var connectionstring = Environment.GetEnvironmentVariable("PostConferenceConnectionString");
                var optionsBuilder = new DbContextOptionsBuilder<PostConferenceDatabaseContext>();
                optionsBuilder.UseSqlServer(connectionstring);

                var attendeeRepository = new AttendeeRepository(new(optionsBuilder.Options));
                var webinarRepository = new WebinarRepository(new(optionsBuilder.Options));

                var webinar = await webinarRepository.GetAsync(1);
                var attende = await attendeeRepository.GetAsync(1);

                AttendeProperties attendeProperties = new()
                {
                    Email = attende.Email,
                    FullName = attende.FullName
                };

                CertificateProperties certificateProperties = new CertificateProperties
                {
                    CourseDate = webinar.StartDateTime.Value.ToShortDateString(),
                    CourseName = webinar.WebinarId.ToString(),
                    DescriptionLine1 = "Has participated in webinar",
                    DescriptionLine2 = "",
                    CertificateTemplateUrl = "https://santatrackapistorage.blob.core.windows.net/certificates/Certificado.jpg?sv=2019-12-12&st=2021-01-27T05%3A32%3A33Z&se=2021-12-29T05%3A32%3A00Z&sr=b&sp=r&sig=ljNYU9K30nqe4UBQDPLcnJJn8HP3U1xgdV11cAL8cls%3D"
                };


                var certificateImage = await diplomaGenerator.GetCertificate(certificateProperties, attendeProperties);

                outputBlob.Metadata.Add("Name", attendeProperties.FullName);
                outputBlob.Metadata.Add("Mail", attendeProperties.Email);
                await outputBlob.UploadFromByteArrayAsync(certificateImage, 0, certificateImage.Length);

            }
            catch (Exception ex)
            {
                var e = ex;
            }

            return new OkResult();
        }
    }
}
