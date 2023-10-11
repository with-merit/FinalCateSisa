using System;
using System.IO;
using System.Windows;

namespace FinalCateSisa.Utilities
{
    public static class Image2StringConverter
    {
        public static string ImageToString(string imagePath)
        {
            try
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                return Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading image file: {ex.Message}");
                return null;
            }
        }
    }
}
