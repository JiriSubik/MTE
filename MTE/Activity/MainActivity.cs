using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Content;
using MTE.DB;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using MTE.Adapter;
using Android.Content.PM;

namespace MTE
{
    [Activity(Label = "@string/app_name", MainLauncher = false, Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        List<History> lstHistory;
        DBHistory db = new DBHistory(Config.DB_History, Config.TBL_History);
        #region Controls
        Button Main_btnSearch;
        Button Main_btnHistory;
        EditText Main_etArtist;
        EditText Main_etAlbum;
        EditText Main_etTrack;
        LinearLayout Main_llHistory;
        ListView Main_lvHistory;
        TextView Main_tvHistory;
        #endregion

        #region SetControls/SetHandlers/RemoveHandlers
        private void SetControls()
        {
            Main_btnSearch = FindViewById<Button>(Resource.Id.btnSearch);
            Main_btnHistory = FindViewById<Button>(Resource.Id.btnHistory);
            Main_etArtist = FindViewById<EditText>(Resource.Id.etArtist);
            Main_etAlbum = FindViewById<EditText>(Resource.Id.etAlbum);
            Main_etTrack = FindViewById<EditText>(Resource.Id.etTrack);
            Main_llHistory = FindViewById<LinearLayout>(Resource.Id.llHistory);
            Main_lvHistory = FindViewById<ListView>(Resource.Id.lvHistory);
            Main_tvHistory = FindViewById<TextView>(Resource.Id.tvHistory);
        }

        private void SetHandlers()
        {
            Main_btnSearch.Click += Main_btnSearch_Click;
            Main_btnHistory.Click += Main_btnHistory_Click;
            Main_lvHistory.ItemClick += Main_lvHistory_ItemClick;
        }

        private void RemoveHandlers()
        {
            Main_btnSearch.Click -= Main_btnSearch_Click;
            Main_btnHistory.Click -= Main_btnHistory_Click;
            Main_lvHistory.ItemClick -= Main_lvHistory_ItemClick;
        }
        #endregion

        #region EventHandlers
        private async void Main_btnHistory_Click(object sender, System.EventArgs e)
        {
            Method.Vibrate(this);
            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle(GetString(Resource.String.Confirm_title));
            alert.SetMessage(GetString(Resource.String.Confirm_message));
            alert.SetPositiveButton(GetString(Resource.String.Btn_OK), async (senderAlert, args) =>
            {
                await db.DeleteAsync();
                FillListView();
            });
            alert.SetNegativeButton(GetString(Resource.String.Btn_Cancel), (senderAlert, args) =>
            {

            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void Main_lvHistory_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Method.Vibrate(this);
            History history = lstHistory[e.Position];

            switch (history.Type)
            {
                case SearchType.Artist:
                    var myIntent = new Intent(this, typeof(ResultActivity));
                    myIntent.PutExtra("artist", history.Query);
                    StartActivity(myIntent);
                    break;
                case SearchType.Album:
                    var myAlbumIntent = new Intent(this, typeof(ResultActivity));
                    myAlbumIntent.PutExtra("album", history.Query);
                    StartActivity(myAlbumIntent);
                    break;
                case SearchType.Track:
                    var myTrackIntent = new Intent(this, typeof(TrackActivity));
                    myTrackIntent.PutExtra("track", history.Query);
                    StartActivity(myTrackIntent);
                    break;
                default:
                    break;
            }
        }
        private void Main_btnSearch_Click(object sender, System.EventArgs e)
        {
            Method.Vibrate(this);
            if ((!string.IsNullOrEmpty(Main_etArtist.Text) || !string.IsNullOrEmpty(Main_etAlbum.Text)) && string.IsNullOrEmpty(Main_etTrack.Text))
            {
                var myIntent = new Intent(this, typeof(ResultActivity));
                myIntent.PutExtra("artist", Main_etArtist.Text);
                myIntent.PutExtra("album", Main_etAlbum.Text);
                StartActivity(myIntent);
            }

            if (!string.IsNullOrEmpty(Main_etTrack.Text))
            {
                var myIntent = new Intent(this, typeof(TrackActivity));
                myIntent.PutExtra("track", Main_etTrack.Text);
                StartActivity(myIntent);
            }
        }

        #endregion

        #region LifeCycleMethods        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            // Set view layout
            SetContentView(Resource.Layout.Main);

            SetControls();
            SetHandlers();

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var title = FindViewById<TextView>(Resource.Id.toolbar_title);
            title.Text = GetString(Resource.String.Main_title);

            //Main_etArtist.Text = "Enrique Iglesias";
            //Main_etTrack.Text = "Fly Away";
            Main_llHistory.Visibility = ViewStates.Invisible;
        }

        protected override void OnStart()
        {
            base.OnStart();
            FillListView();
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

        private async void FillListView()
        {
            try
            {
                lstHistory = await GetHistory();
                if (lstHistory.Count == 0)
                {
                    Main_llHistory.Visibility = ViewStates.Invisible;
                }
                else
                {
                    Main_llHistory.Visibility = ViewStates.Visible;
                }
                Main_lvHistory.Adapter = null;
                Main_lvHistory.Adapter = new HistoryAdapter(this, lstHistory);

            }
            catch (Exception ex)
            {

            }
        }
        private async Task<List<History>> GetHistory()
        {
            return await db.GetAllAsync();
        }
    }
}