using ApplicationTests.Common;
using Application.Features.AccessRequstFeatures.Queries;
using static Application.Features.AccessRequstFeatures.Queries.GetAccessRequestByIdQuery;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using System.Threading.Tasks;

namespace ApplicationTests
{

    public class AccessRequestQueryContextFixture : BaseContextFixture
    {

        public override void SeedDataAsync()
        {
            Profile creator1 = new Profile { IdentityUserId = "1" };
            Profile creator2 = new Profile { IdentityUserId = "2" };
            Context.Profiles.Add(creator1);
            Context.Profiles.Add(creator2);

            Article article1 = new Article { Id = 1, Creator = creator1 };
            Article article2 = new Article { Id = 2, Creator = creator2 };
            Context.Articles.Add(article1);
            Context.Articles.Add(article2);

            Profile profile1 = new Profile { IdentityUserId = "3" };
            Profile profile2 = new Profile { IdentityUserId = "4" };
            Context.Profiles.Add(profile1);
            Context.Profiles.Add(profile2);

            AccessRequest accessRequest1 = new AccessRequest { Id = 1, Profile = profile1, Article = article1 };
            AccessRequest accessRequest2 = new AccessRequest { Id = 2, Profile = profile2, Article = article2 };
            Context.AccessRequests.Add(accessRequest1);
            Context.AccessRequests.Add(accessRequest2);

            Context.SaveChangesAsync();
        }
    }

    public class AccessRequestQueryTests : IClassFixture<AccessRequestQueryContextFixture>
    {
        private readonly AccessRequestQueryContextFixture _fixture;
        public AccessRequestQueryTests(AccessRequestQueryContextFixture fixture)
        {
            _fixture = fixture;
        }

        public static IEnumerable<object[]> GetAccessRequestByIdQueryCases =>
            new List<object[]>
            {
                  new object[] { "1", 1, true},
                  new object[] { "3", 2, false}
            };

        [Theory]
        [MemberData(nameof(GetAccessRequestByIdQueryCases))]
        public async Task GetAccessRequestByIdQueryTestAsync(string identityUserId, int accessRequestId, bool result)
        {
            GetAccessRequestByIdQuery request = new GetAccessRequestByIdQuery
            {
                AccessRequestId = accessRequestId,
                IdentityUserId = identityUserId,
            };
            GetAccessRequestByIdCommandHandler handler = new GetAccessRequestByIdCommandHandler(_fixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());
            Assert.Equal(result, expectedResult != null);
        }

    }

}
