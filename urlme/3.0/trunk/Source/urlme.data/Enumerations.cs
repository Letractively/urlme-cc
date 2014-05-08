using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urlme.data
{
    public class Enumerations
    {
        public enum SaveLinkResult
        {
            Success,
            [Description("Someone else already has this path.")]
            PathAlreadyTaken,
            [Description("You already have a link with this custom path. Overwrite it.")]
            UserAlreadyHasLink,
            [Description("Unkown error, please try again.")]
            UnknownError,
            [Description("Please provide both a long url and path")]
            InvalidInput
        }
    }
}
