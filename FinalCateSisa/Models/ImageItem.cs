namespace FinalCateSisa.Models
{
    public class ImageItem
    {
        public ImageItem(string imagePath)
            => this.ImagePath = imagePath;

        public string ImagePath { get; }
    }
}
