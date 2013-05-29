namespace movies.API
{
    public class TMS
    {
        private const string BaseUrl = "http://data.tmsapi.com/v1/";
        private const string ApiKey = "k5thhwn9b7cvajx2cyy9nj7x";

        // example showtimes
        // http://data.tmsapi.com/v1/lineups?country=USA&postalCode=23221&api_key=k5thhwn9b7cvajx2cyy9nj7x
        // or http://data.tmsapi.com/v1/movies/showings?zip=23221&api_key=k5thhwn9b7cvajx2cyy9nj7x&startDate=2013-05-10

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

        public static string GetMovieClipsJson(string rtMovieId)
        {
            string url = string.Format("{0}movies/{1}/clips.json?apikey={2}", BaseUrl, rtMovieId, ApiKey);
            return Core.Net.HttpWebRequest.GetResponse(url);
        }

        public static string GetMovieByIMDbIdJson(string imdbId)
        {
            if (imdbId.StartsWith("tt"))
            {
                imdbId = imdbId.Substring(2);
            }
            string url = string.Format("{0}movie_alias.json?id={1}&type=imdb&apikey={2}", BaseUrl, imdbId, ApiKey);
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
