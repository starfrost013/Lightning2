using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimTool
{
    public partial class SetLengthForm : Form
    {
        public SetLengthForm()
        {
            InitializeComponent();
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(lengthTextBox.Text, out var newAnimLength))
            {
                // do we use NC
                MessageBox.Show($"Animation length must be a valid integer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (AnimTool.CurAnimation != null)
            {
                AnimTool.CurAnimation.Length = newAnimLength;
            }

            Close();
        }
    }
}
