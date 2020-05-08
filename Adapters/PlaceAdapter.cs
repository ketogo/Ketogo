using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Ketogo.Database;
using Ketogo.Helpers;
using Ketogo.Model;
using Ketogo.ViewHolders;
using Xamarin.Essentials;

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

        public PlaceAdapter(LatLng userLocation)
        {
            _placeDatabase = new DatabaseManager();
            List<Place> allPlaces = _placeDatabase.GetAllRestaurantsAndCafes();
            _places = GetNearPlaces(allPlaces, userLocation);
        }

        public PlaceAdapter()
        {
            _placeDatabase = new DatabaseManager();
            _places = _placeDatabase.GetFavoritePlaces();
        }

        private List<Place> GetNearPlaces(List<Place> allPlaces, LatLng userLocation)
        {
            var lat = userLocation.Latitude;
            var lng = userLocation.Longitude;
            var nearPlacesList = new List<Place>();

            foreach (Place place in allPlaces)
            {
                var placeLat = place.Lat;
                var placeLng = place.Lng;
                var distance = DistanceHelper.CalculateDistance(lat, lng, placeLat, placeLng);
                if (distance <= 0.5)
                {
                    nearPlacesList.Add(place);
                }
            }

            nearPlacesList.OrderByDescending(o => o.Rating);
            return nearPlacesList;
        }

        public override int ItemCount => _places.Count;

        public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is PlaceViewHolder placeViewHolder)
            {
                placeViewHolder.PlaceNameTextView.Text = _places[position].Name;
                placeViewHolder.PlaceRatingTextView.Text = _places[position].Rating.ToString();

                LatLng userLatLng = await GetLocation();
                double distance = Math.Round(DistanceHelper.CalculateDistance(userLatLng.Latitude, userLatLng.Longitude, _places[position].Lat, _places[position].Lng), 2);
                placeViewHolder.PlaceDistanceTextView.Text = distance.ToString() + " km";

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

        private async Task<LatLng> GetLocation()
        {
            double lat = 0;
            double lng = 0;
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                lat = location.Latitude;
                lng = location.Longitude;
            }

            LatLng userLatLng = new LatLng(lat, lng);
            return userLatLng;
        }
    }
}