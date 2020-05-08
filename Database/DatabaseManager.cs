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

        public Place GetPlaceById(int id)
        {
            List<Place> allPlaces = GetAllPlaces();
            return allPlaces.FirstOrDefault(p => p.PlaceId == id);
        }

        public Place GetPlaceByName(string name)
        {
            List<Place> allPlaces = GetAllPlaces();
            return allPlaces.FirstOrDefault(p => p.Name == name);
        }

        public List<Place> GetAllRestaurantsAndCafes()
        {
            return dbConnection.Query<Place>("Select * from [places] where category = 'Restaurant' or category = 'Cafe'");
        }

        public List<Place> GetAllPlacesByCategory(string category)
        {
            return dbConnection.Query<Place>("Select * from [places] where category = '" + category + "'");
        }

        public List<Place> GetTop20PlacesByCategory(string category)
        {
            return dbConnection.Query<Place>("Select * from [places] where category = '" + category + "' and photo is not '' order by rating desc limit 20");
        }

        public List<Place> GetFavoritePlaces()
        {
            return dbConnection.Query<Place>("Select * from [places] where isfavorite = 1");
        }

        public void AddToFavorites(int id)
        {
            dbConnection.Query<Place>("Update [places] Set isfavorite = 1 where placeid = " + id);
        }

        public void RemoveFromFavorites(int id)
        {
            dbConnection.Query<Place>("Update [places] Set isfavorite = 0 where placeid = " + id);
        }

    }
}