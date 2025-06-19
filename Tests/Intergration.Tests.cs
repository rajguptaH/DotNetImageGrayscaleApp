using NUnit.Framework;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageApp;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using Image.API.Services;

namespace DotNetImageGrayscaleApp.Tests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class IntegrationTests
    {
        private MainWindow _window;
        private ImageProcessor _processor;

        [SetUp]
        public void Setup()
        {
            _processor = new ImageProcessor();
            _window = new MainWindow();
            _window.Show();
            _window.UpdateLayout();
        }

        [TearDown]
        public void Cleanup()
        {
            _window.Close();
        }

        [Test]
        public void ConvertImage_UsingRealBackend_ShouldUpdateUI()
        {
            var pngBytes = new byte[]
            {
                137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,
                0,0,0,1,0,0,0,1,8,2,0,0,0,144,119,83,222,
                0,0,0,12,73,68,65,84,8,153,99,248,15,4,
                0,9,251,3,253,160,118,238,124,0,0,0,0,
                73,69,78,68,174,66,96,130
            };

            var bitmapImage = new BitmapImage();
            using (var stream = new MemoryStream(pngBytes))
            {
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            Bitmap inputBitmap;
            using (var ms = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(ms);
                ms.Position = 0;
                inputBitmap = new Bitmap(ms);
            }

            Bitmap grayBitmap = _processor.ConvertToGrayscale(inputBitmap);
            BitmapSource convertedImage = ConvertBitmapToBitmapSource(grayBitmap);

            var bwImage = _window.FindName("BWImage") as System.Windows.Controls.Image;
            Assert.That(bwImage, Is.Not.Null, "BWImage not found in the window.");

            _window.Dispatcher.Invoke(() =>
            {
                bwImage!.Source = convertedImage;
            });

            var result = bwImage!.Source as BitmapSource;
            Assert.That(result, Is.Not.Null, "Final BW image source is still null.");
            Assert.That(result!.Format, Is.EqualTo(PixelFormats.Gray8).Or.EqualTo(PixelFormats.Bgr24).Or.EqualTo(PixelFormats.Bgra32), $"Unexpected format: {result!.Format}");
        }

        [Test]
        public void UIElements_ShouldBePresent()
        {
            Assert.That(_window.FindName("ColoredImage"), Is.InstanceOf<System.Windows.Controls.Image>(), "ColoredImage not found.");
            Assert.That(_window.FindName("BWImage"), Is.InstanceOf<System.Windows.Controls.Image>(), "BWImage not found.");
            Assert.That(_window.FindName("ProgressBar"), Is.InstanceOf<ProgressBar>(), "ProgressBar not found.");
        }

        [Test]
        public void ProgressBar_ShouldAcceptValidRange()
        {
            var progressBar = _window.FindName("ProgressBar") as ProgressBar;
            Assert.That(progressBar, Is.Not.Null);
            Assert.That(progressBar!.Minimum, Is.EqualTo(0));
            Assert.That(progressBar.Maximum, Is.EqualTo(100));
        }

        [Test]
        public void ConvertImage_NullImage_ShouldNotCrash()
        {
            Bitmap nullBitmap = null;

            Assert.DoesNotThrow(() =>
            {
                var result = _processor.ConvertToGrayscale(nullBitmap!);
            }, "Processor threw exception on null input.");
        }

        [Test]
        public void ViewModel_Binding_ShouldBeSet()
        {
            Assert.That(_window.DataContext, Is.Not.Null, "ViewModel binding is null.");
        }

        public static BitmapSource ConvertBitmapToBitmapSource(Bitmap bitmap)
        {
            var hBitmap = bitmap.GetHbitmap();
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return bitmapSource;
        }
    }
}
