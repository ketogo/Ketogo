using System;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Xamarin.Essentials;

namespace Ketogo
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button _mapButton;
        private Button _favoriteButton;
        private Button _topButton;
        private Button _nearButton;
        private Button _aboutButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            FindViews();
            LinkEventHandlers();
        }

        private void LinkEventHandlers()
        {
            _mapButton.Click += MapButton_Click;
            _favoriteButton.Click += FavoriteButton_Click;
            _topButton.Click += TopButton_Click;
            _nearButton.Click += NearButton_Click;
            _aboutButton.Click += AboutButton_Click;
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            var aboutIntent = new Intent(this, typeof(AboutActivity));
            StartActivity(aboutIntent);
        }

        private void NearButton_Click(object sender, EventArgs e)
        {
            var NearIntent = new Intent(this, typeof(NearActivity));
            StartActivity(NearIntent);
        }

        private void TopButton_Click(object sender, EventArgs e)
        {
            var TopIntent = new Intent(this, typeof(PlaceMenuWithTabsActivity));
            StartActivity(TopIntent);
        }

        private void FavoriteButton_Click(object sender, EventArgs e)
        {
            var FavIntent = new Intent(this, typeof(FavoriteActivity));
            StartActivity(FavIntent);
        }

        private void FindViews()
        {
            _mapButton = FindViewById<Button>(Resource.Id.MapButton);
            _favoriteButton = FindViewById<Button>(Resource.Id.FavoriteButton);
            _topButton = FindViewById<Button>(Resource.Id.TopButton);
            _nearButton = FindViewById<Button>(Resource.Id.OptionsButton);
            _aboutButton = FindViewById<Button>(Resource.Id.AboutButton);
        }

        void MapButton_Click(object sender, EventArgs e)
        {
            var MapIntent = new Intent(this, typeof(MapActivity));
            MapIntent.PutExtra("selectedPlaceId", 0);
            StartActivity(MapIntent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}