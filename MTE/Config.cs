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

namespace MTE
{
    public static class Config
    {
        public static string apikey { get; set; } = ""; // LAST.FM apikey
        public static string apisecret { get; set; } = ""; // LAST.FM apisecret
        public static string Folder { get; private set; } = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        // DB                                                                                   
        public const string DB_History = "History.db";
        public const string TBL_History = "TBL_History";
    }
}