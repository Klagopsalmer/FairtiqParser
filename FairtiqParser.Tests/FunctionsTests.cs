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
                    {"Destination","Neuchâtel"},
                    {"Cost","5.7"}
                },
                new Dictionary<string, string>
                {
                    {"Date", "16.02.2021"},
                    {"DepartureTime","019:00"},
                    {"Start", "Neuchâtel"},
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

        //[Theory]
        //[MemberData(nameof(TestFactory.Data), MemberType = typeof(TestFactory))]
        //public async void Http_trigger_should_return_known_string_from_member_data(string queryStringKey, string queryStringValue)
        //{
        //    var request = TestFactory.CreateHttpRequest(queryStringKey, queryStringValue);
        //    var response = (OkObjectResult)await ParseEmailBody.Run(request, logger);
        //    Assert.Equal($"Hello, {queryStringValue}. This HTTP triggered function executed successfully.", response.Value);
        //}
    }
}
