using Application.Features.ArticleFeatures.Responses;
using Application.Features.ArticleFeatures.Commands;
using ApplicationTests.Common;
using static Application.Features.ArticleFeatures.Commands.CreateArticleCommand;
using static Application.Features.ArticleFeatures.Commands.DeleteArticleCommand;
using static Application.Features.ArticleFeatures.Commands.UpdateArticleCommand;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ApplicationTests
{

    public class CreateArticleCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {
            Category category1 = new Category { Id = 1 };
            Category category2 = new Category { Id = 2 };
            Context.Categories.Add(category1);
            Context.Categories.Add(category2);

            Profile profile1 = new Profile { IdentityUserId = "1" };
            Profile profile2 = new Profile { IdentityUserId = "2" };
            Context.Profiles.Add(profile1);
            Context.Profiles.Add(profile2);

            Context.SaveChangesAsync();
        }
    }
    public class CreateArticleCommandTests : IClassFixture<CreateArticleCommandContextFixture>
    {
        private readonly CreateArticleCommandContextFixture _createFixture;
        public CreateArticleCommandTests(CreateArticleCommandContextFixture createFixture)
        {
            _createFixture = createFixture;
        }

        public static IEnumerable<object[]> CreateArticleCommandCases =>
            new List<object[]>
            {
                new object[] {"test_title_1", "test_path_1", "1", 1, true, new CreateArticleResponse {IsSuccessful = true, Message = "Ok", 
                    EditArticleRoleName = "Edittest_title_1", ViewArticleRoleName = "Viewtest_title_1"} },
                new object[] {"test_title_2", "test_path_2", "2", 3, false, new CreateArticleResponse {IsSuccessful = false, Message = "No category found"} },
                new object[] {"test_title_3", "test_path_3", "3", 2, false, new CreateArticleResponse { IsSuccessful = false, Message = "No user found"} }
            };


        [Theory]
        [MemberData(nameof(CreateArticleCommandCases))]
        public async Task CreateArticleCommandTestAsync(string title,
            string titleImagePath, string identityUserId, int categoryId, bool isPublic, CreateArticleResponse result)
        {
            CreateArticleCommand request = new CreateArticleCommand
            {
                Title = title,
                TitleImagePath = titleImagePath,
                IdentityUserId = identityUserId,
                CategoryId = categoryId,
                IsPublic = isPublic,
            };
            CreateArticleCommandHandler handler = new CreateArticleCommandHandler(_createFixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            if (expectedResult.IsSuccessful)
            {
                Article article = await _createFixture.Context.Articles
                    .Include(a => a.TitleImage)
                    .Include(a => a.Creator)
                    .Where(a => a.Id == expectedResult.ArticleId).SingleOrDefaultAsync();

                Assert.NotNull(article);
                Assert.Equal(article.Title, title);
                Assert.Equal(article.TitleImage.Path, titleImagePath);
                Assert.Equal(article.Creator.IdentityUserId, identityUserId);
                Assert.Equal(article.CategoryId, categoryId);
                Assert.Equal(article.IsPublic, isPublic);
            }
            Assert.Equal(expectedResult.Message, result.Message);
        }

    }

    public class DeleteArticleCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {
            Profile creator1 = new Profile { IdentityUserId = "1" };
            Profile creator2 = new Profile { IdentityUserId = "2" };
            Context.Profiles.Add(creator1);
            Context.Profiles.Add(creator2);
            Article article1 = new Article { Id = 1, ViewRoleString = "view_test_1", EditRoleString = "edit_test_1", Creator = creator1 };
            Article article2 = new Article { Id = 2, ViewRoleString = "view_test_2", EditRoleString = "edit_test_2", Creator = creator2 };
            Context.Articles.Add(article1);
            Context.Articles.Add(article2);
            Profile profile1 = new Profile { IdentityUserId = "3" };
            Context.Profiles.Add(profile1);
            Context.SaveChangesAsync();
        }
    }

    public class DeleteArticleCommandTests : IClassFixture<DeleteArticleCommandContextFixture>
    {
        private readonly DeleteArticleCommandContextFixture _deleteFixture;
        public DeleteArticleCommandTests(DeleteArticleCommandContextFixture deleteFixture)
        {
            _deleteFixture = deleteFixture;
        }

        public static IEnumerable<object[]> DeleteArticleCommandCases =>
            new List<object[]>
            {
                new object[]{ "1", 1, new DeleteArticleResponse { IsSuccessful = true, Message = "Ok", ViewRole = "view_test_1", EditRole = "edit_test_1"} },
                new object[]{ "1", 2, new DeleteArticleResponse { IsSuccessful = false, Message = "Can't delete article"} },
                new object[]{ "2", 1, new DeleteArticleResponse { IsSuccessful = false, Message = "No article found" } }
            };

        [Theory]
        [MemberData(nameof(DeleteArticleCommandCases))]
        public async Task DeleteArticleCommandTestAsync(string identityUserId, int articleId, DeleteArticleResponse result)
        {

            DeleteArticleCommand request = new DeleteArticleCommand
            {
                Id = articleId,
                IdentityUserId = identityUserId,
            };
            DeleteArticleCommandHandler handler = new DeleteArticleCommandHandler(_deleteFixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            if (expectedResult.IsSuccessful)
            {
                Article deletedArticle = _deleteFixture.Context.Articles.Find(expectedResult.Id);
                Assert.Null(deletedArticle);
                Assert.Equal(expectedResult.ViewRole, result.ViewRole);
                Assert.Equal(expectedResult.EditRole, result.EditRole);
            }
            Assert.Equal(expectedResult.Message, result.Message);
        }
    }

    public class UpdateArticleCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {
            Profile creator1 = new Profile { IdentityUserId = "1" };
            Profile creator2 = new Profile { IdentityUserId = "2" };
            Context.Profiles.Add(creator1);
            Context.Profiles.Add(creator2);

            Category category1 = new Category { Id = 1 };
            Category category2 = new Category { Id = 2 };
            Context.Categories.Add(category1);
            Context.Categories.Add(category2);

            MediaFile mediaFile1 = new MediaFile { Path = "test_path_1" };
            MediaFile mediaFile2 = new MediaFile { Path = "test_path_2" };
            Context.MediaFiles.Add(mediaFile1);
            Context.MediaFiles.Add(mediaFile2);

            Article article1 = new Article { Id = 1, TitleImage = mediaFile1, 
                ViewRoleString = "view_test_1", EditRoleString = "edit_test_1", Creator = creator1 };
            Article article2 = new Article { Id = 2, TitleImage = mediaFile2, 
                ViewRoleString = "view_test_2", EditRoleString = "edit_test_2", Creator = creator2 };
            Context.Articles.Add(article1);
            Context.Articles.Add(article2);

            Profile profile1 = new Profile { IdentityUserId = "3" };
            Context.Profiles.Add(profile1);

            Context.SaveChangesAsync();
        }
    }

    public class UpdateArticleCommandTests : IClassFixture<UpdateArticleCommandContextFixture>
    {
        private readonly UpdateArticleCommandContextFixture _updateFixture;
        public UpdateArticleCommandTests(UpdateArticleCommandContextFixture updateFixture)
        {
            _updateFixture = updateFixture;
        }
        public static IEnumerable<object[]> UpdateArticleCommandCases =>
            new List<object[]>
                {
                    new object[]{1, new List<string> { "edit_test_1", "some_role"}, "new_title_1", true, "new_image_path_1", 2,
                        new UpdateArticleResponse {IsSuccessful = true, Message = "Ok"} },
                    new object[]{1, new List<string> { "edit_test_1", "some_role"}, "new_title_1", true, "new_image_path_1", 5,
                        new UpdateArticleResponse {IsSuccessful = false, Message = "No category found"} },
                    new object[]{2, new List<string> { "some_role", "view_test_1"}, "new_title_2", false, "new_image_path_2", 1,
                        new UpdateArticleResponse { IsSuccessful = false, Message = "Can't update this"} },
                    new object[]{3, new List<string> { "edit_test_1", "some_role"}, "new_title_1", true, "new_image_path_1", 2,
                        new UpdateArticleResponse {IsSuccessful = false, Message = "No article found"} }
                };
        
        [Theory]
        [MemberData(nameof(UpdateArticleCommandCases))]
        public async Task UpdateArticleCommandTestAsync(int articleId, IEnumerable<string> userRoles, string title,
            bool isPublic, string titleImagePath, int categoryId, UpdateArticleResponse result)
        {
            UpdateArticleCommand request = new UpdateArticleCommand
            {
                ArticleId = articleId,
                Title = title,
                TitleImagePath = titleImagePath,
                UserRoles = userRoles,
                CategoryId = categoryId,
                IsPublic = isPublic,
            };
            UpdateArticleCommandHandler handler = new UpdateArticleCommandHandler(_updateFixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            if (expectedResult.IsSuccessful)
            {
                Article article = await _updateFixture.Context.Articles
                    .Include(a => a.TitleImage)
                    .Include(a => a.Creator)
                    .Where(a => a.Id == expectedResult.ArticleId).SingleOrDefaultAsync();

                Assert.NotNull(article);
                Assert.Equal(article.Title, title);
                Assert.Equal(article.TitleImage.Path, titleImagePath);
                Assert.Equal(article.CategoryId, categoryId);
                Assert.Equal(article.IsPublic, isPublic);
            }
            Assert.Equal(expectedResult.Message, result.Message);
        }
                
    }

}
