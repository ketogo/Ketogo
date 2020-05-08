using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Ketogo.Database;
using Ketogo.Helpers;
using Ketogo.Model;

namespace Ketogo
{
    [Activity(Label = "MapActivity")]
    public class MapActivity : AppCompatActivity, IOnMapReadyCallback, ILocationListener
    {
        private GoogleMap _googleMap;
        private DatabaseManager _placeDatabase;
        private LatLng _userLatLng;
        private LatLng _selectedPlaceLatLng;
        private string _selectedPlaceCategory;
        private LocationManager _locationManager;
        private string _provider;
        private int selectedPlaceId;

        private Marker _userMarker;

        private double _userLat;
        private double _userLng;

        private List<Place> _selectedPlaces = new List<Place>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_map);

            var mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);

            var userLocationButton = FindViewById<ImageButton>(Resource.Id.userLocationButton);
            userLocationButton.Click += UserLocationButton_Click;

            var menuButton = FindViewById<Button>(Resource.Id.menuButton);
            menuButton.Click += MenuButton_Click;

            var spinner = FindViewById<Spinner>(Resource.Id.spinner);
            string firstItem = spinner.SelectedItem.ToString();

            spinner.ItemSelected += (s, e) =>
            {
                if (firstItem.Equals(spinner.SelectedItem.ToString()))
                {
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Reštaurácie")
                {
                    AddMarkers("Restaurant");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Kaviarne")
                {
                    AddMarkers("Cafe");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Bankomaty")
                {
                    AddMarkers("Atm");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Banky")
                {
                    AddMarkers("Bank");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Autoumyvárne")
                {
                    AddMarkers("Car Wash");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Knižnice")
                {
                    AddMarkers("Library");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Nočné Kluby")
                {
                    AddMarkers("Night Club");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Polícia")
                {
                    AddMarkers("Police");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Pošta")
                {
                    AddMarkers("Post Office");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Obchody")
                {
                    AddMarkers("Store");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Školy")
                {
                    AddMarkers("School");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Posilovne")
                {
                    AddMarkers("Gym");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Nemocnice")
                {
                    AddMarkers("Hospital");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Parky")
                {
                    AddMarkers("Park");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Kostoly")
                {
                    AddMarkers("Church");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Pumpy")
                {
                    AddMarkers("Gas Station");
                }
                if (spinner.GetItemAtPosition(e.Position).ToString() == "Doktori")
                {
                    AddMarkers("Doctor");
                }

            };

            _locationManager = (LocationManager)GetSystemService(Context.LocationService);
            _provider = _locationManager.GetBestProvider(new Criteria(), false);
            Location myLocation = GetLastKnownLocation();
            if (myLocation == null)
            {
                System.Diagnostics.Debug.WriteLine("No Location");
            }

            _placeDatabase = new DatabaseManager();

            selectedPlaceId = Intent.Extras.GetInt("selectedPlaceId");
            if (selectedPlaceId == 0)
            {
                _selectedPlaceLatLng = new LatLng(myLocation.Latitude, myLocation.Longitude);
                _userLatLng = new LatLng(myLocation.Latitude, myLocation.Longitude);
            }
            else
            {
                Place selectedPlace = _placeDatabase.GetPlaceById(selectedPlaceId);
                _selectedPlaceLatLng = new LatLng(selectedPlace.Lat, selectedPlace.Lng);
                _selectedPlaceCategory = selectedPlace.Category;
                _userLatLng = new LatLng(myLocation.Latitude, myLocation.Longitude);
            }

        }

        private Location GetLastKnownLocation()
        {
            _locationManager = (LocationManager)GetSystemService(Context.LocationService);
            IList<string> providers = _locationManager.GetProviders(true);
            Location bestLocation = null;
            foreach (string provider in providers)
            {
                Location l = _locationManager.GetLastKnownLocation(provider);
                if (l == null)
                {
                    continue;
                }
                if (bestLocation == null || l.Accuracy < bestLocation.Accuracy)
                {
                    // Found best last known location: %s", l);
                    bestLocation = l;
                }
            }
            return bestLocation;
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            var menuIntent = new Intent(this, typeof(MainActivity));
            StartActivity(menuIntent);
        }

        private void UserLocationButton_Click(object sender, EventArgs e)
        {
            LatLng userLatLng = new LatLng(_userLat, _userLng);
            AddUserMarker(userLatLng);
            UserCameraUpdate(userLatLng);
        }

        public void OnMapReady(GoogleMap map)
        {
            _googleMap = map;

            _googleMap.UiSettings.ZoomControlsEnabled = true;
            _googleMap.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(this, Resource.Raw.mymapstyle));

            if (_selectedPlaceCategory != null)
            {
                AddMarkers(_selectedPlaceCategory);
            }
            AddUserMarker(_userLatLng);
            UpdateCamera(_selectedPlaceLatLng);

            _googleMap.InfoWindowClick += GoogleMap_InfoWindowClick;
        }

        private void GoogleMap_InfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var selectedPlaceName = e.Marker.Title.ToString();
            Place selectedPlace = _placeDatabase.GetPlaceByName(selectedPlaceName);
            var selectedPlaceId = selectedPlace.PlaceId;
            Intent detailIntent = new Intent(Application.Context, typeof(PlaceDetailActivity));
            detailIntent.PutExtra("selectedPlaceId", selectedPlaceId);
            StartActivity(detailIntent);
        }

        private void AddMarkers(string category)
        {
            _googleMap.Clear();
            LatLng userLatLng = new LatLng(_userLat, _userLng);
            AddUserMarker(userLatLng);

            _selectedPlaces = _placeDatabase.GetAllPlacesByCategory(category);

            foreach (Place place in _selectedPlaces)
            {
                LatLng placeLatLng = new LatLng(place.Lat, place.Lng);
                double distance = Math.Round(DistanceHelper.CalculateDistance(GetLastKnownLocation().Latitude, GetLastKnownLocation().Longitude, place.Lat, place.Lng), 2);

                var placeMarker = new MarkerOptions();
                placeMarker.SetPosition(placeLatLng)
                    .SetTitle(place.Name)
                    .SetSnippet("rating: " + place.Rating.ToString() + ", vzdialené: " + distance.ToString() + "km")
                    .SetIcon(BitmapDescriptorFactory.FromResource(GetIconByCategory(place.Category)));
                _googleMap.AddMarker(placeMarker);
            }
        }

        private void AddUserMarker(LatLng userLatLng)
        {
            if (_userMarker != null)
            {
                _userMarker.Remove();
            }
            _userMarker = _googleMap.AddMarker(new MarkerOptions().SetPosition(userLatLng)
                .SetTitle("Moja poloha")
                .SetIcon(BitmapDescriptorFactory.FromResource((int) typeof(Resource.Drawable).GetField("icon_user").GetValue(null))));
        }

        private int GetIconByCategory(string category)
        {
            int icon = 0;

            if (category == "Restaurant")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_restaurant").GetValue(null);
            }
            if (category == "Cafe")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_cafe").GetValue(null);
            }
            if (category == "Atm")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_atm").GetValue(null);
            }
            if (category == "Bank")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_bank").GetValue(null);
            }
            if (category == "Car Wash")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_car_wash").GetValue(null);
            }
            if (category == "Library")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_library").GetValue(null);
            }
            if (category == "Night Club")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_night_club").GetValue(null);
            }
            if (category == "Police")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_police").GetValue(null);
            }
            if (category == "Post Office")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_post_office").GetValue(null);
            }
            if (category == "Store")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_store").GetValue(null);
            }
            if (category == "School")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_school").GetValue(null);
            }
            if (category == "Gym")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_gym").GetValue(null);
            }
            if (category == "Hospital")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_hospital").GetValue(null);
            }
            if (category == "Park")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_park").GetValue(null);
            }
            if (category == "Church")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_church").GetValue(null);
            }
            if (category == "Gas Station")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_gas_station").GetValue(null);
            }
            if (category == "Doctor")
            {
                icon = (int)typeof(Resource.Drawable).GetField("icon_doctor").GetValue(null);
            }

            return icon;
        }

        private void UpdateCamera(LatLng latLng)
        {
            var cameraUpdate = CameraUpdateFactory.NewLatLngZoom(latLng, 16);
            _googleMap.MoveCamera(cameraUpdate);
        }

        private void UserCameraUpdate(LatLng latLng)
        {
            var cameraUpdate = CameraUpdateFactory.NewLatLngZoom(latLng, 16);
            _googleMap.MoveCamera(cameraUpdate);
        }

        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_provider, 400, 1, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {
            _userLat = location.Latitude;
            _userLng = location.Longitude;
            LatLng userLatLng = new LatLng(_userLat, _userLng);
            AddUserMarker(userLatLng);
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }
    }
}