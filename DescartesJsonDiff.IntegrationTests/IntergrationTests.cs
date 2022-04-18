using DescartesJsonDiff.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace DescartesJsonDiff.Testing
{
    [ExcludeFromCodeCoverage]
    public class IntergrationTests : TestBase
    {
        public IntergrationTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("AAAAAA==", "left", "1")]
        [InlineData("AQABAQ==", "right", "1")]
        public async void Add_Side_Data_Should_Return_Created(string data, string side, string id)
        {
            //Adding side data
            var sideInput = new ApiInput()
            {
                data = Convert.FromBase64String(data)
            };

            var inputData = SerializeContent(sideInput);

            //Call to add data
            var response = await _client.PutAsync($"v1/diff/{id}/{side}", inputData);
            
            //Verify response
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var content = JsonConvert.DeserializeObject<ApiInput>(await response.Content.ReadAsStringAsync());
            content.data.Should().BeEquivalentTo(sideInput.data);
        }

        [Theory]
        [InlineData("left")]
        [InlineData("right")]
        public async void Add_Null_Left_Side_Data_Should_Return_BadRequest(string side)
        {
            //Input null data
            var sideInput = new ApiInput();

            var inputData = SerializeContent(sideInput);

            //Call should return 400 error.
            var response = await _client.PutAsync($"v1/diff/1/{side}", inputData);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var error = JsonConvert.DeserializeObject<Error>(await response.Content.ReadAsStringAsync());
            error.value.Should().Be("Data cannot be null");
        }

        [Theory]
        [MemberData(nameof(DifferentialData))]

        public async void Get_Differential_Should_Return_Response(string leftSide, string rightSide, ApiResponse expectedResponse)
        {
            //Add left side data
            var leftSideInput = new ApiInput()
            {
                data = Convert.FromBase64String(leftSide)
            };
            
            var leftInputData = SerializeContent(leftSideInput);

            //Verify left side response
            var leftSideResponse = await _client.PutAsync($"v1/diff/1/left", leftInputData);
            leftSideResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var leftSideContent = JsonConvert.DeserializeObject<ApiInput>(await leftSideResponse.Content.ReadAsStringAsync());
            leftSideContent.data.Should().BeEquivalentTo(leftSideInput.data);

            //Add right side data
            var rightSideInput = new ApiInput()
            {
                data = Convert.FromBase64String(rightSide)
            };

            var rightInputData = SerializeContent(rightSideInput);

            //Verify right side response
            var rightSideResponse = await _client.PutAsync($"v1/diff/1/right", rightInputData);
            rightSideResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var rightSideContent = JsonConvert.DeserializeObject<ApiInput>(await rightSideResponse.Content.ReadAsStringAsync());
            rightSideContent.data.Should().BeEquivalentTo(rightSideInput.data);

            //Call to get diff
            var diffResponse = await _client.GetAsync("v1/diff/1");
            diffResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var differential = JsonConvert.DeserializeObject<ApiResponse>(await diffResponse.Content.ReadAsStringAsync());
            differential.DiffResultType.Should().Be(expectedResponse.DiffResultType);
            differential.Diffs.Should().BeEquivalentTo(expectedResponse.Diffs);
        }

        [Fact]
        public async void Get_Differential_Should_Return_Not_Found()
        {
            //First get request should return 404
            var firstGetRequest = await _client.GetAsync("v1/diff/1");
            firstGetRequest.StatusCode.Should().Be(HttpStatusCode.NotFound);

            //Add left side data
            var leftSideInput = new ApiInput()
            {
                data = Convert.FromBase64String("AAAAAA==")
            };

            var leftInputData = SerializeContent(leftSideInput);

            //Verify left side response
            var leftSideResponse = await _client.PutAsync($"v1/diff/1/left", leftInputData);
            leftSideResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var leftSideContent = JsonConvert.DeserializeObject<ApiInput>(await leftSideResponse.Content.ReadAsStringAsync());
            leftSideContent.data.Should().BeEquivalentTo(leftSideInput.data);

            //Second get request should also return 404 because no right side was added
            var secondGetRequest = await _client.GetAsync("v1/diff/1");
            secondGetRequest.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }


        public static IEnumerable<object[]> DifferentialData()
        {
            var jsonResponseEquals = new ApiResponse { DiffResultType = DiffResultType.Equals };
            var jsonResponseSizeDoNotMatch = new ApiResponse { DiffResultType = DiffResultType.SizeDoNotMatch };
            var jsonResponseContentDoNotMatch = new ApiResponse { 
                DiffResultType = DiffResultType.ContentDoNotMatch, 
                Diffs = new List<Differential>()
                {
                    new Differential()
                    {
                        Offset = 0,
                        Length = 1,
                    },
                    new Differential()
                    {
                        Offset = 2,
                        Length = 2,
                    },
                    new Differential()
                    {
                        Offset = 3,
                        Length = 1,
                    }
                }
            };
           
            return new List<object[]>
            {
                new object[]{"AAAAAA==", "AAAAAA==" , jsonResponseEquals },
                new object[]{"AAAAAA==", "AAA=" , jsonResponseSizeDoNotMatch },
                new object[]{"AAAAAA==", "AQABAQ==" , jsonResponseContentDoNotMatch },
            };

        }
        private static StringContent SerializeContent(ApiInput input)
        {
            var serializedObject = JsonConvert.SerializeObject(input);
            return new StringContent(serializedObject, Encoding.UTF8, "application/json");
        }
    }
}
