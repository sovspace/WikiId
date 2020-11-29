using ApplicationTests.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApplicationTests
{

    /*  class AccessRequestQueryContextFixture : BaseContextFixture
      {

          public override void SeedDataAsync()
          {
              Profile creator1 = new Profile { IdentityUserId = "1" };
              Profile creator2 = new Profile { IdentityUserId = "2" };

              Context.Profiles.Add(creator1);
              Context.Profiles.Add(creator2);

              Article article1 = new Article { Creator = creator1, };



              // AccessRequest accessRequest1 = new Pro


              Context.SaveChangesAsync();
          }
      }

      public class AccessRequestQueryTests : IClassFixture<AccessRequestQueryContextFixture>
      {
          private readonly AccessRequestCreateCommandContextFixture _fixture;
          public AccessRequestQueryTests(AccessRequestCreateCommandContextFixture fixture)
          {
              fixture = _fixture;
          }

          public static IEnumerable<object[]> GetAccessRequestByIdQueryCases =>
              new List<object[]>
              {
                  new object[] { 1, 2},
              };


          [Theory]
          [MemberData(nameof(GetAccessRequestByIdQueryCases))]
          public void GetAccessRequestByIdQueryTest(string identityUserId, int accessRequestId, AccessRequest result)
          {

          }

      }
      */
}
