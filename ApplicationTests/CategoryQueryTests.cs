using ApplicationTests.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApplicationTests
{

    public class CategoryQueryContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {

        }
    }
    public class CategoryQueryTests : IClassFixture<CategoryQueryContextFixture> 
    {
        private readonly CategoryQueryContextFixture _fixture;

        public CategoryQueryTests(CategoryQueryContextFixture fixture)
        {
            _fixture = fixture;
        }

        public static IEnumerable<object[]> GetAllCategoriesQueryCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(GetAllCategoriesQueryCases))]
        public void GetAllCategoriesQueryTest()
        {

        }

        public static IEnumerable<object[]> GetCategoryByIdQueryCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(GetCategoryByIdQueryCases))]
        public void GetCategoryByIdQueryTest()
        {

        }

    }
}
