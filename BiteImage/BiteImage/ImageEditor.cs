using System.Diagnostics;
using System.Drawing;


namespace BiteImage
{
    public static class ImageEditor
    {
        public static Bitmap PixelizationOrNull(Bitmap image, uint row, uint column)
        {
            Debug.Assert(image != null);

            int height = image.Height;
            int width = image.Width;

            if (row == 0 || column == 0)
            {
                return null;
            }
            if (height < row || width < column)
            {
                return null;
            }

            Bitmap mosaic = new Bitmap(width, height);

            int pixelHeight = (int)(height / row);
            int pixelWidth = (int)(width / column);
            int extraRowCnt = (int)(height % row) * -1;
            int extraColCnt = (int)(width % column) * -1;

            int startRow = 0;
            int startCol = 0;
            int endRow = 0;
            int endCol = 0;

            for (int r = 0; r < row; ++r)
            {
                endRow += pixelHeight + (extraRowCnt++ >> 31) * -1;

                for (int c = 0; c < column; ++c)
                {
                    endCol += pixelWidth + (extraColCnt++ >> 31) * -1;
                    int red = 0;
                    int green = 0;
                    int blue = 0;

                    for (int pixelRow = startRow; pixelRow < endRow; ++pixelRow)
                    {
                        for (int pixelCol = startCol; pixelCol < endCol; ++pixelCol)
                        {
                            Color origin = image.GetPixel(pixelCol, pixelRow);
                            red += origin.R;
                            green += origin.G;
                            blue += origin.B;
                        }
                    }

                    int pixelCount = (endCol - startCol) * (endRow - startRow);
                    red /= pixelCount;
                    green /= pixelCount;
                    blue /= pixelCount;

                    for (int pixelRow = startRow; pixelRow < endRow; ++pixelRow)
                    {
                        for (int pixelCol = startCol; pixelCol < endCol; ++pixelCol)
                        {
                            mosaic.SetPixel(pixelCol, pixelRow, Color.FromArgb(red, green, blue));
                        }
                    }

                    startCol = endCol;
                }
                startRow = endRow;
                startCol = 0;
                endCol = 0;
                extraColCnt -= (int)column;
            }
            return mosaic;
        }

    }
}
