using BackendLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
