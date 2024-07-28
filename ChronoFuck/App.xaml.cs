using ChronoFuck.Database;
using ChronoFuck.Pages.Root;

namespace ChronoFuck
{
    public partial class App : Application
    {
        public static string DatabaseLocation = string.Empty;
        public App()
        {
            InitializeComponent();
            DatabaseService.INITIALZE_TABLES();
            //var naviPage = new NavigationPage(new NavContentPage1());
            //naviPage.BarBackgroundColor = Colors.Firebrick;
            ////MainPage = new DemoContentPage1();
            //naviPage.BarTextColor = Colors.Wheat;
            //MainPage = naviPage;

            //MainPage = new DemoFlyoutPage();
            LocalStorage.Boot();
            MainPage = new MainTabsPage();
        }


        ///**
        // * Set database location for different systems
        // */
        //public App(string databaseLocation)
        //{
        //    InitializeComponent();
        //    DatabaseLocation = databaseLocation;
        //    MainPage = new MainTabsPage();
        //}

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

//https://www.svgrepo.com/collection/responsive-solid-icons/2
