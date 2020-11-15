using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace InfoTrackSEOAnalytics.Search
{
    /// <summary>
    /// SearchService provides logic for gathering data from search engines.
    /// </summary>
    public abstract class SearchService : IGoogleSearchService, IBingSearchService
    {
        protected internal string endPoint;
        protected internal string rowRegex;
        protected internal IHttpClientFactory _clientFactory;

        /// <summary>
        /// Search calls search engine to for search results.
        /// </summary>
        /// <param name="searchTerm">SearchTerm</param>
        /// <param name="page">Page number</param>
        public virtual string Search(string searchTerm, int page)
        {
            HttpClient client = _clientFactory.CreateClient();
            var requestUrl = string.Format(endPoint, page.ToString("00"));
            client.BaseAddress = new Uri(requestUrl);
            HttpResponseMessage response = client.GetAsync("").Result;

            var data = response.Content.ReadAsStringAsync().Result;
            return data;
        }

        /// <summary>
        /// Search calls search engine to for multiple pages of search results.
        /// </summary>
        /// <param name="searchTerm">SearchTerm</param>
        /// <param name="pages">Pages</param>
        public virtual string SearchMultiplePages(string searchTerm, int pages)
        {
            var returnedhtml = "";
            for (int i = 1; i <= pages; i++)
            {
                returnedhtml += Search(searchTerm, i);
            }
            return returnedhtml;
        }

        /// <summary>
        /// GetRowLinks returns the rows of links as a list of string.
        /// </summary>
        /// <param name="html">html</param>
        /// <param name="limit">limit </param>
        public virtual List<string> GetRowLinks(string html, int limit)
        {
            if (string.IsNullOrEmpty(html)) return new List<string>();

            var regex = new Regex(rowRegex);
            MatchCollection matches = regex.Matches(html);
            if (matches.Count < 1) return new List<string>();
            var result = matches.Cast<Match>().Select(x => x?.Groups[1]?.Value);

            return result.Take(limit).ToList();
        }

        /// <summary>
        /// GetFiltedRowsIndexes Filters a list of links and returns their index as a list.
        /// </summary>
        /// <param name="html">html</param>
        public virtual List<int> GetFiltedRowsIndexes(List<string> links, string searchTerm)
        {
            if (links == null || links.Count < 1) return new List<int>(){0};
            var matchingIndexes = new List<int>();
            for (int i = 0; i < links.Count; i++)
            {
                if (links[i].ToLower().Contains(searchTerm.ToLower())) matchingIndexes.Add(i+1);
            }
            if (matchingIndexes.Count < 1) return new List<int>() { 0 };
            return matchingIndexes;
        }
    }
}
