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

        public void StartEvent(object sender, EventArgs e)
        {
            string sourcePath = this._view.GetSourcePath();
            string destinationPath = this._view.GetDestinationPath();

            try
            {
                sourcePath = FileManipulator.ValidatePath(sourcePath, "Source");
                destinationPath = FileManipulator.ValidatePath(destinationPath, "Destination");
            } catch(Exception ex)
            {
                this._view.SetTooltipText(ex.Message);
                return;
            }
        }
    }
}
