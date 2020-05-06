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
using Ketogo.Adapters;

namespace Ketogo
{
    [Activity(Label = "FavoriteActivity")]
    public class FavoriteActivity : Activity
    {
        private RecyclerView _placeRecyclerView;
        private RecyclerView.LayoutManager _placeLayoutManager;
        private PlaceAdapter _placeAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.place_menu);
            _placeRecyclerView = FindViewById<RecyclerView>(Resource.Id.placeMenuRecyclerView);

            _placeLayoutManager = new LinearLayoutManager(this);
            _placeRecyclerView.SetLayoutManager(_placeLayoutManager);
            _placeAdapter = new PlaceAdapter();
            _placeAdapter.ItemClick += PlaceAdapter_ItemClick;
            _placeRecyclerView.SetAdapter(_placeAdapter);
        }

        private void PlaceAdapter_ItemClick(object sender, int e)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(PlaceDetailActivity));
            intent.PutExtra("selectedPlaceId", e);
            StartActivity(intent);
        }
    }
}