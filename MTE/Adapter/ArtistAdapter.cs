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
    public class ArtistAdapter : BaseAdapter<LastAlbum>
    {
        IReadOnlyList<LastAlbum> items;
        AppCompatActivity context;
        public ArtistAdapter(AppCompatActivity context, IReadOnlyList<LastAlbum> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override LastAlbum this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.ListViewArtist, null);
            view.FindViewById<TextView>(Resource.Id.Name).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.Artist).Text = item.ArtistName + " " + item.ReleaseDateUtc;
            if (item.Images != null)
            {
                if ((item.Images.Small != null) && !string.IsNullOrEmpty(item.Images.Small.ToString()))
                {
                    var imageBitmap = GetImageBitmapFromUrl(item.Images.Small.ToString());
                    view.FindViewById<ImageView>(Resource.Id.AlbumImage).SetImageBitmap(imageBitmap);
                } 
            }
            return view;
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