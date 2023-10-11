using FinalCateSisa.Views;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;

namespace FinalCateSisa
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PredictionView>();
            containerRegistry.RegisterForNavigation<AttackView>();
        }

        protected override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);
            var regionManager = Container.Resolve<IRegionManager>();
            var prediction = Container.Resolve<PredictionView>();
            regionManager.RegisterViewWithRegion("ContentRegion", () => prediction);
        }
    }
}
