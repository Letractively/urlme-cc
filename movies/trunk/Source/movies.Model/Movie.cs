using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace movies.Model
{
    public class Movie
    {
        public string RtMovieId { get; set; }
        public string ImdbMovieId { get; set; } 
        public string Title { get; set; }
        public string MpaaRating { get; set; }
        public string Duration { get; set; }
        public int RtCriticsRating { get; set; }
        public int RtAudienceRating { get; set; }
    }
}
