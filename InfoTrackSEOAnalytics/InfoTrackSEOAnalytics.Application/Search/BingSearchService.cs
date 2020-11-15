using InfoTrackSiteAnalytics;
using System.Net.Http;

namespace InfoTrackSEOAnalytics.Search
{
    /// <summary>
    /// BingSearchService provides logic for gathering data from Bing search engine.
    /// </summary>
    public class BingSearchService : SearchService, IBingSearchService
    {
        public BingSearchService(IHttpClientFactory clientFactory)
        {
            endPoint = AppConsts.BingServiceURL;
            rowRegex = AppConsts.BingServiceRowRegex;
            _clientFactory = clientFactory;
        }
    }
}
