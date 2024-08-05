using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoPhage.Pages.Elements.Modals
{
    internal class CategoryEditorModal : ContentPage
    {
        VerticalStackLayout baseStack = new VerticalStackLayout();
        public CategoryEditorModal()
        {
            this.baseStack.BackgroundColor = Colors.BlanchedAlmond;

            this.Content = this.baseStack;
        }
    }
}
