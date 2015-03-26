using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Citeseer
{
    public class Favourites
    {
        List<Query> favourites;
        public static String favFile = ".favourites";

        public Favourites()
        {
            favourites = new List<Query>();
            readFavouritesFromFile();
        }

        public List<Query> getFavourites(){
            return favourites;
        }

        public Boolean readFavouritesFromFile(){
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (var stream = File.OpenRead(favFile))
                {
                    favourites = (List<Query>)formatter.Deserialize(stream);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public Boolean writeFavouritesToFile()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (var stream = File.OpenWrite(favFile))
                {
                    formatter.Serialize(stream, favourites);
                }
                return true;
            }
            catch (FileNotFoundException)
            {
            }
            catch (Exception)
            {
            }
            return false;
        }

        public Boolean deleteFavourite(Query query)
        {
            if (favourites.Remove(query))
                return writeFavouritesToFile();
            return false;
        }

        public Boolean clearFavourites()
        {
            favourites.Clear();
            writeFavouritesToFile();
            return true;
        }

        public AddFavReturnCode addFavourite(Query query)
        {
            if (!inFavorites(query))
            {
                favourites.Add(query);
                if (writeFavouritesToFile())
                    return AddFavReturnCode.ADDED;
                return AddFavReturnCode.NOT_ADDED;
            }
            return AddFavReturnCode.EXISTS;
        }

        public enum AddFavReturnCode
        {
            ADDED, NOT_ADDED, EXISTS
        };

        private bool inFavorites(Query q)
        {
            foreach (Query query in favourites)
            {
                if (query.Equals(q))
                    return true;
            }
            return false;
        }
    }
}
