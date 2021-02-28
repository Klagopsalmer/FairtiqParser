using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace FairtiqParser
{
    public static class ParseEmailBody
    {
        [FunctionName("ParseEmailBody")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Parse query parameter
            string emailBodyContent = await new StreamReader(req.Body).ReadToEndAsync();

            //Select only body section
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(emailBodyContent);
            HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

            // Replace HTML with other characters
            string updatedBody = bodyNode.InnerText.Replace(@"&nbsp;", " ");

            // Return cleaned text
            return (ActionResult)new OkObjectResult(new { updatedBody });
        }
    }
}
