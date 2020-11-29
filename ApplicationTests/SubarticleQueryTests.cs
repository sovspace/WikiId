using ApplicationTests.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApplicationTests
{
    public class SubarticleQueryContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {

        }
    }

    class SubarticleQueryTests : IClassFixture<SubarticleCommandContextFixture>
    {
        private readonly SubarticleCommandContextFixture _fixture;

        public SubarticleQueryTests(SubarticleCommandContextFixture fixture)
        {
            _fixture = fixture;
        }


        public IEnumerable<object[]> GetSubarticleByIdCommandCases =>
            new List<object[]>
            {

            };

        public void GetSubarticleByIdCommandTest()
        {

        }

    }
}
