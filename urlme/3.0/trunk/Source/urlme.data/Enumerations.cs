﻿using System;
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
            [Description("This path is already taken.")]
            PathAlreadyTaken,
            [Description("You already have a link with this custom path.")]
            UserAlreadyHasLink,
            [Description("Unkown error, please try again.")]
            UnknownError,
            [Description("Please provide both a long url and path")]
            InvalidInput
        }
    }
}