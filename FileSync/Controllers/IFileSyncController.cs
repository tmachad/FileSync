using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Controllers
{
    interface IFileSyncController
    {
        void StartEvent(object sender, EventArgs e);
    }
}
