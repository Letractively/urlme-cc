﻿using ianhd.core.Extensions;

namespace urlme.data.Operations
{
    public class SaveLink
    {
        public Enumerations.SaveLinkResult Result { get; set; }
        public data.Models.Link Item { get; set; }
        public bool HasError
        {
            get
            {
                return this.Result != Enumerations.SaveLinkResult.Success;
            }
        }
        public string Message
        {
            get
            {
                return this.Result.GetDescription();
            }
        }
        public bool WasSuccessful
        {
            get
            {
                return !this.HasError;
            }
        }

        public SaveLink()
        {
            this.Result = Enumerations.SaveLinkResult.Success;
        }
    }
}
