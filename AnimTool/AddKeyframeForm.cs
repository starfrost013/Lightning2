using LightningGL;
using LightningUtil;

namespace AnimTool
{
    public partial class AddKeyframeForm : Form
    {
        public AddKeyframeForm()
        {
            InitializeComponent();

            if (AnimTool.CurAnimation != null)
            {
                lengthText.Text = $"{AnimTool.CurAnimation.Length}ms";
                lengthTrackBar.Maximum = AnimTool.CurAnimation.Length;
            }
            
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (AnimTool.CurProperty != null
                && AnimTool.CurAnimation != null)
            {
                string propertyType = AnimTool.CurProperty.Type;
                string propertyValue = initialValueTextBox.Text;

                if (!AnimTool.IsPropertyValid(propertyType, propertyValue))
                {
                    Logger.LogError($"Invalid value for AnimationKeyframe type {propertyType}", 162, LoggerSeverity.Warning);
                    return;
                }

                AnimTool.CurProperty.Keyframes.Add(new AnimationKeyframe(lengthTrackBar.Value, propertyValue));

                Close();
            }
        }
    }
}