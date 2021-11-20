using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SC.DevChallenge.Api;
using Sc.DevChallenge.Application.Models.RequestModels;
using Sc.DevChallenge.Application.Models.ResultModels;
using Xunit;

namespace Sc.DevChallenge.Api.IntegrationTests
{
    public class EndpointsIntegrationTests
    {
        private readonly HttpClient _client;

        public EndpointsIntegrationTests()
        {
            _client = new TestServer(new WebHostBuilder()
                .UseConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build()).UseStartup<Startup>()).CreateClient();
        }

        [Fact]
        public async Task GetBenchmarkEndpoint_ShouldReturnProperValue()
        {
            var respose = await _client.GetAsync("api/Prices/benchmark?portfolio=Fannie Mae&date=06/10/2018 00:00:00");

            respose.IsSuccessStatusCode.Should().BeTrue();

            var responseModel = JsonConvert.DeserializeObject<BenchmarkResultModel>(await respose.Content.ReadAsStringAsync());

            responseModel.Date.Should().Be(new DateTime(2018,10,05,21,26,40));
            responseModel.Price.Should().Be(new decimal(145.87));
        }

        [Fact]
        public async Task GetAggregatedBenchmarkEndpoint_ShouldReturnProperValue()
        {
            var response =
                await _client.GetAsync(
                    "api/Prices/aggregate?Portfolio=Fannie Mae&Intervals=7&StartDate=06/10/2018 00:00:00&EndDate=13/10/2018 00:00:00");
            
            response.IsSuccessStatusCode.Should().BeTrue();

            var responseModel = JsonConvert.DeserializeObject<List<AggregatedBenchmarkResultModel>>(await  response.Content.ReadAsStringAsync());

            responseModel.Should().BeEquivalentTo(new List<AggregatedBenchmarkResultModel>
            {
                new AggregatedBenchmarkResultModel
                {
                    Date = new DateTime(2018,10,06,19,40,00),
                    Price = new decimal(314.54)
                },
                new AggregatedBenchmarkResultModel
                {
                    Date = new DateTime(2018,10,07,20,40,00),
                    Price = new decimal(179.89)
                },
                new AggregatedBenchmarkResultModel
                {
                    Date = new DateTime(2018,10,08,21,40,00),
                    Price = new decimal(267.76)
                },
                new AggregatedBenchmarkResultModel
                {
                    Date = new DateTime(2018,10,09,22,40,00),
                    Price = new decimal(263.57)
                },
                new AggregatedBenchmarkResultModel
                {
                    Date = new DateTime(2018,10,10,23,40,00),
                    Price = new decimal(220.37)
                },
                new AggregatedBenchmarkResultModel
                {
                    Date = new DateTime(2018,10,12,00,40,00),
                    Price = new decimal(325.92)
                },
                new AggregatedBenchmarkResultModel
                {
                    Date = new DateTime(2018,10,12,22,53,20),
                    Price = new decimal(185.66)
                }
                
            });
        }
    }
}