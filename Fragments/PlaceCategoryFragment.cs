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
using Fragment = Android.Support.V4.App.Fragment;

namespace Ketogo.Fragments
{
    public class PlaceCategoryFragment: Fragment
    {
        private readonly string _category;

        public PlaceCategoryFragment(string category)
        {
            _category = category;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.place_menu_fragment, container, false);

            var categoryTextView = view.FindViewById<TextView>(Resource.Id.categoryTextView);
            categoryTextView.Text = _category;

            var placeRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.placeMenuRecyclerView);
            var placeLayoutManager = new LinearLayoutManager(this.Context);
            placeRecyclerView.SetLayoutManager(placeLayoutManager);

            var placeAdapter = new PlaceAdapter(_category);
            placeAdapter.ItemClick += PlaceAdapter_ItemClick;
            placeRecyclerView.SetAdapter(placeAdapter);

            return view;
        }

        private void PlaceAdapter_ItemClick(object sender, int e)
        {
            var intent = new Intent();
            intent.SetClass(this.Context, typeof(PlaceDetailActivity));
            intent.PutExtra("selectedPlaceId", e);
            StartActivity(intent);
        }
    }
}