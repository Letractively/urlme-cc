using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace movies.Data.Repository
{
    public class DirectRepository : RepositoryBase
    {
        public Movie MovieGet(int movieId)
        {
            using (var context = CreateContext())
            {
                return context.Movies.FirstOrDefault(x => x.MovieId == movieId);
            }
        }

        public bool MovieSave(Data.Movie movie)
        {
            try
            {
                using (var context = CreateContext(true))
                {
                    if (movie.MovieId == default(int))
                    {
                        // INSERT
                        movie.CreateDate = DateTime.Now;
                        movie.ModifyDate = null;
                        context.Movies.InsertOnSubmit(movie);
                    }
                    else
                    {
                        // UPDATE
                        movie.ModifyDate = DateTime.Now;
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
