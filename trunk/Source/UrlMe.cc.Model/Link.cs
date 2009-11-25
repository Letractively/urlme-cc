using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrlMe.cc.Model
{
    [Serializable]
    public class Link
    {
        #region Properties

        private string path = "";
        public string Path { get { return path; } }
        private string destinationUrl = "";
        public string DestinationUrl { get { return destinationUrl; } }
        private string description = "";
        public string Description { get { return description; } }
        private DateTime expirationDate = DateTime.MinValue;
        public DateTime ExpirationDate { get { return expirationDate; } }
        private bool isPublic = false;
        public bool IsPublic { get { return isPublic; } }
        private bool isActive = false;
        public bool IsActive { get { return isActive; } }

        #endregion

        #region Constructors
        private Link(Data.Link link)
        {
            this.path = link.Path;
            this.destinationUrl = link.DestinationUrl;
            this.description = link.Description;
            this.expirationDate = (DateTime)link.ExpirationDate;
            this.isPublic = link.PublicInd;
            this.isActive = link.ActiveInd;
        }
        #endregion

        public static void AllLinks() {
            //Data.UrlMe_ccDataContext db = new UrlMe.cc.Data.UrlMe_ccDataContext();
            //var a = (from l in db.Links select l).ToList();
            //List<Link> allLinks = new List<Link>();
            //foreach (Data.Link link in a)
            //{
            //    allLinks.Add(new Link(link));
            //}
            //using (Data.UrlMe_ccDataContext db = new UrlMe.cc.Data.UrlMe_ccDataContext())
            //{

            //}
        }
    }
}
