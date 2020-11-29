using ApplicationTests.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApplicationTests
{

    public class ProfileCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {

        }
    }

    public class ProfileCommandTests : IClassFixture<ProfileCommandContextFixture>
    {
        private readonly ProfileCommandContextFixture _fixture;

        public ProfileCommandTests(ProfileCommandContextFixture fixture)
        {
            _fixture = fixture;
        }


        public static IEnumerable<object[]> CreateProfileCommandCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(CreateProfileCommandCases))]
        public void CreateProfileCommandTest()
        {

        }

        public static IEnumerable<object[]> DeleteProfileCommandCases =>
            new List<object[]>
            {

            };


        [Theory]
        [MemberData(nameof(DeleteProfileCommandCases))]
        public void DeleteProfileCommandTest()
        {

        }

    }
}
