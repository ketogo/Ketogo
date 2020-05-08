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
            var menuButton = view.FindViewById<ImageButton>(Resource.Id.menuButton);
            menuButton.Click += MenuButton_Click;

            if (_category == "Restaurant")
            {
                categoryTextView.Text = "Najlepšie hodnotené reštaurácie";
            }
            else if (_category == "Cafe")
            {
                categoryTextView.Text = "Najlepšie hodnotené kaviarne";
            }
            else if (_category == "Store")
            {
                categoryTextView.Text = "Najlepšie hodnotené obchody";
            }
            else if (_category == "School")
            {
                categoryTextView.Text = "Najlepšie hodnotené školy";
            }
            else if (_category == "Doctor")
            {
                categoryTextView.Text = "Najlepšie hodnotení doktori";
            }

            var placeRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.placeMenuRecyclerView);
            var placeLayoutManager = new LinearLayoutManager(this.Context);
            placeRecyclerView.SetLayoutManager(placeLayoutManager);

            var placeAdapter = new PlaceAdapter(_category);
            placeAdapter.ItemClick += PlaceAdapter_ItemClick;
            placeRecyclerView.SetAdapter(placeAdapter);

            return view;
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            var menuIntent = new Intent(Application.Context, typeof(MainActivity));
            StartActivity(menuIntent);
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