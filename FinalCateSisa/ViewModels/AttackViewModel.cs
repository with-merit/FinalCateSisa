using FinalCateSisa.Models;
using FinalCateSisa.Services;
using FinalCateSisa.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Media.Imaging;

namespace FinalCateSisa.ViewModels
{
    public class AttackViewModel : BindableBase, IRegionMemberLifetime
    {
        private readonly IEventAggregator ea;
        public AttackViewModel(IEventAggregator ea)
        {
            this.ea = ea;
            ea.GetEvent<ImageClickedEvent>().Subscribe(OnImagedClicked);
        }

        private async void OnImagedClicked(string imagePath)
        {
            SelectedImage = imagePath;
            var response = await HttpService.MakePredicton(Image2StringConverter.ImageToString(SelectedImage));
            ModelPredictions = String2ImageConverter.StringToImage(response);
        }
        public bool KeepAlive => false;
        private double epsilon = 4.0;
        public double Epsilon
        {
            get { return epsilon; }
            set { SetProperty(ref epsilon, value); }
        }

        private string selectedImage = "";
        public string SelectedImage
        {
            get { return selectedImage; }
            set { SetProperty(ref selectedImage, value); }
        }

        private BitmapImage noise = new();
        public BitmapImage Noise
        {
            get { return noise; }
            set { SetProperty(ref noise, value); }
        }

        private BitmapImage pertubedImage = new();
        public BitmapImage PertubedImage
        {
            get { return pertubedImage; }
            set { SetProperty(ref pertubedImage, value); }
        }

        private BitmapImage modelPredictions = new();
        public BitmapImage ModelPredictions
        {
            get { return modelPredictions; }
            set { SetProperty(ref modelPredictions, value); }
        }

        private BitmapImage attackedPredictions = new();
        

        public BitmapImage AttackedPredictions
        {
            get { return attackedPredictions; }
            set { SetProperty(ref attackedPredictions, value); }
        }

        public DelegateCommand FGSMCommand => new DelegateCommand(ExecuteFGSMCommand);
        private async void ExecuteFGSMCommand()
        {
            var jsonResponse = await HttpService.FGSMAttack(Image2StringConverter.ImageToString(SelectedImage));
            Helper(jsonResponse);
        }


        public DelegateCommand SliderValueCommand => new DelegateCommand(ExecuteSliderValueCommand);
        private async void ExecuteSliderValueCommand()
        {
            var jsonResponse = await HttpService.FGSMAttack(Image2StringConverter.ImageToString(SelectedImage));
            Helper(jsonResponse);
        }

        private void Helper(string jsonResponse)
        {
            Dictionary<string, string> response = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse);
            PertubedImage = String2ImageConverter.StringToImage(JsonSerializer.Deserialize<string>(response["perturbed_image"]));
            AttackedPredictions = String2ImageConverter.StringToImage(JsonSerializer.Deserialize<string>(response["attacked_predictions"]));
            Noise = String2ImageConverter.StringToImage(JsonSerializer.Deserialize<string>(response["noise"]));
        }
    }
}
