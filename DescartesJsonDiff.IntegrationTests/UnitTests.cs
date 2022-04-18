using DescartesJsonDiff.Models;
using DescartesJsonDiff.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Xunit;

namespace DescartesJsonDiff.Testing
{
    [ExcludeFromCodeCoverage]
    public class UnitTests
    {
        [Fact]
        public void Create_Json_Data_Method_Should_Return_Successful()
        {
            var inputData = new ApiInput()
            {
                data = Convert.FromBase64String("AAAAAA==")
            };

            var differentialJsonService = new DifferentialService();
            differentialJsonService.CreateJsonData("1", inputData, "left");

            var resultInCache = InMemoryCache.GetFromCache("1-left");
            resultInCache.Should().NotBeNullOrEmpty();
            resultInCache.Should().BeEquivalentTo(inputData.data);
        }

        [Fact]
        public void Create_Json_Data_Method_Should_Update_And_Return_Successful()
        {
            var inputData = new ApiInput()
            {
                data = Convert.FromBase64String("AAAAAA==")
            };

            var differentialJsonService = new DifferentialService();
            differentialJsonService.CreateJsonData("1", inputData, "left");

            var resultInCacheBeforeUpdate = InMemoryCache.GetFromCache("1-left");
            resultInCacheBeforeUpdate.Should().NotBeNullOrEmpty();
            resultInCacheBeforeUpdate.Should().BeEquivalentTo(inputData.data);

            var updatedData = new ApiInput()
            {
                data = Convert.FromBase64String("AAA=")
            };

            differentialJsonService.CreateJsonData("1", updatedData, "left");

            var resultInCacheAfterUpdate = InMemoryCache.GetFromCache("1-left");
            resultInCacheAfterUpdate.Should().NotBeNullOrEmpty();
            resultInCacheAfterUpdate.Should().BeEquivalentTo(updatedData.data);
        }

        [Fact]
        public void Get_Diff_Should_Return_Null()
        {
            var differentialJsonService = new DifferentialService();
            var result = differentialJsonService.GetJsonDiff("1");
            result.Should().BeNull();
        }

    }
}
