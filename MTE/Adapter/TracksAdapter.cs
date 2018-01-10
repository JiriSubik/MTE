using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using IF.Lastfm.Core.Objects;

namespace MTE.Adapter
{
    public class TracksAdapter : BaseAdapter<LastTrack>
    {
        List<LastTrack> items;
        AppCompatActivity context;
        public TracksAdapter(AppCompatActivity context, List<LastTrack> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override LastTrack this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.ListViewTracks, null);
            view.FindViewById<TextView>(Resource.Id.TrackName).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.Duration).Text = (item.Duration.HasValue) ? item.Duration.Value.ToString(@"mm\:ss") : "";
            return view;
        }
    }
}