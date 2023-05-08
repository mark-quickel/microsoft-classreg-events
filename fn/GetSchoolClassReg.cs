using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.EE
{
    public static class GetSchoolClassReg
    {
        [FunctionName("GetSchoolClassReg")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Sql("select g.CourseId, Title, StudentId, FirstName, LastName from dbo.StudentGrade g inner join dbo.Course c on g.CourseId = c.CourseId inner join dbo.Person p on g.StudentId = p.PersonId", "SQLDBConnection")] IEnumerable<dynamic> enrollments,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult(enrollments);
        }
    }
}
