using ChronoFuck.Pages.Browser.Com;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoFuck.Pages.Browser
{
    class EventEditorPage : ContentPage
    {

        public string id { get; }
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
        private HorizontalStackLayout buttonStack = new HorizontalStackLayout();

        private ScrollView scrollView = new ScrollView();

        private Button StartButton = new Button();
        private Button EndButton   = new Button();
        private Button SaveButton  = new Button();

        private Entry titleInput = new Entry();
        private Editor descriptionInput = new Editor();

        private DatePicker startDatePicker = new DatePicker();
        private TimePicker startTimePicker = new TimePicker();


        private DatePicker endDatePicker = new DatePicker();
        private TimePicker endTimePicker = new TimePicker();

        public EventEditorPage(string id, string title)
        {
            this.body.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) });
            this.body.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto) });

            DateTime now = DateTime.Now;

            this.type_title = "Type title";

            this.id = id;

            this.Title = this.type_title;

            this.titleInput.ClearButtonVisibility = ClearButtonVisibility.WhileEditing;
            this.descriptionInput.HeightRequest = 200;

            HorizontalStackLayout startstack = new HorizontalStackLayout();
            HorizontalStackLayout endstack = new HorizontalStackLayout();

            this.titleInput.Placeholder = "Event title";
            this.stack.Add(titleInput);
            
            this.stack.Add(descriptionInput);
            startstack.Add(startDatePicker);
            startstack.Add(startTimePicker);
            endstack.Add(endDatePicker);
            endstack.Add(endTimePicker);
            stack.Add(new Label
            {
                Text = "Start time",
                FontSize = 12
            });
            stack.Add(startstack);
            stack.Add(new Label
            {
                Text = "End time",
                FontSize = 12
            });
            stack.Add(endstack);


            this.StartButton.Text = "Start event";
            this.StartButton.Margin = new Thickness(12);
            this.StartButton.BackgroundColor = Colors.CadetBlue;

            this.EndButton.Text = "Stop event";
            this.EndButton.Margin = new Thickness(12);
            this.EndButton.BackgroundColor = Colors.OrangeRed;

            this.SaveButton.Text = "Save event";
            this.SaveButton.Margin = new Thickness(12);

            if (this.is_active)
            {
                buttonStack.Children.Add(this.EndButton);

            } else
            {
                buttonStack.Children.Add(this.StartButton);

            }
            buttonStack.Children.Add(this.SaveButton);

            //itemStack.BackgroundColor = Colors.Red;
            //itemStack.Padding = new Thickness(12);

            stack.Padding = new Thickness(12);

            body.Add(stack);
            Grid.SetRow(stack, 0);
            body.Add(buttonStack);
            Grid.SetRow(buttonStack, 1);

            this.scrollView.Content = body;




            Content = this.scrollView;
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
