using System.Drawing;
using NUnit.Framework;
using System.Runtime.Versioning;
using BackendLib; // Make sure this matches your actual namespace

namespace Tests
{
    [SupportedOSPlatform("windows")]
    public class ImageProcessorTests
    {
        private ImageProcessor _processor;

        [SetUp]
        public void Setup()
        {
            _processor = new ImageProcessor();
        }

        [Test]
        public void ConvertToGrayscale_ReturnsSameSizeImage()
        {
            using Bitmap original = new Bitmap(10, 10);
            using Bitmap gray = _processor.ConvertToGrayscale(original);
            Assert.AreEqual(10, gray.Width);
            Assert.AreEqual(10, gray.Height);
        }

        [Test]
        public void ConvertToGrayscale_ConvertsPixelToGray()
        {
            using Bitmap original = new Bitmap(1, 1);
            original.SetPixel(0, 0, Color.FromArgb(100, 150, 200));

            using Bitmap gray = _processor.ConvertToGrayscale(original);
            Color pixel = gray.GetPixel(0, 0);

            int expectedGray = (int)(100 * 0.3 + 150 * 0.59 + 200 * 0.11);
            Assert.AreEqual(Color.FromArgb(expectedGray, expectedGray, expectedGray), pixel);
        }

        [Test]
        public void ConvertToGrayscale_WhiteImage_ReturnsWhiteGray()
        {
            using Bitmap original = new Bitmap(1, 1);
            original.SetPixel(0, 0, Color.White);

            using Bitmap gray = _processor.ConvertToGrayscale(original);
            Color pixel = gray.GetPixel(0, 0);

            Assert.AreEqual(Color.White, pixel);
        }

        [Test]
        public void ConvertToGrayscale_BlackImage_ReturnsBlackGray()
        {
            using Bitmap original = new Bitmap(1, 1);
            original.SetPixel(0, 0, Color.Black);

            using Bitmap gray = _processor.ConvertToGrayscale(original);
            Color pixel = gray.GetPixel(0, 0);

            Assert.AreEqual(Color.Black, pixel);
        }

        [Test]
        public void ConvertToGrayscale_NullProgress_DoesNotThrow()
        {
            using Bitmap original = new Bitmap(5, 5);
            Assert.DoesNotThrow(() =>
            {
                using Bitmap gray = _processor.ConvertToGrayscale(original, null);
            });
        }
    }
}
