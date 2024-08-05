using ChronoPhage.Core;
using ChronoPhage.Pages.Elements.Cards;
using ChronoPhage.Storage;
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

        public CategoryAddMiniCard addCard = new CategoryAddMiniCard();


        Grid categoryFlexContainer = new Grid
        {
            Padding = new Thickness(6),
            RowSpacing = 6,
            ColumnSpacing = 6
        };

        public CategoryStack()
        {
            this.baseGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            this.baseGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            this.title.Text = "Categories:";
            this.titleStack.Children.Add(this.title);



            

            



            this.contentStack.Children.Add(categoryFlexContainer);
            this.baseGrid.Children.Add(this.titleStack);
            Grid.SetRow(this.titleStack, 0);
            this.baseGrid.Children.Add(this.contentStack);
            Grid.SetRow(this.contentStack, 1);
            

            this.Children.Add(this.baseGrid);

            //this.SetCategoryCards();
            Task.Run(() => this.SetCategoryCards().GetAwaiter().GetResult());
        }




        public async Task SetCategoryCards()
        {
            int countOfItems = LocalStorage.Categories.Count;
            double width = categoryFlexContainer.Width;

            int numberOfColumns = 3;
            int numberOfRows = (int)Math.Ceiling((countOfItems + 1) / (double)numberOfColumns);

            this.categoryFlexContainer.RowDefinitions.Clear();
            this.categoryFlexContainer.ColumnDefinitions.Clear();

            for (int y = 0; y < numberOfColumns; y++)
            {
                this.categoryFlexContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            for (int y = 0; y < numberOfRows; y++)
            {
                this.categoryFlexContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            }

            //categoryFlexContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width / 3) });
            this.categoryFlexContainer.Children.Clear();
            int i = 0;
            for (i = 0; i < LocalStorage.Categories.Count; i++)
            {
                var category = LocalStorage.Categories[i];
                bool isActive = false;
                if (LocalStorage.ActiveCategory != null && LocalStorage.ActiveCategory.Id == category.Id)
                {
                    isActive = true;
                }
                int row = i / numberOfColumns;
                int column = i % numberOfColumns;
                var categoryCardItem = new CategoryMiniCard();
                //categoryCardItem.tapHandler.Tapped += (s, e) => { this.CategoryClickHandler(categoryCardItem); };
                this.categoryFlexContainer.Add(categoryCardItem, column, row);
            }
            int rowz = i / numberOfColumns;
            int columnz = i % numberOfColumns;
            this.categoryFlexContainer.Add(new CategoryAddMiniCard(), columnz, rowz);

        }
    }
}
