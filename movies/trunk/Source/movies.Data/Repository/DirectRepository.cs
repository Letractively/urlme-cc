using System;
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
                return context.MovieReviews.Where(x => x.Status == Enumerations.MovieReviewStatus.Approved.ToString() || x.Status == Enumerations.MovieReviewStatus.NotRequired.ToString()).OrderByDescending(x => x.CreateDate).ToList();
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

        public bool LogSave(Data.Log log)
        {
            try
            {
                using (var ctx = CreateContext(true))
                {
                    ctx.Logs.InsertOnSubmit(log);
                    ctx.SubmitChanges();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool MovieReviewSave(Data.MovieReview movieReview, bool requiresApproval, string parentalGuideHtml)
        {
            try
            {
                using (var context = CreateContext(true))
                {
                    var dbMovieReview = context.MovieReviews.FirstOrDefault(x => x.MovieId == movieReview.MovieId);
                    bool onSeeItBlackList = context.MovieReviewSeeItBlackLists.FirstOrDefault(x => x.MovieId == movieReview.MovieId) != null;
                    bool onSeeItWhiteList = context.MovieReviewSeeItWhiteLists.FirstOrDefault(x => x.MovieId == movieReview.MovieId) != null;

                    if (dbMovieReview == null)
                    {
                        // INSERT (b/c it does NOT exist)
                        dbMovieReview = new MovieReview
                        {
                            CreateDate = DateTime.Now,
                            ModifyDate = null,
                            MovieId = movieReview.MovieId,
                            Status = Enumerations.MovieReviewStatus.Pending.ToString() // to figure out after this if/else
                        };

                        context.MovieReviews.InsertOnSubmit(dbMovieReview);
                    }
                    else
                    {
                        // UPDATE (b/c it already exists)
                        dbMovieReview.ModifyDate = DateTime.Now;
                    }

                    // determine status
                    if (dbMovieReview.Status == Enumerations.MovieReviewStatus.Approved.ToString())
                    {
                        // leave as approved
                    }
                    else if (onSeeItBlackList && movieReview.ReviewClass == "seeIt")
                    {
                        // if on seeItBlackList and it's marked as "seeIt", then DISAPPROVED
                        dbMovieReview.Status = Enumerations.MovieReviewStatus.Disapproved.ToString();
                    }
                    else if (!requiresApproval || onSeeItWhiteList || (requiresApproval && movieReview.ReviewClass == "orNot"))
                    {
                        // doesn't require approval, OR on seeIt white list, OR requires approval AND it's OrNot
                        string thumbsUrl = string.Format("http://seeitornot.co/content/images/{0}.png", movieReview.ReviewClass == "orNot" ? "thumbsDown" : "thumbsUp");
                        string body = string.Format("Yay, John submitted a review for <b>{0}</b>. <img style='width:16px; vertical-align:text-bottom' src='{1}' /> \"{2}\".", movieReview.Title, thumbsUrl, movieReview.Review);
                        var mailSuccess = Core.Net.Mail.SendFromNoReply("ihdavis@gmail.com", "Ian Davis", string.Format("Review submitted: '{0}' for {1}", movieReview.ReviewClass, movieReview.Title), body);
                        dbMovieReview.Status = Enumerations.MovieReviewStatus.NotRequired.ToString();
                    }
                    else if (requiresApproval && movieReview.ReviewClass == "seeIt")
                    {
                        dbMovieReview.Status = Enumerations.MovieReviewStatus.Pending.ToString();
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Pending review for <b>{0}</b>.<br/><br/>", movieReview.Title);
                        sb.AppendFormat("Movie page: <a href=\"http://seeitornot.co/{0}/{1}\">http://seeitornot.co/{0}/{1}</a><br/><br/>", movieReview.Title.Slugify(), movieReview.MovieId);
                        sb.AppendFormat("<a href=\"http://seeitornot.co/reviews/approve/{0}\">Approve</a> or <a href=\"http://seeitornot.co/reviews/disapprove/{0}\">disapprove</a>.<br/><br/>", movieReview.MovieId);
                        sb.Append(parentalGuideHtml);
                        var mailSuccess = Core.Net.Mail.SendFromNoReply("ihdavis@gmail.com", "Ian Davis", "Pending review: " + movieReview.Title, sb.ToString());
                    }

                    dbMovieReview.Review = movieReview.Review;
                    dbMovieReview.ReviewClass = movieReview.ReviewClass;
                    dbMovieReview.ReviewUrl = movieReview.ReviewUrl;
                    dbMovieReview.Title = movieReview.Title;
                    dbMovieReview.Year = movieReview.Year;
                    dbMovieReview.DetailedPosterUrl = movieReview.DetailedPosterUrl;
                    dbMovieReview.ProfilePosterUrl = movieReview.ProfilePosterUrl;
                    dbMovieReview.ThumbnailPosterUrl = movieReview.ThumbnailPosterUrl;
                    dbMovieReview.ReleaseDate = movieReview.ReleaseDate;
                    dbMovieReview.Grade = movieReview.Grade;
                    
                    context.SubmitChanges(ConflictMode.FailOnFirstConflict);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool UpdateSeeItBlackList(int movieId, string title, bool addTo)
        {
            try
            {
                using (var context = CreateContext(true))
                {
                    var entry = context.MovieReviewSeeItBlackLists.FirstOrDefault(x => x.MovieId == movieId);

                    if (entry == null)
                    {
                        // DOES NOT EXIST
                        if (addTo)
                        {
                            // create new row
                            entry = new Data.MovieReviewSeeItBlackList
                            {
                                CreateDate = System.DateTime.Now,
                                MovieId = movieId,
                                Title = title
                            };
                            context.MovieReviewSeeItBlackLists.InsertOnSubmit(entry);
                        } // else, to be removed, so do nothing b/c it doesn't exist
                    }
                    else
                    {
                        // EXISTS
                        if (!addTo)
                        {
                            context.MovieReviewSeeItBlackLists.DeleteOnSubmit(entry);
                        } // else, to be added, so leave it there
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

        public bool UpdateSeeItWhiteList(int movieId, string title, bool addTo)
        {
            try
            {
                using (var context = CreateContext(true))
                {
                    var entry = context.MovieReviewSeeItWhiteLists.FirstOrDefault(x => x.MovieId == movieId);

                    if (entry == null)
                    {
                        // DOES NOT EXIST
                        if (addTo)
                        {
                            // create new row
                            entry = new Data.MovieReviewSeeItWhiteList
                            {
                                CreateDate = System.DateTime.Now,
                                MovieId = movieId,
                                Title = title
                            };
                            context.MovieReviewSeeItWhiteLists.InsertOnSubmit(entry);
                        } // else, to be removed, so do nothing b/c it doesn't exist
                    }
                    else
                    {
                        // EXISTS
                        if (!addTo)
                        {
                            context.MovieReviewSeeItWhiteLists.DeleteOnSubmit(entry);
                        } // else, to be added, so leave it there
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
