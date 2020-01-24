using System;
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
            const string IMAGE_FILE_NAME = "beach.jpg";

            using (FileStream fs = File.OpenRead(IMAGE_FILE_NAME))
            using (Bitmap image = new Bitmap(fs))
            using (Bitmap newImage = ImageEditor.PixelizationOrNull(image,50,50))
            {
                newImage.Save($"{Path.GetFileNameWithoutExtension(IMAGE_FILE_NAME)}_box.png", ImageFormat.Png);
            }
        }
    }
}
