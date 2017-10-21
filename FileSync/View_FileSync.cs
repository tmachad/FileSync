using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileSync.Controllers;

namespace FileSync
{
    public partial class View_FileSync : Form, IView
    {
        private Controller _controller;

        public View_FileSync()
        {
            InitializeComponent();

            this._controller = new Controller(this);
        }
    }
}
