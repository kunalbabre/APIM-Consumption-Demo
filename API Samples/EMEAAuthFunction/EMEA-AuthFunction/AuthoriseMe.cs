using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EMEA_AuthFunction
{
    public static class AuthoriseMe
    {
        [FunctionName("AuthoriseMe")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var r = new ResponseObject { active = true, exiresIn = DateTime.UtcNow.AddSeconds(30) };

            // checks for Header Authorization or CustomHeader to confirm user is Authorised
            // Here typically you would implement a call to your system to ensure token is valid etc
            if (req.Headers.ContainsKey("Authorization"))
            {

                log.LogInformation("Authorization: " + req.Headers["Authorization"]);
  
            }
            else if (req.Headers.ContainsKey("CustomHeader"))
            {
                log.LogInformation("CustomHeader: " + req.Headers["CustomHeader"]);
            }
            else
            {
                r.active = false;
                r.exiresIn = null;
            }

            req.HttpContext.Response.Headers.Add("Active", r.active.ToString());
            req.HttpContext.Response.Headers.Add("ExiresIn", r.exiresIn.ToString());
            return (ActionResult)new OkObjectResult(r);

        }

        class ResponseObject
        {
            public bool active;
            public DateTime? exiresIn; 
        }

    }
}
