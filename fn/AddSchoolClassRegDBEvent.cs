// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid; 
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Newtonsoft.Json;

namespace Microsoft.EE
{
    public static class AddSchoolClassRegDBEvent
    {
        [FunctionName("AddSchoolClassRegDBEvent")]
        
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent,
        [Sql("dbo.StudentGrade","SQLDBConnection")] IAsyncCollector<StudentGrade> enrollments,
        ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            var enrollment = JsonConvert.DeserializeObject<StudentGrade>(eventGridEvent.Data.ToString());
            enrollments.AddAsync(enrollment);
            enrollments.FlushAsync();
        }
    }
}