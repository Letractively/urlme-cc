using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ianhd.core.Validation;
using urlme.data.Models;

namespace urlme.data.Services
{
    public static class SiteService
    {
        public static OperationResult<Link> Save(data.Models.Link source)
        {
            var or = new OperationResult<Link>();
            var insert = source.LinkId == default(int);
            var update = !insert;

            // if it exists already, not cool
            var existing = Link.Get(source.Path);
            if (insert && existing != null) {
                if (existing.UserId == source.UserId)
                    or.ErrorMessage = "You already have a link with this custom path";
                else
                    or.ErrorMessage = "This path is already taken";
            }

            if (!or.HasErrorMessage && update && existing != null && existing.LinkId != source.LinkId) {
                if (existing.UserId == source.UserId)
                    or.ErrorMessage = "You already have a link with this custom path";
                else
                    or.ErrorMessage = "This path is already taken";
            }

            return or;
        }
    }
}
