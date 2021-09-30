using System;
using System.Linq;
using System.Threading.Tasks;

using PostConferenceDAL.Repository;
using PostConferenceDAL.PostConferenceDbContext;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using Office365Gateway;
using Microsoft.EntityFrameworkCore;

namespace PostConferenceFunctions
{
    public static class WebinarAttendeesProcessorFunction
    {
        [FunctionName("WebinarAttendeesProcessorFunction")]
        public static async Task Run(
            [TimerTrigger("0 0 9 * * *" 
            #if DEBUG
            , RunOnStartup=true
            #endif
            )] TimerInfo myTimer,
            ILogger log,
            [Queue("queue-webinars", Connection = "AzureWebJobsStorage")] ICollector<string> webinarsQueue)
        {
            log.LogInformation("Searching unprocessed webinars");

            var connectionString = Environment.GetEnvironmentVariable("PostConferenceConnectionString");

            var optionsBuilder = new DbContextOptionsBuilder<PostConferenceDatabaseContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var webinarRepository = new WebinarRepository(new(optionsBuilder.Options));
            var attendeRepository = new AttendeeRepository(new(optionsBuilder.Options));

            var webinars = (await webinarRepository.GetAllAsync())
                .Where(w => !string.IsNullOrWhiteSpace(w.OnlineMeetingJoinUrl)
                        && !w.WebinarStatusReport);

            var tenantId = Environment.GetEnvironmentVariable("TenantId");
            var clientId = Environment.GetEnvironmentVariable("ClientId");
            var clientSecret = Environment.GetEnvironmentVariable("ClientSecret");
            var userId = Environment.GetEnvironmentVariable("UserId");

            var graphService = new Office365Service(clientId, clientSecret);
            await graphService.GetAppToken(tenantId);

            foreach (var webinar in webinars)
            {
                var meeting = await graphService.GetOnlineMeeting(webinar.OnlineMeetingJoinUrl, userId);

                if (meeting == null)
                    continue;

                var attendees = (await graphService.GetMeetingAttendees(meeting, userId)).Where(x => !string.IsNullOrWhiteSpace(x.emailAddress));

                if (attendees == null)
                    continue;

                foreach (var attendee in attendees)
                {
                    var attendeeDb = new Attendee()
                    {
                        WebinarId = webinar.WebinarId,
                        Email = attendee.emailAddress,
                        FullName = attendee.identity.displayName,
                        Duration = attendee.totalAttendanceInSeconds / 60,
                        AttendeeEmailSent = true,
                        DiplomaUrl = string.Empty
                    };

                    var newAttendee = (await attendeRepository.PostAsync(attendeeDb)).Entity as Attendee;
                    //var queueItem = $"{webinar.WebinarId}|{newAttendee.AttendeeId}";
                    //attendeesQueue.Add(queueItem);

                    log.LogInformation($"{newAttendee.FullName} has been sent to the queue");
                }

                var webinarRef = await webinarRepository.GetAsync(webinar.WebinarId);
                webinarRef.WebinarStatusReport = true;
                await webinarRepository.PutAsync(webinarRef);

                var queueItem = webinar.WebinarId.ToString();
                webinarsQueue.Add(queueItem);
            }

            log.LogInformation("All unprocessed webinars have been sent to the queue");
        }
    }
}
