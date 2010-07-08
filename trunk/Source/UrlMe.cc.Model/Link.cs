using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UrlMe.cc.Model.Enums;

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
            // this.expirationDate = (DateTime)link.ExpirationDate;
            // this.isPublic = link.PublicInd;
            this.isActive = link.ActiveInd;
        }
        #endregion

        public static List<Link> AllLinks() {
            List<Link> ret = new List<Link>();
            using (Data.UrlMe_ccDataContext db = new UrlMe.cc.Data.UrlMe_ccDataContext())
            {
                var links = db.Links.ToList();
                foreach (Data.Link link in links)
                {
                    ret.Add(new Link(link));
                }
            }
            return ret;
        }

        public CreateLinkResults CreateLink(int userId, string path, string destinationUrl)
        {
            CreateLinkResults ret = CreateLinkResults.Success;
            using (Data.UrlMe_ccDataContext db = new UrlMe.cc.Data.UrlMe_ccDataContext())
            {
                Data.Link link = new UrlMe.cc.Data.Link();
                link.UserId = userId;
                link.Path = path;
                link.DestinationUrl = destinationUrl;
                db.Links.InsertOnSubmit(link);

                try
                {
                    db.SubmitChanges();
                }
                catch
                {
                    ret = CreateLinkResults.Failure;
                }
            }
            return ret;
        }
    }
}
