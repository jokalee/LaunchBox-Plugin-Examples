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
                return "Use Alternative Emulator";
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
            //we will allow for all games but realy we shouldnt
            return true;
        }

        public void OnSelected(IGame selectedGame)
        {
            //getting a list of emulators
            foreach (var emulator in Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.GetAllEmulators())
            {
                //getting a list of all platforms the emulator supports
                foreach (var platform in emulator.GetAllEmulatorPlatforms())
                {
                    //making sure it can be used for this platform
                    if (selectedGame.Platform == platform.Platform)
                    {
                        //creating a vairiable for the emulator
                        var emulatorinfo = Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.GetEmulatorById(platform.EmulatorId);

                        //making sure its not the default emulator
                        if (selectedGame.EmulatorId != platform.EmulatorId)
                        {
                            //joining relative paths
                            var gamejoin = Path.Combine(Directory.GetCurrentDirectory(), selectedGame.ApplicationPath);
                            string gamepath = '"' + Path.GetFullPath(gamejoin) + '"';
                            var emjoin = Path.Combine(Directory.GetCurrentDirectory(), emulatorinfo.ApplicationPath);
                            string empath = '"' + Path.GetFullPath(emjoin) + '"';
                            //checking command line paramaters
                            if (emulatorinfo.CommandLine == null & platform.CommandLine == null)
                            {
                                //launching game using command line
                                System.Diagnostics.Process process = new System.Diagnostics.Process();
                                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                                startInfo.FileName = empath;
                                startInfo.Arguments = gamepath;
                                process.StartInfo = startInfo;
                                process.Start();
                                //returning so hopefully we dont start the next emulator this has not been tested properly 
                                return;
                            }
                            //checking command line paramaters
                            else if (emulatorinfo.CommandLine != null & platform.CommandLine == null)
                            {
                                //launching game using command line
                                System.Diagnostics.Process process = new System.Diagnostics.Process();
                                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                                startInfo.FileName = empath;
                                startInfo.Arguments = emulatorinfo.CommandLine + " " + gamepath;
                                process.StartInfo = startInfo;
                                process.Start();
                                //returning so hopefully we dont start the next emulator this has not been tested properly 
                                return;
                            }
                            //checking command line paramaters
                            else if (emulatorinfo.CommandLine == null & platform.CommandLine != null)
                            {
                                //launching game using command line
                                System.Diagnostics.Process process = new System.Diagnostics.Process();
                                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                                startInfo.FileName = empath;
                                startInfo.Arguments = platform.CommandLine + " " + gamepath;
                                process.StartInfo = startInfo;
                                process.Start();
                                //returning so hopefully we dont start the next emulator this has not been tested properly 
                                return;
                               
                            }
                            //checking command line paramaters
                            else
                            {
                                //launching game using command line
                                System.Diagnostics.Process process = new System.Diagnostics.Process();
                                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                                startInfo.FileName = empath;
                                startInfo.Arguments = emulatorinfo.CommandLine + " " + platform.CommandLine + " " + gamepath;
                                process.StartInfo = startInfo;
                                process.Start();
                                //returning so hopefully we dont start the next emulator this has not been tested properly 
                                return;
                            }

                        }
                  
                    }
                    
                }
            }

        }

        public void OnSelected(IGame[] selectedGames)
        {
            return;
        }
    }
}
