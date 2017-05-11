using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;
using System.Windows.Forms;
using TheGamesDBAPI;
using System.Net;
using System.IO;

namespace GamesDBScraper
{

    public class Class1 : IGameMenuItemPlugin
    {
        public bool SupportsMultipleGames
        {
            //supports multiple games
            get
            {
                return true;
            }
        }

        public string Caption
        {
            //sets the name of the program
            get
            {
                return "Scrape With GamesDB";
            }
        }

        public System.Drawing.Image IconImage
        {
            get
            {
                //no icon for this project
                return null;
            }
        }

        public bool ShowInLaunchBox
        {
            get
            {
                //lets the project show in launchbox
                return true;
            }
        }

        public bool ShowInBigBox
        {
            get
            {
                //disables the program in bigbox
                return false;
            }
        }

        public bool GetIsValidForGame(IGame selectedGame)
        {
            //valid for all games
            return true;
        }

        public bool GetIsValidForGames(IGame[] selectedGames)
        {
            //valid for all multiple games
            return true;
        }

        public void OnSelected(IGame selectedGame)
        {

            //searches game on gamesdb
            foreach (GameSearchResult game in GamesDB.GetGames(selectedGame.Title))
            {
                //checks if the gamesdb game matches the platform
                if (game.Platform == selectedGame.Platform)
                {
                    //tells you what it found
                    DialogResult dialogResult = MessageBox.Show("Would you like to download the images for this game?", "Found " + game.Title, MessageBoxButtons.YesNo);
                    //if user says yes
                    if (dialogResult == DialogResult.Yes)
                    {
                        //gets the details from gamesdb
                        Game GameDetails = GamesDB.GetGame(game.ID);
                        //gets the images folder
                        var gamejoin = Path.Combine(Directory.GetCurrentDirectory(), "Images\\" + selectedGame.Platform);

                        //checks to see if it found an image
                        if (GameDetails.Images.BoxartFront != null)
                        {
                            //downloads the image
                            using (WebClient client = new WebClient())
                            {

                                client.DownloadFile(new Uri("http://thegamesdb.net/banners/" + GameDetails.Images.BoxartFront.Path), gamejoin + "\\Box - Front\\" + selectedGame.Title + "-01.jpg");

                            }
                        }
                        //checks to see if it found an image
                        if (GameDetails.Images.BoxartBack != null)
                        {
                            //downloads the image
                            using (WebClient client = new WebClient())
                            {

                                client.DownloadFile(new Uri("http://thegamesdb.net/banners/" + GameDetails.Images.BoxartBack.Path), gamejoin + "\\Box - Back\\" + selectedGame.Title + "-01.jpg");

                            }
                        }
                        //checks to see if it found an image
                        if (GameDetails.Images.Fanart != null)
                        {
                            //sets a number to try prevent overwriting if there is multiple images
                            var i = 00;
                            foreach (var fanart in GameDetails.Images.Fanart)
                            {
                                //downloads the image
                                using (WebClient client = new WebClient())
                                {

                                    client.DownloadFile(new Uri("http://thegamesdb.net/banners/" + fanart.Path), gamejoin + "\\Fanart - Background\\" + selectedGame.Title + " " + (i + 1) + ".jpg");


                                }

                            }

                        }
                        //checks to see if it found an image
                        if (GameDetails.Images.Banners != null)
                        {
                            //sets a number to try prevent overwriting if there is multiple images
                            var i = 00;
                            foreach (var banner in GameDetails.Images.Banners)
                            {
                                //downloads the image
                                using (WebClient client = new WebClient())
                                {

                                    client.DownloadFile(new Uri("http://thegamesdb.net/banners/" + banner.Path), gamejoin + "\\Banner\\" + selectedGame.Title + " " + (i + 1) + ".jpg");


                                }

                            }

                        }

                        //checks to see if it found an image
                        if (GameDetails.Images.Screenshots != null)
                        {
                            //sets a number to try prevent overwriting if there is multiple images
                            var i = 00;
                            foreach (var screenshot in GameDetails.Images.Screenshots)
                            {
                                //downloads the image
                                using (WebClient client = new WebClient())
                                {

                                    client.DownloadFile(new Uri("http://thegamesdb.net/banners/" + screenshot.Path), gamejoin + "\\Screenshot - Gameplay\\" + selectedGame.Title + " " + (i + 1) + ".jpg");


                                }

                            }

                        }


                        return;
                    }
                    //checks if user said no
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }

                }
            }


        }

        public void OnSelected(IGame[] selectedGames)
        {
            foreach (var selectedGame in selectedGames)
            {
                foreach (GameSearchResult game in GamesDB.GetGames(selectedGame.Title))
                {
                    if (game.Platform == selectedGame.Platform)
                    {

                        Game GameDetails = GamesDB.GetGame(game.ID);

                        var gamejoin = Path.Combine(Directory.GetCurrentDirectory(), "Images\\" + selectedGame.Platform);

                        //checks to see if it found an image
                        if (GameDetails.Images.BoxartFront != null)
                        {
                            //downloads the image
                            using (WebClient client = new WebClient())
                            {

                                client.DownloadFile(new Uri("http://thegamesdb.net/banners/" + GameDetails.Images.BoxartFront.Path), gamejoin + "\\Box - Front\\" + selectedGame.Title + "-01.jpg");

                            }
                        }
                        //checks to see if it found an image
                        if (GameDetails.Images.BoxartBack != null)
                        {
                            //downloads the image
                            using (WebClient client = new WebClient())
                            {

                                client.DownloadFile(new Uri("http://thegamesdb.net/banners/" + GameDetails.Images.BoxartBack.Path), gamejoin + "\\Box - Back\\" + selectedGame.Title + "-01.jpg");

                            }
                        }
                        //checks to see if it found an image
                        if (GameDetails.Images.Fanart != null)
                        {
                            //sets a number to try prevent overwriting if there is multiple images
                            var i = 00;
                            foreach (var fanart in GameDetails.Images.Fanart)
                            {
                                //downloads the image
                                using (WebClient client = new WebClient())
                                {

                                    client.DownloadFile(new Uri("http://thegamesdb.net/banners/" + fanart.Path), gamejoin + "\\Fanart - Background\\" + selectedGame.Title + " " + (i + 1) + ".jpg");


                                }

                            }

                        }
                        //checks to see if it found an image
                        if (GameDetails.Images.Banners != null)
                        {
                            //sets a number to try prevent overwriting if there is multiple images
                            var i = 00;
                            foreach (var banner in GameDetails.Images.Banners)
                            {
                                //downloads the image
                                using (WebClient client = new WebClient())
                                {

                                    client.DownloadFile(new Uri("http://thegamesdb.net/banners/" + banner.Path), gamejoin + "\\Banner\\" + selectedGame.Title + " " + (i + 1) + ".jpg");


                                }

                            }

                        }

                        //checks to see if it found an image
                        if (GameDetails.Images.Screenshots != null)
                        {
                            //sets a number to try prevent overwriting if there is multiple images
                            var i = 00;
                            foreach (var screenshot in GameDetails.Images.Screenshots)
                            {
                                //downloads the image
                                using (WebClient client = new WebClient())
                                {

                                    client.DownloadFile(new Uri("http://thegamesdb.net/banners/" + screenshot.Path), gamejoin + "\\Screenshot - Gameplay\\" + selectedGame.Title + " " + (i + 1) + ".jpg");


                                }

                            }

                        }


                    }



                }
            }
        }
    }
}
