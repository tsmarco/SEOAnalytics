namespace InfoTrackSiteAnalytics
{
    public class AppConsts
    {
        
        public const string GoogleServiceURL = "https://infotrack-tests.infotrack.com.au/Google/Page{0}.html";
        public const string BingServiceURL = "https://infotrack-tests.infotrack.com.au/Bing/Page{0}.html";
        public const string GoogleServiceRowRegex = @"<div class=""r""><a href=""(.*?)""";
        public const string BingServiceRowRegex = @"class=""b_algo""><h2><a href=""(.*?)""";
        public const string Google = "google";
        public const string Bing = "bing";
        public const int ResultLimit= 50;

    }
}
