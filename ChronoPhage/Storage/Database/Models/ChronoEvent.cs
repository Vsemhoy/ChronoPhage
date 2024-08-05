using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoPhage.Storage.Database.Models
{
    class ChronoEvent
    {
        [PrimaryKey, Unique, NotNull]
        public string Id { get; set; }
        [MaxLength(25), NotNull]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        [MaxLength(64)]
        public string TypeId { get; set; }
        [MaxLength(64)]
        public string CategoryId { get; set; }
        [MaxLength(64)]
        public string GroupId { get; set; }

        public DateTime StartAt { get; set; } = DateTime.UtcNow;
        public DateTime EndAt { get; set; }
        public int Duration { get; set; } = 0;

        public bool IsRunning { get; set; }
        public bool IsStarred { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(64)]
        public string GeoPosition { get; set; } = null;




        /// <summary>
        /// Insert an element and return object if Success
        /// OR return object with Empty ID
        /// </summary>
        /// <returns>ChronoEvent</returns>
        public static async Task<ChronoEvent> InsertItemAsync(ChronoEvent item)
        {
            item.Id = await MakeId();
            int result = await DatabaseService.DB.InsertAsync(item);
            if (result > 0)
            {
                return await GetLastinserted();
            }
            item.Id = "";
            return item;
        }

        /// <summary>
        /// Returns last inserted item depends on CreatedAt field
        /// </summary>
        /// <returns>ChronoEvent</returns>
        public static async Task<ChronoEvent> GetLastinserted()
        {
            return await DatabaseService.DB.Table<ChronoEvent>()
                              .OrderByDescending(item => item.CreatedAt)
                              .FirstOrDefaultAsync();
        }



        public static async Task<ChronoEvent> GetActiveItem()
        {
            return await DatabaseService.DB.Table<ChronoEvent>()
                              .Where(i => i.IsRunning == true)
                              .FirstOrDefaultAsync();
        }


        /// <summary>
        /// Generate unique ID for new row
        /// </summary>
        /// <returns></returns>
        public static async Task<string> MakeId()
        {
            string _id = DatabaseService.GenerateHashId(DatabaseService.RandomString(25));
            ChronoEvent evt = await DatabaseService.DB.Table<ChronoEvent>().Where(i => i.Id == _id).FirstOrDefaultAsync();
            while (evt != null)
            {
                _id = DatabaseService.GenerateHashId(DatabaseService.RandomString(25));
                evt = await DatabaseService.DB.Table<ChronoEvent>().Where(i => i.Id == _id).FirstOrDefaultAsync();
            }
            return _id;
        }

        /// <summary>
        /// Returns only one Item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<ChronoEvent> GetItemByIdAsync(string id)
        {
            return await DatabaseService.DB.Table<ChronoEvent>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Get collection of active items
        /// </summary>
        /// <returns></returns>
        public static async Task<List<ChronoEvent>> GetAllActiveItemsAsync()
        {
            return await DatabaseService.DB.Table<ChronoEvent>().OrderByDescending(x => x.StartAt).ToListAsync();
        }

        /// <summary>
        /// GEt collection of items filtered by category ID
        /// </summary>
        /// <param name="category_id"></param>
        /// <returns></returns>
        public static async Task<List<ChronoEvent>> GetAllItemsFromCategoryAsync(string category_id)
        {
            return await DatabaseService.DB.Table<ChronoEvent>()
                .Where(y => y.CategoryId == category_id)
                .OrderBy(x => x.StartAt).ToListAsync();
        }


        /// <summary>
        /// Update fields inside one item by it's ID (should be defined within object)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static async Task<int> UpdateItemAsync(ChronoEvent item)
        {
            return await DatabaseService.DB.UpdateAsync(item);
        }

        /// <summary>
        /// Remove item by it's ID (should be defined within object)
        /// </summary>
        /// <typeparam name="ChronoEvent"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Task<int> DeleteItemAsync<ChronoEvent>(ChronoEvent item)
        {
            return DatabaseService.DB.DeleteAsync(item);
        }


        public static async Task<int> StopAllTasks()
        {
            var time = DateTime.UtcNow;
            var items = await DatabaseService.DB.Table<ChronoEvent>().Where(i => i.IsRunning == true).ToListAsync();
            int result = -1;
            for (int i = 0; i < items.Count; i++)
            {
                var itoupdate = items[i];
                itoupdate.IsRunning = false;
                itoupdate.EndAt = time;
                var sta = itoupdate.StartAt;
                TimeSpan difference = time - itoupdate.StartAt;
                double totalMinutes = difference.TotalMinutes;
                if (totalMinutes < int.MinValue || totalMinutes > int.MaxValue)
                {
                    itoupdate.Duration = int.MaxValue;
                }
                else
                {
                    itoupdate.Duration = (int)totalMinutes;
                }
                result = await ChronoEvent.UpdateItemAsync(itoupdate);
            }
            return result;
        }
    }
}
