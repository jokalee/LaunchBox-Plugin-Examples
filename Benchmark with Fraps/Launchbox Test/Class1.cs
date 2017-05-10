using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace Launchbox_Test
{
    public class Class1 : IGameMenuItemPlugin
    {

        public bool SupportsMultipleGames
        {
            get
            {
                //we cannot use the plugin for multiple games
                return false;
            }
        }


        public string Caption
        {
            get
            {
                //naming our plugin
                return "Benchmark with Fraps";
            }
        }

        public System.Drawing.Image IconImage
        {
            get
            {
                //we are not using an icon at the momment
                return null;
            }
        }

        public bool ShowInLaunchBox
        {
            get
            {
                //yes we want to show this plugin in launchbox
                return true;
            }
        }

        public bool ShowInBigBox
        {
            get
            {
                //yes we want to show this plugin in bigbox
                return true;
            }
        }

        public bool GetIsValidForGame(IGame selectedGame)
        {
            //we will allow for all games
            return true;
        }

        public bool GetIsValidForGames(IGame[] selectedGames)
        {
            //we will not allow multiple games to be selected
            return false;
        }

        public void OnSelected(IGame selectedGame)
        {
            
            //showing a messagebox of custom fields
            var fields = selectedGame.GetAllCustomFields();
            foreach (var field in fields)
            {
                MessageBox.Show(field.Name + " : " + field.Value);
            }
            //staring the game
            selectedGame.Play();
            //waiting 60 seconds to get past the menus
            System.Threading.Thread.Sleep(60000);
            //sending f11 to start benchmark
            SendKeys.SendWait("{F11}");
            //waiting for benchmark to finish
            System.Threading.Thread.Sleep(60000);
            //reading benchmark results
            string text = System.IO.File.ReadAllText(@"C:\Fraps\Benchmarks\FRAPSLOG.txt");
            //striping out average frames per second
            int pFrom = text.IndexOf("- Avg: ") + "- Avg: ".Length;
            int pTo = text.LastIndexOf(" - Min:");
            String result = text.Substring(pFrom, pTo - pFrom);
            //removing old custom  fields
            var oldfields = selectedGame.GetAllCustomFields();
            foreach (var field in oldfields)
            {
                if (field.Name == "FPS")
                {
                    selectedGame.TryRemoveCustomField(field);
                }
            }
            //setting the custom field
            var fps = selectedGame.AddNewCustomField();
            fps.Name = "FPS";
            fps.Value = result;
            //deleting the log so we can start fresh next time
            System.IO.File.Delete(@"C:\Fraps\Benchmarks\FRAPSLOG.txt");


        }

        public void OnSelected(IGame[] selectedGames)
        {
            return;
        }
    }
}
