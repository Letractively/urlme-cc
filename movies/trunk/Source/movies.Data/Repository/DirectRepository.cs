using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace movies.Data.Repository
{
    public class DirectRepository : RepositoryBase
    {
        public MovieReview MovieGet(int movieId)
        {
            using (var context = CreateContext())
            {
                return context.MovieReviews.FirstOrDefault(x => x.MovieId == movieId);
            }
        }

        public bool MovieSave(Data.MovieReview movie)
        {
            try
            {
                using (var context = CreateContext(true))
                {
                    var dbMovie = context.MovieReviews.FirstOrDefault(x => x.MovieId == movie.MovieId);

                    if (dbMovie == null)
                    {
                        // INSERT
                        dbMovie = new MovieReview
                        {
                            CreateDate = DateTime.Now,
                            ModifyDate = null,
                            MovieId = movie.MovieId
                        };

                        context.MovieReviews.InsertOnSubmit(dbMovie);
                    }
                    else
                    {
                        // UPDATE
                        dbMovie.ModifyDate = DateTime.Now;
                    }

                    dbMovie.Review = movie.Review;
                    dbMovie.ReviewClass = movie.ReviewClass;
                    dbMovie.ReviewUrl = movie.ReviewUrl;
                    dbMovie.Title = movie.Title;
                    dbMovie.Year = movie.Year;
                    dbMovie.DetailedPosterUrl = movie.DetailedPosterUrl;
                    dbMovie.ProfilePosterUrl = movie.ProfilePosterUrl;
                    dbMovie.ThumbnailPosterUrl = movie.ThumbnailPosterUrl;
                    
                    context.SubmitChanges(ConflictMode.FailOnFirstConflict);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool MovieDelete(int movieId)
        {
            try
            {
                using (var context = CreateContext(true))
                {
                    var dbMovie = context.MovieReviews.FirstOrDefault(x => x.MovieId == movieId);

                    if (dbMovie == null)
                    {
                        // DOES NOT EXIST, and it should, so return false b/c it's an error
                        return false;
                    }
                    else
                    {
                        // EXISTS, so add to things to delete
                        context.MovieReviews.DeleteOnSubmit(dbMovie);
                    }

                    context.SubmitChanges(ConflictMode.FailOnFirstConflict);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
