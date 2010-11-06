using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using urlme.Model.Enums;
using urlme.Model;

namespace urlme.Site.ViewModels
{
    public class HomeViewModel
    {
        // properties
        public Model.User CurrentUser { get; private set; }
        public SortOptions Sort { get; private set; }
        public List<Link> Links { get; private set; }
        public Link NewLink { get; private set; }

        // constructors
        public HomeViewModel(User user, SortOptions sort, Link link)
        {
            this.NewLink = link;
            this.CurrentUser = user;
            this.Sort = sort;
            if (user.IsAuthenticated && user.Links != null)
            {
                if (sort == SortOptions.path)
                    this.Links = user.Links.OrderBy(x => x.Path).ToList();
                else if (sort == SortOptions.latest)
                    this.Links = user.Links.OrderByDescending(x => x.CreateDate).ToList();
                else
                    this.Links = user.Links.OrderByDescending(x => x.HitCount).ThenByDescending(x => x.CreateDate).ToList();
            }
            else
            {
                this.Links = new List<Link>();
            }
        }

        public HomeViewModel(User user, SortOptions sort)
        {
            this.NewLink = new Link(); // prep for add link form
            this.CurrentUser = user;
            this.Sort = sort;
            if (user.IsAuthenticated && user.Links != null)
            {
                if (sort == SortOptions.path)
                    this.Links = user.Links.OrderBy(x => x.Path).ToList();
                else if (sort == SortOptions.latest)
                    this.Links = user.Links.OrderByDescending(x => x.CreateDate).ToList();
                else
                    this.Links = user.Links.OrderByDescending(x => x.HitCount).ThenByDescending(x => x.CreateDate).ToList();
            }
            else
            {
                this.Links = new List<Link>();
            }
        }
    }
}