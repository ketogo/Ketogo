using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Ketogo.Database;
using Ketogo.Helpers;
using Ketogo.Model;
using Ketogo.ViewHolders;

namespace Ketogo.Adapters
{
    public class PlaceAdapter: RecyclerView.Adapter
    {
        private List<Place> _places;
        private DatabaseManager _placeDatabase;
        public event EventHandler<int> ItemClick;

        public PlaceAdapter(string category)
        {
            _placeDatabase = new DatabaseManager();
            _places = _placeDatabase.GetTop20PlacesByCategory(category);
        }

        public PlaceAdapter()
        {
            _placeDatabase = new DatabaseManager();
            _places = _placeDatabase.GetFavoritePlaces();
        }

        public override int ItemCount => _places.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is PlaceViewHolder placeViewHolder)
            {
                placeViewHolder.PlaceNameTextView.Text = _places[position].Name;
                placeViewHolder.PlaceRatingTextView.Text = _places[position].Rating.ToString();

                if (_places[position].Photo.Length <= 1)
                {
                    int imageId = (int) typeof(Resource.Drawable).GetField("no_image").GetValue(null);
                    placeViewHolder.PlaceImageView.SetImageResource(imageId);
                }
                else
                {
                    var imageBitmap = ImageHelper.GetImageBitmapFromUrl(_places[position].Photo);
                    placeViewHolder.PlaceImageView.SetImageBitmap(imageBitmap);
                }
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.place_viewholder, parent, false);

            PlaceViewHolder placeViewHolder = new PlaceViewHolder(itemView, OnClick);
            return placeViewHolder;
        }

        private void OnClick(int position)
        {
            var placeId = _places[position].PlaceId;
            ItemClick?.Invoke(this, placeId);
        }
    }
}