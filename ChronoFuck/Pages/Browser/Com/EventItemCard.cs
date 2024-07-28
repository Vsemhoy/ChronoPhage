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
    class EventItemCard
    {

        public int id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Url2 { get; set; }

        public bool IsActive { get; set; }

        private HorizontalStackLayout body = new HorizontalStackLayout();
        private StackLayout swiperBody = new StackLayout();
        private Grid gridRow = new Grid();
        public TapGestureRecognizer tapHandler = new TapGestureRecognizer();

        public TapGestureRecognizer triggerTapHandler = new TapGestureRecognizer();

        public double fullWidth = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density) * (DeviceDisplay.Current.MainDisplayInfo.Density * 200);

        public SwipeView swiper = new SwipeView();
        public SwipeItem swipeLeftItem = new SwipeItem();
        public SwipeItem swipeRightItem = new SwipeItem();

        EventType sourceObject{ get; set;}

        public EventItemCard(EventType typeCard) 
        {
            this.IsActive = false;
            if (LocalStorage.ActiveChrono != null)
            {
                if (LocalStorage.ActiveChrono.TypeId == typeCard.Id)
                {
                    this.IsActive = true;
                }
            }
            this.sourceObject = typeCard;
            

            this.body.WidthRequest = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density);
            gridRow.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridRow.ColumnDefinitions.Add(new ColumnDefinition {Width = 40}); // Icon
            gridRow.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Star}); // Name
            //gridRow.ColumnDefinitions.Add(new ColumnDefinition {Width = 46}); // Activator

            StackLayout iconContainer = new StackLayout();
            Image image = new Image();
            image.Source = "test_icon.png";
            iconContainer.Children.Add(image);
            iconContainer.Padding = new Thickness(3);
            iconContainer.VerticalOptions = LayoutOptions.Center;

            VerticalStackLayout textContainer = new VerticalStackLayout();
            Label title = new Label();
            title.Text = typeCard.Title;
            title.FontSize = 14;
            title.TextColor = Colors.White;
            textContainer.Children.Add(title);
            textContainer.Padding = new Thickness(3);


            Label descr = new Label();
            descr.Text = typeCard.Description;
            descr.FontSize = 9;
            descr.TextColor = Colors.Gainsboro;
            textContainer.Children.Add(descr);
            
          


            this.gridRow.BackgroundColor = Colors.DimGray;

            gridRow.Children.Add(iconContainer);
            Grid.SetRow(iconContainer, 0);
            Grid.SetColumn(iconContainer, 0);
            gridRow.Children.Add(textContainer);
            Grid.SetRow(textContainer, 0);
            Grid.SetColumn(textContainer, 1);


            this.gridRow.WidthRequest = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density);
           

            Button btn = new Button { Text = "Super red ", BackgroundColor = Colors.Red };


            //body.Padding = new Thickness(3);
            body.BackgroundColor = Colors.DimGray;

            //body.GestureRecognizers

            body.GestureRecognizers.Add(tapHandler);
            tapHandler.NumberOfTapsRequired = 2;
            tapHandler.Tapped += this.TapEventStyle;





            this.swipeLeftItem.Text = "Edit type";
            this.swipeLeftItem.BackgroundColor = Colors.Sienna;
            

            this.swipeRightItem.Text = "Run Task";
            this.swipeRightItem.BackgroundColor = Colors.LightSeaGreen;

            this.swiper.LeftItems.Add(swipeLeftItem);
            swiperBody.Children.Add(gridRow);
            this.swiper.Content = swiperBody;
            this.swiper.RightItems.Add(swipeRightItem);
            swipeLeftItem.Clicked += SwipeLeftItem_OpenTypeEditor_Clicked;
            swipeRightItem.Clicked += SwipeRightItem_RunTask_Clicked;
            
         
            body.Children.Add(swiper);
            this.SetTriggerState();
        }

        private async void SwipeRightItem_RunTask_Clicked(object? sender, EventArgs e)
        {
            if (this.IsActive == true)
            {
                this.IsActive = false;
                LocalStorage.ActiveType = null;

            } else
            {
                LocalStorage.ActiveType = this.sourceObject;
                this.IsActive = true;
            }
            this.SetTriggerState();
        }

        private void SwipeLeftItem_OpenTypeEditor_Clicked(object? sender, EventArgs e)
        {
            LocalStorage.EditedType = this.sourceObject;
        }

        public HorizontalStackLayout Get()
        {
            return this.body;
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


        private async void SetTriggerState()
        {
            try
            {
                await Task.Delay(200);
                if (this.IsActive == true)
                {

                    this.swipeRightItem.BackgroundColor = Colors.Crimson;
                    this.swipeRightItem.Text = "Stop";
                    this.swiperBody.BackgroundColor = Microsoft.Maui.Graphics.Color.FromHex(LocalStorage.OpenedCategory.BgColor);
                    this.gridRow.BackgroundColor = Microsoft.Maui.Graphics.Color.FromArgb("#AA555555");

                }
                else
                {
                    this.gridRow.BackgroundColor = Colors.DimGray;
                    this.swipeRightItem.BackgroundColor = Colors.LightSeaGreen;
                    this.swipeRightItem.Text = "Run";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }
           
        }
    }
}
