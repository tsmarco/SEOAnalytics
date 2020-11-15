using System.Collections.Generic;

namespace InfoTrackSEOAnalytics.Search
{
    public interface IGoogleSearchService
    {
        List<int> GetFiltedRowsIndexes(List<string> links, string searchTerm);
        List<string> GetRowLinks(string html, int limit);
        string Search(string searchTerm, int page);
        string SearchMultiplePages(string searchTerm, int pages);
    }
}