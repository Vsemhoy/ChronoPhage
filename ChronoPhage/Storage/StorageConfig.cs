using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoFuck.Database
{
    public static class StorageConfig
    {
        public static string DatabaseFileName = "ChronoPhage.db3";
        //public static string AppDir = "ChronoFuck";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite // Open in RW mode
            | SQLite.SQLiteOpenFlags.Create  // create DB if !exists
            | SQLite.SQLiteOpenFlags.SharedCache;  // enable MultiThread access

        /**
         * A full database path
         */
        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);
    }
}
