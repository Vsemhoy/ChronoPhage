using ChronoFuck.Pages.Browser;
using ChronoFuck.Pages.Chrono;
using ChronoFuck.Pages.Groups;

namespace ChronoFuck.Pages.Root;

public partial class MainTabsPage : Shell
{
    public MainTabsPage()
    {
        InitializeComponent();

        // Optional: Initialize pages in the constructor
        var browserPage = new BrowserRootPage();
        var chronoPage = new ChronoRootPage();
        var groupsPage = new GroupsRootPage();

        // Do something with these pages if needed
    }
}