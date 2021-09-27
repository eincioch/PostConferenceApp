using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostConferenceDAL.Repository;
using PostConferenceDAL.PostConferenceDbContext;

namespace PostConferenceFunctions
{
    public static class WebinarProcessorFunction
    {
        [FunctionName("WebinarProcessorFunction")]
        public static void Run([QueueTrigger("queue-webinars", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var connectionstring = Environment.GetEnvironmentVariable("PostConferenceConnectionString"); 

            var optionsBuilder = new DbContextOptionsBuilder<PostConferenceDatabaseContext>();
            optionsBuilder.UseSqlServer(connectionstring);

            var webinarRepository = new WebinarRepository(new(optionsBuilder.Options));

        }
    }
}
