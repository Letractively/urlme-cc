﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using movies.Model;
using movies.Core.Extensions;

namespace movies.Site.Controllers
{
    public class ApiController : Controller
    {
        [HttpGet]
        public JsonResult GetIMDbMovie(string imdbMovieId)
        {
            return this.Json(Movie.GetIMDbMovie(imdbMovieId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchMovies(string q)
        {
            return PartialView("MovieList", Movie.SearchMovies(q, 20));
        }

        [HttpGet]
        public ActionResult SearchMoviesJson(string term)
        {
            var resultDict = Movie.SearchMovies(term, 5);

            return this.Json(resultDict.Values.Select(x => new { abridgedCast = x.AbridgedCast, url = "/" + x.MovieSlug, title = x.title, img = x.posters.thumbnail, year = x.year }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRottenTomatoesMovie(string rtMovieId)
        {
            return this.Json(Movie.GetRottenTomatoesMovie(rtMovieId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAuthUserJs(int facebookUserId)
        {
            if (Data.DomainModels.User.IsReviewer(facebookUserId))
            {
                return PartialView("ViewerJs", new movies.Site.ViewModels.Shared.ViewerJs { FacebookUserId = facebookUserId });
            }
            return Content("console.log('not authorized');");
        }

        public ActionResult GetMovieHtml(string rtMovieId)
        {
            var movie = Model.Movie.GetRottenTomatoesMovie(rtMovieId);
            movie.MovieType = Model.Movie.GetMovieType(movie.id);

            return PartialView("MovieDetails", movie);
        }

        public ActionResult GetRedboxMovieHtml(string rbSlug)
        {
            var movie = Model.Redbox.GetRottenTomatoesMovie(rbSlug);
            if (movie == null)
            {
                return Content("No corresponding RottenTomatoes movie found :/");
            }
            movie.MovieType = Enumerations.MovieType.AtRedboxes;

            return PartialView("MovieDetails", movie);
        }

        public ActionResult GetMovieMobileHtml(string rtMovieId)
        {
            var movie = Model.Movie.GetRottenTomatoesMovie(rtMovieId);

            return PartialView("MovieDetails.Mobile", movie);
        }

        public ActionResult GetRedboxMovieAvailHtml(string productId, string latitude, string longitude)
        {
            // var movies = Model.Redbox.GetStoresWithMovie(productId, latitude, longitude);
                return null;
        }

        public ActionResult GetRedboxesHtml(string latitude, string longitude)
        {
            var redboxes = Model.Redbox.GetStores(latitude, longitude);

            return PartialView("Redboxes", redboxes.Take(6).ToList());
        }

        [HttpGet]
        public JsonResult GetPostalCode(string date, string zip)
        {
            return this.Json(PostalCode.Get(date, zip), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTheaterMovies(string date, string zip, string theaterId)
        {
            var postalCode = PostalCode.Get(date, zip);
            var theater = postalCode.theaters.FirstOrDefault(x => x.id == theaterId);
            return this.Json(theater, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPostalCodeForMovie(string date, string zip, string rtMovieId)
        {
            var postalCode = PostalCode.Get(date, zip);
            var filteredTheaters = new List<PostalCode.Theater>();
            
            // add to filtered theater list
            foreach (var theater in postalCode.theaters)
            {
                if (theater.movies.Where(x => x.id == rtMovieId).ToList().Any())
                {
                    filteredTheaters.Add(new PostalCode.Theater
                    {
                        address = theater.address,
                        id = theater.id,
                        mapUrl = theater.mapUrl,
                        movies = theater.movies.Where(x => x.id == rtMovieId).ToList(),
                        name = theater.name,
                        theaterUrl = theater.theaterUrl
                    });
                }
            }

            // set return val's theater list to filtered list
            return PartialView("TheatersForMovieList", filteredTheaters);
        }
    }
}
