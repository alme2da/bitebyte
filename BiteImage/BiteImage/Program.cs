using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BiteImage
{
    class Program
    {
        static void Main(string[] args)
        {
            const String origin = "이미지 8.png";
            List<Bitmap> photos = new List<Bitmap>(103);

            for(int i = 1; i <= 103; ++i)
            {
                photos.Add(new Bitmap($"이미지 {i}.png"));
            }

            Bitmap image = new Bitmap(origin);
            Bitmap mosaic = ImageEditor.PhotoPixelization(image, photos, 100, 100);
            mosaic.Save($"{Path.GetFileNameWithoutExtension(origin)}_shift_left.png", ImageFormat.Png);
        }
    }
}
