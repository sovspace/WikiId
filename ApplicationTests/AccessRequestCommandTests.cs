using Application.Features.AccessRequstFeatures.Commands;
using Application.Features.AccessRequstFeatures.Responses;
using ApplicationTests.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using static Application.Features.AccessRequstFeatures.Commands.CreateAccessRequestCommand;
using static Application.Features.AccessRequstFeatures.Commands.DeleteAccessRequestCommand;

namespace ApplicationTests
{
    public class AccessRequestCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {

            Context.Profiles.Add(new Profile { IdentityUserId = "1" });
            Context.Profiles.Add(new Profile { IdentityUserId = "2" });
            Context.Profiles.Add(new Profile { IdentityUserId = "3" });

            Context.Articles.Add(new Article { Id = 1, EditRoleString = "edit_1", ViewRoleString = "view_1"});
            Context.Articles.Add(new Article { Id = 2, EditRoleString = "edit_2", ViewRoleString = "view_2"});
            Context.Articles.Add(new Article { Id = 3, EditRoleString = "edit_3", ViewRoleString = "view_3"});

            // Delete data
            Profile p5 = new Profile { IdentityUserId = "5" };
            Context.Profiles.Add(p5);
            Profile p6 = new Profile { IdentityUserId = "6" };
            Context.Profiles.Add(p6);

            Article a5 = new Article { Id = 5, EditRoleString = "edit_5", ViewRoleString = "view_5", Creator = p5 };

            Context.Articles.Add(a5);

            Article a6 = new Article { Id = 6, EditRoleString = "edit_6", ViewRoleString = "view_6", Creator = p6 };
            Context.Articles.Add(a6);

            Profile p7 = new Profile { IdentityUserId = "7" };
            Context.Profiles.Add(p7);
            Context.AccessRequests.Add(new AccessRequest { Id = 5, Profile = p7, Article = a5 });

            Profile p8 = new Profile { IdentityUserId = "8" };
            Context.Profiles.Add(p8);
            Context.AccessRequests.Add(new AccessRequest { Id = 6, Profile = p8, Article = a6 });

            Context.SaveChangesAsync();
        }
    }

    public class AccessRequestCommandTests : IClassFixture<AccessRequestCommandContextFixture>
    {
        private readonly AccessRequestCommandContextFixture _fixture;
        public AccessRequestCommandTests(AccessRequestCommandContextFixture fixture)
        {
            _fixture = fixture;
        }

        public static IEnumerable<object[]> CreateAccessRequestCommandCases =>
            new List<object[]> 
            {
                new object[] {"1", new List<string>{"some_role", "view_1"}, 1, AccessType.Edit, 
                    new CreateAccessRequestResponse { IsSuccessful = true, Message = "Ok"} },
                new object[] {"5", new List<string>{"some_role", "view_1"}, 1, AccessType.Edit,
                    new CreateAccessRequestResponse {IsSuccessful = false, Message = "Profile not found"} },
                new object[] {"3", new List<string> {"edit_2", "some_role"}, 2, AccessType.Edit,
                    new CreateAccessRequestResponse {IsSuccessful = false, Message = "You have already this role"} },
                new object[] {"3", new List<string> {"edit_2", "some_role"}, 4, AccessType.Edit,
                    new CreateAccessRequestResponse {IsSuccessful = false, Message = "Article not found"}}
            };


        [Theory]
        [MemberData(nameof(CreateAccessRequestCommandCases))]
        public async System.Threading.Tasks.Task CreateAccessRequestCommandTestAsync(string identityUserId, IEnumerable<string> userRoles, 
            int articleId, AccessType accessType, CreateAccessRequestResponse result)
        {
            CreateAccessRequestCommand request = new CreateAccessRequestCommand
            {
                IdentityUserId = identityUserId,
                UserRoles = userRoles,
                ArticleId = articleId,
                AccessType = accessType,
            };
            CreateAccessRequestCommandHandler handler = new CreateAccessRequestCommandHandler(_fixture.Context);
            CreateAccessRequestResponse expectedResult = await handler.Handle(request, new System.Threading.CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            Assert.Equal(expectedResult.Message, result.Message);
        }

        public class DeleteAcceessRequestCommandCase
        {
            public int AccessRequstId { get; set; }
            public string IdentityUserId { get; set; }
            public DeleteAccessRequestResponse Result { get; set; }
        }

        public static IEnumerable<object[]> DeleteAccessRequestCommandCases =>
            new List<object[]>
            {
                new object[] {5, "5", new DeleteAccessRequestResponse { IsSuccessful = true, Message = "Ok"} },
                new object[] {6, "5", new DeleteAccessRequestResponse { IsSuccessful = false, Message = "Can't delete this access request"} }
            };

        [Theory]
        [MemberData(nameof(DeleteAccessRequestCommandCases))]
        public async System.Threading.Tasks.Task DeleteAccessRequestCommandTestAsync(int accessRequestId, string identityUserId, DeleteAccessRequestResponse result)
        {
            DeleteAccessRequestCommand request = new DeleteAccessRequestCommand
            {
                AccessRequstId = accessRequestId,
                IdentityUserId = identityUserId,
            };
            DeleteAccessRequestCommandHandler handler = new DeleteAccessRequestCommandHandler(_fixture.Context);
            DeleteAccessRequestResponse expectedResult = await handler.Handle(request, new System.Threading.CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            Assert.Equal(expectedResult.Message, result.Message);
        }


    }
}
