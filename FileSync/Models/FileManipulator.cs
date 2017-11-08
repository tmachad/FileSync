using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileSync.Models
{
    class FileManipulator
    {

        public static string ValidatePath(string path, string pathName)
        {
            string fullPath;
            try
            {
                fullPath = Path.GetFullPath(path);
            } catch(Exception ex)
            {
                throw new Exception($"Error: {pathName} path is invalid");
            }
            return fullPath;
        }

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

        public static void DeepCopy(DirectoryInfo source, DirectoryInfo dest)
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
                            // Target file is older. Replace with source file
                        }
                    } else
                    {
                        // Copy the file over
                    }
                } else if (item.GetType() == typeof(DirectoryInfo))
                {

                }
            }
        }

        public static string GetRelativePath(string root, string child)
        {
            Uri rootUri = new Uri(root);
            Uri targetUri = new Uri(child);
            return rootUri.MakeRelativeUri(targetUri).ToString();
        }

        public static string GetRelativePath(FileSystemInfo root, FileSystemInfo child)
        {
            return GetRelativePath(root.FullName, child.FullName);
        }
    }
}
