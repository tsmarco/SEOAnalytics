using Xunit;
using InfoTrackSEOAnalytics.Search;
using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using System.Net.Http;
using Moq;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Reflection;

namespace InfoTrackSEOAnalytics.Search.Tests
{

    public class BingSearchServiceTests
    {
        private BingSearchService subject;
        private string BingSearchResponse;

        public BingSearchServiceTests()
        {
            //Arrange
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            var path = Path.Combine(dirPath, @"Search\ExampleData\BingSearchExampleResponse.html");
            BingSearchResponse = System.IO.File.ReadAllText(path);

            Mock<IHttpClientFactory> mockFactory = GenerateMockedHTTPMessageHandler(BingSearchResponse);
            subject = new BingSearchService(mockFactory.Object);
            //_bingSearchService = bingSearchService;
        }

        [Fact()]
        public void SearchTest()
        {
            // Arrange
            var searchTerm = "InfoTrack";
            var expected = BingSearchResponse;

            // Act
            var output = subject.Search(searchTerm, 1);

            // Assert
            output.ShouldNotBeNull();
            output.ShouldBe(expected);
        }

        [Fact()]
        public void SearchMultiplePagesTest()
        {
            // Arrange
            var searchTerm = "InfoTrack";
            var expected = BingSearchResponse + BingSearchResponse;

            // Act
            var output = subject.SearchMultiplePages(searchTerm, 2);

            // Assert
            output.ShouldNotBeNull();
            output.ShouldBe(expected);


        }

        [Fact()]
        public void GetRowLinksTest()
        {
            // Arrange
            var expected = 7;

            // Act
            var output = subject.GetRowLinks(BingSearchResponse, 50);

            // Assert
            output.ShouldNotBeNull();
            output.Count.ShouldBe(expected);
        }

        [Fact()]
        public void GetRowLinksShouldLimitRowsTest()
        {
            // Arrange
            var expected = 3;

            // Act
            var output = subject.GetRowLinks(BingSearchResponse, 3);

            // Assert
            output.ShouldNotBeNull();
            output.Count.ShouldBe(expected);
        }

        [Fact()]
        public void GetRowLinksShouldReturnEmptyListOnNoHTMLTest()
        {
            // Arrange
            var expected = 0;

            // Act
            var output = subject.GetRowLinks(null, 50);

            // Assert
            output.ShouldNotBeNull();
            output.Count.ShouldBe(expected);
        }

        [Fact()]
        public void GetFiltedRowsIndexesTest()
        {
            // Arrange
            var links = new List<string> { "www.Infotrack.com", "www.google.com", "Infotrack.net" };
            var searchterm = "Infotrack";
            var expected = new List<int> { 1, 3 };

            // Act
            var output = subject.GetFiltedRowsIndexes(links, searchterm);

            // Assert
            output.ShouldNotBeNull();
            output.Count.ShouldBeGreaterThan(0);
            output.ShouldBe(expected);
        }

        [Fact()]
        public void GetFiltedRowsIndexesShouldAcceptEmptyLinksListTest()
        {
            // Arrange
            var links = new List<string>();
            var searchterm = "Infotrack";
            var expected = new List<int> { 0 };

            // Act
            var output = subject.GetFiltedRowsIndexes(links, searchterm);

            // Assert
            output.ShouldNotBeNull();
            output.ShouldBe(expected);
        }

        [Fact()]
        public void GetFiltedRowsIndexesShouldAcceptMissingSearchTermTest()
        {
            // Arrange
            var links = new List<string> { "www.Infotrack.com", "www.google.com", "Infotrack.net" };
            var searchterm = String.Empty;
            var expected = new List<int> { 1,2,3 };

            // Act
            var output = subject.GetFiltedRowsIndexes(links, searchterm);

            // Assert
            output.ShouldNotBeNull();
            output.ShouldBe(expected);
        }

        private static Mock<IHttpClientFactory> GenerateMockedHTTPMessageHandler(string response)
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(response),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            return mockFactory;
        }

    }
}