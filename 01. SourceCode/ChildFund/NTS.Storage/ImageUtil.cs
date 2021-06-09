using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NTS.Storage
{
    public class ImageUtil
    {
        public static bool ThumbnailCallback()
        {
            return false;
        }

        public static Bitmap CreateImageThumbnail(HttpPostedFile imageUpload)
        {
            int width = 300;
            Bitmap bitmapResult = null;
            try
            {
                var brush = new SolidBrush(Color.Black);
                Stream inputStream = imageUpload.InputStream;
                Image.GetThumbnailImageAbort thumbnailCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                Bitmap bitmapImage = new Bitmap(inputStream);

                PropertyItem propertie = bitmapImage.PropertyItems.FirstOrDefault(p => p.Id == 274);
                if (propertie != null)
                {
                    int orientation = propertie.Value[0];
                    if (orientation == 6)
                        bitmapImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    if (orientation == 8)
                        bitmapImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                }

                Image image = bitmapImage.GetThumbnailImage(width, (bitmapImage.Height *300)/ bitmapImage.Width, thumbnailCallback, IntPtr.Zero);
                bitmapResult = new Bitmap(image);
            }
            catch { }
            return bitmapResult;
        }
    }
}
