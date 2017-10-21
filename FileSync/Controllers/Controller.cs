using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSync.Models;

namespace FileSync.Controllers
{
    class Controller
    {
        private readonly IView _view;
        private readonly FileManipulator _file;

        public Controller(IView view)
        {
            this._view = view;
            this._file = new FileManipulator();
        }
    }
}
