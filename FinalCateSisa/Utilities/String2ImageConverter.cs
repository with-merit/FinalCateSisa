using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;

namespace FinalCateSisa.Utilities
{
    public static class String2ImageConverter
    {
        public static BitmapImage StringToImage(string base64String)
        {
            try
            {
                base64String = base64String.Trim('"').Trim('b').Trim('\'');
                byte[] imageBytes = Convert.FromBase64String(base64String);
                BitmapImage bitmapImage = new BitmapImage();
                using (var ms = new MemoryStream(imageBytes))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;
                    bitmapImage.EndInit();
                }
                return bitmapImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading image file: {ex.Message}");
                return null;
            }
        }
    }
}
