﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using movies.Core.Extensions;

namespace movies.Data.Repository
{
    public class DirectRepository : RepositoryBase
    {
        public MovieReview MovieReviewGet(int movieId)
        {
            using (var context = CreateContext())
            {
                return context.MovieReviews.FirstOrDefault(x => x.MovieId == movieId);
            }
        }

        public List<MovieReview> MovieReviewGet()
        {
            using (var context = CreateContext())
            {
                return context.MovieReviews.Where(x => x.Status == Enumerations.MovieReviewStatus.Approved.ToString() || x.Status == Enumerations.MovieReviewStatus.NotRequired.ToString()).ToList();
            }
        }

        public bool MovieReviewIsOnSeeItWhiteList(int movieId)
        {
            using (var context = CreateContext())
            {
                return context.MovieReviewSeeItWhiteLists.FirstOrDefault(x => x.MovieId == movieId) != null;
            }
        }

        public bool MovieReviewIsOnSeeItBlackList(int movieId)
        {
            using (var context = CreateContext())
            {
                return context.MovieReviewSeeItBlackLists.FirstOrDefault(x => x.MovieId == movieId) != null;
            }
        }
        
        public bool MovieReviewSave(Data.MovieReview movie, bool requiresApproval)
        {
            try
            {
                using (var context = CreateContext(true))
                {
                    var dbMovie = context.MovieReviews.FirstOrDefault(x => x.MovieId == movie.MovieId);
                    bool onSeeItBlackList = context.MovieReviewSeeItBlackLists.FirstOrDefault(x => x.MovieId == movie.MovieId) != null;
                    bool onSeeItWhiteList = context.MovieReviewSeeItWhiteLists.FirstOrDefault(x => x.MovieId == movie.MovieId) != null;

                    if (dbMovie == null)
                    {
                        // INSERT (b/c it does NOT exist)
                        dbMovie = new MovieReview
                        {
                            CreateDate = DateTime.Now,
                            ModifyDate = null,
                            MovieId = movie.MovieId,
                            Status = Enumerations.MovieReviewStatus.Pending.ToString() // to figure out after this if/else
                        };

                        context.MovieReviews.InsertOnSubmit(dbMovie);
                    }
                    else
                    {
                        // UPDATE (b/c it already exists)
                        dbMovie.ModifyDate = DateTime.Now;
                    }

                    // determine status
                    if (dbMovie.Status == Enumerations.MovieReviewStatus.Approved.ToString())
                    {
                        // leave as approved
                    }
                    else if (onSeeItBlackList && movie.ReviewClass == "seeIt") // if on seeItBlackList and it's marked as "seeIt", then DISAPPROVED
                    {
                        dbMovie.Status = Enumerations.MovieReviewStatus.Disapproved.ToString();
                    }
                    else if (!requiresApproval || onSeeItWhiteList || (requiresApproval && movie.ReviewClass == "orNot"))
                    {
                        dbMovie.Status = Enumerations.MovieReviewStatus.NotRequired.ToString();
                    }
                    else if (requiresApproval && movie.ReviewClass == "seeIt")
                    {
                        dbMovie.Status = Enumerations.MovieReviewStatus.Pending.ToString();
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Pending review for <b>{0}</b>.<br/><br/>", movie.Title);
                        sb.AppendFormat("Movie page: <a href=\"http://seeitornot.co/{0}/{1}\">http://seeitornot.co/{0}/{1}</a><br/><br/>", movie.Title.Slugify(), movie.MovieId);
                        sb.AppendFormat("<a href=\"http://seeitornot.co/reviews/approve/{0}\">Approve</a> or <a href=\"http://seeitornot.co/reviews/disapprove/{0}\">disapprove</a>.", movie.MovieId);
                        Core.Net.Mail.SendFromNoReply("ihdavis@gmail.com", "Ian Davis", "Pending review: " + movie.Title, sb.ToString());
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

        public bool MovieReviewUpdateStatus(int movieId, Enumerations.MovieReviewStatus status)
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
                        // EXISTS, so modify
                        dbMovie.Status = status.ToString();
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

        public bool MovieReviewDelete(int movieId)
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
