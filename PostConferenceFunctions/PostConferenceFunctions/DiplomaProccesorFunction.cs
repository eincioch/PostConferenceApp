using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using PostConferenceDAL.PostConferenceDbContext;
using PostConferenceDAL.Repository;

namespace PostConferenceFunctions
{
    public static class DiplomaProccesorFunction
    {
        [FunctionName("DiplomaProccesorFunction")]
        public static async Task Run([BlobTrigger("diplomas/{name}", Connection = "AzureWebJobsStorage")] CloudBlockBlob diplomaBlob, string name, ILogger log,
            [Queue("queue-emails", Connection = "AzureWebJobsStorage")] CloudQueue attendeesQueue)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name}");

            var connectionstring = Environment.GetEnvironmentVariable("PostConferenceConnectionString");
            var optionsBuilder = new DbContextOptionsBuilder<PostConferenceDatabaseContext>();
            optionsBuilder.UseSqlServer(connectionstring);

            var attendeRepository = new AttendeeRepository(new(optionsBuilder.Options));

            var attendeeId = int.Parse(diplomaBlob.Metadata["AttendeeId"]);

            var attendee = await attendeRepository.GetAsync(attendeeId);

            var signature = diplomaBlob.GetSharedAccessSignature(new SharedAccessBlobPolicy() { SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddDays(2), Permissions= SharedAccessBlobPermissions.Read });

            attendee.DiplomaUrl = $"{diplomaBlob.Uri}{signature}";

            await attendeRepository.PutAsync(attendee);

            await attendeesQueue.AddMessageAsync(new(attendee.AttendeeId.ToString()));
        }
    }
}
