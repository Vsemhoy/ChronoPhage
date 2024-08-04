using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoPhage.Pages.Root
{
    public partial class  RootShell : Shell
    {

        ShellContent MainShellPage { get; set; }
        ShellContent StoryPage {  get; set; }

        public RootShell()
        {
            TabBar tabBar = new TabBar();
            
            this.BackgroundColor = Colors.AliceBlue;

            this.MainShellPage = new ShellContent();
            this.MainShellPage.Content = new MainPage();
            this.MainShellPage.Title = "Browser";
            this.MainShellPage.Icon = "dotnet_bot.png";
            tabBar.Items.Add(this.MainShellPage);


            this.StoryPage = new ShellContent();
            this.StoryPage.Content = new MainPage();
            this.StoryPage.Title = "Story";
            this.StoryPage.Icon = "dotnet_bot.png";
            tabBar.Items.Add(this.StoryPage);


            this.CurrentItem = this.MainShellPage;
            
        }
    }
}


