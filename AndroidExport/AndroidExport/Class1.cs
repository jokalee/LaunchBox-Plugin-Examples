using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace AndroidExport
{
    public class Class1 : IGameMenuItemPlugin
    {
        public static string folderPath { get; set; }

        public bool SupportsMultipleGames => true;

        public string Caption => "Export for Kodi";

        public System.Drawing.Image IconImage => null;

        public bool ShowInLaunchBox => true;

        public bool ShowInBigBox => false;

        public bool GetIsValidForGame(IGame selectedGame)
        {
            return true;
        }

        public bool GetIsValidForGames(IGame[] selectedGames)
        {
            return true;
        }

        public void OnSelected(IGame selectedGame)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    folderPath = fbd.SelectedPath;
                }
                else
                {
                    MessageBox.Show("You need to select a output directory");
                }


                if (System.IO.File.Exists(selectedGame.FrontImagePath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Box - Front\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.FrontImagePath)) != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Box - Front");
                        File.Copy(selectedGame.FrontImagePath, folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Box - Front\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.FrontImagePath));
                    }
                    

                }

                if (System.IO.File.Exists(selectedGame.BackgroundImagePath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Fanart - Background\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.BackgroundImagePath)) != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Fanart - Background");
                        File.Copy(selectedGame.BackgroundImagePath, folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Fanart - Background\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.BackgroundImagePath));
                    }
                }

                if (System.IO.File.Exists(selectedGame.ScreenshotImagePath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Screenshot - Gameplay\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.BackgroundImagePath)) != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Screenshot - Gameplay");
                        File.Copy(selectedGame.ScreenshotImagePath, folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Screenshot - Gameplay\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.BackgroundImagePath));
                    }
                }

                if (System.IO.File.Exists(selectedGame.ClearLogoImagePath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Clear Logo\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.ClearLogoImagePath)) != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Clear Logo");
                        File.Copy(selectedGame.ClearLogoImagePath, folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Clear Logo\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.ClearLogoImagePath));
                    }
                    

                }

                
                string platformLogoPath = Path.Combine(Directory.GetCurrentDirectory(), "Images\\Platforms\\" + selectedGame.Platform + "\\Clear Logo\\" + selectedGame.Platform + ".png");
                string platformBannerPath = Path.Combine(Directory.GetCurrentDirectory(), "Images\\Platforms\\" + selectedGame.Platform + "\\Banner\\" + selectedGame.Platform + ".jpg");
                string platformDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data\\Platforms\\" + selectedGame.Platform + ".xml");
                string emulatorsPath = Path.Combine(Directory.GetCurrentDirectory(), "Data\\Emulators.xml");
                string PlatformsPath = Path.Combine(Directory.GetCurrentDirectory(), "Data\\Platforms.xml");
                string BigBoxPath = Path.Combine(Directory.GetCurrentDirectory(), "BigBox.exe");
                string ApplicationPath = Unbroken.FileHelper.GetFullPath(selectedGame.ApplicationPath, Directory.GetCurrentDirectory());
                string AndroidApplicationPath = Unbroken.FileHelper.GetFullPath(selectedGame.ApplicationPath,folderPath + "\\LaunchBox");

                if (System.IO.File.Exists(platformBannerPath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Images\\Platforms\\" + selectedGame.Platform + "\\Banner\\" + selectedGame.Platform + ".jpg") != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Images\\Platforms\\" + selectedGame.Platform + "\\Banner");
                        File.Copy(platformBannerPath, folderPath + "\\LaunchBox\\Images\\Platforms\\" + selectedGame.Platform + "\\Banner\\" + selectedGame.Platform + ".jpg");
                    }


                }
                if (System.IO.File.Exists(platformLogoPath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Images\\Platforms\\" + selectedGame.Platform + "\\Clear Logo\\" + selectedGame.Platform + ".png") != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Images\\Platforms\\" + selectedGame.Platform + "\\Clear Logo");
                        File.Copy(platformLogoPath, folderPath + "\\LaunchBox\\Images\\Platforms\\" + selectedGame.Platform + "\\Clear Logo\\" + selectedGame.Platform + ".png");
                    }


                }
                if (System.IO.File.Exists(platformDataPath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Data\\Platforms\\" + selectedGame.Platform + ".xml") != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Data\\Platforms");
                        File.Copy(platformDataPath, folderPath + "\\LaunchBox\\Data\\Platforms\\" + selectedGame.Platform + ".xml");
                    }
                    else
                    {
                        File.Delete(folderPath + "\\LaunchBox\\Data\\Platforms\\" + selectedGame.Platform + ".xml");
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Data\\Platforms");
                        File.Copy(platformDataPath, folderPath + "\\LaunchBox\\Data\\Platforms\\" + selectedGame.Platform + ".xml");
                    }
                    

                }
             
                
                if (System.IO.File.Exists(PlatformsPath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Data\\Platforms.xml") != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Data");
                        File.Copy(PlatformsPath, folderPath + "\\LaunchBox\\Data\\Platforms.xml");
                    }
                    else
                    {
                        File.Delete(folderPath + "\\LaunchBox\\Data\\Platforms.xml");
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Data");
                        File.Copy(PlatformsPath, folderPath + "\\LaunchBox\\Data\\Platforms.xml");
                    }
                  

                }

                if (System.IO.File.Exists(emulatorsPath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Data\\Emulators.xml") != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Data\\Platforms");
                        File.Copy(emulatorsPath, folderPath + "\\LaunchBox\\Data\\Emulators.xml");
                    }
                    else
                    {
                        File.Delete(folderPath + "\\LaunchBox\\Data\\Emulators.xml");
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Data\\Platforms");
                        File.Copy(emulatorsPath, folderPath + "\\LaunchBox\\Data\\Emulators.xml");
                    }
                   

                }
                if (System.IO.File.Exists(BigBoxPath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\BigBox.exe") != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox");
                        File.Copy(BigBoxPath, folderPath + "\\LaunchBox\\BigBox.exe");

                    }
                   

                }
               
                if (System.IO.File.Exists(ApplicationPath))
                {
                    if (File.Exists(AndroidApplicationPath) != true)
                    {
                        
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(AndroidApplicationPath));
                        File.Copy(ApplicationPath, AndroidApplicationPath);
                    }
                    

                }
            }

            MessageBox.Show("Copy Complete");
        }

        public void OnSelected(IGame[] selectedGames)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    folderPath = fbd.SelectedPath;
                }
                else
                {
                    MessageBox.Show("You need to select a output directory");
                    return;
                }
            }
            foreach(var selectedGame in selectedGames)
            {

                if (System.IO.File.Exists(selectedGame.FrontImagePath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Box - Front\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.FrontImagePath)) != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Box - Front");
                        File.Copy(selectedGame.FrontImagePath, folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Box - Front\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.FrontImagePath));
                    }


                }

                if (System.IO.File.Exists(selectedGame.BackgroundImagePath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Fanart - Background\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.BackgroundImagePath)) != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Fanart - Background");
                        File.Copy(selectedGame.BackgroundImagePath, folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Fanart - Background\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.BackgroundImagePath));
                    }
                }

                if (System.IO.File.Exists(selectedGame.ScreenshotImagePath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Screenshot - Gameplay\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.BackgroundImagePath)) != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Screenshot - Gameplay");
                        File.Copy(selectedGame.ScreenshotImagePath, folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Screenshot - Gameplay\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.BackgroundImagePath));
                    }
                }

                if (System.IO.File.Exists(selectedGame.ClearLogoImagePath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Clear Logo\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.ClearLogoImagePath)) != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Clear Logo");
                        File.Copy(selectedGame.ClearLogoImagePath, folderPath + "\\LaunchBox\\Images\\" + selectedGame.Platform + "\\Clear Logo\\" + Unbroken.FileHelper.CoerceValidFileName(selectedGame.Title) + "-01" + Unbroken.FileHelper.GetFileExtensionSafe(selectedGame.ClearLogoImagePath));
                    }


                }

               
                string platformLogoPath = Path.Combine(Directory.GetCurrentDirectory(), "Images\\Platforms\\" + selectedGame.Platform + "\\Clear Logo\\" + selectedGame.Platform + ".png");
                string platformBannerPath = Path.Combine(Directory.GetCurrentDirectory(), "Images\\Platforms\\" + selectedGame.Platform + "\\Banner\\" + selectedGame.Platform + ".jpg");
                string platformDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data\\Platforms\\" + selectedGame.Platform + ".xml");
                string emulatorsPath = Path.Combine(Directory.GetCurrentDirectory(), "Data\\Emulators.xml");
                string PlatformsPath = Path.Combine(Directory.GetCurrentDirectory(), "Data\\Platforms.xml");
                string BigBoxPath = Path.Combine(Directory.GetCurrentDirectory(), "BigBox.exe");
                string ApplicationPath = Unbroken.FileHelper.GetFullPath(selectedGame.ApplicationPath, Directory.GetCurrentDirectory());
                string AndroidApplicationPath = Unbroken.FileHelper.GetFullPath(selectedGame.ApplicationPath, folderPath + "\\LaunchBox");

                if (System.IO.File.Exists(platformBannerPath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Images\\Platforms\\" + selectedGame.Platform + "\\Banner\\" + selectedGame.Platform + ".jpg") != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Images\\Platforms\\" + selectedGame.Platform + "\\Banner");
                        File.Copy(platformBannerPath, folderPath + "\\LaunchBox\\Images\\Platforms\\" + selectedGame.Platform + "\\Banner\\" + selectedGame.Platform + ".jpg");
                    }


                }

               
                if (System.IO.File.Exists(platformLogoPath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Images\\Platforms\\" + selectedGame.Platform + "\\Clear Logo\\" + selectedGame.Platform + ".png") != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Images\\Platforms\\" + selectedGame.Platform + "\\Clear Logo");
                        File.Copy(platformLogoPath, folderPath + "\\LaunchBox\\Images\\Platforms\\" + selectedGame.Platform + "\\Clear Logo\\" + selectedGame.Platform + ".png");
                    }


                }
                if (System.IO.File.Exists(platformDataPath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Data\\Platforms\\" + selectedGame.Platform + ".xml") != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Data\\Platforms");
                        File.Copy(platformDataPath, folderPath + "\\LaunchBox\\Data\\Platforms\\" + selectedGame.Platform + ".xml");
                    }
                    else
                    {
                        File.Delete(folderPath + "\\LaunchBox\\Data\\Platforms\\" + selectedGame.Platform + ".xml");
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Data\\Platforms");
                        File.Copy(platformDataPath, folderPath + "\\LaunchBox\\Data\\Platforms\\" + selectedGame.Platform + ".xml");
                    }


                }


                if (System.IO.File.Exists(PlatformsPath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Data\\Platforms.xml") != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Data");
                        File.Copy(PlatformsPath, folderPath + "\\LaunchBox\\Data\\Platforms.xml");
                    }
                    else
                    {
                        File.Delete(folderPath + "\\LaunchBox\\Data\\Platforms.xml");
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Data");
                        File.Copy(PlatformsPath, folderPath + "\\LaunchBox\\Data\\Platforms.xml");
                    }


                }

                if (System.IO.File.Exists(emulatorsPath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\Data\\Emulators.xml") != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Data\\Platforms");
                        File.Copy(emulatorsPath, folderPath + "\\LaunchBox\\Data\\Emulators.xml");
                    }
                    else
                    {
                        File.Delete(folderPath + "\\LaunchBox\\Data\\Emulators.xml");
                        Directory.CreateDirectory(folderPath + "\\LaunchBox\\Data\\Platforms");
                        File.Copy(emulatorsPath, folderPath + "\\LaunchBox\\Data\\Emulators.xml");
                    }


                }
                if (System.IO.File.Exists(BigBoxPath))
                {
                    if (File.Exists(folderPath + "\\LaunchBox\\BigBox.exe") != true)
                    {
                        Directory.CreateDirectory(folderPath + "\\LaunchBox");
                        File.Copy(BigBoxPath, folderPath + "\\LaunchBox\\BigBox.exe");

                    }


                }

                if (System.IO.File.Exists(ApplicationPath))
                {
                    if (File.Exists(AndroidApplicationPath) != true)
                    {

                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(AndroidApplicationPath));
                        File.Copy(ApplicationPath, AndroidApplicationPath);
                    }


                }
               
            }
            MessageBox.Show("Copy Complete");

        }
    }
}
