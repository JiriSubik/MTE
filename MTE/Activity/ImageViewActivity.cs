using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Content;
using System;
using Android.Graphics;
using System.Net;
using Android.Content.PM;

namespace MTE
{
    [Activity(Label = "App1", MainLauncher = false, Theme = "@style/MyTheme", NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class ImageViewActivity : AppCompatActivity
    {
        #region Controls
        ImageView ImageView_image;
        #endregion

        #region SetControls/SetHandlers/RemoveHandlers
        private void SetControls()
        {
            ImageView_image = FindViewById<ImageView>(Resource.Id.image);
        }

        private void SetHandlers()
        {
        }

        private void RemoveHandlers()
        {
        }
        #endregion

        #region EventHandlers

        #endregion

        #region LifeCycleMethods        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set view layout
            SetContentView(Resource.Layout.ImageView);

            SetControls();
            SetHandlers();

            string image = Intent.GetStringExtra("image");
            ShowImage(image);
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void OnRestart()
        {
            base.OnRestart();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }
        #endregion

        #region InstanceStateMethods
        protected override void OnSaveInstanceState(Bundle outState)
        {
            // outState.PutInt ("key", value);
            base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            if (savedInstanceState != null)
            {
                //myValue = savedInstanceState.getInt("key", value;
            }
        }
        #endregion

        private void ShowImage(string image)
        {
            Method.ProgressShow(this, GetString(Resource.String.Progress_message));
            var imageBitmap = GetImageBitmapFromUrl(image);
            ImageView_image.SetImageBitmap(imageBitmap);
            Method.ProgressHide(this);
        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}