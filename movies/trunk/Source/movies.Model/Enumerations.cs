using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace movies.Model
{
    public class Enumerations
    {
        public enum RatingSizes {
            small,
            smaller,
            medium
        }

        public enum MovieLists
        {
            Opening,
            BoxOffice,
            InTheaters,
            Upcoming
        }
    }
}