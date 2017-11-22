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

            this.fileDialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true,
                Multiselect = false,
            };
        }

        public void SetAlertText(string text)
        {
            this.toolStripStatusLabel.Text = text;
        }
        
        public void ClearAlertText()
        {
            this.toolStripStatusLabel.Text = "";
        }

        public void InitialState()
        {
            if (this.InvokeRequired)
            {
                Action d = InitialState;
                this.Invoke(d);
            } else
            {
                this.startButton.Show();
                this.progressBar.Hide();
            }
        }

        public void FindingFiles()
        {
            if (this.InvokeRequired)
            {
                Action d = FindingFiles;
                this.Invoke(d);
            } else
            {
                this.startButton.Hide();
                this.progressBar.Show();
                this.progressBar.Style = ProgressBarStyle.Marquee;
            }
        }

        public void CopyingFiles(int total = 100)
        {
            if (this.InvokeRequired)
            {
                Action<int> d = CopyingFiles;
                this.Invoke(d, new object[] { total });
            } else
            {
                this.startButton.Hide();
                this.progressBar.Show();
                this.progressBar.Style = ProgressBarStyle.Continuous;
                this.progressBar.Minimum = 1;
                this.progressBar.Maximum = total;
                this.progressBar.Step = 1;
            }
        }

        public void DoneCopyingFiles()
        {
            if (this.InvokeRequired)
            {
                Action d = DoneCopyingFiles;
                this.Invoke(d);
            } else
            {
                this.progressBar.Hide();
                this.startButton.Show();
            }
        }

        public void SetProgress(int amount)
        {
            if (this.InvokeRequired)
            {
                Action<int> d = SetProgress;
                this.Invoke(d, new object[] { amount });
            } else
            {
                this.progressBar.Value = amount;
            }
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

        private void startButton_Click(object sender, EventArgs e)
        {
            this._controller.Start(this.sourceTextBox.Text, this.destinationTextBox.Text);
        }
    }
}
