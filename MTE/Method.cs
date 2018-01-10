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
    class Method
    {
        public static ProgressDialog progress;
        public static void ProgressShow(Context context, string str)
        {
            progress = new ProgressDialog(context);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage(str);
            progress.SetCancelable(false);
            progress.Show();
        }

        public static void ProgressHide(Context context)
        {
            if (progress != null)
            {
                progress.Hide();
            }
        }

        public static void Show(Context context, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Intent intent = new Intent();
                intent.SetAction(Intent.ActionView);
                intent.SetDataAndType(Android.Net.Uri.Parse(path), "image/*");
                context.StartActivity(intent);
            }
        }

        public static void Vibrate(Context context)
        {
            Vibrator vibrator = (Vibrator)context.GetSystemService(Context.VibratorService);
            if (vibrator.HasVibrator)
            {
                vibrator.Vibrate(100);
            }
        }
    }
}