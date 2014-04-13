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
        public static Operations.SaveLink Save(data.Models.Link source)
        {
            var op = new data.Operations.SaveLink();
            
            var insert = source.LinkId == default(int);
            var update = !insert;

            // trim everything
            source.DestinationUrl = source.DestinationUrl.Trim();
            source.Path = source.Path.Trim();

            // if it exists already, not cool
            var existing = Link.Get(source.Path);
            if (insert && existing != null) {
                if (existing.UserId == source.UserId)
                    op.Result = Enumerations.SaveLinkResult.UserAlreadyHasLink;
                else
                    op.Result = Enumerations.SaveLinkResult.PathAlreadyTaken;
            }

            if (!op.HasError
                && update 
                && existing != null 
                && existing.LinkId != source.LinkId
            ) {
                if (existing.UserId == source.UserId)
                    op.Result = Enumerations.SaveLinkResult.UserAlreadyHasLink;
                else
                    op.Result = Enumerations.SaveLinkResult.PathAlreadyTaken;
            }

            if (!op.HasError && !Link.Save(source)
            ) {
                op.Result = Enumerations.SaveLinkResult.UnknownError;
            }

            return op;
        }
    }
}
