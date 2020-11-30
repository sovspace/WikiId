using Application.Features.ArticleFeatures.Queries;
using ApplicationTests.Common;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using static Application.Features.ArticleFeatures.Queries.GetArticleByIdQuery;
using static Application.Features.ArticleFeatures.Queries.GetCreatedArticlesQuery;
using System.Linq;
using static Application.Features.ArticleFeatures.Queries.GetEditableArticlesQuery;
using static Application.Features.ArticleFeatures.Queries.GetPublicArticlesQuery;
using static Application.Features.ArticleFeatures.Queries.GetViewableArticlesQuery;

namespace ApplicationTests
{
    public class ArticleQueryContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {
            Profile p1 = new Profile { IdentityUserId = "1" };
            Profile p2 = new Profile { IdentityUserId = "2" };
            Profile p3 = new Profile { IdentityUserId = "3" };
            Profile p4 = new Profile { IdentityUserId = "4" };
            Profile p5 = new Profile { IdentityUserId = "5" };
            Context.Profiles.Add(p1);
            Context.Profiles.Add(p2);
            Context.Profiles.Add(p3);
            Context.Profiles.Add(p4);
            Context.Profiles.Add(p5);
            Category category1 = new Category();
            Context.Categories.Add(category1);
            MediaFile mediaFile1 = new MediaFile();
            Context.MediaFiles.Add(mediaFile1);


            Article article1 = new Article { Id = 1, Creator = p1, ViewRoleString = "view_test_1",
                EditRoleString = "edit_test_1", Category = category1, TitleImage = mediaFile1 };
            Article article2 = new Article { Id = 2, Creator = p1, ViewRoleString = "view_test_2",
                EditRoleString = "edit_test_2", Category = category1, TitleImage = mediaFile1 };
            Article article3 = new Article { Id = 3, Creator = p2, ViewRoleString = "view_test_3",
                EditRoleString = "edit_test_3", Category = category1, TitleImage = mediaFile1 };
            Article article4 = new Article { Id = 4, Creator = p5, ViewRoleString = "view_test_4", EditRoleString = "edit_test_4",
                IsPublic = true, Category = category1, TitleImage = mediaFile1};
            Article article5 = new Article { Id = 5, Creator = p5, ViewRoleString = "view_test_5", EditRoleString = "edit_test_5",
                IsPublic = true, Category = category1, TitleImage = mediaFile1 };

            Context.Articles.Add(article1);
            Context.Articles.Add(article2);
            Context.Articles.Add(article3);
            Context.Articles.Add(article4);
            Context.Articles.Add(article5);
            Context.SaveChangesAsync();
        }
    }

    public class ArticleQueryTests : IClassFixture<ArticleQueryContextFixture>
    {
        private readonly ArticleQueryContextFixture _fixture;
        public ArticleQueryTests(ArticleQueryContextFixture fixture)
        {
            _fixture = fixture;
        }

        public static IEnumerable<object[]> GetArticleByIdQueryCases =>
            new List<object[]>
            {
                new object[] {1, new List<string> { "view_test_1"}, 1},
                new object[] {4, new List<string> { ""}, 4},
                new object[] {3, new List<string> { "view_test_2"}, null},
                new object[] {2, new List<string> { "view_test_1"}, null},
            };

        [Theory]
        [MemberData(nameof(GetArticleByIdQueryCases))]
        public async Task GetArticleByIdQueryTestAsync(int articleId, IEnumerable<string> userRoles, int? result)
        {
            GetArticleByIdQuery request = new GetArticleByIdQuery
            {
                Id = articleId,
                UserRoles = userRoles,
            };
            GetArticleByIdQueryHandler handler = new GetArticleByIdQueryHandler(_fixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());
            Assert.Equal(expectedResult?.Id, result);
        }

        public static IEnumerable<object[]> GetCreatedArticlesQueryCases =>
            new List<object[]>
            {
                new object[] {"1", new List<int> { 1, 2 } },
                new object[] {"5", new List<int> { 4, 5 } },
                new object[] {"3", new List<int>() },
            };


        [Theory]
        [MemberData(nameof(GetCreatedArticlesQueryCases))]
        public async Task GetCreatedArticlesQueryTestAsync(string identityUserId, IEnumerable<int> result)
        {
            GetCreatedArticlesQuery request = new GetCreatedArticlesQuery
            {
                IdentityUserId = identityUserId,
            };
            GetCreatedArticlesQueryHandler handler = new GetCreatedArticlesQueryHandler(_fixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());
            if (result != null)
            {
                var expectedResultIds = expectedResult.Select(a => a.Id).ToList();
                Assert.Equal(result, expectedResultIds);
            } else
            {
                Assert.Null(expectedResult);
            }
        }

        public static IEnumerable<object[]> GetEditableArticlesQueryCases =>
            new List<object[]>
            {
                new object[] { new List<string> { "view_test_1" }, new List<int>() },
                new object[] { new List<string> { "edit_test_1" }, new List<int> { 1 } },
                new object[] { new List<string> { "edit_test_2", "edit_test_3", "edit_test_4" }, new List<int> { 2, 3, 4 } },
                new object[] { new List<string> { "view_test_1", "edit_test_3", "edit_test_4" }, new List<int> { 3, 4 }  },
            };

        [Theory]
        [MemberData(nameof(GetEditableArticlesQueryCases))]
        public async Task GetEditableArticlesQueryTestAsync(IEnumerable<string> userRoles, IEnumerable<int> result)
        {
            GetEditableArticlesQuery request = new GetEditableArticlesQuery
            {
                UserRoles = userRoles,
            };
            GetEditableArticlesQueryHandler handler = new GetEditableArticlesQueryHandler(_fixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());
            if (result != null)
            {
                var expectedResultIds = expectedResult.Select(a => a.Id).ToList();
                Assert.Equal(result, expectedResultIds);
            }
            else
            {
                Assert.Null(expectedResult);
            }
        }


        [Fact]
        public async Task GetPublicArticlesQueryTestAsync()
        {
            GetPublicArticlesQuery request = new GetPublicArticlesQuery();
            GetPublicArticlesQueryHandler handler = new GetPublicArticlesQueryHandler(_fixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());
            var expectedResultIds = expectedResult.Select(a => a.Id).ToList();
            Assert.Equal(new List<int> { 4, 5 }, expectedResultIds);
        }

        public static IEnumerable<object[]> GetViewableArticlesQueryCases =>
            new List<object[]>
            {
                new object[] { new List<string> { "edit_test_1" }, new List<int> { 4, 5 } },
                new object[] { new List<string> { "view_test_1" }, new List<int> { 1, 4, 5 } },
                new object[] { new List<string> { "view_test_2", "view_test_3", "view_test_4" }, new List<int> { 2, 3, 4, 5 } },
                new object[] { new List<string> { "edit_test_1", "view_test_3", "view_test_4" }, new List<int> { 3, 4, 5 }  },
            };

        [Theory]
        [MemberData(nameof(GetViewableArticlesQueryCases))]
        public async Task GetViewableArticlesQueryTestAsync(IEnumerable<string> userRoles, IEnumerable<int> result)
        {
            GetViewableArticlesQuery request = new GetViewableArticlesQuery
            {
                UserRoles = userRoles,
            };
            GetViewableArticlesQueryHandler handler = new GetViewableArticlesQueryHandler(_fixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());
            if (result != null)
            {
                var expectedResultIds = expectedResult.Select(a => a.Id).ToList();
                Assert.Equal(result, expectedResultIds);
            }
            else
            {
                Assert.Null(expectedResult);
            }
        }

    }
}
