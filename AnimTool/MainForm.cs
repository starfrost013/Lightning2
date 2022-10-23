using LightningGL;
using System.Diagnostics;
using System.Windows.Forms;

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
            if (AnimTool.CurAnimation != null)
            {
                // dumb check to make sure we actually added a property
                int curCount = AnimTool.CurAnimation.Properties.Count;

                AddPropertyForm addPropertyForm = new AddPropertyForm();

                // ui code is hell
                // it will dispose itself if we close it
                if (!addPropertyForm.IsDisposed)
                {
                    addPropertyForm.ShowDialog();
                    if (AnimTool.CurAnimation.Properties.Count > curCount) FullUpdateTabContent();
                }

            }
        }

        private void setLengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLengthForm setLengthForm = new SetLengthForm();
            setLengthForm.ShowDialog();
            UpdateTabContent();
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
                if (!string.IsNullOrWhiteSpace(AnimTool.CurAnimation.Path)) Text = $"Lightning Animation Editor - {AnimTool.CurAnimation.Path}";

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
            }
        }

        private void UpdateTabContent()
        {
            foreach (TabPage tabPage in propertiesTabControl.TabPages)
            {
                // safeguard
                if (tabPage.Controls.Count > 0
                    && tabPage.Controls[0].GetType().IsAssignableFrom(typeof(TabContent))) // terrorism I HATE UI UI IS THE ENEMY
                {
                    // we only ever add one control here so we can use the index
                    TabContent control = (TabContent)tabPage.Controls[0];

                    control.UpdateTabContent();
                }

            }
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AnimTool.CurAnimation != null)
            {
                AnimTool.CurAnimation.Properties.Clear();
                AnimTool.CurProperty = null;
                FullUpdateTabContent();
            }
        }

        private void keyframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AnimTool.CurAnimation != null)
            {
                AddKeyframeForm addKeyframeForm = new();
                addKeyframeForm.ShowDialog();
                UpdateTabContent();
            }
        }

        private void loadJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AnimTool.CurAnimation != null)
            {
                OpenFileDialog openFileDialog = new();

                openFileDialog.Title = "Select Animation JSON files";
                openFileDialog.Filter = "Lightning Animation JSON (.json,.ljson)|*.json,*.ljson";

                openFileDialog.ShowDialog();

                if (!string.IsNullOrWhiteSpace(openFileDialog.FileName))
                {
                    AnimTool.CurAnimation = new Animation(openFileDialog.FileName);
                    AnimTool.Load();
                    FullUpdateTabContent();
                }
            }
        }

        private void exportJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AnimTool.CurAnimation != null)
            {
                SaveFileDialog saveFileDialog = new();

                saveFileDialog.Title = "Select JSON file location";
                saveFileDialog.Filter = "Lightning Animation JSON (.json,.ljson)|*.json,*.ljson";

                saveFileDialog.ShowDialog();

                if (!string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                {
                    AnimTool.CurAnimation.Path = saveFileDialog.FileName;
                    AnimTool.Save();
                    FullUpdateTabContent();
                }
            }
        }
    }
}