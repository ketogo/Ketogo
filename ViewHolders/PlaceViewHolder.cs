using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.View.Menu;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Ketogo.ViewHolders
{
    public class PlaceViewHolder : RecyclerView.ViewHolder
    {

        public ImageView PlaceImageView { get; set; }
        public TextView PlaceNameTextView { get; set; }
        public TextView PlaceRatingTextView { get; set; }
        public TextView PlaceDistanceTextView { get; set; }

        public PlaceViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            PlaceImageView = itemView.FindViewById<ImageView>(Resource.Id.placeImageView);
            PlaceNameTextView = itemView.FindViewById<TextView>(Resource.Id.placeNameTextView);
            PlaceRatingTextView = itemView.FindViewById<TextView>(Resource.Id.placeRatingTextView);
            PlaceDistanceTextView = itemView.FindViewById<TextView>(Resource.Id.placeDistanceTextView);


            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }
}