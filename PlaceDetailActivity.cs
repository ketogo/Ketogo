using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Ketogo.Database;
using Ketogo.Helpers;
using Ketogo.Model;

namespace Ketogo
{
    [Activity(Label = "PlaceDetailActivity")]
    public class PlaceDetailActivity : Activity
    {
        private DatabaseManager _placeDatabase;
        private Place _selectedPlace;

        private TextView _placeNameTextView;
        private TextView _addressTextView;
        private TextView _websiteTextView;
        private TextView _numberTextView;
        private ImageView _placeImageView;
        private Button _navButton;
        private Button _mapButton;
        private ImageButton _favButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_place_detail);

            var selectedPlaceId = Intent.Extras.GetInt("selectedPlaceId");

            _placeDatabase = new DatabaseManager();
            _selectedPlace = _placeDatabase.GetPlaceById(selectedPlaceId);

            FindViews();
            BindData();
            LinkEventHandlers();
        }

        private void LinkEventHandlers()
        {
            _mapButton.Click += MapButton_Click;
            _favButton.Click += FavButton_Click;
            _navButton.Click += NavButton_Click;
        }

        private void NavButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FavButton_Click(object sender, EventArgs e)
        {
            int selectedPlaceId = _selectedPlace.PlaceId;
            _placeDatabase = new DatabaseManager();
            Place selectedPlace = _placeDatabase.GetPlaceById(selectedPlaceId);
            if (selectedPlace.IsFavorite == 0)
            {
                _placeDatabase.AddToFavorites(selectedPlaceId);
                _favButton.SetImageResource((int)typeof(Resource.Drawable).GetField("icon_favorite").GetValue(null));
            }
            else
            {
                _placeDatabase.RemoveFromFavorites(selectedPlaceId);
                _favButton.SetImageResource((int)typeof(Resource.Drawable).GetField("icon_notfavorite").GetValue(null));
            }

        }

        private void MapButton_Click(object sender, EventArgs e)
        {
            int selectedPlaceId = _selectedPlace.PlaceId;

            Intent mapIntent = new Intent(Application.Context, typeof(MapActivity));
            mapIntent.PutExtra("selectedPlaceId", selectedPlaceId);
            StartActivity(mapIntent);
        }

        private void BindData()
        {
            _placeNameTextView.Text = _selectedPlace.Name;
            _addressTextView.Text = _selectedPlace.Address;
            _websiteTextView.Text = _selectedPlace.Website;
            _numberTextView.Text = _selectedPlace.PhoneNumber;

            if (_selectedPlace.Photo.Length <= 1)
            {
                int resourceId = (int)typeof(Resource.Drawable).GetField("no_image").GetValue(null);
                _placeImageView.SetImageResource(resourceId);
            }
            else
            {
                var imageBitmap = ImageHelper.GetImageBitmapFromUrl(_selectedPlace.Photo);
                _placeImageView.SetImageBitmap(imageBitmap);
            }

            if (_selectedPlace.IsFavorite == 0)
            {
                _favButton.SetImageResource((int)typeof(Resource.Drawable).GetField("icon_notfavorite").GetValue(null));
            }
            else
            {
                _favButton.SetImageResource((int)typeof(Resource.Drawable).GetField("icon_favorite").GetValue(null));
            }
        }

        private void FindViews()
        {
            _placeNameTextView = FindViewById<TextView>(Resource.Id.placeNameTextView);
            _addressTextView = FindViewById<TextView>(Resource.Id.addressTextView);
            _websiteTextView = FindViewById<TextView>(Resource.Id.websiteTextView);
            _numberTextView = FindViewById<TextView>(Resource.Id.numberTextView);
            _placeImageView = FindViewById<ImageView>(Resource.Id.placeImageView);
            _mapButton = FindViewById<Button>(Resource.Id.mapButton);
            _navButton = FindViewById<Button>(Resource.Id.navButton);
            _favButton = FindViewById<ImageButton>(Resource.Id.favButton);
        }
    }
}