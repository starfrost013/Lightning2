using LightningGL;
using System.Diagnostics;

namespace AnimTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Lightning Animation Editor version 1.0", "About Lightning Animation Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void viewAPIReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}\Lightning Software Development Kit\Documentation\API.docx"
            });
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void propertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPropertyForm addPropertyForm = new AddPropertyForm();
            addPropertyForm.ShowDialog();
            FullUpdateTabContent();
        }

        private void propertiesTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AnimTool.CurAnimation != null
                && AnimTool.CurAnimation.Properties.Count > 0
                && propertiesTabControl.SelectedIndex > -1) AnimTool.CurProperty = AnimTool.CurAnimation.Properties[propertiesTabControl.SelectedIndex];
            UpdateTabContent();
        }

        private void FullUpdateTabContent()
        {
            // this is so fucking dumb i hate UI programming and have no idea how to do it
            // so this code is horrible

            propertiesTabControl.TabPages.Clear();    
            
            if (AnimTool.CurAnimation != null)
            {
                foreach (AnimationProperty property in AnimTool.CurAnimation.Properties)
                {
                    TabPage curTabPage = new()
                    {
                        Name = property.Name,
                        Text = property.Name,
                        Size = propertiesTabControl.Size,
                        Margin = propertiesTabControl.Margin,
                    };

                    TabContent content = new();

                    // add the tab content
                    // workaround for vs winforms designer stupidity
                    curTabPage.Controls.Add(content);

                    propertiesTabControl.TabPages.Add(curTabPage);

                    content.UpdateTabContent();
                }

                // temp?
                
            }
        }

        private void UpdateTabContent()
        {
            foreach (TabPage tabPage in propertiesTabControl.TabPages)
            {
                // we only ever add one control here so we can use the index
                TabContent control = (TabContent)tabPage.Controls[0];

                control.UpdateTabContent();
            }
        }
    }
}