using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using ChronoFuck.Pages.Browser.Com;
using ChronoFuck.Database.Models;
using ChronoFuck.Database;


namespace ChronoFuck.Pages.Browser
{
    public partial class BrowserRootPage : ContentPage
    {
        CategoryTypeEditor categoryTypeEditor { get; set; }
        EventBrowserPage eventBrowser { get; set; }

        Grid mainGrid = new Grid();
        StackLayout activeItemsContainer = new StackLayout();
        ScrollView scrollView = new ScrollView();
        StackLayout categoryStackContainer = new StackLayout();
        Grid categoryFlexContainer = new Grid
        {
            Padding = new Thickness(6),
            RowSpacing = 6,
            ColumnSpacing = 6
        };

        Button addCategoryButton = new Button();

        public BrowserRootPage()
        {
            //InitializeComponent();
            // Load all categories from list
            //this.LoadCategories(true);


            // Define the layout of the main grid
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            // Set up the activeItemsContainer
            activeItemsContainer.BackgroundColor = Colors.DarkCyan;
            mainGrid.Children.Add(activeItemsContainer);
            Grid.SetRow(activeItemsContainer, 0);

            // Set up the scrollView to contain categoryStackContainer


            //categoryFlexContainer.Padding = new Thickness(6);
            categoryStackContainer.Children.Add(categoryFlexContainer);
            scrollView.Content = categoryStackContainer;
            categoryStackContainer.MinimumHeightRequest = 600;
            mainGrid.Children.Add(scrollView);
            Grid.SetRow(scrollView, 1);

            // Set the mainGrid as the Content of the BrowserRootPage
            addCategoryButton.Background = Colors.DarkBlue;
            addCategoryButton.TextColor = Colors.White;
            addCategoryButton.Text = "Create category";
            addCategoryButton.Margin = new Thickness(6);
            addCategoryButton.Clicked += this.Btn_CreateCategory_clicked;

            categoryStackContainer.Children.Add(addCategoryButton);

            Content = mainGrid;

            // Set the active item header and categories
            SetActiveItem("Active item header", Colors.White, Colors.DarkGrey);
            Task.Run(() => this.LoadCategories(true).GetAwaiter().GetResult());

        }


        public void SetActiveItem(string nameOfLabel, Color bgColor, Color textColor)
        {
            activeItemsContainer.Children.Clear();
            this.activeItemsContainer.BackgroundColor = bgColor;
            activeItemsContainer.Padding = new Thickness(0);
            if (nameOfLabel != "HOhoho")
            {

                activeItemsContainer.Padding = new Thickness(12);
                Label label = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    Text = nameOfLabel
                };
                activeItemsContainer.Children.Add(label);
                label.TextColor = textColor;
            }
        }


        public async Task LoadCategories(bool thenSet = false)
        {
            LocalStorage.Categories = await EventCategory.GetAllActiveItemsAsync();
            int count = LocalStorage.Categories.Count;
            int newCount = count;
            int attempts = 12;
            while (count != newCount || attempts == 0)
            {
                //Thread.Sleep(1000);
                Task.Delay(1000);
                LocalStorage.Categories = await EventCategory.GetAllActiveItemsAsync();
                newCount = LocalStorage.Categories.Count;
                attempts--;
            }
            if (thenSet)
            {
                await this.SetCategories();
            }
        }

        public async Task SetCategories()
        {
            int countOfItems = LocalStorage.Categories.Count;
            double width = categoryFlexContainer.Width;

            int numberOfColumns = 3;
            int numberOfRows = (int)Math.Ceiling(countOfItems / (double)numberOfColumns);

            categoryFlexContainer.RowDefinitions.Clear();
            categoryFlexContainer.ColumnDefinitions.Clear();

            for (int i = 0; i < numberOfColumns; i++)
            {
                categoryFlexContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            for (int i = 0; i < numberOfRows; i++)
            {
                categoryFlexContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            }

            //categoryFlexContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width / 3) });
            categoryFlexContainer.Children.Clear();

            for (int i = 0; i < LocalStorage.Categories.Count; i++)
            {
                var category = LocalStorage.Categories[i];

                int row = i / numberOfColumns;
                int column = i % numberOfColumns;
                var categoryCardItem = new CategoryCardItem(ref category);
                categoryCardItem.tapHandler.Tapped += (s, e) => { this.CategoryClickHandler(categoryCardItem); };
                categoryFlexContainer.Add(categoryCardItem.Get(), column, row);
            }
        }


        /// <summary>
        /// Click on category Square 
        /// </summary>
        /// <param name="card"></param>
        private async void CategoryClickHandler(CategoryCardItem card)
        {
            //await DisplayAlert("Alarm", card.id.ToString(), "Ok");
            this.SetActiveItem(card.title, card.bgColor, card.txColor);
            this.eventBrowser = new EventBrowserPage(card);

            this.eventBrowser.editCategoryButton.Clicked += EditCategoryButton_Clicked;

            for (int i = 0; i < LocalStorage.Categories.Count; i++)
            {
                if (card.id == LocalStorage.Categories[i].Id)
                {
                    LocalStorage.OpenedCategory = LocalStorage.Categories[i];
                    break;
                }
            }
            await Navigation.PushAsync(eventBrowser);
            this.eventBrowser.Disappearing += EventBrowser_Disappearing;
            
        }

        private void EventBrowser_Disappearing(object? sender, EventArgs e)
        {
            if (this.eventBrowser.countUpdated)
            {
                this.SetCategories();
                this.eventBrowser = null;
            }

        }

        /// <summary>
        /// Call to edit category from Modal page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void EditCategoryButton_Clicked(object? sender, EventArgs e)
        {
            LocalStorage.EditedCategory = LocalStorage.OpenedCategory;
            this.categoryTypeEditor = new CategoryTypeEditor(ref LocalStorage.EditedCategory);
            await Navigation.PushAsync(this.categoryTypeEditor);
            this.categoryTypeEditor.SetButtonBlock();
            // Set modal buttons handlers
            this.categoryTypeEditor.SaveButton.Clicked += SaveCategoryButton_Clicked;
            this.categoryTypeEditor.CreateButton.Clicked += CreateCategoryButton_Clicked;
            this.categoryTypeEditor.RemoveButton.Clicked += RemoveCategoryButton_Clicked;
            //LocalStorage.OpenedCategory = null;
        }

        private async void RemoveCategoryButton_Clicked(object? sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Question?", "Would you like to remove category?", "Yes", "No");
            if (answer == true)
            {
                if (await EventCategory.DeleteItemAsync(LocalStorage.EditedCategory) == 1)
                {
                    LocalStorage.EditedCategory = null;
                    LocalStorage.OpenedCategory = null;
                    await this.LoadCategories(true);
                    await Navigation.PopAsync();
                    await Navigation.PopAsync();
                };
            }
        }

        private async void Btn_CreateCategory_clicked(object? sender, EventArgs e)
        {
            LocalStorage.EditedCategory = new EventCategory();
            LocalStorage.EditedCategory.CountItems = await EventCategory.CountAll();
            this.categoryTypeEditor = new CategoryTypeEditor(ref LocalStorage.EditedCategory);
            await Navigation.PushAsync(this.categoryTypeEditor);
            this.categoryTypeEditor.SetButtonBlock();
            // Set modal buttons handlers
            this.categoryTypeEditor.SaveButton.Clicked += SaveCategoryButton_Clicked;
            this.categoryTypeEditor.CreateButton.Clicked += CreateCategoryButton_Clicked;
        }


        /// CATEGORY EDITOR HANDLER - Create new
        private async void CreateCategoryButton_Clicked(object? sender, EventArgs e)
        {
            if (LocalStorage.EditedCategory.Id != "" || LocalStorage.EditedCategory.Id != null)
            {
                await Task.Delay(1000);
                await  this.LoadCategories();
                await  this.SetCategories();
            }
            LocalStorage.EditedCategory = null;
            
        }

        /// CATEGORY EDITOR HANDLER - Save existed
        private async void SaveCategoryButton_Clicked(object? sender, EventArgs e)
        {
            await Task.Delay(1000);
            await this.LoadCategories();
            await this.SetCategories();
            LocalStorage.OpenedCategory = LocalStorage.EditedCategory;
            if (LocalStorage.ActiveCategory != null)
            {
                if (LocalStorage.ActiveCategory.Id == LocalStorage.EditedCategory.Id)
                {
                    LocalStorage.ActiveCategory = LocalStorage.EditedCategory;
                }
            }
            LocalStorage.EditedCategory = null;
            if (this.eventBrowser != null)
            {
                this.eventBrowser.Title = LocalStorage.OpenedCategory.Title;
            }
        }
    }

   
 
}

//var flexLayout = new FlexLayout
//{
//    Direction = FlexDirection.Row,
//    Wrap = FlexWrap.Wrap,
//    JustifyContent = FlexJustify.SpaceEvenly,
//    AlignItems = FlexAlignItems.Center,
//    Margin = new Thickness(10)
//};

//// Loop to create and add CategoryCard items to the FlexLayout
//for (int i = 1; i <= 10; i++)
//{
//    var icon = "test_icon.png"; // Change to your icon path
//    var category = $"Category {i}";
//    var itemsCount = $"{i * 2} Items";
//    var tapCommand = new Command(() => OnCardTapped(category));

//    var categoryCard = new CategoryCard(icon, category, itemsCount, tapCommand);
//    flexLayout.Children.Add(categoryCard);
//}

//// Add the FlexLayout to the Content of the Page
//Content = new ScrollView { Content = flexLayout };