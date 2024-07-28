using ChronoFuck.Database.Models;
using ChronoFuck.Pages.Browser.Com;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

using ChronoFuck.Utils;
using ChronoFuck.Database;

namespace ChronoFuck.Pages.Browser
{
    class CategoryTypeEditor : ContentPage
    {
        
        EventCategory eventCategory { get; set; }
        public string id { get; set; }
        public string type_id { get; set; }
        public string type_title { get; set; }


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

        private Button colorMark = new Button();

        public Button CloseButton = new Button();
        public Button SaveButton   = new Button();
        public Button CreateButton = new Button();
        public Button RemoveButton = new Button();

        private Entry  titleInput = new Entry();
        private Editor descriptionInput = new Editor();
        
        private Picker colorPicker = new Picker
        {
            Title = "Select background color",
            ItemsSource = StringUtils.Generate256HexColors()
        };
        private Picker colorPickerText = new Picker
        {
            Title = "Select text color",
            ItemsSource = StringUtils.Generate256HexColors()
        };


        public Picker positionPicker = new Picker
        {
            Title = "Position"
        };


        public CategoryTypeEditor(ref EventCategory evcat)
        {
            this.body.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) });
            this.body.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });
            this.body.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });

            this.eventCategory = evcat;
            

            DateTime now = DateTime.Now;

            this.type_title = "Category";

            this.id = evcat.Id;



            this.titleInput.ClearButtonVisibility = ClearButtonVisibility.WhileEditing;
            this.descriptionInput.HeightRequest = 200;
            this.descriptionInput.Text = evcat.Description;

            HorizontalStackLayout startstack = new HorizontalStackLayout();
            HorizontalStackLayout endstack = new HorizontalStackLayout();

            this.titleInput.Placeholder = "Category name";
            this.stack.Add(titleInput);
            this.titleInput.Text = evcat.Title;

            this.descriptionInput.Text = evcat.Description;

            this.stack.Add(descriptionInput);
            Label lbl1 = new Label { Text = "Backgound color", FontSize = 12};
            this.stack.Add(lbl1);

            colorPicker.SelectedIndexChanged += ColorPicker_SelectedIndexChanged;
            colorPickerText.SelectedIndexChanged += ColorPickerText_SelectedIndexChanged;

            colorPicker.SelectedItem = evcat.BgColor;
            colorPickerText.SelectedItem = evcat.TextColor;

            this.stack.Add(colorPicker);
            Label lbl2 = new Label { Text = "Foreground color", FontSize = 12 };
            this.stack.Add(lbl2);
            this.stack.Add(colorPickerText);



            colorMark.Text = "Color demo";
            colorMark.BackgroundColor = Colors.White;
            this.colorMark.BackgroundColor = Microsoft.Maui.Graphics.Color.FromHex(evcat.BgColor);
            this.colorMark.TextColor = Microsoft.Maui.Graphics.Color.FromHex(evcat.TextColor);
            this.stack.Add(colorMark);


            var selectedIndex = -1;
            for (int i = 0; i < LocalStorage.Categories.Count; i++)
            {
                this.positionPicker.Items.Add(i.ToString() + " - " + LocalStorage.Categories[i].Title);
                if (evcat.Title == LocalStorage.Categories[i].Title)
                {
                    selectedIndex = i;
                }
            }
            if (selectedIndex != -1)
            {
                this.positionPicker.SelectedIndex = selectedIndex;
            } else
            {
                this.positionPicker.Items.Add(LocalStorage.Categories.Count.ToString());
                this.positionPicker.SelectedIndex = evcat.CountItems;
            }
            this.stack.Add(this.positionPicker);



            this.SaveButton.Text = "Save category";
            this.SaveButton.Margin = new Thickness(12);
         

            this.CreateButton.Text = "Create category";
            this.CreateButton.Margin = new Thickness(12);
         

            this.CloseButton.Text = "Close";
            this.CloseButton.Margin = new Thickness(12);
            this.CloseButton.Clicked += CloseButton_Clicked;

            this.RemoveButton.Text = "Delete category";
            this.RemoveButton.BackgroundColor = Colors.Salmon;
            this.RemoveButton.Margin = new Thickness(12);

            buttonStack.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            buttonStack.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            buttonStack.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });


            //itemStack.BackgroundColor = Colors.Red;
            //itemStack.Padding = new Thickness(12);

            stack.Padding = new Thickness(12);

            body.Add(stack);
            Grid.SetRow(stack, 0);
            body.Add(removeStack);
            Grid.SetRow(removeStack, 1);
            body.Add(buttonStack);
            Grid.SetRow(buttonStack, 2);

            this.scrollView.Content = body;

            SaveButton.Clicked += SaveButton_Clicked;
            CreateButton.Clicked += CreateButton_Clicked;

            Content = this.scrollView;
            this.SetButtonBlock();

            if (evcat.CountItems == 0)
            {
                removeStack.Children.Add(RemoveButton);
            }
        }


        private void CloseButton_Clicked(object? sender, EventArgs e)
        {
            LocalStorage.EditedCategory = null;
            Navigation.PopAsync();
        }

        public void SetButtonBlock()
        {
            this.buttonStack.Children.Clear();
            if (this.id == "" || this.id == null)
            {
                buttonStack.Children.Add(this.CreateButton);
                Grid.SetColumn(this.CreateButton, 0);
                this.Title = "Create new category";
            }
            else
            {
                buttonStack.Children.Add(this.SaveButton);
                Grid.SetColumn(this.SaveButton, 0);
                this.Title = "Edit category";

            }
            buttonStack.Children.Add(this.CloseButton);
            Grid.SetColumn(this.CloseButton, 1);
        }



        private async void CreateButton_Clicked(object? sender, EventArgs e)
        {
            this.eventCategory.Title = this.titleInput.Text;
            this.eventCategory.Description = this.descriptionInput.Text;
            this.eventCategory.BgColor = colorPicker.SelectedItem as string;
            this.eventCategory.TextColor = colorPickerText.SelectedItem as string;
            this.eventCategory.CountItems = this.positionPicker.SelectedIndex;
            try
            {

                this.eventCategory = await EventCategory.InsertItemAsync(this.eventCategory);
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
            this.eventCategory.Title = this.titleInput.Text;
            this.eventCategory.Description = this.descriptionInput.Text;
            this.eventCategory.BgColor = colorPicker.SelectedItem as string;
            this.eventCategory.TextColor = colorPickerText.SelectedItem as string;
            this.eventCategory.CountItems = this.positionPicker.SelectedIndex;
            LocalStorage.EditedCategory = this.eventCategory;
            try
            {
                if (await EventCategory.UpdateItemAsync(this.eventCategory) == 1)
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




        private void ColorPickerText_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender is Picker colorPicker && colorPicker.SelectedItem is string selectedColor)
            {
                Console.WriteLine($"Selected color: {selectedColor}");

                // If you want to use the color in your app, you can convert it to a Color object
                if (Color.TryParse(selectedColor, out Color parsedColor))
                {
                    // Use the parsedColor
                    Console.WriteLine($"Parsed color: R={parsedColor.Red}, G={parsedColor.Green}, B={parsedColor.Blue}");
                    //this.colorPicker.BackgroundColor = Microsoft.Maui.Graphics.Color.FromHex(selectedColor);
                    this.colorMark.TextColor = Microsoft.Maui.Graphics.Color.FromHex(selectedColor);
                }
            }
        }

        private void ColorPicker_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender is Picker colorPicker && colorPicker.SelectedItem is string selectedColor)
            {
                Console.WriteLine($"Selected color: {selectedColor}");

                // If you want to use the color in your app, you can convert it to a Color object
                if (Color.TryParse(selectedColor, out Color parsedColor))
                {
                    // Use the parsedColor
                    Console.WriteLine($"Parsed color: R={parsedColor.Red}, G={parsedColor.Green}, B={parsedColor.Blue}");
                    //this.colorPicker.BackgroundColor = Microsoft.Maui.Graphics.Color.FromHex(selectedColor);
                    this.colorMark.BackgroundColor = Microsoft.Maui.Graphics.Color.FromHex(selectedColor);
                }
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
