using ChronoPhage.Core;
using ChronoPhage.Pages.Elements.Cards;
using ChronoPhage.Storage;
using ChronoPhage.Storage.Database.Models;
using ChronoPhage.Style;
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
            RowSpacing = 9,
            ColumnSpacing = 9
        };

        public CategoryStack()
        {
            HorizontalStackLayout topMarger = new HorizontalStackLayout();
            topMarger.Padding = BaseTheme.THEME.modalLegendTopMargin / 2;
            this.baseGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            this.baseGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            this.title.Text = "Categories:";
            this.title.Padding = BaseTheme.THEME.modalLegendTopMargin / 2;
            this.titleStack.Children.Add(topMarger);
            this.titleStack.Children.Add(this.title);


            this.addCard.onClick.Tapped += OnClick_AddCard_EditorOpen;



            this.contentStack.Children.Add(categoryFlexContainer);
            this.baseGrid.Children.Add(this.titleStack);
            Grid.SetRow(this.titleStack, 0);
            this.baseGrid.Children.Add(this.contentStack);
            Grid.SetRow(this.contentStack, 1);
            

            this.Children.Add(this.baseGrid);

            //this.SetCategoryCards();
            //Task.Run(() => this.SetCategoryCards());
            this.SetCategoryCards();
            //Task.Run(() => this.SetCategoryCards().GetAwaiter().GetResult());

            ModalManager.categoryEditorModal.button_create.Clicked += Editor_Button_create_Clicked;
            ModalManager.categoryEditorModal.button_save.Clicked += Editor_Button_save_Clicked;
        }

        private async void Editor_Button_save_Clicked(object? sender, EventArgs e)
        {
            if (ModalManager.categoryEditorModal.CheckValid())
            {
                ModalManager.CategoryEditorModal_ActionSave(sender, e);
            }
        }

        private async void Editor_Button_create_Clicked(object? sender, EventArgs e)
        {
            if (ModalManager.categoryEditorModal.CheckValid())
            {
                ModalManager.CategoryEditorModal_ActionCreate(sender, e);
            }
        }

        private async void OnClick_AddCard_EditorOpen(object? sender, TappedEventArgs e)
        {
            await Task.Delay(100);
            await Navigation.PushAsync(ModalManager.categoryEditorModal);
        }

        public async void SetCategoryCards()
        {
            await Task.Delay(1000);
            int countOfItems = LocalStorage.Categories.Count;

            if (countOfItems == 0)
            {
                LocalStorage.Categories = await EventCategory.GetAllActiveItemsAsync() ;
                countOfItems = LocalStorage.Categories.Count;
            }

            double width = categoryFlexContainer.Width;

            int numberOfColumns = 3;
            int numberOfRows = 1;
          
                if ((countOfItems + 1) < numberOfColumns)
                {
                    numberOfRows = 1;
                } else
                {
                    numberOfRows = (int)Math.Ceiling((countOfItems + 1) / (double)numberOfColumns);

                }

            try
            {
                this.categoryFlexContainer.Children.Clear();
                this.categoryFlexContainer.RowDefinitions.Clear();
                this.categoryFlexContainer.ColumnDefinitions.Clear();

            } catch (Exception ex)
            {
                var m = ex.Message;
            }

            for (int y = 0; y < numberOfColumns; y++)
            {
                this.categoryFlexContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            for (int y = 0; y < numberOfRows; y++)
            {
                this.categoryFlexContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            }

            //categoryFlexContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width / 3) });
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
                var categoryCardItem = new CategoryMiniCard(category);
                //categoryCardItem.tapHandler.Tapped += (s, e) => { this.CategoryClickHandler(categoryCardItem); };
                this.categoryFlexContainer.Add(categoryCardItem, column, row);
            }
            int rowz = i / numberOfColumns;
            int columnz = i % numberOfColumns;
            this.categoryFlexContainer.Add(this.addCard, columnz, rowz);

        }
    }
}
