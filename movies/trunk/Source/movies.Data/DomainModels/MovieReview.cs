using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using movies.Core.Web.Caching;
using movies.Core.Extensions;

namespace movies.Data.DomainModels
{
    public class MovieReview
    {
        public int MovieId { get; set; }
        public string Text { get; set; }
        public string ClassName { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string ThumbnailPosterUrl { get; set; }
        public string ProfilePosterUrl { get; set; }
        public string DetailedPosterUrl { get; set; }
        public string OnSeeItWhiteList { get; set; }
        public string OnSeeItBlackList { get; set; }
        public string Status { get; set; }
        public string MovieUrl { get { return "/" + this.Title.Slugify() + "/" + this.MovieId; } }

        private static readonly Data.Repository.DirectRepository repo = new Repository.DirectRepository();

        public static bool Save(Data.DomainModels.MovieReview movieReview, string mpaaRating)
        {
            Data.MovieReview dbMovie = new Data.MovieReview
            {
                Review = movieReview.Text,
                MovieId = movieReview.MovieId,
                ReviewUrl = movieReview.Url,
                ReviewClass = movieReview.ClassName,
                Title = movieReview.Title,
                Year = movieReview.Year,
                ThumbnailPosterUrl = movieReview.ThumbnailPosterUrl,
                ProfilePosterUrl = movieReview.ProfilePosterUrl,
                DetailedPosterUrl = movieReview.DetailedPosterUrl
            };

            bool requiresApproval = mpaaRating.ToLower() == "r" || mpaaRating.ToLower() == "unrated" || mpaaRating == "";

            return repo.MovieReviewSave(dbMovie, requiresApproval);
        }

        public static bool Delete(int movieId)
        {
            return repo.MovieDelete(movieId);
        }

        public static bool IsOnSeeItWhiteList(int movieId)
        {
            return repo.MovieReviewIsOnSeeItWhiteList(movieId);
        }

        public static bool IsOnSeeItBlackList(int movieId)
        {
            return repo.MovieReviewIsOnSeeItWhiteList(movieId);
        }

        public static List<MovieReview> Get()
        {
            return Cache.GetValue<List<MovieReview>>(
                "codejkjk.movies.Data.DomainModels.MovieReview.Get",
                () =>
                {
                    return repo.MovieReviewGet().Select(x => new MovieReview
                    {
                        MovieId = int.Parse(x.MovieId.ToString()),
                        Text = x.Review,
                        ClassName = x.ReviewClass,
                        Url = x.ReviewUrl,
                        DetailedPosterUrl = x.DetailedPosterUrl,
                        ProfilePosterUrl = x.ProfilePosterUrl,
                        ThumbnailPosterUrl = x.ThumbnailPosterUrl,
                        Title = x.Title,
                        Year = x.Year,
                        Status = x.Status
                    }).ToList();
                });
        }

        public static MovieReview Get(int movieId, bool approvedOnly = true)
        {
            var dbMovieReview = repo.MovieReviewGet(movieId);

            if (dbMovieReview != null && approvedOnly && dbMovieReview.Status != movies.Data.Enumerations.MovieReviewStatus.Approved.ToString() && dbMovieReview.Status != movies.Data.Enumerations.MovieReviewStatus.NotRequired.ToString())
            {
                return null;
            }

            if (dbMovieReview != null)
            {
                return new MovieReview
                {
                    MovieId = int.Parse(dbMovieReview.MovieId.ToString()),
                    Text = dbMovieReview.Review,
                    ClassName = dbMovieReview.ReviewClass,
                    Url = dbMovieReview.ReviewUrl,
                    DetailedPosterUrl = dbMovieReview.DetailedPosterUrl,
                    ProfilePosterUrl = dbMovieReview.ProfilePosterUrl,
                    ThumbnailPosterUrl = dbMovieReview.ThumbnailPosterUrl,
                    Title = dbMovieReview.Title,
                    Year = dbMovieReview.Year,
                    Status = dbMovieReview.Status
                };
            }
            return null;
        }
    }
}
