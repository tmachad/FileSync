using System;
using System.IO;

namespace FileSync.Models
{
    class FileManipulator
    {
        /// <summary>
        /// Counts the number of files and directories contained
        /// in the given directory. Does not include the given
        /// directory in the count.
        /// </summary>
        /// <param name="dir">The directory to count the contents of</param>
        /// <returns>The number of items contained in dir</returns>
        public static int DeepFileCount(DirectoryInfo dir)
        {
            int count = 0;
            foreach(FileSystemInfo item in dir.GetFileSystemInfos())
            {
                if (item.GetType() == typeof(FileInfo))
                {
                    count++;
                } else if (item.GetType() == typeof(DirectoryInfo))
                {
                    count += DeepFileCount((DirectoryInfo)item);
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Counts the number of files and directories contained
        /// in the given directory. Does not include the given
        /// directory in the count.
        /// </summary>
        /// <param name="dir">The directory to count the contents of</param>
        /// <returns>The number of items contained in dir</returns>
        public static int DeepFileCount(string dir)
        {
            return DeepFileCount(new DirectoryInfo(dir));
        }

        /// <summary>
        /// Copies all of the contents of source into dest. If an item already exists in dest,
        /// it is replace if and only if the item in source was modified more recently. The
        /// progressCallback is called each time an item is copied or ignored, and is passed 
        /// a 'true' if the item was copied and 'false' if it was ignored.
        /// </summary>
        /// <param name="source">The folder to copy files from</param>
        /// <param name="dest">The folder to copy files to</param>
        /// <param name="progressCallback">Callback function that gets called when an item 
        /// is copied or ignored</param>
        public static void DeepCopy(DirectoryInfo source, DirectoryInfo dest, Action<bool> progressCallback)
        {
            foreach(FileSystemInfo item in source.GetFileSystemInfos())
            {
                if (item.GetType() == typeof(FileInfo))
                {
                    string pathToDestFile = Path.Combine(dest.FullName, GetRelativePath(source, item));
                    if (File.Exists(pathToDestFile))
                    {
                        FileInfo targetFile = new FileInfo(pathToDestFile);
                        if (item.LastWriteTimeUtc > targetFile.LastWriteTimeUtc)
                        {
                            // Target file is older. Remove old file, then copy new file
                            File.Copy(item.FullName, pathToDestFile, true);
                            progressCallback(true);
                        } else
                        {
                            progressCallback(false);
                        }
                    } else
                    {
                        // No destination file found. Copy file
                        File.Copy(item.FullName, pathToDestFile);
                        progressCallback(true);
                    }
                    
                } else if (item.GetType() == typeof(DirectoryInfo))
                {
                    string pathToDestFolder = Path.Combine(dest.FullName, GetRelativePath(source, item));
                    if (Directory.Exists(pathToDestFolder))
                    {
                        progressCallback(false);
                    }
                    else
                    {
                        Directory.CreateDirectory(pathToDestFolder);
                        progressCallback(true);
                    }
                    DeepCopy(item.FullName, pathToDestFolder, progressCallback);
                }
            }
        }

        /// <summary>
        /// Copies all of the contents of source into dest. If an item already exists in dest,
        /// it is replace if and only if the item in source was modified more recently. The
        /// progressCallback is called each time an item is copied or ignored, and is passed 
        /// a 'true' if the item was copied and 'false' if it was ignored.
        /// </summary>
        /// <param name="source">The folder to copy files from</param>
        /// <param name="dest">The folder to copy files to</param>
        /// <param name="progressCallback">Callback function that gets called when an item 
        /// is copied or ignored</param>
        public static void DeepCopy(string source, string dest, Action<bool> progressCallback)
        {
            DeepCopy(new DirectoryInfo(source), new DirectoryInfo(dest), progressCallback);
        }

        /// <summary>
        /// Gets the relative path from root to child.
        /// If the child path is not actually a descendent of root,
        /// undefined behaviour will occur.
        /// </summary>
        /// <param name="root">The root path</param>
        /// <param name="child">The child path</param>
        /// <returns>The relative path from root to child</returns>
        public static string GetRelativePath(string root, string child)
        {
            string relativePath = child.Replace(root, null);

            if (relativePath[0] == '\\')
            {
                int count = 0;
                while (relativePath[count] == '\\')
                {
                    count++;
                }
                relativePath = relativePath.Remove(0, count);
            }

            return relativePath;
        }

        /// <summary>
        /// Gets the relative path from root to child.
        /// If the child path is not actually a descendent of root,
        /// undefined behaviour will occur.
        /// </summary>
        /// <param name="root">The root filesystem object</param>
        /// <param name="child">The child filesystem object</param>
        /// <returns>The relative path from root to child</returns>
        public static string GetRelativePath(FileSystemInfo root, FileSystemInfo child)
        {
            return GetRelativePath(root.FullName, child.FullName);
        }
    }
}
