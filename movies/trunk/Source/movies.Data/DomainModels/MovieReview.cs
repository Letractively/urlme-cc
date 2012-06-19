using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace movies.Data.DomainModels
{
    public class MovieReview
    {
        public int MovieId { get; set; }
        public string Text { get; set; }
        public string ClassName { get; set; }
        public string Url { get; set; }

        private static readonly Data.Repository.DirectRepository repo = new Repository.DirectRepository();

        public static bool Save(Data.DomainModels.MovieReview movieReview)
        {
            Data.Movie dbMovie = new Movie { 
                Review = movieReview.Text,
                MovieId = movieReview.MovieId,
                ReviewUrl = movieReview.Url,
                ReviewClass = movieReview.ClassName
            };
            return repo.MovieSave(dbMovie);
        }

        public static MovieReview Get(int movieId)
        {
            var dbMovieReview = repo.MovieGet(movieId);
            if (dbMovieReview != null)
            {
                return new MovieReview { Text = dbMovieReview.Review, ClassName = dbMovieReview.ReviewClass, Url = dbMovieReview.ReviewUrl };
            }
            return null;
        }
    }
}
