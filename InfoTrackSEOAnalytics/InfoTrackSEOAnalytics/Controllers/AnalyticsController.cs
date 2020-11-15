using InfoTrackSEOAnalytics.Search;
using InfoTrackSiteAnalytics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace InfoTrackSEOAnalytics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IGoogleSearchService _googleSearchService;
        private readonly IBingSearchService _bingSearchService;


        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(ILogger<AnalyticsController> logger, IGoogleSearchService googleSearchService, IBingSearchService bingSearchService)
        {
            _logger = logger;
            _googleSearchService = googleSearchService;
            _bingSearchService = bingSearchService;
        }

        [HttpGet]
        public List<int> Get(string searchTerm, string targetTag = "www.infotrack.com.au", string searchProvider = AppConsts.Google)
        {
            if (string.IsNullOrEmpty(searchTerm)) throw new ArgumentException($"A search term was not entered.");
            if (string.IsNullOrEmpty(targetTag)) throw new ArgumentException($"A target tag was not entered.");

            switch (searchProvider)
            {
                case (AppConsts.Google):
                    var googleSearchHtml = _googleSearchService.SearchMultiplePages(searchTerm, 5);
                    var googleRowLinks = _googleSearchService.GetRowLinks(googleSearchHtml, AppConsts.ResultLimit);
                    var googleFiltedRows = _googleSearchService.GetFiltedRowsIndexes(googleRowLinks, targetTag);
                    return googleFiltedRows;
                case (AppConsts.Bing):
                    var bingSearchHtml = _bingSearchService.SearchMultiplePages(searchTerm, 6);
                    var bingRowLinks = _bingSearchService.GetRowLinks(bingSearchHtml, AppConsts.ResultLimit);
                    var bingFiltedRows = _bingSearchService.GetFiltedRowsIndexes(bingRowLinks, targetTag);
                    return bingFiltedRows;
                default:
                    throw new ArgumentException($"The search provider is not supported.");
            }
        }
    }
}
