using ChronoFuck.Database;
using ChronoFuck.Database.Models;
using ChronoFuck.Pages.Browser.Com;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoFuck.Pages.Browser
{
    class EventTypeEditor : ContentPage
    {

        public string id { get; }

        public EventType targetObject { get; set; }

        public bool is_active { get; set; }

        public string title { get; set; }
        public string description { get; set; }

        public int duration { get; set; }

        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }

        private Grid body = new Grid();
        private VerticalStackLayout stack = new VerticalStackLayout();
        private Grid buttonStack = new Grid();
        private HorizontalStackLayout removeStack = new HorizontalStackLayout();

        private ScrollView scrollView = new ScrollView();

        public Button RemoveButton = new Button();
        public Button StartButton = new Button();
        public Button EndButton = new Button();
        public Button SaveButton = new Button();
        public Button CreateButton = new Button();
        public Button CloseButton = new Button();

        private Entry titleInput = new Entry();
        private Editor descriptionInput = new Editor();

        private Entry durationHours = new Entry
        {
            Keyboard = Keyboard.Numeric,
            Text = "0"
        };
        private Entry durationMinutes = new Entry
        {
            Keyboard = Keyboard.Numeric,
            Text = "0"
        };

        private Picker categoryChooser = new Picker();

        public EventTypeEditor(EventType eventtype)
        {
            this.targetObject = eventtype;
            this.body.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) });
            this.body.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
            this.body.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });

            DateTime now = DateTime.Now;


            this.id = eventtype.Id;


            this.titleInput.ClearButtonVisibility = ClearButtonVisibility.WhileEditing;
            this.descriptionInput.HeightRequest = 200;
            this.descriptionInput.Text = eventtype.Description;

            HorizontalStackLayout startstack = new HorizontalStackLayout();
            HorizontalStackLayout endstack = new HorizontalStackLayout();

            this.titleInput.Text = eventtype.Title;
            this.titleInput.Placeholder = "Event name";
            this.stack.Add(titleInput);

            this.stack.Add(descriptionInput);


            var validStyle = new Style(typeof(Entry));
            validStyle.Setters.Add(new Setter
            {
                Property = Entry.TextColorProperty,
                Value = Colors.Green
            });

            var invalidStyle = new Style(typeof(Entry));
            invalidStyle.Setters.Add(new Setter
            {
                Property = Entry.TextColorProperty,
                Value = Colors.Red
            });



            this.durationHours.TextChanged += DurationHours_TextChanged;
            this.durationMinutes.TextChanged += DurationMinutes_TextChanged;
            startstack.Add(durationHours);
            startstack.Add(durationMinutes);
            this.stack.Add(this.categoryChooser);
            //endstack.Add(endDatePicker);
            //endstack.Add(endTimePicker);
            stack.Add(new Label
            {
                Text = "Max event duration",
                FontSize = 12
            });
            stack.Add(startstack);
            //stack.Add(new Label
            //{
            //    Text = "End time",
            //    FontSize = 12
            //});
            //stack.Add(endstack);


            this.StartButton.Text = "Start event";
            this.StartButton.Margin = new Thickness(12);
            this.StartButton.BackgroundColor = Colors.CadetBlue;

            this.EndButton.Text = "Stop event";
            this.EndButton.Margin = new Thickness(12);
            this.EndButton.BackgroundColor = Colors.OrangeRed;

            this.SaveButton.Text = "Save Type";
            this.SaveButton.Margin = new Thickness(12);
            this.SaveButton.Clicked += SaveButton_Clicked;

            this.CreateButton.Text = "Create type";
            this.CreateButton.Margin = new Thickness(12);
            this.CreateButton.Clicked += CreateButton_Clicked;

            this.CloseButton.Text = "Close";
            this.CloseButton.Margin = new Thickness(12);
            this.CloseButton.Clicked += CloseButton_Clicked;

            this.RemoveButton.Margin = new Thickness(12);
            this.RemoveButton.BackgroundColor = Colors.Salmon;
            this.RemoveButton.Text = "Delete";
            this.RemoveButton.Margin = new Thickness(12);
            

            buttonStack.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            buttonStack.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            buttonStack.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            //itemStack.BackgroundColor = Colors.Red;
            //itemStack.Padding = new Thickness(12);


            this.LoadCategorySelect(eventtype);


            stack.Padding = new Thickness(12);

            body.Add(stack);
            Grid.SetRow(stack, 0);
            body.Add(removeStack);
            Grid.SetRow(removeStack, 1);
            body.Add(buttonStack);
            Grid.SetRow(buttonStack, 2);

            this.scrollView.Content = body;


            this.SetButtonBlock();

            Content = this.scrollView;
        }

        private void CloseButton_Clicked(object? sender, EventArgs e)
        {
            LocalStorage.EditedCategory = null;
            Navigation.PopAsync();
        }






        private async void CreateButton_Clicked(object? sender, EventArgs e)
        {
            this.targetObject.Title = this.titleInput.Text;
            this.targetObject.Description = this.descriptionInput.Text;
            string categoryId = "";
            for (int i = 0; i < LocalStorage.Categories.Count; i++)
            {
                if (LocalStorage.Categories[i].Title == this.categoryChooser.SelectedItem.ToString())
                {
                    categoryId = LocalStorage.Categories[i].Id;
                    break;
                }
            }
            this.targetObject.CategoryId = categoryId;
            try
            {

                this.targetObject = await EventType.InsertItemAsync(this.targetObject);
                await Task.Delay(300);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Navigation.PopAsync();
        }




        private async void SaveButton_Clicked(object? sender, EventArgs e)
        {
            this.targetObject.Title = this.titleInput.Text;
            this.targetObject.Description = this.descriptionInput.Text;
            this.targetObject.Title = this.titleInput.Text;
            this.targetObject.Description = this.descriptionInput.Text;
            string categoryId = "";
            for (int i = 0; i < LocalStorage.Categories.Count; i++)
            {
                if (LocalStorage.Categories[i].Title == this.categoryChooser.SelectedItem.ToString())
                {
                    categoryId = LocalStorage.Categories[i].Id;
                    break;
                }
            }
            this.targetObject.CategoryId = categoryId;
            try
            {
                if (await EventType.UpdateItemAsync(this.targetObject) == 1)
                {
                    await Task.Delay(300);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Navigation.PopAsync();
        }


        private void LoadCategorySelect(EventType eventtype) {
            this.categoryChooser.Items.Clear();
            int index = 0;
            for (int i = 0; i < LocalStorage.Categories.Count; i++)
            {
                this.categoryChooser.Items.Add(LocalStorage.Categories[i].Title);
                if (LocalStorage.Categories[i].Id == eventtype.CategoryId)
                {
                    index = i;
                }
            }
            this.categoryChooser.SelectedIndex = index;
        }



        public void SetButtonBlock()
        {
            this.buttonStack.Children.Clear();
            if (this.id == "" || this.id == null)
            {
                buttonStack.Children.Add(this.CreateButton);
                Grid.SetColumn(this.CreateButton, 0);
                this.Title = "Create event type";
            }
            else
            {
                buttonStack.Children.Add(this.SaveButton);
                Grid.SetColumn(this.SaveButton, 0);
                this.Title = "Edit event type";

                this.removeStack.Children.Add(RemoveButton);
            }
            buttonStack.Children.Add(this.CloseButton);
            Grid.SetColumn(this.CloseButton, 1);
        }


        private void DurationMinutes_TextChanged(object? sender, TextChangedEventArgs e)
        {
            Entry value = (Entry)sender;
            string input = value.Text.ToString();
            if (int.TryParse(input, out int num))
            {
                // Check the parsed number and update the durationHours label accordingly
                if (num < 0)
                {
                    this.durationMinutes.Text = "0"; // Set to 0 if negative
                }
                else
                {
                    int hours = num / 60;
                    int minutes = (num + 60) % 60;
                    this.durationHours.Text = hours.ToString(); // Cap at 24
                    this.durationMinutes.Text = minutes.ToString(); // Cap at 24
                }
            }
            else
            {
                // If parsing fails (e.g., input is not a number), you can handle it here
                // Optionally clear the label or set it to a default value
                this.durationMinutes.Text = "0"; // Default to 0 if invalid input
            }
        }



        private void DurationHours_TextChanged(object? sender, TextChangedEventArgs e)
        {
            Entry value = (Entry)sender;
            string input = value.Text.ToString();
            if (int.TryParse(input, out int num))
            {
                // Check the parsed number and update the durationHours label accordingly
                if (num < 0)
                {
                    this.durationHours.Text = "0"; // Set to 0 if negative
                }
                else
                {
                    this.durationHours.Text = num > 24 ? "24" : num.ToString(); // Cap at 24
                }
            }
            else
            {
                // If parsing fails (e.g., input is not a number), you can handle it here
                // Optionally clear the label or set it to a default value
                this.durationHours.Text = "0"; // Default to 0 if invalid input
            }
        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
            // Handle any logic when the page appears
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Handle any cleanup when the page disappears
            //DisposeResources();
            Navigation.PopAsync();
        }

        private void DisposeResources()
        {
            // Dispose or cleanup any resources

        }
    }
}
