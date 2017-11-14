using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Controllers
{
    interface IFileSyncController
    {
        /// <summary>
        /// Initiates the file copy process with the given
        /// source and destination directories.
        /// </summary>
        /// <param name="sourcePath">Path to the source directory</param>
        /// <param name="destinationPath">Path to the destination directory</param>
        void Start(string sourcePath, string destinationPath);
    }
}
