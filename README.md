# LaunchBox-Plugin-Examples

a Bunch of examples on how to create a plugin for launchbox and bigbox

# Benchmark with Fraps

a example of creating and setting game custom fields

# Namespaces
Unbroken.LaunchBox.Plugins
Unbroken.LaunchBox.Plugins.Data

# Interfaces
IGameMenuItemPlugin

#Usage
selectedGame.GetAllCustomFields()
selectedGame.Play()
selectedGame.TryRemoveCustomField()
selectedGame.AddNewCustomField()


# Use Alternative Emulator

a example of getting a list of emulators and commands used to launch the specifix game

# Namespaces
Unbroken.LaunchBox.Plugins
Unbroken.LaunchBox.Plugins.Data

# Interfaces
IGameMenuItemPlugin

#Usage
Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.GetAllEmulators()
Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.GetAllEmulatorPlatforms()
Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.GetAllEmulatorPlatforms().CommandLine()
Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.GetEmulatorById()
Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.GetEmulatorById().ApplicationPath()
Unbroken.LaunchBox.Plugins.PluginHelper.DataManager.GetEmulatorById().CommandLine()
selectedGame.Platform()
selectedGame.EmulatorId()
selectedGame.ApplicationPath()

# GamesDB Scraper

a example of using an external api

# Namespaces
Unbroken.LaunchBox.Plugins
Unbroken.LaunchBox.Plugins.Data

# Interfaces
IGameMenuItemPlugin

#Usage
selectedGame.Platform()
selectedGame.Title()


