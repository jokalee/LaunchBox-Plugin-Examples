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

namespace MobyGamesScraper
{
    public partial class Form2 : Form,IGameMenuItemPlugin
    {
        public string platform { get; set; }
        public string platformid { get; set; }
        public string gamename { get; set; }
        public string apikey { get; set; }
        public string gamesdownloaded { get; set; }
        public string gamesnotdownloaded { get; set; }
        public string gameid { get; set; }
        public int progressvalue { get; set; }
        public string logdir { get; set; }
        public string Notes { get; set; }
        public string CartFrontImagePath { get; set; }
        public string ScreenshotImagePath { get; set; }
        public string FrontImagePath { get; set; }
        public string BackImagePath { get; set; }
        public string Title { get; set; }
       

        public Form2()
        {
            InitializeComponent();
        }

        public bool SupportsMultipleGames => false;

        public string Caption => "Scrape With MobyGames";

        public Image IconImage => Resource1.mobygames;

        public bool ShowInLaunchBox => true;

        public bool ShowInBigBox => false;

        public bool GetIsValidForGame(IGame selectedGame)
        {
            return true;
        }

        public bool GetIsValidForGames(IGame[] selectedGames)
        {
            return false;
        }

        public void OnSelected(IGame selectedGame)
        {
            gameid = null;
            gamename = null;
            platformid = null;
            Title = null;
            Notes = null;
            FrontImagePath = null;
            BackImagePath = null;
            ScreenshotImagePath = null;
            CartFrontImagePath = null;
            Title = null;
            label3.Text = "Awaiting Selection";
            button2.Text = "Exit";
            this.ControlBox = false;
            this.Show();
            apikey = Settings1.Default.ApiKey;
            textBox1.Text = selectedGame.Title;
            
            WebClient client = new WebClient();

            string query = client.DownloadString("https://api.mobygames.com/v1/platforms?api_key=" + apikey);


            if (query == "")
            {
                //failed to get platform list
            }
            else
            {
                dynamic result = JObject.Parse(query);
                foreach (var property in result.platforms)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = property.platform_name;
                    item.Value = property.platform_id;
                    comboBox1.Items.Add(item);

                    comboBox1.SelectedIndex = 0;
                }
            }
            
            Notes = selectedGame.Notes;
            FrontImagePath = selectedGame.FrontImagePath;
            BackImagePath = selectedGame.BackImagePath;
            ScreenshotImagePath = selectedGame.ScreenshotImagePath;
            CartFrontImagePath = selectedGame.CartFrontImagePath;
            Title = selectedGame.Title;
            platform = selectedGame.Platform;

        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        public void OnSelected(IGame[] selectedGames)
        {
            return;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {


            logdir = Path.Combine(Directory.GetCurrentDirectory(), "MobyGames.log");
            progressvalue = 0;
            backgroundWorker1.ReportProgress(0);
            try
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(logdir, true))
                {
                    file.WriteLine(Title);
                }
            }
            catch
            {
                //logfile is busy
            }
            if (Notes == "" || FrontImagePath == null || BackImagePath == null || ScreenshotImagePath == null || CartFrontImagePath == null)
            {
                Thread.Sleep(Settings1.Default.WaitBetweenApiCalls * 1000);
                WebClient client = new WebClient();
                string gamejoin = Path.Combine(Directory.GetCurrentDirectory(), "Images\\" + platform);
                string gamequery = client.DownloadString("https://api.mobygames.com/v1/games?title=" + gamename + "&platform=" + platformid + "&api_key=" + apikey);
                if (gamequery != "")
                {
                    dynamic games = JObject.Parse(gamequery);
                    foreach (var game in games.games)
                    {
                        gameid = game.game_id;
                        try
                        {
                            using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter(logdir, true))
                            {
                                file.WriteLine("GameID: " + gameid);
                            }

                            using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter(logdir, true))
                            {
                                file.WriteLine("PlatformID: " + platformid);
                            }
                        }
                        catch
                        {
                            //logfile busy
                        }
                        if (Notes == "")
                        {
                            Notes = game.description;
                        }
                    }
                    progressvalue = 15;
                    backgroundWorker1.ReportProgress(15);
                    if (gameid != null || platformid != null)
                    {


                        if (ScreenshotImagePath == null)
                        {
                            try
                            {
                                Thread.Sleep(Settings1.Default.WaitBetweenApiCalls * 1000);

                                string screenquery = client.DownloadString("https://api.mobygames.com/v1/games/" + gameid + "/platforms/" + platformid + "/screenshots?api_key=" + apikey);

                                dynamic screenshots = JObject.Parse(screenquery);
                                try
                                {
                                    using (System.IO.StreamWriter file =
                                    new System.IO.StreamWriter(logdir, true))
                                    {
                                        file.WriteLine("Found ScreenShots");
                                    }
                                }
                                catch
                                {
                                    //logfile busy
                                }
                                foreach (var screenshot in screenshots.screenshots)
                                {
                                    if (screenshot.caption == "Title image")
                                    {
                                        if (File.Exists(gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg") == false)
                                        {

                                            Directory.CreateDirectory(gamejoin + "\\Screenshot - Game Title");
                                            startDownload(screenshot.image.ToString(), gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg");
                                           
                                            try
                                            {
                                                using (System.IO.StreamWriter file =
                                                new System.IO.StreamWriter(logdir, true))
                                                {
                                                    file.WriteLine(gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg");
                                                }
                                            }
                                            catch
                                            {
                                                //log file busy
                                            }
                                        }
                                       

                                    }
                                    else
                                    if (screenshot.caption == "Title screen")
                                    {
                                        if (File.Exists(gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg") == false)
                                        {
                                            Directory.CreateDirectory(gamejoin + "\\Screenshot - Game Title");
                                            startDownload(screenshot.image.ToString(), gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg");
                                           
                                            try
                                            {
                                                using (System.IO.StreamWriter file =
                                                new System.IO.StreamWriter(logdir, true))
                                                {
                                                    file.WriteLine(gamejoin + "\\Screenshot - Game Title\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg");
                                                }
                                            }
                                            catch
                                            {
                                                //log file busy
                                            }
                                        }
                                        

                                    }
                                    else
                                    {
                                        if (File.Exists(gamejoin + "\\Screenshot - GamePlay\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg") == false)
                                        {
                                            Directory.CreateDirectory(gamejoin + "\\Screenshot - GamePlay");
                                            startDownload(screenshot.image.ToString(), gamejoin + "\\Screenshot - GamePlay\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg");
                                           
                                            try
                                            {
                                                using (System.IO.StreamWriter file =
                                                new System.IO.StreamWriter(logdir, true))
                                                {
                                                    file.WriteLine(gamejoin + "\\Screenshot - GamePlay\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg");
                                                }
                                            }
                                            catch
                                            {
                                                //log file busy
                                            }
                                        }
                                        

                                    }
                                }



                            }
                            catch
                            {
                                //cannot retrieve imagaes
                            }
                            progressvalue = 60;
                            backgroundWorker1.ReportProgress(60);
                            try
                            
                            {
                                Thread.Sleep(Settings1.Default.WaitBetweenApiCalls * 1000);

                                if (FrontImagePath == null || BackImagePath == null || CartFrontImagePath == null)
                                {

                                    string coversquery = client.DownloadString("https://api.mobygames.com/v1/games/" + gameid + "/platforms/" + platformid + "/covers?api_key=" + apikey);
                                    try
                                    {
                                        using (System.IO.StreamWriter file =
                                        new System.IO.StreamWriter(logdir, true))
                                        {
                                            file.WriteLine("Found Images");
                                        }
                                    }
                                    catch
                                    { 
                                        //logfile busy
                                    }


                                    dynamic covers = JObject.Parse(coversquery);
                                    foreach (var cover in covers.cover_groups)
                                    {
                                        foreach (var coverreg in cover.covers)
                                        {
                                            if (coverreg.scan_of == "Front Cover")
                                            {
                                                if (FrontImagePath == null)
                                                {
                                                    if (File.Exists(gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg") == false)
                                                    {

                                                        Directory.CreateDirectory(gamejoin + "\\Box - Front");
                                                        startDownload(coverreg.image.ToString(), gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg");
                                                        
                                                        try
                                                        {
                                                            using (System.IO.StreamWriter file =
                                                            new System.IO.StreamWriter(logdir, true))
                                                            {
                                                                file.WriteLine(gamejoin + "\\Box - Front\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg");
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            //log file busy
                                                        }
                                                    }
                                                }
                                            }
                                            
                                            if (coverreg.scan_of == "Box - Back")
                                            {
                                                if (BackImagePath == null)
                                                {
                                                    if (File.Exists(gamejoin + "\\Box - Back\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg") == false)
                                                    {


                                                        Directory.CreateDirectory(gamejoin + "\\Box - Back");
                                                        startDownload(coverreg.image.ToString(), gamejoin + "\\Box - Back\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg");
                                                        gamesdownloaded = gamesdownloaded + 1;
                                                        try
                                                        {
                                                            using (System.IO.StreamWriter file =
                                                            new System.IO.StreamWriter(logdir, true))
                                                            {
                                                                file.WriteLine(gamejoin + "\\Box - Back\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg");
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            //log file busy
                                                        }
                                                    }
                                                }
                                            }
                                            
                                            if (coverreg.scan_of == "Media")
                                            {
                                                if (CartFrontImagePath == null)
                                                {
                                                    if (File.Exists(gamejoin + "\\Cart - Front\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg") == false)
                                                    {

                                                        Directory.CreateDirectory(gamejoin + "\\Cart - Front");
                                                        startDownload(coverreg.image.ToString(), gamejoin + "\\Cart - Front\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg");
                                                        try
                                                        {
                                                            using (System.IO.StreamWriter file =
                                                            new System.IO.StreamWriter(logdir, true))
                                                            {
                                                                file.WriteLine(gamejoin + "\\Cart - Front\\" + Unbroken.FileHelper.CoerceValidFileName(Title) + "-01.jpg");
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            //log file busy
                                                        }

                                                    }

                                                }

                                            }
                                            progressvalue = 100;
                                            backgroundWorker1.ReportProgress(100);
                                        }
                                    }
                                }


                            }
                            catch
                            {
                                //images not found
                            }

                            
                        }
                    }
                    else
                    {
                        //game not found
                    }
                }
            }
            backgroundWorker1.ReportProgress(100);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label3.Text = e.ProgressPercentage.ToString() + "%";
            button2.Text = "CANCEL";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                label3.Text = "Awaiting Selection";
                button2.Text = "EXIT";

                Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.Save();
            }
            else if (e.Error != null)
            {
                label3.Text = e.Error.Message;
            }
            else
            {
                Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.Save();
                label3.Text = "Complete";
                button2.Text = "EXIT";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gamename = textBox1.Text;
            platformid = (comboBox1.SelectedItem as ComboboxItem).Value.ToString();
            apikey = Settings1.Default.ApiKey;
            if (!backgroundWorker1.IsBusy)
            {
                // This method will start the execution asynchronously in the background
                backgroundWorker1.RunWorkerAsync();
            }
        }

                private void startDownload(string url, string dest)
                {
                    Thread thread = new Thread(() => {
                        WebClient client = new WebClient();
                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                        client.DownloadFileAsync(new Uri(url), dest);


                    });
                    thread.Start();
                }



                void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
                {
                    this.BeginInvoke((MethodInvoker)delegate {
                        double bytesIn = double.Parse(e.BytesReceived.ToString());
                        double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                        double percentage = bytesIn / totalBytes * 100;
                        label3.Text = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive;
                        progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
                    });
                }
                void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
                {
                    this.BeginInvoke((MethodInvoker)delegate {
                        label3.Text = progressvalue.ToString() + "%";
                        progressBar1.Value = progressvalue;

                        
                    });
                }
                private void button2_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                // Cancel the asynchronous operation if still in progress
                backgroundWorker1.CancelAsync();
            }
            progressBar1.Value = 0;

            this.Hide();
        }
    }
}
