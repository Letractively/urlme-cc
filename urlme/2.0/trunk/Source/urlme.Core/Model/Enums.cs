using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using urlme.Utils.Extensions;
using urlme.Utils.Enum;

namespace urlme.Model.Enums
{
    public enum CrudLinkResults
    {
        [StringValue("Success!")]
        Success,
        [StringValue("Error. Please try again.")]
        Failure,
        [StringValue("Error: path already exists.")]
        PathAlreadyExistsByOwner,
        PathAlreadyExistsByNonOwner,
        [StringValue("Error: please provide both.")]
        InsufficientInput
    }

    public enum SortOptions
    {
        latest,
        path,
        hits
    }
}
