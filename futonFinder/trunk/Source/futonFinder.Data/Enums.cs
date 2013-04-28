using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace futonFinder.Data
{
    public class Enums
    {
        public enum FormMethod
        {
            Get,
            Post,
            Delete
        }

        public enum MovieReviewStatus
        {
            Approved,
            Pending,
            Disapproved,
            NotRequired
        }

        public enum LogType
        {
            Error,
            Info
        }
    }
}