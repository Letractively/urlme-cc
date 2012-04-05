using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using urlme.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace urlme.Model
{
    public class Link
    {
        #region Properties
        public int LinkId { get; private set; }
        
        [Required(ErrorMessage="Path required")]
        [StringLength(50, ErrorMessage = "Path must be no longer than 50 characters")]
        public string Path { get; set; }

        [Required(ErrorMessage = "Destination Url required")]
        public string DestinationUrl { get; set; }

        public DateTime CreateDate { get; private set; }
        public int HitCount { get; private set; }
        #endregion

        #region Constructors
        private Link(Data.Link link)
        {
            this.LinkId = link.LinkId;
            this.Path = link.Path;
            this.DestinationUrl = link.DestinationUrl;
            this.CreateDate = link.CreateDate;
            this.HitCount = link.HitCount;
        }
        public Link() {
            this.Path = string.Empty;
            this.DestinationUrl = string.Empty;
        }
        #endregion

        #region Public Static Methods
        public static List<Link> GetLinksByUserId(int userId)
        {
            List<Link> ret = new List<Link>();
            using (Data.urlmeDataContext db = new Data.urlmeDataContext())
            {
                var links = db.Links.Where(x => x.UserId == userId).ToList();
                foreach (Data.Link link in links)
                {
                    ret.Add(new Link(link));
                }
            }
            return ret;  
        }

        public static string GetDestinationUrlByPathAndIncrementHitCount(string path) {
            string ret = null;
            using (Data.urlmeDataContext db = new Data.urlmeDataContext())
            {
                //int? nullableInt = null;
                //var t = db.tests.Where(x => x.NullableInt == null);
                //var t2 = db.tests.Where(x => x.NullableInt == nullableInt);
                
                var link = db.Links.Where(x => x.Path.ToLower().Trim() == path.ToLower().Trim()).SingleOrDefault();
                if (link != null)
                {
                    ret = link.DestinationUrl;
                    link.HitCount += 1;

                    db.SubmitChanges();
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
            using (Data.urlmeDataContext db = new Data.urlmeDataContext())
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

        public static CrudLinkResults CreateLink(string newPath, string newDestinationUrl)
        {
            // TODO: this should happen on the front-end UI
            if (string.IsNullOrEmpty(newPath) || string.IsNullOrEmpty(newDestinationUrl))
                return CrudLinkResults.InsufficientInput;
            
            CrudLinkResults ret = CrudLinkResults.Success;
            using (Data.urlmeDataContext db = new Data.urlmeDataContext())
            {
                Data.Link link = db.Links.Where(x => x.Path == newPath).SingleOrDefault();
                // green light. link does not already exist
                if (link == null)
                {
                    link = new Data.Link();
                    link.UserId = (int)Model.User.Current.UserId;
                    link.Path = newPath;
                    link.DestinationUrl = newDestinationUrl.ToLower().IndexOf("http") == 0 ? newDestinationUrl : "http://" + newDestinationUrl;
                    link.Description = null;
                    link.HitCount = 0;
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
                else
                {
                    // red light. link already exists
                    ret = CrudLinkResults.PathAlreadyExistsByOwner;
                }
            }
            return ret;
        }
        #endregion
    }
}
