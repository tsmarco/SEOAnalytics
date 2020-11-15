using InfoTrackSiteAnalytics;
using System.Net.Http;

namespace InfoTrackSEOAnalytics.Search
{
    /// <summary>
    /// GoogleSearchService provides logic for gathering data from Google search engine.
    /// </summary>
    public class GoogleSearchService : SearchService, IGoogleSearchService
    {
        public GoogleSearchService(IHttpClientFactory clientFactory)
        {
            endPoint = AppConsts.GoogleServiceURL;
            rowRegex = AppConsts.GoogleServiceRowRegex;
            _clientFactory = clientFactory;
        }

    } 
}
