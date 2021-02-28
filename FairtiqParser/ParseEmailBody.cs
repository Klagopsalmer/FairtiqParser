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
using System.Collections.Generic;

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

            //Find date
            Regex dateRx = new Regex(@"\d{2} [A-z]{3} \d{4}");
            MatchCollection matches = dateRx.Matches(updatedBody);

            DateTime date = DateTime.Parse(matches[1].ToString());

            //Find journeys
            Regex rx = new Regex(@"(\d{2}:\d{2}) ([A-zÀ-ÿ -\/]+)   [^\x00-\x7F] (\d{2}:\d{2}) ([A-zÀ-ÿ -\/]+)  CHF (\d*.\d{2})");

            matches = rx.Matches(updatedBody);

            var result = new Dictionary<string, string>[matches.Count];

            int index = 0;
            foreach (Match match in matches)
            {
                var journey = new Dictionary<string, string>()
                {
                    {"Date", date.ToShortDateString()},
                    {"DepartureTime",match.Groups[1].ToString()},
                    {"Start", match.Groups[2].ToString()},
                    {"ArrivalTime",match.Groups[3].ToString()},
                    {"Destination",match.Groups[4].ToString()},
                    {"Cost",match.Groups[5].ToString()}
                };
                result[index] = journey;
                index++;
            }

            // Return cleaned text
            return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(result));
        }
    }
}
