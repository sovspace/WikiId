using ApplicationTests.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApplicationTests
{
    public class ArticleQueryContextFixture : BaseContextFixture
    {
        public override void SeedDataAsync()
        {

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

            };

        [Theory]
        [MemberData(nameof(GetArticleByIdQueryCases))]
        public void GetArticleByIdQueryTest(int articleId, IEnumerable<string> userRoles, Article article)
        {

        }

        public static IEnumerable<object[]> GetCreatedArticlesQueryCases =>
            new List<object[]>
            {

            };


        [Theory]
        [MemberData(nameof(GetCreatedArticlesQueryCases))]
        public void GetCreatedArticlesQueryTest(string identityUserId)
        {

        }

        public static IEnumerable<object[]> GetEditableArticlesQueryCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(GetEditableArticlesQueryCases))]
        public void GetEditableArticlesQueryTest()
        {

        }

        public static IEnumerable<object[]> GetPublicArticlesQueryCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(GetPublicArticlesQueryCases))]
        public void GetPublicArticlesQueryTest()
        {

        }

        public static IEnumerable<object[]> GetViewableArticlesQueryCases =>
            new List<object[]>
            {

            };

        [Theory]
        [MemberData(nameof(GetViewableArticlesQueryCases))]
        public void GetViewableArticlesQueryTest()
        {

        }

    }
}
