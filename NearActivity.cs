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
using Ketogo.Adapters;
using Xamarin.Essentials;

namespace Ketogo
{
    [Activity(Label = "FavoriteActivity")]
    public class NearActivity : Activity
    {
        private RecyclerView _placeRecyclerView;
        private RecyclerView.LayoutManager _placeLayoutManager;
        private PlaceAdapter _placeAdapter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_near);
            _placeRecyclerView = FindViewById<RecyclerView>(Resource.Id.placeMenuRecyclerView);

            var menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);
            menuButton.Click += MenuButton_Click;

            LatLng userLocation = await GetLocation();

            _placeLayoutManager = new LinearLayoutManager(this);
            _placeRecyclerView.SetLayoutManager(_placeLayoutManager);
            _placeAdapter = new PlaceAdapter(userLocation);
            _placeAdapter.ItemClick += PlaceAdapter_ItemClick;
            _placeRecyclerView.SetAdapter(_placeAdapter);
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            var menuIntent = new Intent(this, typeof(MainActivity));
            StartActivity(menuIntent);
        }

        private void PlaceAdapter_ItemClick(object sender, int e)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(PlaceDetailActivity));
            intent.PutExtra("selectedPlaceId", e);
            StartActivity(intent);
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