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
using System.Linq;

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
            string updatedBody = Regex.Replace(bodyNode.InnerText, "<.*?>", string.Empty);
            updatedBody = updatedBody.Replace("\r\n", " ");
            updatedBody = updatedBody.Replace(@"&nbsp;", " ");
            updatedBody = Regex.Replace(updatedBody, " {2,}", " ");

            // Split by dates
            string[] journeysStrings = Regex.Split(updatedBody, @"(\d{2} [A-z]{3} \d{4})");

            // Journeys start at index 5
            journeysStrings = journeysStrings.Skip(5).ToArray();

            // Iterates over journeys
            var result = new List<Dictionary<string, string>>();
            for (int i = 0; i < journeysStrings.Count(); i += 2)
            {
                DateTime date = DateTime.Parse(journeysStrings[i]);

                //Find journeys
                Regex rx = new Regex(@"(\d{2}:\d{2}) ([A-zÀ-ÿ -\/]+) [^\x00-\x7F] (\d{2}:\d{2}) ([A-zÀ-ÿ -\/]+) 2nd class [^\x00-\x7F] Reduced fare CHF (\d*.\d{2})");

                var matches = rx.Matches(journeysStrings[i+1]);

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
                    result.Add(journey);
                }
            }

            // Return journeys
            return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(result.ToArray()));
        }
    }
}
