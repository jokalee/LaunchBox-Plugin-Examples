using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbroken.LaunchBox.Plugins.Data;
using Unbroken.LaunchBox.Plugins;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net;

namespace Import_TV_Shows
{
    public class Class1 : ISystemMenuItemPlugin
    {
        public static string imdbid { get; set; }
        public static string moviedbid { get; set; }
        public static string plot { get; set; }
        public static string movietitle { get; set; }
        public static string movieyear { get; set; }
        public static string path { get; set; }
        public static string movieposter { get; set; }
        public static string moviebackground { get; set; }
        public string Caption
        {
            get
            {
                //sets the name
                return "Import Tv Shows";
            }
        }



        public System.Drawing.Image IconImage => Resource1.TV_Shows;
        

        public bool ShowInLaunchBox
        {
            get
            {
                return true;
            }
        }

        public bool ShowInBigBox
        {
            get
            {
                return false;
            }
        }

        public bool AllowInBigBoxWhenLocked
        {
            get
            {
                return false;
            }
        }

        public string CleanerSystemMenuItems_ParentMenuItem => "importToolStripMenuItem";

        public void OnSelected()
        {

            try
            {
                Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.AddNewPlatform("TV Shows");
            }
            catch
            {
               //already exists
            }
            try
            {
                var emulators = Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.GetAllEmulators().Where(s => s.Title == "Tv Show Emulator");
                if (emulators == null)
                {
                    var emulator = Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.AddNewEmulator();
                    var Platforms = Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.GetPlatformByName("TV Shows");

                    emulator.Title = "Tv Show Emulator";
                    emulator.ApplicationPath = @"VLC\x64\vlc.exe";
                    emulator.DefaultPlatform = Platforms.Name;
                    emulator.CommandLine = "-Idummy --fullscreen";
                }

                
            }
            catch
            {
                //emulator exists
            }
             
            var Platform = Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.GetPlatformByName("TV Shows");
            
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    path = fbd.SelectedPath;
                }
            }
            var TvShows = Directory.GetDirectories(path);
            foreach(var tvshow in TvShows)
            {

                var SeriesName = new DirectoryInfo(System.IO.Path.GetDirectoryName(tvshow + "\\fix.file")).Name;
                
                var Series = Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.AddNewGame(SeriesName);
                Series.Platform = Platform.Name;
                Series.Title = SeriesName;
                FanartTv.API.Key = "f4fe7cd51f40b169f1cc9ef1786dc8a2";
                // Sets client api key i left this blank in compiled version aswell
                FanartTv.API.cKey = "a2a8eaf1f3b0107643b4ad9d126f6629";
                string query = FanartTv.Helper.Json.GetJson("https://api.themoviedb.org/3/search/tv?api_key=6caeea089cc15cefb0ecb71d257b8c86&query=" + SeriesName);
                dynamic imdbsearch = JObject.Parse(query);
                string results = imdbsearch.results.ToString();
                string strip = results.TrimStart('[');
                var matches = Regex.Matches(strip, @"{(.*?)}", RegexOptions.Singleline);
                if (matches.Count > 1)
                {
                    Tv_Select dlg = new Tv_Select(strip, SeriesName);
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

                        moviedbid = result.id;

                        string movieinfo = FanartTv.Helper.Json.GetJson("https://api.themoviedb.org/3/tv/" + moviedbid + "?api_key=6caeea089cc15cefb0ecb71d257b8c86&language=en-US");
                        dynamic imdbsearch2 = JObject.Parse(movieinfo);

                       


                        plot = imdbsearch2.overview.ToString();
                        movietitle = imdbsearch2.original_name.ToString();


                    }
                }
                if (plot != null)
                {
                    Series.Notes = plot;
                }
                if (movietitle != null)
                {
                    Series.Title = movietitle;
                }



                string altid = FanartTv.Helper.Json.GetJson("https://api.themoviedb.org/3/tv/" + moviedbid + "/external_ids?api_key=6caeea089cc15cefb0ecb71d257b8c86&language=en-US");
                dynamic idsearch = JObject.Parse(altid);
                var gamejoin = Path.Combine(Directory.GetCurrentDirectory(), "Images\\TV Shows");


                if (moviebackground != null)
                {

                    if (File.Exists(gamejoin + "\\Fanart - Background\\" + SeriesName + "-01.jpg") == false)
                    {
                        using (WebClient client = new WebClient())

                        {
                            Directory.CreateDirectory(gamejoin + "\\Fanart - Background");
                            client.DownloadFile(new Uri("https://image.tmdb.org/t/p/w500" + moviebackground), gamejoin + "\\Fanart - Background\\" + SeriesName + "-01.jpg");

                        }
                    }
                }

                if (movieposter != null)
                {

                    if (File.Exists(gamejoin + "\\Box - Front\\" + SeriesName + "-01.jpg") == false)
                    {
                        using (WebClient client = new WebClient())

                        {
                            Directory.CreateDirectory(gamejoin + "\\Box - Front");
                            client.DownloadFile(new Uri("https://image.tmdb.org/t/p/w500" + movieposter), gamejoin + "\\Box - Front\\" + SeriesName + "-01.jpg");

                        }
                    }
                }
                if (idsearch.tvdb_id != null)
                {
                    string result = idsearch.tvdb_id;
                    var mo0 = new FanartTv.TV.Show(result);

                    if (mo0.List.Clearart != null)
                    {
                        foreach (var poster in mo0.List.Clearart)
                        {
                            if (File.Exists(gamejoin + "\\Clear Logo\\" + SeriesName + "-01.png") == false)
                            {
                                using (WebClient client = new WebClient())

                                {
                                    Directory.CreateDirectory(gamejoin + "\\Clear Logo");
                                    client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Clear Logo\\" + SeriesName + "-01.png");

                                }
                            }


                        }
                    }

                    if (mo0.List.Hdclearart != null)
                    {
                        foreach (var poster in mo0.List.Hdclearart)
                        {
                            if (File.Exists(gamejoin + "\\Clear Logo\\" + SeriesName + "-01.png") == false)
                            {
                                using (WebClient client = new WebClient())

                                {
                                    Directory.CreateDirectory(gamejoin + "\\Clear Logo");
                                    client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Clear Logo\\" + SeriesName + "-01.png");

                                }
                            }


                        }
                    }

                    if (mo0.List.Tvposter != null)
                    {
                        foreach (var poster in mo0.List.Tvposter)
                        {
                            if (File.Exists(gamejoin + "\\Box - Front\\" + SeriesName + "-01.jpg") == false)
                            {
                                using (WebClient client = new WebClient())

                                {
                                    Directory.CreateDirectory(gamejoin + "\\Box - Front");
                                    client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Box - Front\\" + SeriesName + "-01.jpg");

                                }
                            }


                        }
                    }

                    if (mo0.List.Tvbanner != null)
                    {
                        foreach (var poster in mo0.List.Tvbanner)
                        {
                            if (File.Exists(gamejoin + "\\Banner\\" + SeriesName + "-01.jpg") == false)
                            {
                                using (WebClient client = new WebClient())

                                {
                                    Directory.CreateDirectory(gamejoin + "\\Banner");
                                    client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Banner\\" + SeriesName + "-01.jpg");

                                }
                            }


                        }
                    }

                    if (mo0.List.Showbackground != null)
                    {
                        foreach (var poster in mo0.List.Showbackground)
                        {
                            if (File.Exists(gamejoin + "\\Fanart - Background\\" + SeriesName + "-01.jpg") == false)
                            {
                                using (WebClient client = new WebClient())

                                {
                                    Directory.CreateDirectory(gamejoin + "\\Fanart - Background");
                                    client.DownloadFile(new Uri(poster.Url), gamejoin + "\\Fanart - Background\\" + SeriesName + "-01.jpg");

                                }
                            }


                        }
                    }

                   

                }
                var ext = new List<string> { "mkv", "mp4", "avi" };
                List<string> Episodes = Directory.GetFiles(tvshow, "*.*", SearchOption.AllDirectories).Where(file => new string[] { ".mkv", ".mp4", ".avi" }
      .Contains(Path.GetExtension(file)))
      .ToList(); ;
                foreach (var episode in Episodes)
                {
                    
                    Series.ApplicationPath = episode;
                    var epinumber = Series.AddNewAdditionalApplication();
                    epinumber.ApplicationPath = episode;
                    epinumber.Version = Path.GetFileNameWithoutExtension(episode);
                    epinumber.Name = Path.GetFileNameWithoutExtension(episode);

                }
                plot = null;
                imdbid = null;
                movietitle = null;
                movieposter = null;
                moviebackground = null;

            }


            MessageBox.Show("Import Complete");
            Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.Save();
            
        }
    }
}
