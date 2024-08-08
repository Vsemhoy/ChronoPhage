using ChronoPhage.Pages.Elements.Modals;
using ChronoPhage.Storage;
using ChronoPhage.Storage.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoPhage.Core
{

    internal class ModalManager
    {
        public static CategoryEditorModal categoryEditorModal = new CategoryEditorModal();

        public static async void CategoryEditorModal_ActionCreate(object? sender, EventArgs e)
        {
            // Check for Duplicates
            var duplex = await EventCategory.GetItemByTitleAsync(categoryEditorModal.titleText);
            if (duplex != null)
            {
                categoryEditorModal.Alert("Error", "Category with name '" + categoryEditorModal.titleText + "' already exists...");
                return;
            }
            var newItem = new EventCategory();
            newItem.Title = categoryEditorModal.titleText;
            newItem.Description = categoryEditorModal.descriptionText;
            newItem.BgColor = categoryEditorModal.bgColor.ToHex();
            newItem.TextColor = categoryEditorModal.txColor.ToHex();
            newItem.Position = LocalStorage.Categories.Count;

            var result = await EventCategory.InsertItemAsync(newItem);

            if (result.Id != string.Empty)
            {
                // success
                LocalStorage.Categories.Add(result);
                categoryEditorModal.Hide();
                return;
            } else
            {
                categoryEditorModal.Alert("Error", "Can't save, something wrong :(");
                return;
            }
        }

        public static async void CategoryEditorModal_ActionSave(object? sender, EventArgs e)
        {

        }
    }
}
