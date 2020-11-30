using ApplicationTests.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using Domain.Entities;
using Application.Features.CategoryFeatures.Queries;
using static Application.Features.CategoryFeatures.Queries.GetAllCategoriesQuery;
using System.Linq;
using static Application.Features.CategoryFeatures.Queries.GetCategoryByIdQuery;

namespace ApplicationTests
{

    public class CategoryQueryContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {

            Profile profile1 = new Profile { IdentityUserId = "1" };
            Profile profile2 = new Profile { IdentityUserId = "2" };
            Profile profile3 = new Profile { IdentityUserId = "3" };
            Context.Profiles.Add(profile1);
            Context.Profiles.Add(profile2);
            Context.Profiles.Add(profile3);

            MediaFile mediaFile1 = new MediaFile { Path = "test_path_1" };
            MediaFile mediaFile2 = new MediaFile { Path = "test_path_2" };
            MediaFile mediaFile3 = new MediaFile { Path = "test_path_3" };
            Context.MediaFiles.Add(mediaFile1);
            Context.MediaFiles.Add(mediaFile2);
            Context.MediaFiles.Add(mediaFile3);

            Category category1 = new Category { Id = 1, Creator = profile1, TitleImage = mediaFile1 };
            Category category2 = new Category { Id = 2, Creator = profile2, TitleImage = mediaFile2 };
            Category category3 = new Category { Id = 3, Creator = profile2, TitleImage = mediaFile3 };
            Context.Categories.Add(category1);
            Context.Categories.Add(category2);
            Context.Categories.Add(category3);
            Context.SaveChangesAsync();
        }
    }
    public class CategoryQueryTests : IClassFixture<CategoryQueryContextFixture> 
    {
        private readonly CategoryQueryContextFixture _fixture;

        public CategoryQueryTests(CategoryQueryContextFixture fixture)
        {
            _fixture = fixture;
        }

        
        [Fact]
        public async Task GetAllCategoriesQueryTestAsync()
        {
            GetAllCategoriesQuery request = new GetAllCategoriesQuery();
            GetAllCategoriesQueryHandler handler = new GetAllCategoriesQueryHandler(_fixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());
            var expecteResultIds = expectedResult.Select(c => c.Id);
            Assert.Equal(expecteResultIds, new List<int> { 1, 2, 3 });
        }

        public static IEnumerable<object[]> GetCategoryByIdQueryCases =>
            new List<object[]>
            {
                new object[]{ 1, 1 },
                new object[]{ 4, null },
            };

        [Theory]
        [MemberData(nameof(GetCategoryByIdQueryCases))]
        public async Task GetCategoryByIdQueryTestAsync(int categoryId, int? result) 
        {
            GetCategoryByIdQuery request = new GetCategoryByIdQuery
            {
                CategoryId = categoryId,
            };
            GetCategoryByIdQueryHandler handler = new GetCategoryByIdQueryHandler(_fixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());
            Assert.Equal(expectedResult?.Id, result);

        }

    }
}
