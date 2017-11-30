using System;
using System.Windows.Forms;
using FileSync.Views;
using FileSync.Controllers;
using Microsoft.WindowsAPICodePack.Dialogs;

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

        /// <summary>
        /// Sets the text on the status strip.
        /// </summary>
        /// <param name="text">The text to display</param>
        public void DisplayMessage(string text)
        {
            this.toolStripStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel.Text = text;
        }
        
        /// <summary>
        /// Clears the text on the status strip.
        /// </summary>
        public void ClearMessage()
        {
            this.toolStripStatusLabel.Text = "";
        }

        /// <summary>
        /// Sets the text on the status strip, 
        /// prepending "Error: " to it.
        /// </summary>
        /// <param name="text">The error text to display</param>
        public void DisplayErrorMessage(string text)
        {
            this.toolStripStatusLabel.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel.Text = "Error: " + text;
        }

        /// <summary>
        /// Clears the error text.
        /// </summary>
        public void ClearErrorMessage()
        {
            this.toolStripStatusLabel.Text = "";
        }

        /// <summary>
        /// Sets the text on the status strip.
        /// </summary>
        /// <param name="text">The text to display</param>
        public void DisplayProgressMessage(string text)
        {
            this.toolStripStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel.Text = text;
        }

        /// <summary>
        /// Clears the status strip of text.
        /// </summary>
        public void ClearProgressMessage()
        {
            this.toolStripStatusLabel.Text = "";
        }

        /// <summary>
        /// Hides the progress bar, shows the start button,
        /// and clears the source text box, destination text
        /// box, and status strip.
        /// </summary>
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
                this.toolStripStatusLabel.Text = "";
                this.sourceTextBox.Clear();
                this.sourceTextBox.Enabled = true;
                this.destinationTextBox.Clear();
                this.destinationTextBox.Enabled = true;
            }
        }

        /// <summary>
        /// Hides the start button, shows the progress
        /// bar in a marquee style, and disables the 
        /// source and destination text boxes.
        /// </summary>
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
                this.sourceTextBox.Enabled = false;
                this.destinationTextBox.Enabled = false;
            }
        }

        /// <summary>
        /// Hides the start button, sets up the progress
        /// bar for displaying copy progress using the given
        /// total, and disables the source and destination text
        /// boxes.
        /// </summary>
        /// <param name="total">Total number of items being copied</param>
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
                this.sourceTextBox.Enabled = false;
                this.destinationTextBox.Enabled = false;
            }
        }

        /// <summary>
        /// Hides the progress bar, shows the start button,
        /// and enables teh source and destination text boxes.
        /// </summary>
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
                this.sourceTextBox.Enabled = true;
                this.destinationTextBox.Enabled = true;
            }
        }

        /// <summary>
        /// Sets the amount of progress to show on the progress
        /// bar. The amount should be smaller than the total given
        /// when CopyingFiles(int) was called.
        /// </summary>
        /// <param name="amount">The progress amount</param>
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
