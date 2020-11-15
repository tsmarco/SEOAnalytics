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

        /// <summary>
        /// Queries search engine for a search term and returns a list of indexes containing the target tag.
        /// </summary>
        /// <param name="searchTerm">Search Term</param>
        /// <param name="targetTag">Taret Tag</param>
        /// <param name="searchProvider">Search Provider (google or bing)</param>
        [HttpGet]
        public IActionResult Get(string searchTerm, string targetTag, string searchProvider = AppConsts.Google)
        {
            if (string.IsNullOrEmpty(searchTerm)) return BadRequest($"A search term was not entered.");
            if (string.IsNullOrEmpty(targetTag)) return BadRequest($"A target tag was not entered.");

            try
            {
                switch (searchProvider)
                {
                    case (AppConsts.Google):
                        var googleSearchHtml = _googleSearchService.SearchMultiplePages(searchTerm, 5);
                        var googleRowLinks = _googleSearchService.GetRowLinks(googleSearchHtml, AppConsts.ResultLimit);
                        var googleFiltedRows = _googleSearchService.GetFiltedRowsIndexes(googleRowLinks, targetTag);
                        return Ok(googleFiltedRows);
                    case (AppConsts.Bing):
                        var bingSearchHtml = _bingSearchService.SearchMultiplePages(searchTerm, 6);
                        var bingRowLinks = _bingSearchService.GetRowLinks(bingSearchHtml, AppConsts.ResultLimit);
                        var bingFiltedRows = _bingSearchService.GetFiltedRowsIndexes(bingRowLinks, targetTag);
                        return Ok(bingFiltedRows);
                    default:
                        return BadRequest($"The search provider is not supported.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
