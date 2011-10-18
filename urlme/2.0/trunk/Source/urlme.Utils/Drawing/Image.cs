using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace urlme.Utils.Drawing
{
    public static class Image
    {
        public static System.Drawing.Image ResizeImage(System.Drawing.Image originalPic, int maxWidth, int maxHeight)
        {
            int height = maxHeight, width = maxWidth;
            int originalW = originalPic.Width, originalH = originalPic.Height;
            float percentW, percentH, percent;

            percentW = (float)maxWidth / (float)originalW;
            percentH = (float)maxHeight / (float)originalH;
            if (percentH < percentW)
            {
                percent = percentH;
                width = (int)(originalW * percent);
            }
            else
            {
                percent = percentW;
                height = (int)(originalH * percent);
            }

            Bitmap newPic = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            newPic.SetResolution(originalPic.HorizontalResolution, originalPic.VerticalResolution);

            using (Graphics g = Graphics.FromImage(newPic))
            {
                g.Clear(Color.Black);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalPic,
                    new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, originalW, originalH),
                    GraphicsUnit.Pixel);
            }

            return newPic;
        }

        /// <summary>
        /// Resizes an image to the exact dimensions given, using padding to make the image fit the dimensions.
        /// </summary>
        /// <param name="originalPic">The image to resize.</param>
        /// <param name="width">The width to make the resulting image.</param>
        /// <param name="height">The height to make the resulting image.</param>
        /// <param name="backgroundColor">The color to make the background.</param>
        /// <returns>The resized image.</returns>
        public static System.Drawing.Image FitImage(System.Drawing.Image originalPic, int width, int height, Color backgroundColor)
        {
            int iHeight = height, iWidth = width;
            int originalW = originalPic.Width, originalH = originalPic.Height;
            float ratio;

            float percentW = (float)width / (float)originalW;
            float percentH = (float)height / (float)originalH;
            if (percentH < percentW)
            {
                ratio = percentH;
                iWidth = (int)(originalW * ratio);
            }
            else
            {
                ratio = percentW;
                iHeight = (int)(originalH * ratio);
            }

            // figure out starting position
            float offsetX = (iWidth - width) / 2 * (-1);
            float offsetY = (iHeight - height) / 2 * (-1);
            
            Bitmap newPic = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            newPic.SetResolution(originalPic.HorizontalResolution, originalPic.VerticalResolution);

            using (Graphics g = Graphics.FromImage(newPic))
            {
                g.Clear(backgroundColor);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalPic, offsetX, offsetY, iWidth, iHeight);
            }

            return newPic;
        }

        public static System.Drawing.Image GetThumbnail(System.Drawing.Image originalPic, int width, int hieght)
        {
            return originalPic.GetThumbnailImage(width, hieght, null, new System.IntPtr());
        }

        /// <summary>
        /// Gets an <see cref="System.Drawing.Image"/> from a byte array of data
        /// </summary>
        /// <param name="data">The binary data to create an image out of.</param>
        /// <returns>The image object for the data.</returns>
        public static System.Drawing.Image GetImage(byte[] data)
        {
            return System.Drawing.Image.FromStream(
                    new System.IO.MemoryStream(data));
        }
    }
}
