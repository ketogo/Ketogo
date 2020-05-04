using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Ketogo.Model;
using SQLite;

namespace Ketogo.Database
{
    public class DatabaseManager
    {
        SQLiteConnection dbConnection;

        public DatabaseManager()
        {
            DatabaseService dbService = new DatabaseService();
            dbConnection = dbService.CreateConnection();
        }

        public List<Place> GetAllPlaces()
        {
            return dbConnection.Query<Place>("Select * From [places]");
        }

        public List<Place> GetAllRestaurants()
        {
            List<Place> allPlaces = GetAllPlaces();
            return allPlaces.Where(p => p.Category == "Restaurant").ToList();
        }

        public Place GetPlaceById(int id)
        {
            List<Place> allPlaces = GetAllPlaces();
            return allPlaces.FirstOrDefault(p => p.PlaceId == id);
        }

        public List<Place> GetTop20PlacesByCategory(string category)
        {
            return dbConnection.Query<Place>("Select * from [places] where category = '" + category + "' and photo is not '' order by rating desc limit 20");
        }

        public List<Place> GetTop20Places()
        {
            return dbConnection.Query<Place>("Select * from [places] where photo is not '' order by rating desc limit 20");
        }

    }
}