using System;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Unbroken.LaunchBox.Plugins;
using Unbroken;
using Unbroken.LaunchBox.Plugins.Data;

namespace Movie_Scrapper
{

    public class Class1 : IGameMenuItemPlugin
    {
        public static string imdbid { get; set; }
        public static string moviedbid { get; set; }
        public static string plot { get; set; }
        public static string movietitle { get; set; }
        public static string movieyear { get; set; }
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
                return Resource1.koala;
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
            imdbid = null;
            FanartTv.API.Key = "f4fe7cd51f40b169f1cc9ef1786dc8a2";
            // Sets client api key i left this blank in compiled version aswell
            FanartTv.API.cKey = "a2a8eaf1f3b0107643b4ad9d126f6629";
            string MoviePath = selectedGame.ApplicationPath;
            string MovieName = selectedGame.Title;
            var gamejoin = Path.Combine(Directory.GetCurrentDirectory(), "Images\\" + selectedGame.Platform);
            string MovieTrimmed = MoviePath.Remove(MoviePath.Length - 4);
            if (File.Exists(MovieTrimmed + ".nfo") == true)
            {
                try
                {
                    string[] nfo = System.IO.File.ReadAllLines(MovieTrimmed + ".nfo");
                    foreach (string line in nfo)
                    {
                        if (line.Contains("<id>"))
                        {
                            int pFrom = line.IndexOf("<id>") + "<id>".Length;
                            int pTo = line.LastIndexOf("</id>");

                            imdbid = line.Substring(pFrom, pTo - pFrom);

                        }

                    }
                }
                catch
                {
                    //could not find imdb listing
                }


            }

            if (File.Exists(MovieTrimmed + ".nfo") == true)
            {
                try
                {
                    string[] nfo = System.IO.File.ReadAllLines(MovieTrimmed + ".nfo");
                    foreach (string line in nfo)
                    {
                        if (line.Contains("<studio>"))
                        {

                            int pFrom = line.IndexOf("<studio>") + "<studio>".Length;
                            int pTo = line.LastIndexOf("</studio>");

                            String plot = line.Substring(pFrom, pTo - pFrom);
                            selectedGame.Publisher = plot;
                        }

                    }
                }
                catch
                {
                    //did unable to read studio from nfo
                }

            }

            if (File.Exists(MovieTrimmed + ".nfo") == true)
            {
                try
                {
                    string[] nfo = System.IO.File.ReadAllLines(MovieTrimmed + ".nfo");
                    foreach (string line in nfo)
                    {
                        if (line.Contains("<title>"))
                        {

                            int pFrom = line.IndexOf("<title>") + "<title>".Length;
                            int pTo = line.LastIndexOf("</title>");

                            String plot = line.Substring(pFrom, pTo - pFrom);
                            selectedGame.Title = plot;
                        }

                    }
                }
                catch
                {
                    //did unable to read movie title from nfo
                }

            }

            if (File.Exists(MovieTrimmed + ".nfo") == true)
            {
                try
                {
                    string[] nfo = System.IO.File.ReadAllLines(MovieTrimmed + ".nfo");
                    foreach (string line in nfo)
                    {
                        if (line.Contains("<year>"))
                        {
                            int year = 0;
                            int pFrom = line.IndexOf("<year>") + "<year>".Length;
                            int pTo = line.LastIndexOf("</year>");

                            String plot = line.Substring(pFrom, pTo - pFrom);
                            int.TryParse(plot, out year);
                            selectedGame.ReleaseYear = year;
                        }

                    }
                }
                catch
                {
                    //cannot read year from nfo
                }
            }
            if (File.Exists(MovieTrimmed + ".nfo") == true)
            {
                try
                {
                    string[] nfo = System.IO.File.ReadAllLines(MovieTrimmed + ".nfo");
                    foreach (string line in nfo)
                    {
                        if (line.Contains("<plot>"))
                        {

                            int pFrom = line.IndexOf("<plot>") + "<plot>".Length;
                            int pTo = line.LastIndexOf("</plot>");

                            String plot = line.Substring(pFrom, pTo - pFrom);
                            selectedGame.Notes = plot;
                        }

                    }
                }
                catch
                {
                    //unable to read plot from nfo
                }
            }

            if (File.Exists(gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg") == false)
            {
                if (File.Exists(MovieTrimmed + "-poster.jpg") == true)
                {
                    try
                    {
                        Directory.CreateDirectory(gamejoin + "\\Box - Front");
                        File.Copy(MovieTrimmed + "-poster.jpg", gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg");
                    }
                    catch
                    {
                        //could not copy file
                    }

                }
            }
            
            if (File.Exists(gamejoin + "\\Banner\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg") == false)
            {
                
                if (File.Exists(MovieTrimmed + "-landscape.jpg") == true)
                {
                    try
                    {
                        Directory.CreateDirectory(gamejoin + "\\Banner");
                        File.Copy(MovieTrimmed + "-landscape.jpg", gamejoin + "\\Banner\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg");
                    }
                    catch
                    {
                        //could not copy file
                    }

                }
            }

            if (File.Exists(gamejoin + "\\Clear Logo\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.png") == false)
            {
                if (File.Exists(MovieTrimmed + "-clearlogo.png") == true)
                {
                    try
                    {
                        Directory.CreateDirectory(gamejoin + "\\Clear Logo");
                        File.Copy(MovieTrimmed + "-clearlogo.png", gamejoin + "\\Clear Logo\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.png");
                    }
                    catch
                    {
                        //could not copy file
                    }

                }
            }



            if (imdbid == null)
            {
                string query = FanartTv.Helper.Json.GetJson("https://api.themoviedb.org/3/search/movie?api_key=6caeea089cc15cefb0ecb71d257b8c86&query=" + selectedGame.Title);
                dynamic imdbsearch = JObject.Parse(query);
                string results = imdbsearch.results.ToString();
                string strip = results.TrimStart('[');

                var matches = Regex.Matches(strip, @"{(.*?)}", RegexOptions.Singleline);
                if (matches.Count > 1)
                {
                    MovieSelect dlg = new MovieSelect(strip);
                    dlg.ShowDialog();

                }
                else if(matches.Count < 1)

                {
                    MessageBox.Show("Cannot Find the movies id");
                }
                else
                {
                    foreach (var match in matches)
                    {
                        dynamic result = JObject.Parse(match.ToString());

                        imdbid = result.id;

                        string movieinfo = FanartTv.Helper.Json.GetJson("https://api.themoviedb.org/3/movie/" + imdbid + "?api_key=6caeea089cc15cefb0ecb71d257b8c86&language=en-US");
                        dynamic imdbsearch2 = JObject.Parse(movieinfo);

                        imdbid = imdbsearch2.imdb_id.ToString();
                        moviedbid = imdbsearch2.id.ToString();
                        if (imdbsearch2.overview != null) { }
                        plot = imdbsearch2.overview.ToString();
                        movietitle = imdbsearch2.original_title.ToString();
                        movieyear = imdbsearch2.release_date.ToString();

                    }
                }
            }

            if (movietitle != null)
            {
                selectedGame.Title = movietitle;
            }
            if (plot != null)
            {
                selectedGame.Notes = plot;
            }
           


            if (imdbid != null)
            {
                string result = imdbid;
                var mo0 = new FanartTv.Movies.Movie(result);

                if (mo0.List.Movielogo != null)
                {
                    foreach (var poster in mo0.List.Movielogo)
                    {
                        if (File.Exists(gamejoin + "\\Clear Logo\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.png") == false)
                        {
                            using (WebClient client = new WebClient())

                            {
                                Directory.CreateDirectory(gamejoin + "\\Clear Logo");
                                client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Clear Logo\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.png");

                            }
                        }


                    }
                }

                if (mo0.List.Moviebanner != null)
                {
                    foreach (var poster in mo0.List.Moviebanner)
                    {
                        if (File.Exists(gamejoin + "\\Banner\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg") == false)
                        {
                            using (WebClient client = new WebClient())

                            {
                                Directory.CreateDirectory(gamejoin + "\\Banner");
                                client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Banner\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg");

                            }
                        }


                    }
                }

                if (mo0.List.Moviebackground != null)
                {
                    foreach (var poster in mo0.List.Moviebackground)
                    {
                        if (File.Exists(gamejoin + "\\Fanart - Background\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg") == false)
                        {
                            using (WebClient client = new WebClient())

                            {
                                Directory.CreateDirectory(gamejoin + "\\Fanart - Background");
                                client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Fanart - Background\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg");

                            }
                        }


                    }
                }

                if (mo0.List.Movieposter != null)
                {
                    foreach (var poster in mo0.List.Movieposter)
                    {
                        if (File.Exists(gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg") == false)
                        {
                            using (WebClient client = new WebClient())

                            {
                                Directory.CreateDirectory(gamejoin + "\\Box - Front");
                                client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg");

                            }

                        }



                    }
                }
            }

                //save changes
                PluginHelper.DataManager.Save();
            MessageBox.Show("Scrape Complete Remember to refresh imagaes with f5");
        }

        public void OnSelected(IGame[] selectedGames)
        {


            foreach (var selectedGame in selectedGames)
            {
                imdbid = null;
                FanartTv.API.Key = "f4fe7cd51f40b169f1cc9ef1786dc8a2";
                // Sets client api key i left this blank in compiled version aswell
                FanartTv.API.cKey = "a2a8eaf1f3b0107643b4ad9d126f6629";
                string MoviePath = selectedGame.ApplicationPath;
                string MovieName = selectedGame.Title;
                var gamejoin = Path.Combine(Directory.GetCurrentDirectory(), "Images\\" + selectedGame.Platform);
                string MovieTrimmed = MoviePath.Remove(MoviePath.Length - 4);
                if (File.Exists(MovieTrimmed + ".nfo") == true)
                {
                    try
                    {
                        string[] nfo = System.IO.File.ReadAllLines(MovieTrimmed + ".nfo");
                        foreach (string line in nfo)
                        {
                            if (line.Contains("<id>"))
                            {
                                int pFrom = line.IndexOf("<id>") + "<id>".Length;
                                int pTo = line.LastIndexOf("</id>");

                                imdbid = line.Substring(pFrom, pTo - pFrom);

                            }

                        }
                    }
                    catch
                    {
                        //could not find imdb listing
                    }


                }

                if (File.Exists(MovieTrimmed + ".nfo") == true)
                {
                    try
                    {
                        string[] nfo = System.IO.File.ReadAllLines(MovieTrimmed + ".nfo");
                        foreach (string line in nfo)
                        {
                            if (line.Contains("<studio>"))
                            {

                                int pFrom = line.IndexOf("<studio>") + "<studio>".Length;
                                int pTo = line.LastIndexOf("</studio>");

                                String plot = line.Substring(pFrom, pTo - pFrom);
                                selectedGame.Publisher = plot;
                            }

                        }
                    }
                    catch
                    {
                        //did unable to read studio from nfo
                    }

                }

                if (File.Exists(MovieTrimmed + ".nfo") == true)
                {
                    try
                    {
                        string[] nfo = System.IO.File.ReadAllLines(MovieTrimmed + ".nfo");
                        foreach (string line in nfo)
                        {
                            if (line.Contains("<title>"))
                            {

                                int pFrom = line.IndexOf("<title>") + "<title>".Length;
                                int pTo = line.LastIndexOf("</title>");

                                String plot = line.Substring(pFrom, pTo - pFrom);
                                selectedGame.Title = plot;
                            }

                        }
                    }
                    catch
                    {
                        //did unable to read movie title from nfo
                    }

                }

                if (File.Exists(MovieTrimmed + ".nfo") == true)
                {
                    try
                    {
                        string[] nfo = System.IO.File.ReadAllLines(MovieTrimmed + ".nfo");
                        foreach (string line in nfo)
                        {
                            if (line.Contains("<year>"))
                            {
                                int year = 0;
                                int pFrom = line.IndexOf("<year>") + "<year>".Length;
                                int pTo = line.LastIndexOf("</year>");

                                String plot = line.Substring(pFrom, pTo - pFrom);
                                int.TryParse(plot, out year);
                                selectedGame.ReleaseYear = year;
                            }

                        }
                    }
                    catch
                    {
                        //cannot read year from nfo
                    }
                }
                if (File.Exists(MovieTrimmed + ".nfo") == true)
                {
                    try
                    {
                        string[] nfo = System.IO.File.ReadAllLines(MovieTrimmed + ".nfo");
                        foreach (string line in nfo)
                        {
                            if (line.Contains("<plot>"))
                            {

                                int pFrom = line.IndexOf("<plot>") + "<plot>".Length;
                                int pTo = line.LastIndexOf("</plot>");

                                String plot = line.Substring(pFrom, pTo - pFrom);
                                selectedGame.Notes = plot;
                            }

                        }
                    }
                    catch
                    {
                        //unable to read plot from nfo
                    }
                }

                if (File.Exists(gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg") == false)
                {
                    if (File.Exists(MovieTrimmed + "-poster.jpg") == true)
                    {
                        try

                        {
                            Directory.CreateDirectory(gamejoin + "\\Box - Front");
                            File.Copy(MovieTrimmed + "-poster.jpg", gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg");
                        }
                        catch
                        {
                            //could not copy file
                        }

                    }
                }

                if (File.Exists(gamejoin + "\\Banner\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg") == false)
                {

                    if (File.Exists(MovieTrimmed + "-landscape.jpg") == true)
                    {
                        try
                        {
                            Directory.CreateDirectory(gamejoin + "\\Banner");
                            File.Copy(MovieTrimmed + "-landscape.jpg", gamejoin + "\\Banner\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg");
                        }
                        catch
                        {
                            //could not copy file
                        }

                    }
                }

                if (File.Exists(gamejoin + "\\Clear Logo\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.png") == false)
                {
                    if (File.Exists(MovieTrimmed + "-clearlogo.png") == true)
                    {
                        try
                        {
                            Directory.CreateDirectory(gamejoin + "\\Clear Logo");
                            File.Copy(MovieTrimmed + "-clearlogo.png", gamejoin + "\\Clear Logo\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.png");
                        }
                        catch
                        {
                            //could not copy file
                        }

                    }
                }



                if (imdbid == null)
                {
                    string query = FanartTv.Helper.Json.GetJson("https://api.themoviedb.org/3/search/movie?api_key=6caeea089cc15cefb0ecb71d257b8c86&query=" + selectedGame.Title);
                    dynamic imdbsearch = JObject.Parse(query);
                    string results = imdbsearch.results.ToString();
                    string strip = results.TrimStart('[');

                    var matches = Regex.Matches(strip, @"{(.*?)}", RegexOptions.Singleline);
                    if (matches.Count > 1)
                    {
                        MovieSelect dlg = new MovieSelect(strip);
                        dlg.ShowDialog();

                    }
                    else if (matches.Count < 1)

                    {
                        MessageBox.Show("Cannot Find the movies id");
                    }
                    else
                    {
                        foreach (var match in matches)
                        {
                            dynamic result = JObject.Parse(match.ToString());

                            imdbid = result.id;

                            string movieinfo = FanartTv.Helper.Json.GetJson("https://api.themoviedb.org/3/movie/" + imdbid + "?api_key=6caeea089cc15cefb0ecb71d257b8c86&language=en-US");
                            dynamic imdbsearch2 = JObject.Parse(movieinfo);

                            imdbid = imdbsearch2.imdb_id.ToString();
                            moviedbid = imdbsearch2.id.ToString();
                            if (imdbsearch2.overview != null) { }
                            plot = imdbsearch2.overview.ToString();
                            movietitle = imdbsearch2.original_title.ToString();
                            movieyear = imdbsearch2.release_date.ToString();

                        }
                    }
                }

                if (movietitle != null)
                {
                    selectedGame.Title = movietitle;
                }
                if (plot != null)
                {
                    selectedGame.Notes = plot;
                }



                if (imdbid != null)
                {
                    string result = imdbid;
                    var mo0 = new FanartTv.Movies.Movie(result);

                    if (mo0.List.Movielogo != null)
                    {
                        foreach (var poster in mo0.List.Movielogo)
                        {
                            if (File.Exists(gamejoin + "\\Clear Logo\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.png") == false)
                            {
                                using (WebClient client = new WebClient())

                                {
                                    Directory.CreateDirectory(gamejoin + "\\Clear Logo");
                                    client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Clear Logo\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.png");

                                }
                            }


                        }
                    }

                    if (mo0.List.Moviebanner != null)
                    {
                        foreach (var poster in mo0.List.Moviebanner)
                        {
                            if (File.Exists(gamejoin + "\\Banner\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg") == false)
                            {
                                using (WebClient client = new WebClient())

                                {
                                    Directory.CreateDirectory(gamejoin + "\\Banner");
                                    client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Banner\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg");

                                }
                            }


                        }
                    }

                    if (mo0.List.Moviebackground != null)
                    {
                        foreach (var poster in mo0.List.Moviebackground)
                        {
                            if (File.Exists(gamejoin + "\\Fanart - Background\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg") == false)
                            {
                                using (WebClient client = new WebClient())

                                {
                                    Directory.CreateDirectory(gamejoin + "\\Fanart - Background");
                                    client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Fanart - Background\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg");

                                }
                            }


                        }
                    }

                    if (mo0.List.Movieposter != null)
                    {
                        foreach (var poster in mo0.List.Movieposter)
                        {
                            if (File.Exists(gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg") == false)
                            {
                                using (WebClient client = new WebClient())

                                {
                                    Directory.CreateDirectory(gamejoin + "\\Box - Front");
                                    client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.GetFileNameWithoutExtensionSafe(selectedGame.Title) + "-01.jpg");

                                }

                            }



                        }
                    }
                }

            }



        PluginHelper.DataManager.Save();
            MessageBox.Show("Scrape Complete Remember to refresh imagaes with f5");
        }
    }
}
