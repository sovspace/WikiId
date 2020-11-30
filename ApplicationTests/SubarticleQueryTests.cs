using ApplicationTests.Common;
using Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using Application.Features.SubarticleFeatures.Queries;
using static Application.Features.SubarticleFeatures.Queries.GetSubarticleByIdQuery;

namespace ApplicationTests
{
    public class SubarticleQueryContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {
            Article article1 = new Article { Id = 1, ViewRoleString = "view_test_1", EditRoleString = "edit_test_1" };
            Article article2 = new Article { Id = 2, ViewRoleString = "view_test_2", EditRoleString = "edit_test_2" };
            Article article3 = new Article { Id = 3, ViewRoleString = "view_test_3", EditRoleString = "edit_test_3" };
            Context.Articles.Add(article1);
            Context.Articles.Add(article2);
            Context.Articles.Add(article3);

            Subarticle subarticle1 = new Subarticle
            {
                Id = 1,
                Title = "test_title_1",
                Content = "test_content_1",
                Article = article1
            };
            Subarticle subarticle2 = new Subarticle
            {
                Id = 2,
                Title = "test_title_1",
                Content = "test_content_1",
                Article = article2
            };
            Subarticle subarticle3 = new Subarticle
            {
                Id = 3,
                Title = "test_title_1",
                Content = "test_content_1",
                Article = article3
            };
            Context.Subarticles.Add(subarticle1);
            Context.Subarticles.Add(subarticle2);
            Context.Subarticles.Add(subarticle3);
            Context.SaveChangesAsync();
        }
    }

    public class SubarticleQueryTests : IClassFixture<SubarticleQueryContextFixture>
    {
        private readonly SubarticleQueryContextFixture _fixture;

        public SubarticleQueryTests(SubarticleQueryContextFixture fixture)
        {
            _fixture = fixture;
        }


        public static IEnumerable<object[]> GetSubarticleByIdCommandCases =>
            new List<object[]>
            {
                new object[]{1, new List<string> { "edit_test_1", "view_test_1" }, 1},
                new object[]{2, new List<string> { "edit_test_2", "view_test_1" }, null},
                new object[]{4,  new List<string> { "edit_test_4", "view_test_2" }, null},
            };


        [Theory]
        [MemberData(nameof(GetSubarticleByIdCommandCases))]
        public async Task GetSubarticleByIdCommandTestAsync(int subarticleId, IEnumerable<string> userRoles, int? result)
        {
            GetSubarticleByIdQuery request = new GetSubarticleByIdQuery
            {
                SubarticleId = subarticleId,
                UserRoles = userRoles,
            };
            GetSubarticleByIdQueryHandler handler = new GetSubarticleByIdQueryHandler(_fixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());
            Assert.Equal(expectedResult?.Id, result);
        }

    }
}
