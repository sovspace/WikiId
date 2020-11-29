using ApplicationTests.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApplicationTests
{

    public class SubarticleCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {

        }
    }

    public class SubarticleCommandTests : IClassFixture<SubarticleCommandContextFixture>
    {
        private readonly SubarticleCommandContextFixture _fixture;

        public SubarticleCommandTests(SubarticleCommandContextFixture fixture)
        {
            _fixture = fixture;
        }

        public static IEnumerable<object[]> CreateSubarticleCommandCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(CreateSubarticleCommandCases))]
        public void CreateSubarticleCommandTest()
        {

        }

        public static IEnumerable<object[]> DeleteSubarticleCommandCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(DeleteSubarticleCommandCases))]
        public void DeleteSubarticleCommandTest()
        {

        }

        public static IEnumerable<object[]> UpdateSubarticleCommandCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(UpdateSubarticleCommandCases))]
        public void UpdateSubarticleCommandTest()
        {

        }

    }
}
