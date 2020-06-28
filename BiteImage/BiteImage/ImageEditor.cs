using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;


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

        public static Bitmap PhotoPixelization(Bitmap background, List<Bitmap> materials, int row, int column)
        {
            Color[,] backgroundInfo = GetPixelInfo(background, row, column);

            Debug.Assert(background != null);

            const float BACKGROUND_RATIO = 0.7f;
            const float MATERIAL_RATIO = 0.3f;

            int materialHeightResolution = 10;
            int materialWidthResolution = 10;

            Bitmap result = new Bitmap(materialHeightResolution * column, materialWidthResolution * row);
            List<Color[,]> materialInfos = new List<Color[,]>(256);

            foreach (Bitmap material in materials) 
            {
                materialInfos.Add(GetPixelInfo(material, materialHeightResolution, materialWidthResolution));
            }

            Random random = new Random();

            for (int backgroundRow = 0; backgroundRow < row; ++backgroundRow)
            {
                int startRow = materialHeightResolution * backgroundRow;

                for (int backgroundCol = 0; backgroundCol < column; ++backgroundCol)
                {
                    int resultRow = startRow;

                    int startCol = materialWidthResolution * backgroundCol;
                    int resultCol = startCol;

                    Color[,] materialInfo = materialInfos[random.Next(0, materialInfos.Count - 1)];

                    for (int materialRow = 0; materialRow < materialHeightResolution; ++materialRow)
                    {
                        for (int materialCol = 0; materialCol < materialWidthResolution; ++materialCol)
                        {
                            Color color = Color.FromArgb((int)(backgroundInfo[backgroundRow, backgroundCol].R * BACKGROUND_RATIO + materialInfo[materialRow, materialCol].R * MATERIAL_RATIO),
                                                         (int)(backgroundInfo[backgroundRow, backgroundCol].G * BACKGROUND_RATIO + materialInfo[materialRow, materialCol].G * MATERIAL_RATIO),
                                                         (int)(backgroundInfo[backgroundRow, backgroundCol].B * BACKGROUND_RATIO + materialInfo[materialRow, materialCol].B * MATERIAL_RATIO));
                         
                            result.SetPixel(resultCol, resultRow, color);
                            resultCol++;
                        }
                        resultCol = startCol;
                        resultRow++;
                        
                    }
                }
            }

            return result;
        }

        private static Color[,] GetPixelInfo(Bitmap image, int row, int column)
        {
            Debug.Assert(image != null);
            Debug.Assert((0 < row && row <= image.Height) && (0 < column && column <= image.Width));
            
            int height = image.Height;
            int width = image.Width;

            Color[,] info = new Color[row, column];

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

                    info[r, c] = Color.FromArgb(red, green, blue);
                    
                    startCol = endCol;
                }
                startRow = endRow;
                startCol = 0;
                endCol = 0;
                extraColCnt -= (int)column;
            }
            return info;
        }
    }
}

