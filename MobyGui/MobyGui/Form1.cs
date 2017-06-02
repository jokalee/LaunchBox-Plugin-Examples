using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace MobyGui
{
    public partial class Form1 : Form, IGameMenuItemPlugin
    {
        public static string platformid { get; set; }
        public static string gameid { get; set; }
        public static int gamesdownloaded { get; set; }
        public static int gamesnotdownloaded { get; set; }
        public static string notes { get; set; }
        public static string gamename { get; set; }
        public static string platform { get; set; }
        public static string apikey { get; set; }
        public static string gamejoin { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        bool IGameMenuItemPlugin.SupportsMultipleGames => true;

        string IGameMenuItemPlugin.Caption => "Scape With MobyGames";

        System.Drawing.Image IGameMenuItemPlugin.IconImage => Resource1.mobygames;

        bool IGameMenuItemPlugin.ShowInLaunchBox => true;

        bool IGameMenuItemPlugin.ShowInBigBox => false;

        bool IGameMenuItemPlugin.GetIsValidForGame(IGame selectedGame)
        {
            return true;
        }

        bool IGameMenuItemPlugin.GetIsValidForGames(IGame[] selectedGames)
        {
            return true;
        }


        public void OnSelected(IGame selectedGame)
        {
            gameid = null;
            platformid = null;
            gamesdownloaded = 0;
            gamesdownloaded = 0;
            this.Show();
            
            gamename = selectedGame.Title;
            platform = selectedGame.Platform;
            apikey = Settings1.Default.ApiKey;
            gamejoin = Path.Combine(Directory.GetCurrentDirectory(), "Images\\" + platform);
            label1.Text = selectedGame.Title;

           
            string query = FanartTv.Helper.Json.GetJson("https://api.mobygames.com/v1/platforms?api_key=" + apikey);
            if (query == "")
            {
                //
            }
            else
            {
                dynamic result = JObject.Parse(query);
                foreach (var property in result.platforms)
                {
                    if (property.platform_name == platform)
                    {
                        platformid = property.platform_id;
                        Thread.Sleep(Settings1.Default.WaitBetweenApiCalls * 1000);
                        string gamequery = FanartTv.Helper.Json.GetJson("https://api.mobygames.com/v1/games?title=" + gamename + "&platform=" + platformid + "&api_key=" + apikey);
                        if (gamequery != "")
                        {
                            dynamic games = JObject.Parse(gamequery);
                            foreach (var game in games.games)
                            {
                                gameid = game.game_id;
                                if (selectedGame.Notes == "")
                                {
                                    selectedGame.Notes = game.description;
                                }
                            }
                            Thread.Sleep(Settings1.Default.WaitBetweenApiCalls * 1000);
                            string screenquery = FanartTv.Helper.Json.GetJson("https://api.mobygames.com/v1/games/" + gameid + "/platforms/" + platformid + "/screenshots?api_key=" + apikey);
                            if (screenquery != "")
                            {
                                dynamic screenshots = JObject.Parse(screenquery);
                                foreach (var screenshot in screenshots.screenshots)
                                {
                                    if (screenshot.caption == "Title image")
                                    {
                                        if (File.Exists(gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg") == false)
                                        {
                                            label2.Text = "Downloading..";
                                            using (WebClient client = new WebClient())

                                            {
                                                Directory.CreateDirectory(gamejoin + "\\Screenshot - Game Title");
                                                client.DownloadFile(new Uri(screenshot.image.ToString()), gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg");
                                                gamesdownloaded = gamesdownloaded + 1;
                                            }
                                            label2.Text = "Complete";
                                        }
                                        else
                                        {
                                            gamesnotdownloaded = gamesnotdownloaded + 1;
                                        }

                                    }
                                    else
                                    if (screenshot.caption == "Title screen")
                                    {
                                        if (File.Exists(gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg") == false)
                                        {
                                            label2.Text = "Downloading..";
                                            using (WebClient client = new WebClient())

                                            {
                                                Directory.CreateDirectory(gamejoin + "\\Screenshot - Game Title");
                                                client.DownloadFile(new Uri(screenshot.image.ToString()), gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg");
                                                gamesdownloaded = gamesdownloaded + 1;
                                            }
                                            label2.Text = "Complete";
                                        }
                                        else
                                        {
                                            gamesnotdownloaded = gamesnotdownloaded + 1;
                                        }

                                    }
                                    else
                                    {
                                        if (File.Exists(gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg") == false)
                                        {
                                            label2.Text = "Downloading..";
                                            using (WebClient client = new WebClient())

                                            {
                                                Directory.CreateDirectory(gamejoin + "\\Screenshot - Gameplay");
                                                client.DownloadFile(new Uri(screenshot.image.ToString()), gamejoin + "\\Screenshot - Gameplay\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg");
                                                gamesdownloaded = gamesdownloaded + 1;
                                            }
                                            label2.Text = "Complete";
                                        }
                                        else
                                        {
                                            gamesnotdownloaded = gamesnotdownloaded + 1;
                                        }

                                    }
                                }
                            }
                            Thread.Sleep(Settings1.Default.WaitBetweenApiCalls * 1000);
                            string coversquery = FanartTv.Helper.Json.GetJson("https://api.mobygames.com/v1/games/" + gameid + "/platforms/" + platformid + "/covers?api_key=" + apikey);
                            if (coversquery != "")
                            {
                                dynamic covers = JObject.Parse(coversquery);
                                foreach (var cover in covers.cover_groups)
                                {
                                    foreach (var coverreg in cover.covers)
                                    {
                                        if (coverreg.scan_of == "Front Cover")
                                        {
                                            if (File.Exists(gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg") == false)
                                            {
                                                label2.Text = "Downloading..";
                                                using (WebClient client = new WebClient())

                                                {
                                                    Directory.CreateDirectory(gamejoin + "\\Box - Front");
                                                    client.DownloadFile(new Uri(coverreg.image.ToString()), gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg");
                                                    gamesdownloaded = gamesdownloaded + 1;
                                                }
                                                label2.Text = "Complete";
                                            }

                                        }
                                        else
                                        {
                                            gamesnotdownloaded = gamesnotdownloaded + 1;
                                        }
                                        if (coverreg.scan_of == "Box - Back")
                                        {
                                            if (File.Exists(gamejoin + "\\Box - Back\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg") == false)
                                            {
                                                label2.Text = "Downloading..";
                                                using (WebClient client = new WebClient())

                                                {
                                                    Directory.CreateDirectory(gamejoin + "\\Box - Back");
                                                    client.DownloadFile(new Uri(coverreg.image.ToString()), gamejoin + "\\Box - Back\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg");
                                                    gamesdownloaded = gamesdownloaded + 1;
                                                }
                                                label2.Text = "Complete";
                                            }

                                        }
                                        else
                                        {
                                            gamesnotdownloaded = gamesnotdownloaded + 1;
                                        }
                                        if (coverreg.scan_of == "Media")
                                        {
                                            if (File.Exists(gamejoin + "\\Cart - Front\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg") == false)
                                            {
                                                label2.Text = "Downloading..";
                                                using (WebClient client = new WebClient())

                                                {
                                                    Directory.CreateDirectory(gamejoin + "\\Cart - Front");
                                                    client.DownloadFile(new Uri(coverreg.image.ToString()), gamejoin + "\\Cart - Front\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg");
                                                    gamesdownloaded = gamesdownloaded + 1;
                                                }
                                                label2.Text = "Complete";
                                            }

                                        }
                                        else
                                        {
                                            gamesnotdownloaded = gamesnotdownloaded + 1;
                                        }
                                    }

                                }
                            }



                        }
                    }

                }
            }

            label3.Text = "Downloaded: " + gamesdownloaded.ToString();
            label4.Text = "Not Downloaded: " + gamesnotdownloaded.ToString();
            Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.Save();
           
        }

        public void OnSelected(IGame[] selectedGames)
        {
            gamesdownloaded = 0;
            gamesdownloaded = 0;
          
            this.Show();

            
            foreach (var selectedGame in selectedGames)
            {
                
                    gameid = null;
                    platformid = null;
                    gamename = selectedGame.Title;
                    platform = selectedGame.Platform;
                    apikey = Settings1.Default.ApiKey;
                    gamejoin = Path.Combine(Directory.GetCurrentDirectory(), "Images\\" + platform);

                    label1.Text = selectedGame.Title;
             
                string query = FanartTv.Helper.Json.GetJson("https://api.mobygames.com/v1/platforms?api_key=" + apikey);
                    if (query == "")
                    {
                        //
                    }
                    else
                    {
                        dynamic result = JObject.Parse(query);
                        foreach (var property in result.platforms)
                        {
                            if (property.platform_name == platform)
                            {
                                platformid = property.platform_id;
                            Thread.Sleep(Settings1.Default.WaitBetweenApiCalls * 1000);
                            string gamequery = FanartTv.Helper.Json.GetJson("https://api.mobygames.com/v1/games?title=" + gamename + "&platform=" + platformid + "&api_key=" + apikey);
                                if (gamequery != "")
                                {
                                    dynamic games = JObject.Parse(gamequery);
                                    foreach (var game in games.games)
                                    {
                                        gameid = game.game_id;
                                        if (selectedGame.Notes == "")
                                        {
                                            selectedGame.Notes = game.description;
                                        }
                                    }
                                Thread.Sleep(Settings1.Default.WaitBetweenApiCalls * 1000);
                                string screenquery = FanartTv.Helper.Json.GetJson("https://api.mobygames.com/v1/games/" + gameid + "/platforms/" + platformid + "/screenshots?api_key=" + apikey);
                                    if (screenquery != "")
                                    {
                                        dynamic screenshots = JObject.Parse(screenquery);
                                        foreach (var screenshot in screenshots.screenshots)
                                        {
                                            if (screenshot.caption == "Title image")
                                            {
                                                if (File.Exists(gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg") == false)
                                                {

                                                    label2.Text = "Downloading..";


                                                    using (WebClient client = new WebClient())

                                                    {
                                                        Directory.CreateDirectory(gamejoin + "\\Screenshot - Game Title");
                                                        client.DownloadFile(new Uri(screenshot.image.ToString()), gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg");
                                                        gamesdownloaded = gamesdownloaded + 1;
                                                    }

                                                    label2.Text = "Complete";


                                                }
                                                else
                                                {
                                                    gamesnotdownloaded = gamesnotdownloaded + 1;
                                                }

                                            }



                                            else
                                            if (screenshot.caption == "Title screen")
                                            {
                                                if (File.Exists(gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg") == false)
                                                {

                                                    label2.Text = "Downloading..";

                                                    using (WebClient client = new WebClient())

                                                    {
                                                        Directory.CreateDirectory(gamejoin + "\\Screenshot - Game Title");
                                                        client.DownloadFile(new Uri(screenshot.image.ToString()), gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg");
                                                        gamesdownloaded = gamesdownloaded + 1;
                                                    }

                                                    label2.Text = "Complete";

                                                }
                                                else
                                                {
                                                    gamesnotdownloaded = gamesnotdownloaded + 1;
                                                }

                                            }
                                            else
                                            {
                                                if (File.Exists(gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg") == false)
                                                {

                                                    label2.Text = "Downloading..";

                                                    using (WebClient client = new WebClient())

                                                    {
                                                        Directory.CreateDirectory(gamejoin + "\\Screenshot - Gameplay");
                                                        client.DownloadFile(new Uri(screenshot.image.ToString()), gamejoin + "\\Screenshot - Gameplay\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg");
                                                        gamesdownloaded = gamesdownloaded + 1;
                                                    }

                                                    label2.Text = "Complete";

                                                }
                                                else
                                                {
                                                    gamesnotdownloaded = gamesnotdownloaded + 1;
                                                }

                                            }
                                        }
                                    }
                                Thread.Sleep(Settings1.Default.WaitBetweenApiCalls * 1000);
                                    string coversquery = FanartTv.Helper.Json.GetJson("https://api.mobygames.com/v1/games/" + gameid + "/platforms/" + platformid + "/covers?api_key=" + apikey);
                                    if (coversquery != "")
                                    {
                                        dynamic covers = JObject.Parse(coversquery);
                                        foreach (var cover in covers.cover_groups)
                                        {
                                            foreach (var coverreg in cover.covers)
                                            {
                                                if (coverreg.scan_of == "Front Cover")
                                                {
                                                    if (File.Exists(gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg") == false)
                                                    {

                                                        label2.Text = "Downloading..";

                                                        using (WebClient client = new WebClient())

                                                        {
                                                            Directory.CreateDirectory(gamejoin + "\\Box - Front");
                                                            client.DownloadFile(new Uri(coverreg.image.ToString()), gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg");
                                                            gamesdownloaded = gamesdownloaded + 1;
                                                        }

                                                        label2.Text = "Complete";

                                                    }

                                                }
                                                else
                                                {
                                                    gamesnotdownloaded = gamesnotdownloaded + 1;
                                                }
                                                if (coverreg.scan_of == "Box - Back")
                                                {
                                                    if (File.Exists(gamejoin + "\\Box - Back\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg") == false)
                                                    {

                                                        label2.Text = "Downloading..";

                                                        using (WebClient client = new WebClient())

                                                        {
                                                            Directory.CreateDirectory(gamejoin + "\\Box - Back");
                                                            client.DownloadFile(new Uri(coverreg.image.ToString()), gamejoin + "\\Box - Back\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg");
                                                            gamesdownloaded = gamesdownloaded + 1;
                                                        }

                                                        label2.Text = "Complete";

                                                    }

                                                }
                                                else
                                                {
                                                    gamesnotdownloaded = gamesnotdownloaded + 1;
                                                }
                                                if (coverreg.scan_of == "Media")
                                                {
                                                    if (File.Exists(gamejoin + "\\Cart - Front\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg") == false)
                                                    {

                                                        label2.Text = "Downloading..";

                                                        using (WebClient client = new WebClient())

                                                        {
                                                            Directory.CreateDirectory(gamejoin + "\\Cart - Front");
                                                            client.DownloadFile(new Uri(coverreg.image.ToString()), gamejoin + "\\Cart - Front\\" + Unbroken.FileHelper.CoerceValidFileName(gamename) + "-01.jpg");
                                                            gamesdownloaded = gamesdownloaded + 1;
                                                        }

                                                        label2.Text = "Complete";

                                                    }

                                                }
                                                else
                                                {
                                                    gamesnotdownloaded = gamesnotdownloaded + 1;
                                                }
                                            }

                                        }
                                    }



                                }
                            }

                        }
                    }
                   
                    label1.Text = "Complete";
                    label3.Text = "Downloaded: " + gamesdownloaded.ToString();
                    label4.Text = "Not Downloaded: " + gamesnotdownloaded.ToString();
                Thread.Sleep(Settings1.Default.WaitBetweenMultiGames * 1000);
                
               
               
            }
            progressBar1.Value = 100;
            Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
