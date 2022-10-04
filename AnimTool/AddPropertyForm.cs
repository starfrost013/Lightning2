using LightningGL;
using System.Reflection;


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

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (AnimTool.CurAnimation != null) AnimTool.CurProperty = AnimTool.CurAnimation.Properties[propertiesComboBox.SelectedIndex];
            Close();
        }

    }
}
