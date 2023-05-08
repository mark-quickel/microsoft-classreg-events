using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace Microsoft.EE
{
    public static class AddSchoolClassReg
    {
        [FunctionName("AddSchoolClassReg")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [EventGrid(TopicEndpointUri = "EventGridEndpoint", TopicKeySetting = "EventGridKey")] IAsyncCollector<EventGridEvent> eventCollector,
            ILogger log)
        {
            log.LogInformation("Incoming request for eventgrid topic");

            var cid = 0;
            var sid = 0;
            if (!int.TryParse(req.Query["course"], out cid) || cid == 0)
            {
                return new BadRequestObjectResult("Course ID is required");
            }
            if (!int.TryParse(req.Query["student"], out sid) || sid == 0)
            {
                return new BadRequestObjectResult("Student ID is required");
            }
        
            var enrollment = new StudentGrade() { CourseID = cid, StudentID = sid };
            log.LogInformation(JsonConvert.SerializeObject(enrollment));

            var addEvent = new EventGridEvent("add-registration", "add-registration", "1.0", enrollment, typeof(StudentGrade));
            await eventCollector.AddAsync(addEvent);
            return new OkResult();
        }
    }
}
