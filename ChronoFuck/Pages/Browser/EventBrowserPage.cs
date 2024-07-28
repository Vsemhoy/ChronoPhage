

using ChronoFuck.Database;
using ChronoFuck.Database.Models;
using ChronoFuck.Pages.Browser.Com;
using Microsoft.Maui.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ChronoFuck.Pages.Browser
{
    class EventBrowserPage : ContentPage
    {
        private EventTypeEditor eventTypeEditor {  get; set; }
        public string id { get; }
        public string color { get; set; }

        public int   countOfItems { get; set; }
        public bool  countUpdated { get; set; }

        private StackLayout contentPage = new StackLayout();
        
        private StackLayout acitveStack = new StackLayout();
        private VerticalStackLayout itemStack = new VerticalStackLayout();
        
        private ScrollView scrollView = new ScrollView();
        public Button createItemButton = new Button();
        public Button editCategoryButton = new Button();

        private EventEditorPage currentEventEditor { get; set; }

        public EventBrowserPage(CategoryCardItem category)
        {
            this.id    = category.id;
            this.Title = category.title;
            this.countOfItems = category.counter;
            this.createItemButton.Text     = "Create type";
            this.createItemButton.Margin   = new Thickness(12);
            this.createItemButton.Clicked += this.Btn_CreateType_clicked;

            this.editCategoryButton.Text = "Edit category";
            this.editCategoryButton.Margin = new Thickness(12);

            

            Grid buttonStack = new Grid();
            buttonStack.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            buttonStack.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            buttonStack.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            VerticalStackLayout stck = new VerticalStackLayout();
            stck.Children.Add(this.acitveStack);
            stck.VerticalOptions = LayoutOptions.End;
            stck.BackgroundColor = Colors.Black;
            stck.Children.Add(this.itemStack);
            buttonStack.Children.Add(this.editCategoryButton);
            Grid.SetColumn(this.editCategoryButton, 0);
            buttonStack.Children.Add(this.createItemButton);
            Grid.SetColumn(this.createItemButton, 1);
            stck.Children.Add(buttonStack);
            this.itemStack.VerticalOptions = LayoutOptions.End;
            //itemStack.BackgroundColor = Colors.Red;
            //itemStack.Padding = new Thickness(12);
            this.scrollView.Content = stck;

            

            this.LoadAndSetItems(category.id);
            Content = this.scrollView;
        }


        private async Task LoadAndSetItems(string category_id)
        {
            this.itemStack.Clear();
            LocalStorage.Types = await EventType.GetAllActiveItemsByCategoryAsync(category_id);
            StackLayout fdivider = new StackLayout();
            fdivider.BackgroundColor = Color.FromArgb("#77777777");
            fdivider.HeightRequest = 1;
            this.itemStack.Add(fdivider);

            for (int i = 0; i < LocalStorage.Types.Count; i++)
            {
                var type = LocalStorage.Types[i];
                EventItemCard itemCard = new EventItemCard(type);
                itemCard.tapHandler.Tapped += this.DoubleTapEventHandler;
                itemCard.swipeLeftItem.Clicked += OpenTypeEditorWindow_Edit;
                itemCard.swipeRightItem.Clicked += RunEvent_Task;

                this.itemStack.Add(itemCard.Get());
                StackLayout divider = new StackLayout();
                divider.BackgroundColor = Color.FromArgb("#77777777");
                divider.HeightRequest = 1;
                this.itemStack.Add(divider);
            }
            await Task.Delay(100);
        }



        private async void RunEvent_Task(object? sender, EventArgs e)
        {
            try
            {
                int wasLoaded = await ChronoEvent.StopAllTasks();
                await Task.Delay(300);
                if (LocalStorage.ActiveType != null)
                {

                    this.acitveStack.Children.Clear();
                    LocalStorage.ActiveCategory = LocalStorage.OpenedCategory;
                    await this.WriteEventIntoDataBase_StartEvent();
                    ActiveEventCard activeCard = new ActiveEventCard(LocalStorage.ActiveChrono);
                    this.acitveStack.Children.Add(activeCard.Get());
                    LocalStorage.ActiveCategory = LocalStorage.OpenedCategory;
                    if (wasLoaded > 0)
                    {
                        await this.LoadAndSetItems(this.id);
                    }
                }
                else
                {
                    this.acitveStack.Children.Clear();
                    LocalStorage.ActiveCategory = null;
                    LocalStorage.ActiveChrono = null;
                    //await this.LoadAndSetItems(this.id);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private async Task WriteEventIntoDataBase_StartEvent()
        {
            ChronoEvent chronoEvent = new ChronoEvent();
            chronoEvent.CategoryId = LocalStorage.OpenedCategory.Id;
            chronoEvent.CreatedAt = DateTime.Now;
            chronoEvent.Duration = 0;
            chronoEvent.Description = string.Empty;
            chronoEvent.Title = string.Empty;
            chronoEvent.TypeId = LocalStorage.ActiveType.Id;
            chronoEvent.IsRunning = true;

            var chrono = await ChronoEvent.InsertItemAsync(chronoEvent);
            LocalStorage.ActiveChrono = chrono;
        }



        private async void OpenTypeEditorWindow_Edit(object? sender, EventArgs e)
        {
            await Task.Delay(100);
            this.eventTypeEditor = new EventTypeEditor(LocalStorage.EditedType);
            await Navigation.PushAsync(this.eventTypeEditor);

            
            this.eventTypeEditor.SaveButton.Clicked += TypeSaveButton_Clicked;
        }

        private async void Btn_CreateType_clicked(object? sender, EventArgs e)
        {
            EventType evt = new EventType();
            evt.CategoryId = LocalStorage.OpenedCategory.Id;
            this.eventTypeEditor = new EventTypeEditor(evt);
            await Navigation.PushAsync(this.eventTypeEditor);

            this.eventTypeEditor.CreateButton.Clicked += TypeCreateButton_Clicked;
            this.eventTypeEditor.SaveButton.Clicked += TypeSaveButton_Clicked;
        }



        private void TypeSaveButton_Clicked(object? sender, EventArgs e)
        {
            this.LoadAndSetItems(LocalStorage.OpenedCategory.Id);

        }



        private async void TypeCreateButton_Clicked(object? sender, EventArgs e)
        {
            await Task.Delay(300);
            await this.LoadAndSetItems(LocalStorage.OpenedCategory.Id);
            LocalStorage.OpenedCategory.CountItems = LocalStorage.Types.Count;
            await EventCategory.UpdateItemAsync(LocalStorage.OpenedCategory);
            this.countOfItems = LocalStorage.Types.Count;
            this.countUpdated = true;
        }



        private async void DoubleTapEventHandler(object? sender, EventArgs e)
        {
            this.currentEventEditor = new EventEditorPage("dkfas", "Title");
            await Navigation.PushAsync(this.currentEventEditor);
        }

        protected override void OnAppearing()
        {
  
            base.OnAppearing();
            // Handle any logic when the page appears
            Thread.Sleep(200);
            this.itemStack.MinimumHeightRequest = (DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density) - (DeviceDisplay.Current.MainDisplayInfo.Density * 90);

            if (LocalStorage.ActiveChrono != null)
            {
                if (LocalStorage.ActiveChrono.CategoryId == this.id)
                {
                    this.acitveStack.Children.Clear();
                    ActiveEventCard activeCard = new ActiveEventCard(LocalStorage.ActiveChrono);
                    this.acitveStack.Children.Add(activeCard.Get());
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Handle any cleanup when the page disappears
            //DisposeResources();
            //Navigation.PopAsync();
        }

        private void DisposeResources()
        {
            // Dispose or cleanup any resources

        }
    }
}
