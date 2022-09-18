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
    }
}