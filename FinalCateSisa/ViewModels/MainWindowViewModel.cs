using FinalCateSisa.Models;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinalCateSisa.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager rm;
        private readonly IEventAggregator ea;

        public MainWindowViewModel(IRegionManager rm, IEventAggregator ea)
        {
            this.rm = rm;
            this.ea = ea;
        }

        #region Enablers
        private bool rules = false;
        public bool Rules
        {
            get { return rules; }
            set { SetProperty(ref rules, value); }
        }

        private bool canAttack = false;
        public bool CanAttack
        {
            get { return canAttack; }
            set { SetProperty(ref canAttack, value); }
        }

        private bool canDefend = false;
        public bool CanDefend
        {
            get { return canDefend; }
            set { SetProperty(ref canDefend, value); }
        }
        #endregion

        private ObservableCollection<ImageItem> imageItems = new();
        public ObservableCollection<ImageItem> ImageItems
        {
            get { return imageItems; }
            set { SetProperty(ref imageItems, value); }
        }

        public Prism.Commands.DelegateCommand PredictionModeCommand => new Prism.Commands.DelegateCommand(ExecutePredictionModeCommand);
        private void ExecutePredictionModeCommand()
        {
            CanAttack = false;
            rm.RequestNavigate("ContentRegion", "PredictionView");

        }

        public Prism.Commands.DelegateCommand AttackModeCommand => new Prism.Commands.DelegateCommand(ExecuteAttackModeCommand);
        private void ExecuteAttackModeCommand()
        {
            CanAttack = true;
            rm.RequestNavigate("ContentRegion", "AttackView");
        }

        public Prism.Commands.DelegateCommand VanillaModelCommand => new Prism.Commands.DelegateCommand(ExecuteVanillaModelCommand);
        private void ExecuteVanillaModelCommand()
        {
            CanDefend = false;
        }

        public Prism.Commands.DelegateCommand DefendedModelCommand => new Prism.Commands.DelegateCommand(ExecuteDefendedModelCommand);
        private void ExecuteDefendedModelCommand()
        {
            CanDefend = true;
        }

        public DelegateCommand<string> ImageClickedCommand => new Prism.Commands.DelegateCommand<string>(ExecuteImageClickedCommand);
        private void ExecuteImageClickedCommand(string imagePath)
        {
            ea.GetEvent<ImageClickedEvent>().Publish(imagePath);
        }

        public Prism.Commands.DelegateCommand LoadImagesCommand => new Prism.Commands.DelegateCommand(ExecuteLoadImagesCommand);

        private async void ExecuteLoadImagesCommand()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.FileName = "Select folder";
            dialog.Filter = "Folders|*.";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (dialog.ShowDialog() == true)
            {
                string folderPath = Path.GetDirectoryName(dialog.FileName);
                var imageFilePaths = Directory.GetFiles(folderPath)
                    .Where(file => file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".png") || file.ToLower().EndsWith(".jpeg"));

                var images = await Task.Run(() => LoadImages(imageFilePaths));

                imageItems.Clear();
                images.ForEach(s => imageItems.Add(new ImageItem(s)));
                Rules = true;
            }
            List<string> LoadImages(IEnumerable<string> imageFilePaths)
            {
                var images = new List<string>();
                foreach (var imagePath in imageFilePaths)
                {
                    images.Add(imagePath);
                }
                Thread.Sleep(1000);
                return images;
            }
        }
    }
}
