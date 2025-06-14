using System;
using System.Drawing;

namespace BackendLib
{
    public interface IImageProcessor
    {
        Bitmap ConvertToGrayscale(Bitmap input, IProgress<int> progress = null);
    }
}
