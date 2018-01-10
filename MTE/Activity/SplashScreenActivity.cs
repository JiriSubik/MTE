using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Content;
using System.Threading.Tasks;
using Android.Content.PM;

namespace MTE.Activity
{
    [Activity(MainLauncher = true, Theme = "@style/MyTheme.Splash", NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreenActivity : AppCompatActivity
    {
        #region Controls

        #endregion

        #region SetControls/SetHandlers/RemoveHandlers
        private void SetControls()
        {

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

            //// Set view layout
            //SetContentView(Resource.Layout.SplashScreen);
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { Startup(); });
            startupWork.Start();
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

        public override void OnBackPressed()
        {
            // Vypnutí hardarového tlačítka ZPĚT
            return;
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

        async void Startup()
        {
            await Task.Delay(3000); // Simulate a bit of startup work.
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}