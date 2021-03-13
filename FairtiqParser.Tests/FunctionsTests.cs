using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using FairtiqParser;
using Functions.Tests;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FairtiqParser.Tests
{
    public class FunctionsTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        private readonly Dictionary<string, string>[] expectedResponseValueDict = new Dictionary<string, string>[]
            {
                new Dictionary<string, string>
                {
                    {"Date", "12.02.2021"},
                    {"DepartureTime","18:33"},
                    {"Start", "La Chaux-de-Fonds"},
                    {"ArrivalTime","18:44"},
                    {"Destination","St-Imier" },
                    {"Cost","2.20"}
                },
                new Dictionary<string, string>
                {
                    {"Date", "12.02.2021"},
                    {"DepartureTime","23:17"},
                    {"Start", "St-Imier"},
                    {"ArrivalTime","23:32"},
                    {"Destination","La Chaux-de-Fonds"},
                    {"Cost","2.20"}
                },
                new Dictionary<string, string>
                {
                    {"Date", "16.02.2021"},
                    {"DepartureTime","06:57"},
                    {"Start", "La Chaux-de-Fonds"},
                    {"ArrivalTime","07:56"},
                    {"Destination","Murten/Morat" },
                    {"Cost","8.20"}
                },
                new Dictionary<string, string>
                {
                    {"Date", "16.02.2021"},
                    {"DepartureTime","17:02"},
                    {"Start", "Murten/Morat"},
                    {"ArrivalTime","17:24"},
                    {"Destination","Neuchâtel"},
                    {"Cost","5.70"}
                },
                new Dictionary<string, string>
                {
                    {"Date", "16.02.2021"},
                    {"DepartureTime","19:00"},
                    {"Start", "Neuchâtel"},
                    {"ArrivalTime","19:27"},
                    {"Destination","La Chaux-de-Fonds"},
                    {"Cost","1.40"}
                },
                new Dictionary<string, string>
                {
                    {"Date", "22.02.2021"},
                    {"DepartureTime","07:02"},
                    {"Start", "La Chaux-de-Fonds"},
                    {"ArrivalTime","07:57"},
                    {"Destination","Murten/Morat" },
                    {"Cost","7.10"}
                },
                new Dictionary<string, string>
                {
                    {"Date", "22.02.2021"},
                    {"DepartureTime","17:04"},
                    {"Start", "Murten/Morat"},
                    {"ArrivalTime","18:00"},
                    {"Destination","La Chaux-de-Fonds" },
                    {"Cost","6.50"}
                }
            };


        [Fact]
        public async void Http_trigger_should_return_known_string()
        {
            var request = TestFactory.CreateHttpRequest();
            var response = (OkObjectResult)await ParseEmailBody.Run(request, logger);
            Assert.Equal(JsonConvert.SerializeObject(expectedResponseValueDict), response.Value);
        }
    }
}
