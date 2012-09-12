namespace movies.API
{
    public class IMDb
    {
        public static string GetParentalGuideUrl(string imdbMovieId)
        {
            // e.g., http://www.imdb.com/title/tt1606389/parentalguide
            // make sure passed-in imbMovieId starts with "tt", per url example above
            if (!imdbMovieId.StartsWith("tt"))
                imdbMovieId = "tt" + imdbMovieId;

            return string.Format("http://www.imdb.com/title/{0}/parentalguide", imdbMovieId);
        }

        public static string GetParentalGuideMobileUrl(string imdbMovieId)
        {
            if (!imdbMovieId.StartsWith("tt"))
                imdbMovieId = "tt" + imdbMovieId;

            return string.Format("http://m.imdb.com/title/{0}/parentalguide", imdbMovieId);
        }

        public static string GetMovieUrl(string imdbMovieId)
        {
            // e.g., http://www.imdb.com/title/tt1606389/
            // make sure passed-in imbMovieId starts with "tt", per url example above
            if (!imdbMovieId.StartsWith("tt"))
                imdbMovieId = "tt" + imdbMovieId;

            return string.Format("http://www.imdb.com/title/{0}/", imdbMovieId);
        }

        public static string GetPluginHtmlWide(string imdbMovieId, string movieTitle)
        {
            // customize here: http://www.imdb.com/plugins?titleId=tt0328107&ref_=tt_plg_rt
            
            // make sure passed-in imbMovieId starts with "tt", per url example above
            if (!imdbMovieId.StartsWith("tt"))
                imdbMovieId = "tt" + imdbMovieId;

            return "<span class=\"imdbRatingPlugin\" data-user=\"ur33306141\" data-title=\"{ID}\" data-style=\"p3\"><a href=\"http://www.imdb.com/title/{ID}/?ref_=plg_rt_1\"><img src=\"http://g-ecx.images-amazon.com/images/G/01/imdb/plugins/rating/images/imdb_37x18.png\" alt=\"{TITLE} on IMDb\" /></a></span><script>(function(d,s,id){var js,stags=d.getElementsByTagName(s)[0];if(d.getElementById(id)){return;}js=d.createElement(s);js.id=id;js.src=\"http://g-ec2.images-amazon.com/images/G/01/imdb/plugins/rating/js/rating.min.js\";stags.parentNode.insertBefore(js,stags);})(document,'script','imdb-rating-api');</script>".Replace("{ID}", imdbMovieId).Replace("{TITLE}", movieTitle);
        }

        public static string GetPluginHtmlSmall(string imdbMovieId, string movieTitle)
        {
            // customize here: http://www.imdb.com/plugins?titleId=tt0328107&ref_=tt_plg_rt

            // make sure passed-in imbMovieId starts with "tt", per url example above
            if (!imdbMovieId.StartsWith("tt"))
                imdbMovieId = "tt" + imdbMovieId;

            return "<span class=\"imdbRatingPlugin\" data-user=\"ur33306141\" data-title=\"{ID}\" data-style=\"p4\"><a href=\"http://www.imdb.com/title/{ID}/?ref_=plg_rt_1\"><span class=\"icon\"></span></a></span><script>(function(d,s,id){var js,stags=d.getElementsByTagName(s)[0];if(d.getElementById(id)){return;}js=d.createElement(s);js.id=id;js.src=\"http://g-ec2.images-amazon.com/images/G/01/imdb/plugins/rating/js/rating.min.js\";stags.parentNode.insertBefore(js,stags);})(document,'script','imdb-rating-api');</script>".Replace("{ID}", imdbMovieId).Replace("{TITLE}", movieTitle);
        }
    }
}
