using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Content;
using IF.Lastfm.Core.Api;
using MTE.Adapter;
using System.Collections.Generic;
using IF.Lastfm.Core.Objects;
using System.Linq;
using MTE.DB;
using Android.Content.PM;
using System.Threading.Tasks;

namespace MTE
{
    [Activity(Label = "App1", MainLauncher = false, Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class TrackActivity : AppCompatActivity
    {
        DBHistory db = new DBHistory(Config.DB_History, Config.TBL_History);
        IReadOnlyList<LastTrack> tracks;
        #region Controls
        ListView Track_lvArtist;
        #endregion

        #region SetControls/SetHandlers/RemoveHandlers
        private void SetControls()
        {
            Track_lvArtist = FindViewById<ListView>(Resource.Id.lvArtist);
        }

        private void SetHandlers()
        {
            Track_lvArtist.ItemClick += Track_lvArtist_ItemClick;
        }

        private void RemoveHandlers()
        {
            Track_lvArtist.ItemClick -= Track_lvArtist_ItemClick;
        }
        #endregion

        #region EventHandlers
        private void Track_lvArtist_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Method.Vibrate(this);
            LastTrack track = tracks[e.Position];

            if (!string.IsNullOrEmpty(track.Mbid) && !string.IsNullOrEmpty(track.ArtistName))
            {
                var myIntent = new Intent(this, typeof(AlbumActivity));
                myIntent.PutExtra("AlbumName", track.AlbumName);
                myIntent.PutExtra("ArtistName", track.ArtistName);
                myIntent.PutExtra("ID", track.Mbid);
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
            SetContentView(Resource.Layout.Track);

            SetControls();
            SetHandlers();

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var title = FindViewById<TextView>(Resource.Id.toolbar_title);
            //title.Text = GetString(Resource.String.Tracks_title);
            title.Text = string.Empty;
            string track = Intent.GetStringExtra("track");
            title.Text = track;

            int records = await Request(track);
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

        private async Task<int> Request(string track)
        {
            int records = 0;
            Method.ProgressShow(this, GetString(Resource.String.Progress_message));
            var client = new LastfmClient(Config.apikey, Config.apisecret);

            if (!string.IsNullOrEmpty(track))
            {
                var responseTracks = await client.Track.SearchAsync(track);
                tracks = responseTracks.Content;
                if (tracks.Count > 0)
                {
                    records = tracks.Count;
                    await db.AddAsync(new History() { Query = track, Type = SearchType.Track });
                    List<LastTrack> lst = (from t in tracks select t).ToList();
                    Track_lvArtist.Adapter = new TrackListAdapter(this, lst);
                }

            }
            Method.ProgressHide(this);
            return records;
        }
    }
}