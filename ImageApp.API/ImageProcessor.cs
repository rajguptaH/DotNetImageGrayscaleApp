using System;
using System.Drawing;
using System.Runtime.Versioning;

namespace BackendLib
{
    public class ImageProcessor : IImageProcessor
    {
        public Bitmap ConvertToGrayscale(Bitmap input, IProgress<int> progress = null)
        {
            Logger.Log("User requested image conversion.");
            Bitmap output = new Bitmap(input.Width, input.Height);

            for (int y = 0; y < input.Height; y++)
            {
                for (int x = 0; x < input.Width; x++)
                {
                    Color pixel = input.GetPixel(x, y);
                    int gray = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    output.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
                progress?.Report((y * 100) / input.Height);
            }

            Logger.Log("Image conversion completed.");
            return output;
        }
    }
}
