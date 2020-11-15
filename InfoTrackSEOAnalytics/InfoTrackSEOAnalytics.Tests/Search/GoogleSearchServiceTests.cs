using Xunit;
using System;
using System.Collections.Generic;
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

    public class GoogleSearchServiceTests
    {
        private GoogleSearchService subject;
        private string GoogleSearchResponse;

        public GoogleSearchServiceTests()
        {
            //Arrange
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            var path = Path.Combine(dirPath, @"Search\ExampleData\GoogleSearchExampleResponse.html");
            GoogleSearchResponse = System.IO.File.ReadAllText(path);

            Mock<IHttpClientFactory> mockFactory = GenerateMockedHTTPMessageHandler(GoogleSearchResponse);
            subject = new GoogleSearchService(mockFactory.Object);
            //_bingSearchService = bingSearchService;
        }

        [Fact()]
        public void SearchTest()
        {
            // Arrange
            var searchTerm = "InfoTrack";
            var expected = GoogleSearchResponse;

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
            var expected = GoogleSearchResponse + GoogleSearchResponse;

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
            var expected = 10;

            // Act
            var output = subject.GetRowLinks(GoogleSearchResponse, 50);

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
            var output = subject.GetRowLinks(GoogleSearchResponse, 3);

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