using ImageApp.Commands;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private BitmapImage _coloredImage;
        private BitmapImage _bwImage;
        private int _progress;

        public ICommand OpenImageCommand { get; }
        public ICommand ConvertImageCommand { get; }

        public BitmapImage ColoredImage
        {
            get => _coloredImage;
            set { _coloredImage = value; OnPropertyChanged(); }
        }

        public BitmapImage BWImage
        {
            get => _bwImage;
            set { _bwImage = value; OnPropertyChanged(); }
        }

        public int Progress
        {
            get => _progress;
            set { _progress = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            OpenImageCommand = new RelayCommand(OpenImage);
            ConvertImageCommand = new ConvertImageCommand(this);
        }

        private void OpenImage()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (dlg.ShowDialog() == true)
            {
                var bmp = new System.Drawing.Bitmap(dlg.FileName);
                ColoredImage = ConvertToBitmapImage(bmp);
                Progress = 0;
            }
        }

        public ICommand CloseAppCommand => new RelayCommand(() =>
        {
            Application.Current.Shutdown();
        });

        public static BitmapImage ConvertToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            using var memory = new MemoryStream();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            memory.Position = 0;

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}