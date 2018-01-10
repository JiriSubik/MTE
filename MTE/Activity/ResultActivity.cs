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
using MTE.Adapter;
using System.Linq;
using MTE.DB;
using Android.Content.PM;
using System.Threading.Tasks;

namespace MTE
{
    [Activity(Label = "@string/app_name", MainLauncher = false, Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ResultActivity : AppCompatActivity
    {
        DBHistory db = new DBHistory(Config.DB_History, Config.TBL_History);
        IReadOnlyList<LastAlbum> albums;
        #region Controls
        ListView Result_lvArtist;
        #endregion

        #region SetControls/SetHandlers/RemoveHandlers
        private void SetControls()
        {
            Result_lvArtist = FindViewById<ListView>(Resource.Id.lvArtist);
        }

        private void SetHandlers()
        {
            Result_lvArtist.ItemClick += Result_lvArtist_ItemClick;
        }

        private void RemoveHandlers()
        {
            Result_lvArtist.ItemClick -= Result_lvArtist_ItemClick;
        }
        #endregion

        #region EventHandlers
        private void Result_lvArtist_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Method.Vibrate(this);
            LastAlbum album = albums[e.Position];

            if (!string.IsNullOrEmpty(album.Mbid))
            {
                var myIntent = new Intent(this, typeof(AlbumActivity));
                myIntent.PutExtra("AlbumID", album.Mbid);
                StartActivity(myIntent);
            }
            else
            {
                Toast.MakeText(
                    this,
                    GetString(Resource.String.Album_error),
                    ToastLength.Long).Show();
            }
        }
        #endregion

        #region LifeCycleMethods        
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            // Set view layout
            SetContentView(Resource.Layout.Result);

            SetControls();
            SetHandlers();

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var title = FindViewById<TextView>(Resource.Id.toolbar_title);
            //title.Text = GetString(Resource.String.Result_title);
            title.Text = string.Empty;
            string artist = Intent.GetStringExtra("artist");
            string album = Intent.GetStringExtra("album");
            title.Text = artist + " " + album;

            int records = await Request(artist, album);
            if (records == 0)
            {
                Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
                alert.SetTitle(GetString(Resource.String.Confirm_title_records));
                alert.SetMessage(GetString(Resource.String.Confirm_message_records));
                alert.SetPositiveButton(GetString(Resource.String.Btn_OK), (senderAlert, args) =>
                {
                    base.OnBackPressed();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
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

        private async Task<int> Request(string artist, string album)
        {
            int records = 0;
            Method.ProgressShow(this, GetString(Resource.String.Progress_message));
            var client = new LastfmClient(Config.apikey, Config.apisecret);

            if (!string.IsNullOrEmpty(artist))
            {
                var responseAlbums = await client.Artist.GetTopAlbumsAsync(artist);
                albums = responseAlbums.Content;
                if (albums.Count > 0)
                {
                    records = albums.Count;
                    await db.AddAsync(new History() { Query = artist, Type = SearchType.Artist });
                    Result_lvArtist.Adapter = new ArtistAdapter(this, albums);
                }
            }
            else if (!string.IsNullOrEmpty(album))
            {
                var responseAlbums = await client.Album.SearchAsync(album);
                albums = responseAlbums.Content;
                if (albums.Count > 0)
                {
                    records = albums.Count;
                    await db.AddAsync(new History() { Query = album, Type = SearchType.Album });
                    Result_lvArtist.Adapter = new ArtistAdapter(this, albums);
                }
            }
            Method.ProgressHide(this);

            return records;
        }
    }
}