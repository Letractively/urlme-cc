using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace movies.Data.DomainModels
{
    public class User
    {
        public static bool IsReviewer(int facebookUserId)
        {
            List<int> viewerIds = new List<int>();
            viewerIds.Add(13002624); // John
            viewerIds.Add(6207283); // Ian
            viewerIds.Add(762136697); // Shari
            viewerIds.Add(507867483); // Beasley

            return viewerIds.Contains(facebookUserId);
        }
    }
}
