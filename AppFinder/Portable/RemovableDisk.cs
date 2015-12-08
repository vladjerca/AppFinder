using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppFinder.Portable
{
    public static class RemovableDisk
    {
        /// <summary>
        /// Enumerates all removable devices.
        /// </summary>
        public static IEnumerable<DriveInfo> RemovableDevices
        {
            get
            {
                return DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.Removable);
            }
        }

        /// <summary>
        /// Checks if any removable devices are connected.
        /// </summary>
        public static bool IsRemovableConnected
        {
            get
            {
                return RemovableDevices.Count() > 0;
            }
        }

        /// <summary>
        /// Looks for the portable file in all of the removable devices.
        /// </summary>
        /// <param name="executable"></param>
        /// <param name="rootDir"></param>
        /// <returns></returns>
        public static FileInfo GetTarget(string executable, string rootDir)
        {
            if (!string.IsNullOrWhiteSpace(rootDir))
                foreach (var device in RemovableDevices)
                {
                    var filePath = string.Format("{0}\\{1}\\{2}", device.RootDirectory.FullName, rootDir, executable);

                    if (File.Exists(filePath))
                    {
                        return new FileInfo(filePath);
                    }
                }

            return null;
        }

        /// <summary>
        /// Searches for files in the removable devices.
        /// </summary>
        /// <param name="executable"></param>
        /// <returns></returns>
        public static IList<FileInfo> SearchFor(string executable)
        {
            List<FileInfo> results = new List<FileInfo>();

            // if the directory structure has changed since the links have been generated, look for the file again
            foreach (var device in RemovableDevices)
            {
                var folders = device.RootDirectory.GetDirectories();

                foreach (var folder in folders)
                {
                    if ((folder.Attributes & FileAttributes.System) != FileAttributes.System)
                    {
                        results.AddRange(folder.GetFiles(executable, SearchOption.AllDirectories));
                    }
                }
            }

            return results;
        }

        /// <summary>
        ///  Searches for all portable executables in the removable drives.
        /// </summary>
        /// <param name="executable"></param>
        /// <returns></returns>
        public static FileInfo SearchForTarget(string executable)
        {
            FileInfo exeInfo = default(FileInfo);

            var results = SearchFor(executable);
                
            if (results.Count > 1)
                throw new Exception("Multiple executables with the specified name exist, please use the -folder/-f argument to specify a more specific region.");

            exeInfo = results.FirstOrDefault();

            return exeInfo;
        }
    }
}
