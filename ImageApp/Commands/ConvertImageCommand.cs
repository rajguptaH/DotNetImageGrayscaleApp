using ImageApp.ViewModels;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageApp.Commands
{
    public class ConvertImageCommand : ICommand
    {
        private readonly MainViewModel _vm;

        public ConvertImageCommand(MainViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _ = ExecuteAsync(); 
        }

        private async Task ExecuteAsync()
        {
            try
            {
                _vm.Progress = 10;

                var asm = Assembly.LoadFrom("BackendLib.dll");
                var type = asm.GetType("BackendLib.ImageProcessor");
                dynamic processor = Activator.CreateInstance(type);

                using var ms = new MemoryStream();
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(_vm.ColoredImage));
                encoder.Save(ms);
                using var bmp = new Bitmap(ms);

                var result = await Task.Run(() => processor.ConvertToGrayscale(bmp));

                _vm.BWImage = MainViewModel.ConvertToBitmapImage(result);
                _vm.Progress = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during conversion: {ex.Message}", "Conversion Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}