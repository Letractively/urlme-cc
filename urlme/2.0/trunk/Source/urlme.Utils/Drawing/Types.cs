using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace urlme.Utils.Drawing
{
    public enum ImageTypeOptions
    {
        [Utils.Enum.StringValue("application/octet-stream")]
        Unknown = 0,
        [Utils.Enum.StringValue("image/jpeg")]
        Jpg = 1,
        [Utils.Enum.StringValue("image/gif")]
        Gif = 2,
        [Utils.Enum.StringValue("image/x-png")]
        Png = 3,
        [Utils.Enum.StringValue("image/x-ms-bmp")]
        Bmp = 4
    }
}
