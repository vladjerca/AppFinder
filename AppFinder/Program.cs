using AppFinder.Config;
using AppFinder.Portable;
using CommandLine;
using System.Configuration;
using System.Windows.Forms;

/*
 * Credits for icon extraction go to TsudaKageyu 
 * GitHub Link: https://github.com/TsudaKageyu/IconExtractor
 *
 * AppFinder has one goal, make portable apps easy to use and stop you from browsing your directory structure every time you wish to launch an app from your USB drive.
 * Enjoy!
 */

namespace AppFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();

            if(!RemovableDisk.IsRemovableConnected)
            {
                MessageBox.Show("No usb drives are connected. Cannot search for / launch portable app.");
                return;
            }

            // if using command line open the associated process
            if(Parser.Default.ParseArguments(args, options))
            {
                var file = Portable.RemovableDisk.GetTarget(options.ExeName, options.Folder);

                // if the application cannot be found search for it again (maybe directory structure has changed)
                if (file == null)
                {
                    file = Portable.RemovableDisk.SearchForTarget(options.ExeName);

                    // heal the link while we still have mana 
                    if(file != null)
                        ShortcutGenerator.GenerateShortcut(file, System.Environment.SpecialFolder.Desktop);
                }              

                // run the process if it's found
                if (file != null)
                {
                    System.Diagnostics.Process.Start(file.FullName);
                }
                else MessageBox.Show(string.Format(@"Could not find '{0}'. Please make sure the removable media containing the specified 'exe' is present.", options.ExeName));
            }
            else
            {
                // ask the user if he wants shortcuts to be generated
                var result = MessageBox.Show("Do you want to generate shortcuts for the applications on your USB drive?", "Generate shortcuts", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    var files = Portable.RemovableDisk.SearchFor(ConfigurationManager.AppSettings["PortableAppPattern"]);
                    ShortcutGenerator.GenerateShortcuts(files, System.Environment.SpecialFolder.Desktop);
                    MessageBox.Show("Shortcuts successfully generated! Enjoy.");
                }
            }
        }
    }
}
