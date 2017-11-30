using System;
using FileSync.Models;
using FileSync.Views;
using System.Threading;
using System.IO;

namespace FileSync.Controllers
{
    class Controller: IFileSyncController
    {
        private readonly IFileSyncView _view;
        private readonly FileManipulator _file;

        private string sourcePath = "";
        private string destinationPath = "";

        public Controller(IFileSyncView view)
        {
            this._view = view;
            this._file = new FileManipulator();
        }

        /// <summary>
        /// Starts the file copy process using the source and destination
        /// directories given by sourcePath and destinationPath.
        /// </summary>
        /// <param name="sourcePath">Path to source directory</param>
        /// <param name="destinationPath">Path to destination directory</param>
        public void Start(string sourcePath, string destinationPath)
        {
            this._view.ClearErrorMessage();
            try
            {
                sourcePath = Path.GetFullPath(sourcePath);
            } catch (Exception ex)
            {
                this._view.DisplayErrorMessage("Source path is invalid");
                return;
            }
            try
            {
                destinationPath = Path.GetFullPath(destinationPath);
            } catch (Exception ex)
            {
                this._view.DisplayErrorMessage("Destination path is invalid");
                return;
            }

            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;

            this._view.DisplayMessage("Finding files...");
            this._view.FindingFiles();

            int numberOfFiles = FileManipulator.DeepFileCount(sourcePath);

            this._view.DisplayMessage($"Copying {numberOfFiles} items");
            this._view.CopyingFiles(numberOfFiles);

            Thread copyThread = new Thread(StartFileCopy);
            copyThread.Start();
        }

        /// <summary>
        /// Actually starts the file copy process on a seperate thread.
        /// </summary>
        private void StartFileCopy()
        {
            int totalCount = 0;
            int copiedCount = 0;
            FileManipulator.DeepCopy(sourcePath, destinationPath, (copied) =>
            {
                totalCount ++;
                if (copied)
                {
                    copiedCount++;
                }
                this._view.SetProgress(totalCount);
                this._view.DisplayProgressMessage($"Copied {copiedCount}, ignored {totalCount - copiedCount} of {totalCount} items");
            });
            this._view.DisplayMessage($"Done copying. Copied {copiedCount}, ignored {totalCount - copiedCount} of {totalCount} items");
            this._view.DoneCopyingFiles();
        }
    }
}
