using Application.Features.SubarticleFeatures.Commands;
using Application.Features.SubarticleFeatures.Responses;
using ApplicationTests.Common;
using Domain.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Xunit;
using static Application.Features.SubarticleFeatures.Commands.CreateSubarticleCommand;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Application.Features.SubarticleFeatures.Commands.DeleteSubarticleCommand;
using static Application.Features.SubarticleFeatures.Commands.UpdateSubarticleCommand;

namespace ApplicationTests
{

    public class CreateSubarticleCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {
            Article article1 = new Article { Id = 1, ViewRoleString = "view_test_1", EditRoleString = "edit_test_1"};
            Article article2 = new Article { Id = 2, ViewRoleString = "view_test_2", EditRoleString = "edit_test_2"};
            Article article3 = new Article { Id = 3, ViewRoleString = "view_test_3", EditRoleString = "edit_test_3" };
            Context.Articles.Add(article1);
            Context.Articles.Add(article2);
            Context.Articles.Add(article3);
            Context.SaveChangesAsync();
        }
    }


    public class DeleteSubarticleCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {
            Article article1 = new Article { Id = 1, ViewRoleString = "view_test_1", EditRoleString = "edit_test_1" };
            Article article2 = new Article { Id = 2, ViewRoleString = "view_test_2", EditRoleString = "edit_test_2" };
            Article article3 = new Article { Id = 3, ViewRoleString = "view_test_3", EditRoleString = "edit_test_3" };
            Context.Articles.Add(article1);
            Context.Articles.Add(article2);
            Context.Articles.Add(article3);

            Subarticle subarticle1 = new Subarticle { Id = 1, Article = article1 };
            Subarticle subarticle2 = new Subarticle { Id = 2, Article = article2 };
            Subarticle subarticle3 = new Subarticle { Id = 3, Article = article3 };
            Context.Subarticles.Add(subarticle1);
            Context.Subarticles.Add(subarticle2);
            Context.Subarticles.Add(subarticle3);
            Context.SaveChangesAsync();
        }
    }


    public class UpdateSubarticleCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {
            Article article1 = new Article { Id = 1, ViewRoleString = "view_test_1", EditRoleString = "edit_test_1" };
            Article article2 = new Article { Id = 2, ViewRoleString = "view_test_2", EditRoleString = "edit_test_2" };
            Article article3 = new Article { Id = 3, ViewRoleString = "view_test_3", EditRoleString = "edit_test_3" };
            Context.Articles.Add(article1);
            Context.Articles.Add(article2);
            Context.Articles.Add(article3);

            Subarticle subarticle1 = new Subarticle { Id = 1, Title = "test_title_1", Content = "test_content_1",
                Article = article1 };
            Subarticle subarticle2 = new Subarticle { Id = 2, Title = "test_title_1", Content = "test_content_1",
                Article = article2 };
            Subarticle subarticle3 = new Subarticle { Id = 3, Title = "test_title_1", Content = "test_content_1",
                Article = article3 };
            Context.Subarticles.Add(subarticle1);
            Context.Subarticles.Add(subarticle2);
            Context.Subarticles.Add(subarticle3);
            Context.SaveChangesAsync();
        }
    }

    public class SubarticleCommandTests : IClassFixture<CreateSubarticleCommandContextFixture>, IClassFixture<DeleteSubarticleCommandContextFixture>,
        IClassFixture<UpdateSubarticleCommandContextFixture>
    {
        private readonly CreateSubarticleCommandContextFixture _createFixture;
        private readonly DeleteSubarticleCommandContextFixture _deleteFixture;
        private readonly UpdateSubarticleCommandContextFixture _updateFixture;

        public SubarticleCommandTests(CreateSubarticleCommandContextFixture createFixture, DeleteSubarticleCommandContextFixture deleteFixture, 
            UpdateSubarticleCommandContextFixture updateFixture)
        {
            _createFixture = createFixture;
            _deleteFixture = deleteFixture;
            _updateFixture = updateFixture;
        }

        public static IEnumerable<object[]> CreateSubarticleCommandCases =>
            new List<object[]>
            {
                new object[] { 2, new List<string> { "some_role", "edit_test_2"}, "test_title_1", "test_content_1", 
                    new CreateSubarticleResponse { IsSuccessful = true, Message = "Ok"} },
                new object[] { 4, new List<string> { "some_role", "view_test_3"}, "test_title_2", "test_content_2",
                    new CreateSubarticleResponse { IsSuccessful = false, Message = "No article found"}},
                new object[] { 1, new List<string> { "some_role", "edit_test_3", "edit_test_2", }, "test_title_3", "test_content_3",
                    new CreateSubarticleResponse { IsSuccessful = false, Message = "Can't edit article"}},

            };

        [Theory]
        [MemberData(nameof(CreateSubarticleCommandCases))]
        public async Task CreateSubarticleCommandTestAsync(int articleId, IEnumerable<string> userRoles, string title, string content, CreateSubarticleResponse result)
        {
            CreateSubarticleCommand request = new CreateSubarticleCommand
            {
                ArticleId = articleId,
                Content = content,
                Title = title,
                UserRoles = userRoles,
            };
            CreateSubarticleCommandHandler handler = new CreateSubarticleCommandHandler(_createFixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            if (expectedResult.IsSuccessful)
            {
                Subarticle subarticle = await _createFixture.Context.Subarticles
                    .Include(s => s.Article)
                    .Where(s => s.Id == expectedResult.Id)
                    .SingleOrDefaultAsync();
                Assert.Equal(subarticle?.Article?.Id, articleId);
                Assert.Equal(subarticle?.Title, title);
                Assert.Equal(subarticle?.Content, content);
            }
            Assert.Equal(expectedResult.Message, result.Message);

        }

        public static IEnumerable<object[]> DeleteSubarticleCommandCases =>
            new List<object[]>
            {
                new object[]{ 1, new List<string> { "edit_test_1", "some_role"},
                    new DeleteSubarticleResponse {IsSuccessful = true, Message = "Ok"} },
                new object[]{4, new List<string> { "edit_test_1", "some_role"},
                    new DeleteSubarticleResponse{ IsSuccessful = false, Message = "No subarticle found"} },
                new object[]{3, new List<string> { "edit_test_2", "some_role"},
                    new DeleteSubarticleResponse { IsSuccessful = false, Message = "Can't delete subarticle"} }
            };

        [Theory]
        [MemberData(nameof(DeleteSubarticleCommandCases))]
        public async Task DeleteSubarticleCommandTestAsync(int subarticleId, IEnumerable<string> userRoles, DeleteSubarticleResponse result)
        {
            DeleteSubarticleCommand request = new DeleteSubarticleCommand
            {
                SubarticleId = subarticleId,
                UserRoles = userRoles,
            };
            DeleteSubarticleCommandHandler handler = new DeleteSubarticleCommandHandler(_deleteFixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            if (expectedResult.IsSuccessful)
            {
                Subarticle subarticle = await _deleteFixture.Context.Subarticles
                    .Include(s => s.Article)
                    .Where(s => s.Id == expectedResult.Id)
                    .SingleOrDefaultAsync();
                Assert.Null(subarticle);
            }
            Assert.Equal(expectedResult.Message, result.Message);

        }

        public static IEnumerable<object[]> UpdateSubarticleCommandCases =>
            new List<object[]>
            {
                new object[]{ 1, new List<string> { "edit_test_1", "some_role"}, "new_title_1", "new_content_1",
                    new UpdateSubarticleResponse {IsSuccessful = true, Message = "Ok"} },
                new object[]{4, new List<string> { "edit_test_1", "some_role"}, "new_title_2", "new_content_2",
                    new UpdateSubarticleResponse{ IsSuccessful = false, Message = "No subarticle found"} },
                new object[]{3, new List<string> { "edit_test_2", "some_role"}, "new_title_3", "new_content_3",
                    new UpdateSubarticleResponse { IsSuccessful = false, Message = "Can't update subarticle"} }
            };

        [Theory]
        [MemberData(nameof(UpdateSubarticleCommandCases))]
        public async Task UpdateSubarticleCommandTestAsync(int subarticleId, IEnumerable<string> userRoles, string title, string content, UpdateSubarticleResponse result)
        {
            UpdateSubarticleCommand request = new UpdateSubarticleCommand
            {
                SubarticleId = subarticleId,
                Content = content,
                Title = title,
                UserRoles = userRoles,
            };
            UpdateSubarticleCommandHandler handler = new UpdateSubarticleCommandHandler(_updateFixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            if (expectedResult.IsSuccessful)
            {
                Subarticle subarticle = await _updateFixture.Context.Subarticles
                    .Where(s => s.Id == expectedResult.Id)
                    .SingleOrDefaultAsync();
                Assert.Equal(subarticle?.Id, subarticleId);
                Assert.Equal(subarticle?.Title, title);
                Assert.Equal(subarticle?.Content, content);
            }
            Assert.Equal(expectedResult.Message, result.Message);
        }

    }
}
