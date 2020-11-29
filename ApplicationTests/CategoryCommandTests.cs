using ApplicationTests.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApplicationTests
{

    public class CategoryCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {

        }
    }

    public class CategoryCommandTests : IClassFixture<ArticleCommandContextFixture>
    {
        private readonly CategoryCommandContextFixture _fixture;

        public CategoryCommandTests(CategoryCommandContextFixture fixture)
        {
            _fixture = fixture;
        }


        public static IEnumerable<object[]> CreateCategoryCommandCases =>
            new List<object[]>
            {

            };


        [Theory]
        [MemberData(nameof(CreateCategoryCommandCases))]
        public void CreateCategoryCommandTest()
        {

        }

        public static IEnumerable<object[]> DeleteCategoryCommandCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(DeleteCategoryCommandCases))]
        public void DeleteCategoryCommandTest()
        {

        }

        public static IEnumerable<object[]> UpdateCategoryCommandCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(UpdateCategoryCommandCases))]
        public void UpdateCategoryCommandTest()
        {

        }

    }
}
