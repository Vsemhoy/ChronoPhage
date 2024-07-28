using ChronoFuck.Database;
using ChronoFuck.Database.Models;

namespace ChronoFuck.Pages.Chrono;

public partial class ChronoRootPage : ContentPage
{
    Grid body = new Grid();

    VerticalStackLayout filterStack = new VerticalStackLayout();
    ScrollView scrollView = new ScrollView();
    VerticalStackLayout itemStack = new VerticalStackLayout();


	public ChronoRootPage()
	{
		InitializeComponent();


        this.body.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
        this.body.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) });
        //this.body.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });

        this.scrollView.Content = this.itemStack;

        this.body.Children.Add(this.filterStack);
        Grid.SetRow(this.filterStack, 0);
        this.body.Children.Add(this.scrollView);
        Grid.SetRow(this.scrollView, 1);

        this.Content = this.body;

    }


    public async void Boot()
    {
        LocalStorage.AllCategories = await EventCategory.GetAllItemsAsync();
        LocalStorage.AllTypes = await EventType.GetAllItemsAsync();
    }



    public async Task loadDefaults()
    {
        this.Boot();

        this.itemStack.Children.Clear();
         LocalStorage.ChronoEventCollection = await ChronoEvent.GetAllActiveItemsAsync();
        var items = LocalStorage.ChronoEventCollection;

        if (items.Count > 0)
        {
            StackLayout divider = new StackLayout();
            divider.BackgroundColor = Color.FromArgb("#77777777");
            divider.HeightRequest = 1;
            this.itemStack.Add(divider);
        }

        for (int i = 0; i < items.Count; i++)
        {

            var item = items[i];
            if (item.IsRunning == true)
            {
                continue;
            }
            var cat = LocalStorage.AllCategories.Where(i => i.Id == item.CategoryId).Single();
            var typ = LocalStorage.AllTypes.Where(i => i.Id == item.TypeId).Single() ;

            var bgColor = Microsoft.Maui.Graphics.Color.FromHex(cat.BgColor);
            var textColor = Microsoft.Maui.Graphics.Color.FromHex(cat.TextColor);


            DateTime fin = item.EndAt;
            DateTime start = item.StartAt;

            TimeSpan difference = fin - start.ToLocalTime();

            var D = difference.Days;
            var H = difference.Hours;
            var M = difference.Minutes;
            var S = difference.Seconds;

            string totalstring = "";
            if (D != 0 && D.ToString() != "")
            {
                totalstring += D.ToString() + " Days and " + H.ToString() + " Hours";
            }
            else
            {
                totalstring += H.ToString("D2") + ":" + M.ToString("D2") + ":" + S.ToString("D2");
            }


            VerticalStackLayout shell = new VerticalStackLayout();
            shell.Padding = new Thickness(6);
            shell.BackgroundColor = bgColor;
            Label typeLabel = new Label();
            typeLabel.TextColor = textColor;
            typeLabel.Text = typ.Title;
            shell.Children.Add(typeLabel);

            Label duration = new Label();
            duration.TextColor = textColor;
            duration.Text = totalstring;
            shell.Children.Add(duration);

            this.itemStack.Children.Add(shell);

            StackLayout divider = new StackLayout();
            divider.BackgroundColor = Color.FromArgb("#77777777");
            divider.HeightRequest = 1;
            this.itemStack.Add(divider);
        }


    }


    protected override void OnAppearing()
    {

        base.OnAppearing();
        // Handle any logic when the page appears
       this.loadDefaults();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Handle any cleanup when the page disappears
        //DisposeResources();
        Navigation.PopAsync();
    }

    private void DisposeResources()
    {
        // Dispose or cleanup any resources

    }
}