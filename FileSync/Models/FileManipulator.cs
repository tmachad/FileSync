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

        public static int DeepFileCount(string dir)
        {
            return DeepFileCount(new DirectoryInfo(dir));
        }

        public static void DeepCopy(DirectoryInfo source, DirectoryInfo dest, Action<int> progressCallback)
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
                            File.Replace(item.FullName, pathToDestFile, null);
                        }
                    } else
                    {
                        // No destination file found. Copy file
                        File.Copy(item.FullName, pathToDestFile);
                    }
                    progressCallback(1);
                } else if (item.GetType() == typeof(DirectoryInfo))
                {
                    string pathToDestFolder = Path.Combine(dest.FullName, GetRelativePath(source, item));
                    Directory.CreateDirectory(pathToDestFolder);    // Only creates missing directories
                    progressCallback(1);
                    DeepCopy(item.FullName, pathToDestFolder, progressCallback);
                }
            }
        }

        public static void DeepCopy(string source, string dest, Action<int> progressCallback)
        {
            DeepCopy(new DirectoryInfo(source), new DirectoryInfo(dest), progressCallback);
        }

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

        public static string GetRelativePath(FileSystemInfo root, FileSystemInfo child)
        {
            return GetRelativePath(root.FullName, child.FullName);
        }
    }
}
