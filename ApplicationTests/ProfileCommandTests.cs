using Application.Features.ProfileFeatures.Commands;
using Application.Features.ProfileFeatures.Responses;
using ApplicationTests.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using static Application.Features.ProfileFeatures.Commands.CreateProfileCommand;
using Domain.Entities;
using static Application.Features.ProfileFeatures.Commands.DeleteProfileCommand;

namespace ApplicationTests
{

    public class CreateProfileCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {
            return;
        }
    }

    public class DeleteProfileCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {
            Profile profile1 = new Profile { Id = 1, IdentityUserId = "1" };
            Profile profile2 = new Profile { Id = 2, IdentityUserId = "2" };
            Profile profile3 = new Profile { Id = 3, IdentityUserId = "3" };
            Context.Profiles.Add(profile1);
            Context.Profiles.Add(profile2);
            Context.Profiles.Add(profile3);
            Context.SaveChangesAsync();
        }
    }


    public class ProfileCommandTests : IClassFixture<CreateProfileCommandContextFixture>, IClassFixture<DeleteProfileCommandContextFixture>
    {
        private readonly CreateProfileCommandContextFixture _createFixture;
        private readonly DeleteProfileCommandContextFixture _deleteFixture;

        public ProfileCommandTests(CreateProfileCommandContextFixture createFixture, DeleteProfileCommandContextFixture deleteFixture)
        {
            _createFixture = createFixture;
            _deleteFixture = deleteFixture;
        }


        public static IEnumerable<object[]> CreateProfileCommandCases =>
            new List<object[]>
            {
                new object[]{ "1", new CreateProfileResponse { IsSuccessful = true, Message = "Ok"} },
                new object[]{ "2", new CreateProfileResponse { IsSuccessful = true, Message = "Ok"} }
            };

        [Theory]
        [MemberData(nameof(CreateProfileCommandCases))]
        public async Task CreateProfileCommandTestAsync(string identityUserId, CreateProfileResponse result)
        {
            CreateProfileCommand request = new CreateProfileCommand
            {
                IdentityUserId = identityUserId,
            };
            CreateProfileCommandHandler handler = new CreateProfileCommandHandler(_createFixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            if (result.IsSuccessful)
            {
                Profile profile = _createFixture.Context.Profiles.Find(expectedResult.Id);
                Assert.Equal(profile?.IdentityUserId, identityUserId);
            }
            Assert.Equal(expectedResult.Message, result.Message);

        }

        public static IEnumerable<object[]> DeleteProfileCommandCases =>
            new List<object[]>
            {
                new object[] {"1", new DeleteProfileResponse { IsSuccessful = true, Message = "Ok"} },
                new object[] {"2", new DeleteProfileResponse { IsSuccessful = true, Message = "Ok"} },
                new object[] {"2", new DeleteProfileResponse { IsSuccessful = false, Message = "Profile not found"} }
            };


        [Theory]
        [MemberData(nameof(DeleteProfileCommandCases))]
        public async Task DeleteProfileCommandTestAsync(string identityUserId, DeleteProfileResponse result)
        {
            DeleteProfileCommand request = new DeleteProfileCommand
            {
                IdentityUserId = identityUserId,
            };
            DeleteProfileCommandHandler handler = new DeleteProfileCommandHandler(_deleteFixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            if (result.IsSuccessful)
            {
                Profile profile = _deleteFixture.Context.Profiles.Find(expectedResult.Id);
                Assert.Null(profile);
            }
            Assert.Equal(expectedResult.Message, result.Message);
        }

    }
}
