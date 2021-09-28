using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostConferenceDAL.Repository;
using PostConferenceDAL.PostConferenceDbContext;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace PostConferenceFunctions
{
    public static class WebinarProcessorFunction
    {
        [FunctionName("WebinarProcessorFunction")]
        public static async Task Run([QueueTrigger("queue-webinars", Connection = "AzureWebJobsStorage")] string webinarQueueItem, ILogger log,
            [Queue("queue-attendees", Connection = "AzureWebJobsStorage")] CloudQueue attendeesQueue)
        {
            log.LogInformation($"C# Queue trigger function processed: {webinarQueueItem}");

            var connectionstring = Environment.GetEnvironmentVariable("PostConferenceConnectionString"); 

            var optionsBuilder = new DbContextOptionsBuilder<PostConferenceDatabaseContext>();
            optionsBuilder.UseSqlServer(connectionstring);

            var webinarRepository = new WebinarRepository(new(optionsBuilder.Options));
            var attendeRepository = new AttendeeRepository(new(optionsBuilder.Options));

            int webinarId = int.Parse(webinarQueueItem);

            var webinar = await webinarRepository.GetAsync(webinarId);

            var attendees = (await attendeRepository.GetAllAsync()).Where(a => a.WebinarId == webinarId);

            foreach (var attendee in attendees)
            {

                if (attendee.Duration > (webinar.LiveDuration * .60))
                {
                    await attendeesQueue.AddMessageAsync(new($"{attendee.WebinarId}|{attendee.AttendeeId}"));
                }

            }
        }
    }
}
