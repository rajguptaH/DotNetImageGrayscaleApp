using Image.API.Services;
using NUnit.Framework;
using System.Drawing;

namespace Tests
{
    public class EndToEndTest
    {
        [Test]
        public void ShouldConvertImageFullyWithoutExceptions()
        {
            // Arrange
            Bitmap input = new Bitmap(100, 100);
            var processor = new ImageProcessor();

            // Act
            Bitmap output = processor.ConvertToGrayscale(input);

            // Assert
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    var px = output.GetPixel(x, y);
                    Assert.AreEqual(px.R, px.G);
                    Assert.AreEqual(px.G, px.B);
                }
            }
        }
    }
}
