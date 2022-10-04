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
    public partial class TabContent : UserControl
    {
        public TabContent()
        {
            InitializeComponent();
        }

        internal void UpdateTabContent()
        {
            if (AnimTool.CurAnimation != null
                && AnimTool.CurProperty != null)
            {
                animationLengthLabel.Text = $"Animation Length: {AnimTool.CurAnimation.Length}ms";

                foreach (AnimationKeyframe keyframe in AnimTool.CurProperty.Keyframes)
                {
                    keyframeListBox.Items.Add($"{keyframe.Position}ms (ID {keyframe.Id})");
                }
            }
        }
    }
}
