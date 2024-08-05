using ChronoPhage.Style;
using ChronoPhage.Core;
using ChronoPhage.Pages.Root;
using ChronoPhage.Storage;
using ChronoPhage.Storage.Database;

namespace ChronoPhage
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DatabaseService.INITIALZE_TABLES();

            LocalStorage.Boot();
            BaseTheme.SetTheme();

            MainPage = PageManager.RootShell;
        }



        protected override void OnStart()
        {
            base.OnStart();
        }


        protected override void OnSleep()
        {
            base.OnSleep();
        }


        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}
