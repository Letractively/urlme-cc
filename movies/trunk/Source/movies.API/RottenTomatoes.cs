namespace movies.API
{
    public class RottenTomatoes
    {
        private const string BaseUrl = "http://api.rottentomatoes.com/api/public/v1.0/";
        private const string ApiKey = "pfrwfgnr53tpaydw8pnhrymy";

        public static string GetOpeningJson()
        {
            string url = string.Format("{0}lists/movies/opening.json?page_limit=20&page=1&country=us&apikey={1}", BaseUrl, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string GetBoxOfficeJson()
        {
            string url = string.Format("{0}lists/movies/box_office.json?page_limit=20&page=1&country=us&apikey={1}", BaseUrl, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string GetInTheatersJson()
        {
            string url = string.Format("{0}lists/movies/in_theaters.json?page_limit=25&page=1&country=us&apikey={1}", BaseUrl, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string SearchMoviesJson(string q, int pageLimit)
        {
            string url = string.Format("{0}movies.json?page_limit={1}&page=1&country=us&apikey={2}&q={3}", BaseUrl, pageLimit, ApiKey, q);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string GetUpcomingJson()
        {
            string url = string.Format("{0}lists/movies/upcoming.json?page_limit=20&page=1&country=us&apikey={1}", BaseUrl, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string GetMovieJson(string rtMovieId)
        {
            string url = string.Format("{0}movies/{1}.json?apikey={2}", BaseUrl, rtMovieId, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

    //SearchMovies: function (q, callback) {
    //    // first, check cache
    //    var cacheKey = "rt-SearchMovies-{0}".format(q);
    //    var cached = $.cacheItem(cacheKey);
    //    if (cached) {
    //        return callback(cached);
    //    }

    //    var url = "{0}movies.json?page_limit=20&page=1&country=us&apikey={1}&q={2}".format(codejkjk.movies.RottenTomatoes.BaseUrl, codejkjk.movies.RottenTomatoes.ApiKey, encodeURI(q));
    //    codejkjk.movies.RottenTomatoes.AjaxGetMovies(url, callback, cacheKey);
    //},
    //GetMovie: function (movieId, callback) {
    //    // first, check cache
    //    var cacheKey = "rt-GetMovie-{0}".format(movieId);
    //    var cached = $.cacheItem(cacheKey);
    //    if (cached) {
    //        return callback(cached);
    //    }

    //    var url = "{0}movies/{1}.json?apikey={2}".format(codejkjk.movies.RottenTomatoes.BaseUrl, movieId, codejkjk.movies.RottenTomatoes.ApiKey);
    //    codejkjk.movies.RottenTomatoes.AjaxGetMovie(url, callback, cacheKey);
    //},
    }
}
