using FinalCateSisa.Models;
using FinalCateSisa.Services;
using FinalCateSisa.Utilities;
using FinalCateSisa.Views;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace FinalCateSisa.ViewModels
{
    public class PredictionViewModel : BindableBase, IRegionMemberLifetime
    {
        private readonly IEventAggregator ea;
        private readonly IRegionManager regionManager;

        public PredictionViewModel(IEventAggregator ea, IRegionManager regionManager)
        {
            this.ea = ea;
            this.regionManager = regionManager;
            ea.GetEvent<ImageClickedEvent>().Subscribe(OnImagedClicked);
        }
        private string selectedImage = "";
        public string SelectedImage
        {
            get { return selectedImage; }
            set { SetProperty(ref selectedImage, value); }
        }

        private BitmapImage modelPredictions = new();
        public BitmapImage ModelPredictions
        {
            get { return modelPredictions; }
            set { SetProperty(ref modelPredictions, value); }
        }

        public bool KeepAlive => false;

        private async void OnImagedClicked(string imagePath)
        {
            SelectedImage = imagePath;
            var response = await HttpService.MakePredicton(Image2StringConverter.ImageToString(SelectedImage));
            ModelPredictions = String2ImageConverter.StringToImage(response);
        }

        
    }
}
