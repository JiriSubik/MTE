using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Content;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;
using System.Collections.Generic;
using Android.Graphics;
using System.Net;
using MTE.Adapter;
using System.Linq;
using Android.Content.PM;

namespace MTE
{
    [Activity(Label = "App1", MainLauncher = false, Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AlbumActivity : AppCompatActivity
    {
        #region Controls
        Button Album_Artist;
        ImageButton Album_Image;
        ListView Album_lvTracks;
        TextView Album_Name;
        TextView Album_TrackName;
        TextView Album_Duration;
        #endregion

        #region SetControls/SetHandlers/RemoveHandlers
        private void SetControls()
        {
            Album_Artist = FindViewById<Button>(Resource.Id.Artist);
            Album_Image = FindViewById<ImageButton>(Resource.Id.Image);
            Album_lvTracks = FindViewById<ListView>(Resource.Id.lvTracks);
            Album_Name = FindViewById<TextView>(Resource.Id.Name);
            Album_TrackName = FindViewById<TextView>(Resource.Id.TrackName);
            Album_Duration = FindViewById<TextView>(Resource.Id.Duration);
        }

        private void SetHandlers()
        {
            Album_Artist.Click += Album_Artist_Click;
            Album_lvTracks.ItemClick += Album_lvTracks_ItemClick;
        }

        private void RemoveHandlers()
        {
            Album_Artist.Click -= Album_Artist_Click;
            Album_lvTracks.ItemClick -= Album_lvTracks_ItemClick;
        }
        #endregion

        #region EventHandlers
        private void Album_Artist_Click(object sender, System.EventArgs e)
        {
            Method.Vibrate(this);
            if (!string.IsNullOrEmpty(Album_Artist.Text))
            {
                var myIntent = new Intent(this, typeof(ResultActivity));
                myIntent.PutExtra("artist", Album_Artist.Text);
                StartActivity(myIntent); 
            }
        }
        private void Album_lvTracks_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

        }
        #endregion

        #region LifeCycleMethods        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            // Set view layout
            SetContentView(Resource.Layout.Album);

            SetControls();
            SetHandlers();

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var title = FindViewById<TextView>(Resource.Id.toolbar_title);
            title.Text = GetString(Resource.String.Album_title);

            string albumID = Intent.GetStringExtra("AlbumID");
            string artist = Intent.GetStringExtra("ArtistName");
            string album = Intent.GetStringExtra("AlbumName");
            string ID = Intent.GetStringExtra("ID");

            if (!string.IsNullOrEmpty(albumID))
            {
                Request(albumID);
            }
            else
            {
                Request(artist, album, ID);
            }
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

        private async void Request(string albumID)
        {
            if (!string.IsNullOrEmpty(albumID))
            {
                var client = new LastfmClient(Config.apikey, Config.apisecret);

                var responseAlbums = await client.Album.GetInfoByMbidAsync(albumID);

                LastAlbum album = responseAlbums.Content;

                Album_Name.Text = album.Name;
                Album_Artist.Text = album.ArtistName;

                if (!string.IsNullOrEmpty(album.Images.Large.ToString()))
                {
                    var imageBitmap = GetImageBitmapFromUrl(album.Images.Large.ToString());
                    Album_Image.SetImageBitmap(imageBitmap);
                    Album_Image.Click += (sender, ea) =>
                    {
                        Method.Vibrate(this);
                        //Method.Show(this, album.Images.Large.ToString());
                        if (!string.IsNullOrEmpty(album.Images.ExtraLarge.ToString()))
                        {
                            var myIntent = new Intent(this, typeof(ImageViewActivity));
                            myIntent.PutExtra("image", album.Images.ExtraLarge.ToString());
                            StartActivity(myIntent);
                        }
                    };
                }

                List<LastTrack> lst = (from t in album.Tracks select t).ToList();
                Album_lvTracks.Adapter = new TracksAdapter(this, lst);
            }
        }

        private async void Request(string artist, string album, string ID)
        {
            if (!string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(artist))
            {
                var client = new LastfmClient(Config.apikey, Config.apisecret);

                var responseTracks = await client.Track.GetInfoByMbidAsync(ID);
                LastTrack lastTrack = responseTracks.Content;

                var responseAlbums = await client.Album.GetInfoAsync(lastTrack.ArtistName, lastTrack.AlbumName);

                LastAlbum lastAlbum = responseAlbums.Content;

                Album_Name.Text = lastAlbum.Name;
                Album_Artist.Text = lastAlbum.ArtistName;

                if (!string.IsNullOrEmpty(lastAlbum.Images.Large.ToString()))
                {
                    var imageBitmap = GetImageBitmapFromUrl(lastAlbum.Images.Large.ToString());
                    Album_Image.SetImageBitmap(imageBitmap);
                    Album_Image.Click += (sender, ea) =>
                    {
                        Method.Vibrate(this);
                        //Method.Show(this, lastAlbum.Images.Large.ToString());
                        if (!string.IsNullOrEmpty(lastAlbum.Images.ExtraLarge.ToString()))
                        {
                            var myIntent = new Intent(this, typeof(ImageViewActivity));
                            myIntent.PutExtra("image", lastAlbum.Images.ExtraLarge.ToString());
                            StartActivity(myIntent);
                        }
                    };
                }

                List<LastTrack> lst = (from t in lastAlbum.Tracks select t).ToList();
                Album_lvTracks.Adapter = new TracksAdapter(this, lst);
            }
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