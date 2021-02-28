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
                    {"Destination","Neuch�tel"},
                    {"Cost","5.70"}
                },
                new Dictionary<string, string>
                {
                    {"Date", "16.02.2021"},
                    {"DepartureTime","19:00"},
                    {"Start", "Neuch�tel"},
                    {"ArrivalTime","19:27"},
                    {"Destination","La Chaux-de-Fonds"},
                    {"Cost","1.40"}
                },
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
