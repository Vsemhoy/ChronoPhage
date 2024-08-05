using ChronoPhage.Core;
using ChronoPhage.Pages.Com.MainPage.Stacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChronoPhage.Pages.Com.MainPage
{
    internal class MainPage : ContentPage
    {
        
        public StackLayout baseStack = new StackLayout();

        private Grid baseGrid = new Grid();

        public VerticalStackLayout ActiveStack = new VerticalStackLayout();

        public ScrollView ScrollStack = new ScrollView();

        public VerticalStackLayout ContentStack = new VerticalStackLayout();


        CategoryStack categoryStack { get; set; }
        RecentTypeStack recentTypeStack { get; set; }




        public Button MainButton = new Button();

        public MainPage() 
        {
            this.Title = "Super Main page";

            this.MainButton.Text = "Hello woof";

            ToolbarItem tbItem1 = new ToolbarItem();
            tbItem1.IconImageSource = "dotnet_bot.png";
            tbItem1.Text = "Create Category";
            tbItem1.Order = ToolbarItemOrder.Secondary;
            tbItem1.Clicked += TbItem1_Clicked;
            this.ToolbarItems.Add(tbItem1);


            this.categoryStack = new CategoryStack();
            this.recentTypeStack = new RecentTypeStack();


            Label label = new Label();
            label.Text = "OOOHO";

            Label label2 = new Label();
            label2.Text = "OOOHdf fsdaf asdfasO";

            this.baseGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            this.baseGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            this.ScrollStack.BackgroundColor = Colors.Blue;


            this.ActiveStack.Children.Add(label2);
            this.ActiveStack.BackgroundColor = Colors.Red;
            this.ActiveStack.Padding = 12;


            this.ScrollStack.Content = this.ContentStack;


            this.ContentStack.Children.Add(this.categoryStack);
            this.ContentStack.Children.Add(this.recentTypeStack);
            this.ContentStack.Children.Add(label);


            this.baseGrid.Children.Add(this.ActiveStack);
            Grid.SetRow(this.ActiveStack, 0);
            this.baseGrid.Children.Add(this.ScrollStack);
            Grid.SetRow(this.ScrollStack, 1);






            this.baseStack.Children.Add(this.baseGrid);
            this.Content = this.baseStack;
        }

        private async void TbItem1_Clicked(object? sender, EventArgs e)
        {
            Navigation.PushAsync(PageManager.TypeBrowserInCategoryPage);
        }
    }
}
