using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSync.Views
{
    interface IFileSyncView
    {
        void SetTooltipText(string text);

        string GetSourcePath();

        string GetDestinationPath();

        void ConfigProgressIndeterminate();

        void ConfigProgressDeterminite(int min, int max, int stepSize);

        void StepProgress();
    }
}
