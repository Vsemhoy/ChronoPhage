using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoPhage.Pages.Com.MainPage.Stacks
{
    internal class CategoryStack : VerticalStackLayout
    {
        private Grid baseGrid = new Grid();

        private VerticalStackLayout titleStack = new VerticalStackLayout();
        private VerticalStackLayout contentStack = new VerticalStackLayout();

        public Label title = new Label();

        public CategoryStack()
        {
            this.baseGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            this.baseGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            this.title.Text = "Categories:";
            this.titleStack.Children.Add(this.title);

            this.baseGrid.Children.Add(this.titleStack);
            Grid.SetRow(this.titleStack, 0);
            this.baseGrid.Children.Add(this.contentStack);
            Grid.SetRow(this.contentStack, 1);

            this.Children.Add(this.baseGrid);
        }
    }
}
