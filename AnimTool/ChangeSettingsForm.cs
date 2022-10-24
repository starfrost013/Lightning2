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
    public partial class ChangeSettingsForm : Form
    {
        public ChangeSettingsForm()
        {
            InitializeComponent();
        }

        private void ChangeSettingsForm_Load(object sender, EventArgs e)
        {
            if (AnimTool.CurAnimation != null)
            {
                if (AnimTool.CurAnimation.Repeat < 0) 
                {
                    repeatCount.Enabled = false;
                    repeatInfinitely.Checked = true;
                }
                else
                {
                    repeatCount.Value = AnimTool.CurAnimation.Repeat;
                }

                repeatInfinitely.Checked = (AnimTool.CurAnimation.Repeat == -1);
                reverseAnimation.Checked = AnimTool.CurAnimation.Reverse;
            }
        }

        private void repeatInfinitely_CheckedChanged(object sender, EventArgs e)
        {
            repeatCount.Enabled = !(repeatInfinitely.Checked);
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (AnimTool.CurAnimation != null)
            {
                AnimTool.CurAnimation.Repeat = Convert.ToInt32(repeatCount.Value);
                if (repeatInfinitely.Checked) AnimTool.CurAnimation.Repeat = -1;
                AnimTool.CurAnimation.Reverse = reverseAnimation.Checked;
                Close();
            }
        }
    }
}
