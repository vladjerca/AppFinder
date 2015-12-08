using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using TsudaKageyu;

namespace AppFinder.Portable
{
    public static class ShortcutGenerator
    {
        private static string ICON_DIR = "Icons",
                              EXE_EXT = ".exe",
                              BEAUTIFIER = ConfigurationManager.AppSettings["ShortcutBeautifierString"],
                              SHORTCUT_DIR = "AppFinder - Portable Shortcuts";

        /// <summary>
        /// Generates shortcuts for a list of files in the specified special folder.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="type"></param>
        public static void GenerateShortcuts(IList<FileInfo> files, Environment.SpecialFolder type)
        {
            foreach (var file in files)
            {
                if (!Directory.Exists(ICON_DIR))
                    Directory.CreateDirectory(ICON_DIR);

                IconExtractor ie = new IconExtractor(file);

                var icon = ie.GetAllIcons().OrderByDescending(x => x.Width).FirstOrDefault();

                string appName = file.Name.Replace(EXE_EXT, string.Empty).Replace(BEAUTIFIER, string.Empty),
                       iconName = string.Format("{0}.ico", appName);

                using (var fs = new FileStream(string.Format("{0}\\{1}", ICON_DIR, iconName),
                                               FileMode.OpenOrCreate))
                {
                    icon.Save(fs);
                }

                GenerateShortcut(file, type);
            }
        }

        /// <summary>
        /// Generates a shortcut for a specific file in the specified special folder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        public static void GenerateShortcut(FileInfo file, Environment.SpecialFolder type)
        {
            string appName = file.Name.Replace(EXE_EXT, string.Empty).Replace(BEAUTIFIER, string.Empty),
                   shortcutName = string.Format("{0}.lnk", appName),
                   iconName = string.Format("{0}.ico", appName);

            WshShellClass wsh = new WshShellClass();

            string shortcutFolder = string.Format("{0}\\{1}", Environment.GetFolderPath(type), SHORTCUT_DIR);

            if (!Directory.Exists(shortcutFolder))
                Directory.CreateDirectory(shortcutFolder);

            IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(
                string.Format("{0}\\{1}", shortcutFolder, shortcutName)) as IWshRuntimeLibrary.IWshShortcut;

            string appFinderLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            var driveLetter = Directory.GetDirectoryRoot(file.FullName);

            shortcut.Arguments = string.Format("-f \"{0}\" -d \"{1}\"", file.Name, file.Directory.FullName.Replace(driveLetter, string.Empty) );
            shortcut.TargetPath = string.Format("{0}\\AppFinder.exe", appFinderLocation);
            shortcut.WindowStyle = 1;
            shortcut.Description = string.Format("App Finder shortcut for {0}.", appName);
            shortcut.WorkingDirectory = appFinderLocation;
            shortcut.IconLocation = string.Format("{0}\\{1}\\{2}", appFinderLocation, ICON_DIR, iconName);
            shortcut.Save();
        }
    }
}
