using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;

namespace Ketogo.Model
{
    public class Place
    {
        public int PlaceId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double Rating { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string PhoneNumber { get; set; }
        public string GooglePlaceId { get; set; }
        public string Url { get; set; }
        public string PhotoReference { get; set; }
        public string Category { get; set; }

        public Place(int placeId, double lat, double lng, double rating, string name, string address, string website,
            string phoneNumber, string googlePlaceId, string url, string photoReference, string category)
        {
            PlaceId = placeId;
            Lat = lat;
            Lng = lng;
            Rating = rating;
            Name = name;
            Address = address;
            Website = website;
            PhoneNumber = phoneNumber;
            GooglePlaceId = googlePlaceId;
            Url = url;
            PhotoReference = photoReference;
            Category = category;
        }

        public Place()
        {

        }

    }
}