using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Ketogo.Adapters;

namespace Ketogo
{
    [Activity(Label = "PlaceMenuWithTabsActivity", Theme= "@style/AppTheme")]
    public class PlaceMenuWithTabsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.place_menu_tabs);
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.placePager);
            CategoryFragmentAdapter categoryFragmentAdapter = new CategoryFragmentAdapter(SupportFragmentManager);
            viewPager.Adapter = categoryFragmentAdapter;
        }
    }
}