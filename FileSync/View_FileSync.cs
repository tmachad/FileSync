using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileSync.Views;
using FileSync.Controllers;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;

namespace FileSync
{
    public partial class View_FileSync : Form, IFileSyncView
    {
        private IFileSyncController _controller;
        private CommonOpenFileDialog fileDialog;

        public View_FileSync()
        {
            InitializeComponent();

            this._controller = new Controller(this);
            this.startButton.Click += new EventHandler(_controller.StartEvent);

            this.fileDialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                Multiselect = false,
            };
        }

        public void SetTooltipText(string text)
        {
            this.toolStripStatusLabel.Text = text;
        }

        public string GetSourcePath()
        {
            return this.sourceTextBox.Text;
        }

        public string GetDestinationPath()
        {
            return this.destinationTextBox.Text;
        }
        
        public void ConfigProgressIndeterminate()
        {
            this.startButton.Hide();
            this.progressBar.Show();
            this.progressBar.Style = ProgressBarStyle.Marquee;
        }

        public void ConfigProgressDeterminite(int min = 1, int max = 100, int stepSize = 1)
        {
            this.startButton.Hide();
            this.progressBar.Show();
            this.progressBar.Style = ProgressBarStyle.Continuous;
            this.progressBar.Minimum = min;
            this.progressBar.Maximum = max;
            this.progressBar.Step = stepSize;
        }

        public void StepProgress()
        {
            this.progressBar.PerformStep();
        }

        private void sourceButton_Click(object sender, EventArgs e)
        {
            if (this.fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.sourceTextBox.Text = this.fileDialog.FileName;
            }
        }

        private void destinationButton_Click(object sender, EventArgs e)
        {
            if (this.fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.destinationTextBox.Text = this.fileDialog.FileName;
            }
        }
    }
}
