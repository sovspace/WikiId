using Application.Features.CategoryFeatures.Commands;
using Application.Features.CategoryFeatures.Responses;
using ApplicationTests.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.Features.CategoryFeatures.Commands.CreateCategoryCommand;
using static Application.Features.CategoryFeatures.Commands.DeleteCategoryCommand;
using static Application.Features.CategoryFeatures.Commands.UpdateCategoryCommand;

namespace ApplicationTests
{

    public class CreateCategoryCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {
            Profile profile1 = new Profile { IdentityUserId = "1" };
            Profile profile2 = new Profile { IdentityUserId = "2" };
            Context.Profiles.Add(profile1);
            Context.Profiles.Add(profile2);
            Context.SaveChangesAsync();
        }
    }

    public class DeleteCategoryCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {

            Profile profile1 = new Profile { IdentityUserId = "1" };
            Profile profile2 = new Profile { IdentityUserId = "2" };
            Profile profile3 = new Profile { IdentityUserId = "3" };
            Context.Profiles.Add(profile1);
            Context.Profiles.Add(profile2);
            Context.Profiles.Add(profile3);

            Category category1 = new Category { Id = 1, Creator = profile1 };
            Category category2 = new Category { Id = 2, Creator = profile2 };
            Context.Categories.Add(category1);
            Context.Categories.Add(category2);
            Context.SaveChangesAsync();
        }
    }

    public class UpdateCategoryCommandContextFixture : BaseContextFixture
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
            Context.MediaFiles.Add(mediaFile1);
            Context.MediaFiles.Add(mediaFile2);

            Category category1 = new Category { Id = 1, Creator = profile1, TitleImage = mediaFile1 };
            Category category2 = new Category { Id = 2, Creator = profile2, TitleImage = mediaFile2 };
            Context.Categories.Add(category1);
            Context.Categories.Add(category2);
            Context.SaveChangesAsync();
        }
    }


    public class CategoryCommandTests : IClassFixture<CreateCategoryCommandContextFixture>, IClassFixture<DeleteCategoryCommandContextFixture>, 
        IClassFixture<UpdateCategoryCommandContextFixture>
    {
        private readonly CreateCategoryCommandContextFixture _createFixture;
        private readonly DeleteCategoryCommandContextFixture _deleteFixture;
        private readonly UpdateCategoryCommandContextFixture _updateFixture;

        public CategoryCommandTests(CreateCategoryCommandContextFixture createFixture,
            DeleteCategoryCommandContextFixture deleteFixture, UpdateCategoryCommandContextFixture updateFixture)
        {
            _createFixture = createFixture;
            _deleteFixture = deleteFixture;
            _updateFixture = updateFixture;
        }


        public static IEnumerable<object[]> CreateCategoryCommandCases =>
            new List<object[]>
            {
                new object[] { "1", "test_title_1", "test_title_path_1", new CreateCategoryResponse { IsSuccessful = true, Message = "Ok"} },
                new object[] { "4", "test_title_2", "test_title_path_2", new CreateCategoryResponse { IsSuccessful = false, Message = "No profile found"} }
            };


        [Theory]
        [MemberData(nameof(CreateCategoryCommandCases))]
        public async Task CreateCategoryCommandTestAsync(string identityUserId, string title, string titleImagePath, CreateCategoryResponse result)
        {
            CreateCategoryCommand request = new CreateCategoryCommand
            {
                IdentityUserId = identityUserId,
                Title = title,
                TitleImagePath = titleImagePath,
            };
            CreateCategoryCommandHandler handler = new CreateCategoryCommandHandler(_createFixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            if (expectedResult.IsSuccessful)
            {
                Category category = await _createFixture.Context.Categories
                    .Include(c => c.Creator)
                    .Include(c => c.TitleImage)
                    .Where(c => c.Id == expectedResult.Id)
                    .SingleOrDefaultAsync();

                Assert.Equal(category.Title, title);
                Assert.Equal(category.TitleImage.Path, titleImagePath);
                Assert.Equal(category.Creator.IdentityUserId, identityUserId);
            }
            Assert.Equal(expectedResult.Message, result.Message);

        }

        public static IEnumerable<object[]> DeleteCategoryCommandCases =>
            new List<object[]>
            {
                new object[] { "1", 1, new DeleteCategoryResponse { IsSuccessful = true, Message = "Ok"} },
                new object[] { "2", 3, new DeleteCategoryResponse { IsSuccessful = false, Message = "No category found"} },
                new object[] { "1", 2, new DeleteCategoryResponse { IsSuccessful = false, Message = "Can't delete category" } }
            };

        [Theory]
        [MemberData(nameof(DeleteCategoryCommandCases))]
        public async Task DeleteCategoryCommandTestAsync(string identityUserId, int categoryId, DeleteCategoryResponse result)
        {
            DeleteCategoryCommand request = new DeleteCategoryCommand
            {
                IdentityUserId = identityUserId,
                CategoryId = categoryId,
            };
            DeleteCategoryCommandHandler handler = new DeleteCategoryCommandHandler(_deleteFixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            if (expectedResult.IsSuccessful)
            {
                Category category = await _deleteFixture.Context.Categories.FindAsync(expectedResult.Id);
                Assert.Null(category);
            }
            Assert.Equal(expectedResult.Message, result.Message);
        }

        public static IEnumerable<object[]> UpdateCategoryCommandCases =>
            new List<object[]>
            {
                new object[] { "1", 1, "new_title_1", "new_image_path_1", new CreateCategoryResponse { IsSuccessful = true, Message = "Ok" } },
                new object[] { "3", 2, "new_title_2", "new_image_path_2", new CreateCategoryResponse { IsSuccessful = false, Message = "Can't update category" } },
                new object[] { "2", 3, "new_title_3", "new_image_path_3", new CreateCategoryResponse { IsSuccessful = false, Message = "Category is not found" } }
            };

        [Theory]
        [MemberData(nameof(UpdateCategoryCommandCases))]
        public async Task UpdateCategoryCommandTestAsync(string identityUserId, int categoryId, string title, string titleImagePath, CreateCategoryResponse result)
        {
            UpdateCategoryCommand request = new UpdateCategoryCommand
            {
                CategoryId = categoryId,
                IdentityUserId = identityUserId,
                Title = title,
                TitleImagePath = titleImagePath,
            };
            UpdateCategoryCommandHandler handler = new UpdateCategoryCommandHandler(_updateFixture.Context);
            var expectedResult = await handler.Handle(request, new CancellationToken());

            Assert.Equal(expectedResult.IsSuccessful, result.IsSuccessful);
            if (expectedResult.IsSuccessful)
            {
                Category category = await _updateFixture.Context.Categories
                    .Include(c => c.Creator)
                    .Include(c => c.TitleImage)
                    .Where(c => c.Id == expectedResult.Id)
                    .SingleOrDefaultAsync();

                Assert.Equal(category.Title, title);
                Assert.Equal(category.TitleImage.Path, titleImagePath);
                Assert.Equal(category.Creator.IdentityUserId, identityUserId);
            }
            Assert.Equal(expectedResult.Message, result.Message);
        }

    }
}
