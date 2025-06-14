using BackendLib;
using NUnit.Framework;
using System.Drawing;

namespace UnitTests
{
    public class BackendLibTests
    {
        [Test]
        public void ConvertToGrayscale_ShouldReturnGrayscaleImage()
        {
            Bitmap bmp = new Bitmap(1, 1);
            bmp.SetPixel(0, 0, Color.Red);

            var processor = new ImageProcessor();
            Bitmap gray = processor.ConvertToGrayscale(bmp);

            Assert.AreEqual(gray.GetPixel(0, 0).R, gray.GetPixel(0, 0).G);
            Assert.AreEqual(gray.GetPixel(0, 0).G, gray.GetPixel(0, 0).B);
        }
    }
}