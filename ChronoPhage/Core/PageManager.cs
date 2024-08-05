using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChronoPhage.Pages.Root;
using ChronoPhage.Pages.Com.MainPage;
using ChronoPhage.Pages.Com.StoryPage;


namespace ChronoPhage.Core
{
    internal class PageManager
    {

        public static MainPage MainPage = new MainPage();

        public static TypeBrowserInCategoryPage TypeBrowserInCategoryPage = new TypeBrowserInCategoryPage();
        


        public static StoryPage StoryPage = new StoryPage();



        // All other pages can be initialized before call it within RootShell
        public static Shell RootShell = new RootShell();



    }
}
