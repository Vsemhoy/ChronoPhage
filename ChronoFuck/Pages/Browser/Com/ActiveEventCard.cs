using ChronoFuck.Database;
using ChronoFuck.Database.Models;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChronoFuck.Pages.Browser.Com
{
    class ActiveEventCard
    {

        public int id { get; set; }


        public Color bgColor { get; set; }
        public Color textColor { get; set; }

        private HorizontalStackLayout body = new HorizontalStackLayout();
        private StackLayout swiperBody = new StackLayout();
        private Grid gridRow = new Grid();
        public TapGestureRecognizer tapHandler = new TapGestureRecognizer();

        public TapGestureRecognizer triggerTapHandler = new TapGestureRecognizer();

        public double fullWidth = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density) * (DeviceDisplay.Current.MainDisplayInfo.Density * 200);

        public SwipeView swiper = new SwipeView();
        public SwipeItem swipeLeftItem = new SwipeItem();
        public SwipeItem swipeRightItem = new SwipeItem();

        public HorizontalStackLayout durationStack = new HorizontalStackLayout();

        private Label title = new Label();
        ChronoEvent sourceObject { get; set; }

        public ActiveEventCard(ChronoEvent typeCard)
        {
            this.body.Margin = 2;
            this.sourceObject = typeCard;

            this.bgColor   = Microsoft.Maui.Graphics.Color.FromHex(LocalStorage.ActiveCategory.BgColor);
            this.textColor = Microsoft.Maui.Graphics.Color.FromHex(LocalStorage.ActiveCategory.TextColor);

            this.body.WidthRequest = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density);
            gridRow.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridRow.ColumnDefinitions.Add(new ColumnDefinition { Width = 40 }); // Icon
            gridRow.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); // Name
            gridRow.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Auto}); // Timer

            StackLayout iconContainer = new StackLayout();
            Image image = new Image();
            image.Source = "test_icon.png";
            iconContainer.Children.Add(image);
            iconContainer.Padding = new Thickness(3);
            iconContainer.VerticalOptions = LayoutOptions.Center;

            VerticalStackLayout textContainer = new VerticalStackLayout();
            this.title.Text = LocalStorage.ActiveType.Title;
            this.title.FontSize = 14;
            this.title.TextColor = this.textColor;
            textContainer.Children.Add(title);
            textContainer.Padding = new Thickness(3);



            //textContainer.Children.Add(this.durationStack);




            this.gridRow.BackgroundColor = this.bgColor;

            gridRow.Children.Add(iconContainer);
            Grid.SetRow(iconContainer, 0);
            Grid.SetColumn(iconContainer, 0);
            gridRow.Children.Add(textContainer);
            Grid.SetRow(textContainer, 0);
            Grid.SetColumn(textContainer, 1);
            gridRow.Children.Add(this.durationStack);
            Grid.SetRow(this.durationStack, 0);
            Grid.SetColumn(this.durationStack, 2);

            

            this.gridRow.WidthRequest = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density);


            Button btn = new Button { Text = "Super red ", BackgroundColor = Colors.Red };


            //body.Padding = new Thickness(3);
            body.BackgroundColor = Colors.DimGray;

            //body.GestureRecognizers

            body.GestureRecognizers.Add(tapHandler);
   




            this.swipeLeftItem.Text = "Edit item";
            this.swipeLeftItem.BackgroundColor = Colors.Sienna;


            this.swipeRightItem.Text = "Stop";
            this.swipeRightItem.BackgroundColor = Colors.Crimson;

            this.swiper.LeftItems.Add(swipeLeftItem);
            swiperBody.Children.Add(gridRow);
            this.swiper.Content = swiperBody;
            this.swiper.RightItems.Add(swipeRightItem);
            swipeLeftItem.Clicked += SwipeLeftItem_OpenTypeEditor_Clicked;



            body.Children.Add(swiper);
            this.SetDurationText_Process();
        }



        private void SwipeLeftItem_OpenTypeEditor_Clicked(object? sender, EventArgs e)
        {
            
        }

        public HorizontalStackLayout Get()
        {
            return this.body;
        }

        
        public async void SetDurationText_Process()
        {
            while (LocalStorage.ActiveCategory != null)
            {
                if (this.bgColor.ToHex() != LocalStorage.ActiveCategory.BgColor)
                {
                    this.bgColor = Microsoft.Maui.Graphics.Color.FromHex(LocalStorage.ActiveCategory.BgColor);
                    this.gridRow.BackgroundColor = this.bgColor;
                }
                if (this.textColor.ToHex() != LocalStorage.ActiveCategory.TextColor)
                {
                    this.textColor = Microsoft.Maui.Graphics.Color.FromHex(LocalStorage.ActiveCategory.TextColor);
                    this.title.TextColor = this.textColor;
                }

                int duration = 0;
                DateTime now = DateTime.Now;
                DateTime start = this.sourceObject.StartAt;

                TimeSpan difference = now - start.ToLocalTime();

                var D = difference.Days;
                var H = difference.Hours;
                var M = difference.Minutes;
                var S = difference.Seconds;

                string totalstring = "";
                if (D != 0 && D.ToString() != "")
                {
                    totalstring += D.ToString() + " Days and " + H.ToString() + " Hours";
                } else
                {
                    totalstring += H.ToString("D2") + ":" + M.ToString("D2") + ":" + S.ToString("D2");
                }

                Label timer = new Label();
                timer.Text = totalstring;
                timer.FontAttributes = FontAttributes.Bold;
                timer.FontSize = 18;
                timer.TextColor = this.textColor;
                timer.Padding = new Thickness(6);
                timer.VerticalOptions = LayoutOptions.Center;
                this.durationStack.Children.Clear();
                this.durationStack.Children.Add(timer);
                await Task.Delay(1000);
            }
        }


    }
}
