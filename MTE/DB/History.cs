using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace MTE.DB
{
    [Table(Config.TBL_History)]
    public class History
    {
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }

        [Indexed(Name = "Query", Unique = true)]
        public string Query { get; set; }
        public SearchType Type { get; set; }
    }

    public enum SearchType
    {
        Artist,
        Album,
        Track
    }
}