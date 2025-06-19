
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageApp
{
    public partial class MainWindow : Window
    {
        private Bitmap originalBitmap;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (ofd.ShowDialog() == true)
            {
                originalBitmap = new Bitmap(ofd.FileName);

                using MemoryStream originalStream = new MemoryStream();
                originalBitmap.Save(originalStream, ImageFormat.Bmp);
                originalStream.Position = 0;

                BitmapImage originalImage = new BitmapImage();
                originalImage.BeginInit();
                originalImage.StreamSource = originalStream;
                originalImage.CacheOption = BitmapCacheOption.OnLoad;
                originalImage.EndInit();

                ColoredImage.Source = originalImage;
            }
        }
        private async void BtnConvert_Click(object sender, RoutedEventArgs e)
        {
            if (originalBitmap == null)
            {
                MessageBox.Show("Please open an image first.");
                return;
            }

            ProgressBar.Value = 0;

            try
            {
                using var client = new HttpClient();

                using var memoryStream = new MemoryStream();
                originalBitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                var byteContent = new ByteArrayContent(memoryStream.ToArray());
                byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                ProgressBar.Value = 20;
                var response = await client.PostAsync("https://localhost:7265/image/grayscale", byteContent); // use your actual API URL

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Image conversion failed. Server returned error.");
                    return;
                }
                ProgressBar.Value = 50;
                using var responseStream = await response.Content.ReadAsStreamAsync();

                BitmapImage resultImage = new BitmapImage();
                resultImage.BeginInit();
                resultImage.StreamSource = responseStream;
                resultImage.CacheOption = BitmapCacheOption.OnLoad;
                resultImage.EndInit();

                BWImage.Source = resultImage;
                ProgressBar.Value = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

    }
}