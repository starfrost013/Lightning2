using LightningGL;
using System.Reflection;

namespace AnimTool
{
    public partial class AddPropertyForm : Form
    {
        public AddPropertyForm()
        {
            InitializeComponent();

            Type renderableType = typeof(Renderable);

            if (AnimTool.CurAnimation != null)
            {
                foreach (PropertyInfo property in renderableType.GetProperties())
                {
                    // don't add delegates or properties with internal/private get methods

                    if (!typeof(Delegate).IsAssignableFrom(property.PropertyType)
                        && !typeof(Animation).IsAssignableFrom(property.PropertyType)
                        && property.PropertyType.IsPublic)
                    {
                        bool addCurrentName = true;

                        foreach (AnimationProperty animationProperty in AnimTool.CurAnimation.Properties)
                        {
                            addCurrentName = (animationProperty.Name != property.Name);

                            // bad code, but this is required for now, will improve
                            if (!addCurrentName) break;
                        }

                        if (addCurrentName) propertiesComboBox.Items.Add($"{property.Name} ({property.PropertyType.Name})");
                    }
                }
            }

            propertiesComboBox.SelectedIndex = 0;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (AnimTool.CurAnimation != null)
            {
                // TEMPORARY CODE -- WILL BE OPTIMISED
                Type renderableType = typeof(Renderable);

                foreach (PropertyInfo property in renderableType.GetProperties())
                {
                    // fucking hack

                    if (propertiesComboBox.Text.Contains(property.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        AnimationProperty newAnimationProperty = new AnimationProperty(property.Name, property.PropertyType.Name);
                        AnimTool.CurAnimation.Properties.Add(newAnimationProperty); // temp
                        AnimTool.CurProperty = newAnimationProperty;
                    }

                }
                // TEMPORARY CODE -- WILL BE OPTIMISED
            }

            Close();
        }
    }
}