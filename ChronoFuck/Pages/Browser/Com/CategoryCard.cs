using System.Runtime.CompilerServices;
using System.Windows.Input;

using ChronoFuck.Database.Models;
using Microsoft.Maui.Graphics.Text;
using Microsoft.Maui.Layouts;

namespace ChronoFuck.Pages.Browser.Com;

internal partial class CategoryCardItem
{
    Frame body = new Frame();

    public string id { get; }
    public string title { get; set; }
    public int counter { get; set; }

    public Color bgColor { get; set; }
    public Color txColor { get; set; }

    public TapGestureRecognizer tapHandler = new TapGestureRecognizer();
    Grid mainGrid = new Grid();
    //mainGrid.BackgroundColor = System.Drawing.Color.White;
    StackLayout topStack = new StackLayout();
    StackLayout middleStack = new StackLayout();
    StackLayout bottomStack = new StackLayout();

    int itemsCount = 0;
    string counterString = "Items: ";




    public CategoryCardItem(ref EventCategory category)
    {
           
        //InitializeComponent();
        this.title = category.Title;
        this.id = category.Id;
        this.counter = category.CountItems;



        this.bgColor = (category.BgColor != null && category.BgColor != "")      ? Microsoft.Maui.Graphics.Color.FromHex(category.BgColor) : Colors.White;
        this.txColor = (category.TextColor != null && category.TextColor != "")  ? Microsoft.Maui.Graphics.Color.FromHex(category.TextColor) : Colors.Black;

        body.CornerRadius = 1;
        body.Padding = new Thickness(1);


        body.GestureRecognizers.Add(tapHandler);


        //var gradientColors = new GradientStopCollection
        //    {
        //        new GradientStop { Color = Colors.Blue, Offset = 0.0f },
        //        new GradientStop { Color = Colors.Purple, Offset = 1.0f }
        //    };
        //var gradientBrush = new LinearGradientBrush(gradientColors, new Point(0, 1), new Point(1, 0));

        body.Background = this.txColor;

        this.itemsCount = category.CountItems;
        //mainGrid.BackgroundColor = Microsoft.Maui.Graphics.Colors.Transparent;

        //Image image = new Image();
        //image.Source = "test_icon.png";
        //image.HeightRequest = 22;
        //topStack.Children.Add(image);

        Label labelTitle = new Label();
        labelTitle.Text = title;
        labelTitle.HorizontalTextAlignment = TextAlignment.Center;
        labelTitle.VerticalTextAlignment = TextAlignment.Center;
        labelTitle.FontSize = 14;
        labelTitle.TextColor = this.txColor;
        labelTitle.Padding = new Thickness(4);

        Label labelDescription = new Label();
        labelDescription.Text = this.counterString + this.itemsCount;
        labelDescription.HorizontalTextAlignment = TextAlignment.Center;
        labelDescription.FontSize = 10;
        labelDescription.TextColor = this.txColor;
        labelDescription.Padding = new Thickness(6);

        middleStack.Children.Add(labelTitle);
        bottomStack.Children.Add(labelDescription);
        mainGrid.Background = this.bgColor;

        mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
        mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) });
        mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Star) });

        mainGrid.Children.Add(topStack);
        Grid.SetRow(topStack, 0);
        mainGrid.Children.Add(middleStack);
        Grid.SetRow(middleStack, 1);
        mainGrid.Children.Add(bottomStack);
        Grid.SetRow(bottomStack, 2);
        //mainGrid.BackgroundColor = Colors.Aqua;
        
        body.Content = mainGrid;

        this.tapHandler.Tapped += TapEventStyle;
        //body.Children.Add(mainGrid);
        
    }

    public Frame Get()
    {
        return body;
    }

    private async void TapEventStyle(object? sender, EventArgs e)
    {

        body.BackgroundColor = Colors.WhiteSmoke;
        await Task.Delay(50);
        body.BackgroundColor = Colors.Gainsboro;
        await Task.Delay(50);
        body.BackgroundColor = Colors.LightGray;
        await Task.Delay(100);
        body.BackgroundColor = Colors.Gainsboro;
        await Task.Delay(100);
        body.BackgroundColor = Colors.White;
    }
}