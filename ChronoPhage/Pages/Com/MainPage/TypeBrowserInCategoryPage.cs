using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoPhage.Pages.Com.MainPage
{
    internal class TypeBrowserInCategoryPage : ContentPage
    {
        public StackLayout baseStack = new StackLayout();

        public Button MainButton = new Button();

        public TypeBrowserInCategoryPage() {
            this.Title = "Type bro";

            ToolbarItem tbItem1 = new ToolbarItem();
            tbItem1.IconImageSource = "dotnet_bot.png";
            tbItem1.Text = "Create Category";
            tbItem1.Order = ToolbarItemOrder.Secondary;
            this.ToolbarItems.Add(tbItem1);

            this.MainButton.Text = "Hello wolf!";

            this.baseStack.Children.Add(this.MainButton);

            this.Content = this.baseStack;
        }
    }
}
