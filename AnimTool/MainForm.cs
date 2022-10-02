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
        }
    }
}