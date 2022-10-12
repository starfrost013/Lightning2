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
        private AnimationKeyframe CurKeyframe
        {
            get
            {
                if (AnimTool.CurProperty != null
                    && keyframeListBox.SelectedIndex > -1) return AnimTool.CurProperty.Keyframes[keyframeListBox.SelectedIndex];
                throw new InvalidOperationException("No CurProperty, this should never happen!"); // make an ncexception?
            }
        }

        public TabContent()
        {
            InitializeComponent();
        }

        internal void UpdateTabContent()
        {
            if (AnimTool.CurAnimation != null
                && AnimTool.CurProperty != null)
            {
                animationLengthText.Text = $"Animation Length: {AnimTool.CurAnimation.Length}ms";

                foreach (AnimationKeyframe keyframe in AnimTool.CurProperty.Keyframes)
                {
                    keyframeListBox.Items.Add($"{keyframe.Position}ms (ID {keyframe.Id})");
                }
            }
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            if (keyframeListBox.SelectedIndex >= 0
                && keyframeListBox.SelectedIndex < keyframeListBox.Items.Count)
            {
                keyframeListBox.Items.RemoveAt(keyframeListBox.SelectedIndex);
                
                if (AnimTool.CurProperty != null)
                {
                    AnimTool.CurProperty.Keyframes.RemoveAt(keyframeListBox.SelectedIndex);
                }

                keyframeListBox.SelectedIndex = keyframeListBox.Items.Count - 1; // will not select any if we delete last item as -1 = no item selected and 0 - 1 = -1
            }
        }

        private void keyframeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (keyframeListBox.SelectedIndex >= 0
                && AnimTool.CurProperty != null)
            {
                propertyPositionValueText.Text = $"{CurKeyframe.Position}ms";
                propertyTypeValueText.Text = AnimTool.CurProperty.Type;
                valueValueText.Text = CurKeyframe.Value.ToString();
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            AddKeyframeForm addKeyframeForm = new AddKeyframeForm();
            addKeyframeForm.ShowDialog();
            UpdateTabContent();
        }
    }
}
