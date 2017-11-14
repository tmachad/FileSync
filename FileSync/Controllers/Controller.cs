using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSync.Models;
using FileSync.Views;

namespace FileSync.Controllers
{
    class Controller: IFileSyncController
    {
        private readonly IFileSyncView _view;
        private readonly FileManipulator _file;

        public Controller(IFileSyncView view)
        {
            this._view = view;
            this._file = new FileManipulator();
        }

        public void Start(string sourcePath, string destinationPath)
        {
            this._view.ClearAlertText();
            try
            {
                sourcePath = FileManipulator.ValidatePath(sourcePath, "Source");
                destinationPath = FileManipulator.ValidatePath(destinationPath, "Destination");
            } catch(Exception ex)
            {
                this._view.SetAlertText(ex.Message);
                return;
            }

            this._view.SetAlertText("Finding files");
            this._view.FindingFiles();

            int numberOfFiles = FileManipulator.DeepFileCount(sourcePath);

            this._view.SetAlertText($"Copying {numberOfFiles} files");
            this._view.CopyingFiles(numberOfFiles);
            int count = 0;
            FileManipulator.DeepCopy(sourcePath, destinationPath, (increment) =>
            {
                count += increment;
                this._view.SetProgress(count);
            });
        }
    }
}
