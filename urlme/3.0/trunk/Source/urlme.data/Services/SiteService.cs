using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ianhd.core.Validation;
using urlme.data.Models;
using ianhd.core.Extensions;

namespace urlme.data.Services
{
    public static class SiteService
    {
        public static Operations.SaveLink LinkSave(data.Models.Link source)
        {
            var op = new data.Operations.SaveLink();

            var insert = source.LinkId == default(int);
            var update = !insert;

            // trim everything
            source.DestinationUrl = source.DestinationUrl.TrimToNull();
            source.Path = source.Path.TrimToNull();

            // valid input?
            if (string.IsNullOrWhiteSpace(source.DestinationUrl) || string.IsNullOrWhiteSpace(source.Path))
            {
                op.Result = Enumerations.SaveLinkResult.InvalidInput;
            }

            // if it exists already, not cool
            var existing = Link.Get(source.Path);
            var exists = false;

            if (!op.HasError && insert && existing != null)
            {
                exists = true;
            }

            if (!op.HasError && update && existing != null && existing.LinkId != source.LinkId)
            {
                exists = true;
            }

            if (exists)
            {
                if (existing.UserId == source.UserId)
                    op.Result = Enumerations.SaveLinkResult.UserAlreadyHasLink;
                else
                    op.Result = Enumerations.SaveLinkResult.PathAlreadyTaken;
            }

            if (!op.HasError && !Link.Save(source))
            {
                op.Result = Enumerations.SaveLinkResult.UnknownError;
            }

            if (op.Result == Enumerations.SaveLinkResult.Success || op.Result == Enumerations.SaveLinkResult.UserAlreadyHasLink)
            {
                op.Item = Link.Get(source.Path);
            }

            return op;
        }

        public static Operations.SaveLink LinkOverwrite(data.Models.Link source)
        {
            var op = new data.Operations.SaveLink();

            // trim everything
            source.DestinationUrl = source.DestinationUrl.TrimToNull();

            // valid input?
            if (string.IsNullOrWhiteSpace(source.DestinationUrl))
            {
                op.Result = Enumerations.SaveLinkResult.InvalidInput;
                return op;
            }

            // make sure existing's userId matches source's userId
            var existing = Link.Get(source.LinkId);
            if (existing.UserId != source.UserId)
            {
                op.Result = Enumerations.SaveLinkResult.PathAlreadyTaken;
                return op;
            }

            // save existing again, but with different destinationUrl
            existing.DestinationUrl = source.DestinationUrl;

            if (!op.HasError && !Link.Save(existing))
            {
                op.Result = Enumerations.SaveLinkResult.UnknownError;
            }

            if (op.Result == Enumerations.SaveLinkResult.Success)
            {
                op.Item = Link.Get(source.LinkId);
            }

            return op;
        }
    }
}
