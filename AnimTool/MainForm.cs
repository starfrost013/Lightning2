using LightningGL;
using NuCore.Utilities;
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
                AddPropertyForm addPropertyForm = new AddPropertyForm();
                addPropertyForm.ShowDialog();
                FullUpdateTabContent();
            }
        }

        private void setLengthToolStripMenuItem_Click(object sender, EventArgs e) => StartSetLengthWindow();

        private void StartSetLengthWindow()
        {
            if (AnimTool.CurAnimation != null)
            {
                SetLengthForm setLengthForm = new SetLengthForm();
                setLengthForm.ShowDialog();
                UpdateTabContent();
            }
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

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e) => RemoveAll();


        private void RemoveAll()
        {
            if (AnimTool.CurAnimation != null 
                && AnimTool.CurProperty != null)
            {
                AnimTool.CurAnimation.Properties.Clear();
                AnimTool.CurProperty = null;
                FullUpdateTabContent();
            }
        }

        private void keyframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AnimTool.CurAnimation != null 
                && AnimTool.CurProperty != null)
            {
                AddKeyframeForm addKeyframeForm = new();
                addKeyframeForm.ShowDialog();
                UpdateTabContent();
            }
        }

        private void loadJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (AnimTool.CurAnimation != null)
                {
                    OpenFileDialog openFileDialog = new();

                    openFileDialog.Title = "Select Animation JSON files";
                    openFileDialog.Filter = "Lightning Animation JSON (.json)|*.json";

                    openFileDialog.ShowDialog();

                    if (!string.IsNullOrWhiteSpace(openFileDialog.FileName))
                    {
                        AnimTool.CurAnimation = new Animation(openFileDialog.FileName);
                        AnimTool.Load();
                        FullUpdateTabContent();
                    }
                }
            }
            catch
            {
                _ = new NCException("An error occurred while loading the JSON file.", 176, "An exception occurred in AnimTool::Load", NCExceptionSeverity.Error);
                AnimTool.CurAnimation = null;
                AnimTool.CurProperty = null;
                FullUpdateTabContent();
                return;
            }
        }

        private void exportJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (AnimTool.CurAnimation != null
                    && AnimTool.CurProperty != null)
                {
                    SaveFileDialog saveFileDialog = new();

                    saveFileDialog.Title = "Select JSON file location";
                    saveFileDialog.Filter = "Lightning Animation JSON (.json)|*.json";

                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                    {
                        AnimTool.CurAnimation.Path = saveFileDialog.FileName;
                        AnimTool.Save();
                        FullUpdateTabContent();
                    }
                }
            }
            catch
            {
                _ = new NCException("An error occurred while saving the JSON file.", 177, "An exception occurred in AnimTool::Save", NCExceptionSeverity.Error);
                return;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) => RemoveAll();

        private void removeCurrentPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // iterate through so we can move to the one previos
            if (AnimTool.CurAnimation != null
                && AnimTool.CurProperty != null)
            {
                for (int propertyId = 0; propertyId < AnimTool.CurAnimation.Properties.Count; propertyId++)
                {
                    AnimationProperty animProperty = AnimTool.CurAnimation.Properties[propertyId];

                    if (animProperty == AnimTool.CurProperty)
                    {
                        AnimTool.CurAnimation.Properties.Remove(animProperty);

                        // select the last one OR null 9god i ahte UI programming)
                        if (AnimTool.CurAnimation.Properties.Count == 0)
                        {
                            AnimTool.CurProperty = null;
                        }
                        else
                        {
                            AnimTool.CurProperty = AnimTool.CurAnimation.Properties[AnimTool.CurAnimation.Properties.Count - 1];
                        }
                    }
                }

                FullUpdateTabContent();
            }
        }

        private void MainForm_Load(object sender, EventArgs e) => StartSetLengthWindow();

        private void changeSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}