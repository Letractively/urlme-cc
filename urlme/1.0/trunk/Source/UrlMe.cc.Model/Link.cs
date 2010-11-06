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

        #region Public Static Methods
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

        public static CrudLinkResults CreateUserLink(int userId, string path, string destinationUrl)
        {
            CrudLinkResults ret = CrudLinkResults.Success;
            using (Data.UrlMe_ccDataContext db = new UrlMe.cc.Data.UrlMe_ccDataContext())
            {
                Data.Link link = new UrlMe.cc.Data.Link();
                link.UserId = userId;
                link.Path = path;
                link.DestinationUrl = destinationUrl;
                link.Description = null;
                link.CreateDate = System.DateTime.Now;
                link.ExpirationDate = null;
                link.ActiveInd = true;
                db.Links.InsertOnSubmit(link);

                try
                {
                    db.SubmitChanges();
                }
                catch
                {
                    ret = CrudLinkResults.Failure;
                }
            }
            return ret;
        }

        public static CrudLinkResults CreateGuestLink(string path, string destinationUrl)
        {
            CrudLinkResults ret = CrudLinkResults.Success;
            using (Data.UrlMe_ccDataContext db = new UrlMe.cc.Data.UrlMe_ccDataContext())
            {
                Data.Link link = new UrlMe.cc.Data.Link();
                link.UserId = 19; // guest user id
                link.Path = path;
                link.DestinationUrl = destinationUrl;
                link.Description = null;
                link.CreateDate = System.DateTime.Now;
                link.ExpirationDate = System.DateTime.Now.AddDays(7);
                link.ActiveInd = true;
                db.Links.InsertOnSubmit(link);

                try
                {
                    db.SubmitChanges();
                }
                catch
                {
                    ret = CrudLinkResults.Failure;
                }
            }
            return ret;
        }

        public static string GetDestinationUrlByPath(string path)
        {
            string ret = null;
            using (Data.UrlMe_ccDataContext db = new UrlMe.cc.Data.UrlMe_ccDataContext())
            {
                var link = db.Links.Where(x => x.Path.ToLower().Trim() == path.ToLower().Trim()).SingleOrDefault();
                if (link != null)
                {
                    ret = link.DestinationUrl;
                }
            }
            return ret;
        }

        public static CrudLinkResults DeleteLinks(string linkIds)
        {
            CrudLinkResults ret = CrudLinkResults.Success;
            foreach (string linkId in linkIds.Split(','))
            {
                try
                {
                    DeleteLink(int.Parse(linkId));
                }
                catch
                {
                    ret = CrudLinkResults.Failure;
                }
            }
            return ret;
        }

        public static CrudLinkResults DeleteLink(int linkId)
        {
            CrudLinkResults ret = CrudLinkResults.Success;
            using (Data.UrlMe_ccDataContext db = new UrlMe.cc.Data.UrlMe_ccDataContext())
            {
                var link = db.Links.Where(x => x.LinkId == linkId).SingleOrDefault();
                db.Links.DeleteOnSubmit(link);

                try
                {
                    db.SubmitChanges();
                }
                catch
                {
                    ret = CrudLinkResults.Failure;
                }
            }
            return ret;
        }

        // todo: have a List<Link> links as property of User, and in future to User u; u.Links and use that as a datasource
        public static List<Link> GetLinksByUser(int userId)
        {
            List<Link> ret = new List<Link>();
            using (Data.UrlMe_ccDataContext db = new UrlMe.cc.Data.UrlMe_ccDataContext())
            {
                var links = db.Links.Where(x => x.UserId == userId).ToList();
            }
            return ret;            
        }
        #endregion
    }
}
