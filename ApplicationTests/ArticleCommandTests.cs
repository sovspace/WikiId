using Application.Features.ArticleFeatures.Responses;
using ApplicationTests.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace ApplicationTests { 


    public class ArticleCommandContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {

        }
    }

    public class ArticleCommandTests : IClassFixture<ArticleCommandContextFixture>
    {
        private readonly ArticleCommandContextFixture _fixture;
        public ArticleCommandTests(ArticleCommandContextFixture fixture)
        {
            _fixture = fixture;
        }

        public static IEnumerable<object[]> CreateArticleCommandCases =>
            new List<object[]>
            {

            };


        [Theory]
        [MemberData(nameof(CreateArticleCommandCases))]
        public void CreateArticleCommandTest(string title, string titleImagePath, string identityUserId, int categoryId, bool isPublic, CreateArticleResponse result)
        {

        }

        public static IEnumerable<object[]> DeleteArticleCommandCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(DeleteArticleCommandCases))]
        public void DeleteArticleCommandTest(string identityUserId, int articleId, DeleteArticleResponse result)
        {

        }

        public static IEnumerable<object[]> UpdateArticleCommandCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(UpdateArticleCommandCases))]
        public void UpdateArticleCommandTest(int articleId, IEnumerable<string> userRoles, string title, 
            bool isPublic, string titleImagePath, int categoryId, UpdateArticleResponse result)
        {

        }
    }
}
