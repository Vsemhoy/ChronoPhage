using ChronoPhage.Core;
using ChronoPhage.Storage.Database.Models;
using ChronoPhage.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoPhage.Pages.Elements.Cards
{
    internal class CategoryMiniCard : Frame
    {
        public Frame inFrame = new Frame();


        public VerticalStackLayout body = new VerticalStackLayout();

        public Image image = new Image();

        public Label textLabel = new Label();

        public TapGestureRecognizer onClick = new TapGestureRecognizer();

        public CategoryMiniCard(EventCategory itemObject)
        {
            this.MinimumHeightRequest = BaseTheme.THEME.categoryItemMiniCardMinHeight;
            this.Padding = new Thickness(0);
            this.CornerRadius = BaseTheme.THEME.CardCorner;
            this.BackgroundColor = Color.FromHex(itemObject.TextColor);
            this.BorderColor = Color.FromArgb("EEAAAAAA");
            this.inFrame.Padding = new Thickness(1);
            this.inFrame.CornerRadius = BaseTheme.THEME.CardCorner - 1;
            this.inFrame.BackgroundColor = Color.FromHex(itemObject.BgColor);
            this.inFrame.BorderColor = Color.FromHex(itemObject.BgColor);
            this.inFrame.Padding = 2;



            this.image.Source = "emoji_smile.svg";
            this.image.HeightRequest = BaseTheme.THEME.MiniCardHeightMinHeight;
            this.image.Opacity = 1;
            this.image.VerticalOptions = LayoutOptions.Center;
            CommunityToolkit.Maui.Behaviors.IconTintColorBehavior tintColor = new CommunityToolkit.Maui.Behaviors.IconTintColorBehavior();
            tintColor.TintColor = Color.FromHex(itemObject.TextColor);
            this.image.Behaviors.Add(tintColor);

            

            this.body.VerticalOptions = LayoutOptions.Center;


            this.textLabel.Text = itemObject.Title;
            this.textLabel.Padding = 2;
            this.textLabel.TextColor = Color.FromHex(itemObject.TextColor);
            this.textLabel.HorizontalTextAlignment = TextAlignment.Center;

            this.body.Children.Add(this.image);
            this.body.Children.Add(this.textLabel);
            this.inFrame.Content = this.body;
            this.Content = this.inFrame;

            this.inFrame.GestureRecognizers.Add(this.onClick);

            this.onClick.Tapped += OnClick_Tapped;
        }

        /// <summary>
        /// Open Category Editor as Create new Item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClick_Tapped(object? sender, TappedEventArgs e)
        {
            await Task.Delay(40);
            this.image.Opacity = 0.95;
            this.inFrame.Opacity = 0.85;
            this.Padding = 3;
            this.inFrame.Padding = 3;
            await Task.Delay(40);
            this.image.Opacity = 0.9;
            this.inFrame.Opacity = 0.9;
            await Task.Delay(40);
            this.image.Opacity = 0.8;
            this.inFrame.Opacity = 0.95;
            this.Padding = 2;
            this.inFrame.Padding = 4;
            await Task.Delay(40);
            this.image.Opacity = 1;
            this.inFrame.Opacity = 1;
            this.Padding = 0;
            this.inFrame.Padding = 5;

            //await Navigation.PushAsync(ModalManager.categoryEditorModal);
            //await Navigation.PushAsync(ModalManager.categoryEditorModal);
        }

    }
}