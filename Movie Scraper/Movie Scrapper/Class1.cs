using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FanartTv;
using Unbroken.LaunchBox.Plugins;
using System.Text.RegularExpressions;
using Unbroken.LaunchBox.Plugins.Data;
using System.Drawing;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Movie_Scrapper
{
    public class Class1 : IGameMenuItemPlugin
    {
        public bool SupportsMultipleGames
        {
            get
            {
                // supports selecting multiple Movies
                return true;
            }
        }

        public string Caption
        {
            get
            {
                //sets the name
                return "Scrape Movie";
            }
        }

        public System.Drawing.Image IconImage
        {
            get
            {
                //not using an icon
                return null;
            }
        }

        public bool ShowInLaunchBox
        {
            get
            {
                //shows in Launchbox
                return true;
            }
        }

        public bool ShowInBigBox
        {
            get
            {
                //does not show in big box
                return false;
            }
        }

        public bool GetIsValidForGame(IGame selectedGame)
        {
            //always valid for game
            return true;
        }

        public bool GetIsValidForGames(IGame[] selectedGames)
        {
            //always valid for games
            return true;
        }

        public void OnSelected(IGame selectedGame)
        {
            //sets api key obtained by Fanart.TV i am not sharing my key
            FanartTv.API.Key = "";
            // Sets client api key i left this blank in compiled version aswell
            FanartTv.API.cKey = "";
            //checks the movies name
            string MovieName = selectedGame.Title;

            //searches for movie using omdbapi
            var search = FanartTv.Helper.Json.GetJson("http://www.omdbapi.com/?t=" + MovieName + "&plot=full");
            //converts json to object
            dynamic data = JObject.Parse(search);
            //checks  if the game matched the search
            if (data.imdbID != null)
            {
                //sets the imdb id
                string result = data.imdbID;
                //checks for image results
                var mo0 = new FanartTv.Movies.Movie(result);
                //joins the images directory
                var gamejoin = Path.Combine(Directory.GetCurrentDirectory(), "Images\\" + selectedGame.Platform);
                //check if it found an image
                if (mo0.List.Movieposter != null)
                {
                    foreach (var poster in mo0.List.Movieposter)
                    {
                        //dets download path
                        string path = gamejoin + "\\Box - Front\\" + selectedGame.Title + "-01.jpg";
                        //checks if file exists
                        if (File.Exists(path) == false)
                        {
                            //downloads image
                            using (WebClient client = new WebClient())

                            {

                                client.DownloadFile(new Uri(poster.Url), path);

                            }
                        }
                    }
                

                }
                if (mo0.List.Moviebanner != null)
                {
                    foreach (var poster in mo0.List.Moviebanner)
                    {
                        string path = gamejoin + "\\Banner\\" + selectedGame.Title + "-01.jpg";

                        if (File.Exists(path) == false)
                        {
                            using (WebClient client = new WebClient())

                            {

                                client.DownloadFile(new Uri(poster.Url), path);

                            }
                        }
                    }

                }
                
                if (mo0.List.Movielogo != null)
                {
                    foreach (var poster in mo0.List.Movielogo)
                    {
                        string path = gamejoin + "\\Clear Logo\\" + selectedGame.Title + "-01.jpg";

                        if (File.Exists(path) == false)
                        {
                            using (WebClient client = new WebClient())

                            {

                                client.DownloadFile(new Uri(poster.Url), path);

                            }
                        }
                    }
                }
                
                if (mo0.List.Hdmovielogo != null)
                {
                    foreach (var poster in mo0.List.Hdmovielogo)
                    {
                        string path = gamejoin + "\\Clear Logo\\" + selectedGame.Title + "-01.jpg";

                        if (File.Exists(path) == false)
                        {
                            using (WebClient client = new WebClient())

                            {

                                client.DownloadFile(new Uri(poster.Url), path);

                            }
                        }
                    }
                }
                
                if (mo0.List.Moviebackground != null)
                {
                    foreach (var poster in mo0.List.Moviebackground)
                    {
                        string path = gamejoin + "\\Fanart - Background\\" + selectedGame.Title + "-01.jpg";

                        if (File.Exists(path) == false)
                        {
                            using (WebClient client = new WebClient())

                            {

                                client.DownloadFile(new Uri(poster.Url), path);

                            }
                        }
                    }
                }
                //check if it found a discription
                if (data.Plot != null)
                {
                    //sets the discription
                    selectedGame.Notes = data.Plot;

                }
                //check if it found the year
                if (data.Year != null)
                {
                    //sets the Movie release yeat
                    selectedGame.ReleaseYear = data.Year;

                }


                //checks if it found the Production
                if (data.Production != null)
                {
                    //sets the production
                    selectedGame.Publisher = data.Production;
                }
          
            }
            //saves changes
            PluginHelper.DataManager.Save();
            
        }

        public void OnSelected(IGame[] selectedGames)
        {
            foreach (var selectedGame in selectedGames)
            {
                FanartTv.API.Key = "f4fe7cd51f40b169f1cc9ef1786dc8a2";
                // Set your ClientKey
                FanartTv.API.cKey = "";
                string MovieName = selectedGame.Title;


                var search = FanartTv.Helper.Json.GetJson("http://www.omdbapi.com/?t=" + MovieName + "&plot=full");
                dynamic data = JObject.Parse(search);

                if (data.imdbID != null)
                {
                    string result = data.imdbID;

                    var mo0 = new FanartTv.Movies.Movie(result);
                    var gamejoin = Path.Combine(Directory.GetCurrentDirectory(), "Images\\" + selectedGame.Platform);
                    if (mo0.List.Movieposter != null)
                    {
                        foreach (var poster in mo0.List.Movieposter)
                        {
                            string path = gamejoin + "\\Box - Front\\" + selectedGame.Title + "-01.jpg";

                            if (File.Exists(path) == false)
                            {
                                using (WebClient client = new WebClient())

                                {

                                    client.DownloadFile(new Uri(poster.Url), path);

                                }
                            }
                        }


                    }
                    if (mo0.List.Moviebanner != null)
                    {
                        foreach (var poster in mo0.List.Moviebanner)
                        {
                            string path = gamejoin + "\\Banner\\" + selectedGame.Title + "-01.jpg";

                            if (File.Exists(path) == false)
                            {
                                using (WebClient client = new WebClient())

                                {

                                    client.DownloadFile(new Uri(poster.Url), path);

                                }
                            }
                        }

                    }

                    if (mo0.List.Movielogo != null)
                    {
                        foreach (var poster in mo0.List.Movielogo)
                        {
                            string path = gamejoin + "\\Clear Logo\\" + selectedGame.Title + "-01.jpg";

                            if (File.Exists(path) == false)
                            {
                                using (WebClient client = new WebClient())

                                {

                                    client.DownloadFile(new Uri(poster.Url), path);

                                }
                            }
                        }
                    }

                    if (mo0.List.Hdmovielogo != null)
                    {
                        foreach (var poster in mo0.List.Hdmovielogo)
                        {
                            string path = gamejoin + "\\Clear Logo\\" + selectedGame.Title + "-01.jpg";

                            if (File.Exists(path) == false)
                            {
                                using (WebClient client = new WebClient())

                                {

                                    client.DownloadFile(new Uri(poster.Url), path);

                                }
                            }
                        }
                    }

                    if (mo0.List.Moviebackground != null)
                    {
                        foreach (var poster in mo0.List.Moviebackground)
                        {
                            string path = gamejoin + "\\Fanart - Background\\" + selectedGame.Title + "-01.jpg";

                            if (File.Exists(path) == false)
                            {
                                using (WebClient client = new WebClient())

                                {

                                    client.DownloadFile(new Uri(poster.Url), path);

                                }
                            }
                        }
                    }

                    if (data.Plot != null)
                    {
                        selectedGame.Notes = data.Plot;

                    }

                    if (data.Year != null)
                    {
                        selectedGame.ReleaseYear = data.Year;

                    }

                    if (data.Year != null)
                    {
                        selectedGame.ReleaseYear = data.Year;

                    }

                    if (data.Production != null)
                    {
                        selectedGame.Publisher = data.Production;
                    }

                }

            }
            PluginHelper.DataManager.Save();
        }
    }
}
