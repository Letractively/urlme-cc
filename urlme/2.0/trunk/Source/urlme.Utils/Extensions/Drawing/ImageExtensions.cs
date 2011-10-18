using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace urlme.Utils.Extensions.Drawing
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Converts an image to a byte[] for streaming to users.
        /// </summary>
        /// <param name="image">The <see cref="System.Drawing.Image"/> object we are extending.</param>
        /// <param name="format">The <see cref="ImageFormat"/> to save the image as.</param>
        /// <returns>The byte[] of data.</returns>
        public static byte[] ToByteArray(this System.Drawing.Image image, ImageFormat format)
        {
            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
            {
                image.Save(memStream, format);

                return memStream.ToArray();
            }
        }
    }
}
