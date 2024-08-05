using ChronoPhage.Core;
using ChronoPhage.Pages.Root;

namespace ChronoPhage
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = PageManager.RootShell;
        }
    }
}
