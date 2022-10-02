using LightningGL;
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
    public partial class AddPropertyForm : Form
    {
        public AddPropertyForm()
        {
            InitializeComponent();
            
            if (AnimTool.CurAnimation != null)
            {
                foreach (AnimationProperty property in AnimTool.CurAnimation.Properties)
                {
                    propertiesComboBox.Items.Add(property.Name);
                }
            }

            if (propertiesComboBox.Items.Count > 0) propertiesComboBox.SelectedIndex = 0;
        }

    }
}
