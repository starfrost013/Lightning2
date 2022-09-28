namespace AnimTool
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.animationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setLengthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAPIReferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesTabPage = new System.Windows.Forms.TabPage();
            this.animationLengthLabel = new System.Windows.Forms.Label();
            this.propertyTypeTextLabel = new System.Windows.Forms.Label();
            this.propertyNameTextLabel = new System.Windows.Forms.Label();
            this.removeBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.valueListBox = new System.Windows.Forms.TextBox();
            this.valueLabel = new System.Windows.Forms.Label();
            this.propertyTypeLabel = new System.Windows.Forms.Label();
            this.propertyNameLabel = new System.Windows.Forms.Label();
            this.keyframeLabel = new System.Windows.Forms.Label();
            this.keyframeListBox = new System.Windows.Forms.ListBox();
            this.propertiesTabControl = new System.Windows.Forms.TabControl();
            this.menuStrip1.SuspendLayout();
            this.propertiesTabPage.SuspendLayout();
            this.propertiesTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.animationToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadJSONToolStripMenuItem,
            this.exportJSONToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadJSONToolStripMenuItem
            // 
            this.loadJSONToolStripMenuItem.Name = "loadJSONToolStripMenuItem";
            this.loadJSONToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadJSONToolStripMenuItem.Text = "Load to JSON";
            // 
            // exportJSONToolStripMenuItem
            // 
            this.exportJSONToolStripMenuItem.Name = "exportJSONToolStripMenuItem";
            this.exportJSONToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportJSONToolStripMenuItem.Text = "Save to JSON";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // animationToolStripMenuItem
            // 
            this.animationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setLengthToolStripMenuItem,
            this.addNewToolStripMenuItem,
            this.removeCurrentToolStripMenuItem,
            this.removeAllToolStripMenuItem});
            this.animationToolStripMenuItem.Name = "animationToolStripMenuItem";
            this.animationToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.animationToolStripMenuItem.Text = "Animation";
            // 
            // setLengthToolStripMenuItem
            // 
            this.setLengthToolStripMenuItem.Name = "setLengthToolStripMenuItem";
            this.setLengthToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.setLengthToolStripMenuItem.Text = "Set Length";
            // 
            // addNewToolStripMenuItem
            // 
            this.addNewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertyToolStripMenuItem,
            this.keyframeToolStripMenuItem});
            this.addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            this.addNewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addNewToolStripMenuItem.Text = "Create New";
            // 
            // propertyToolStripMenuItem
            // 
            this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
            this.propertyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.propertyToolStripMenuItem.Text = "Property";
            // 
            // keyframeToolStripMenuItem
            // 
            this.keyframeToolStripMenuItem.Name = "keyframeToolStripMenuItem";
            this.keyframeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.keyframeToolStripMenuItem.Text = "Keyframe";
            // 
            // removeCurrentToolStripMenuItem
            // 
            this.removeCurrentToolStripMenuItem.Name = "removeCurrentToolStripMenuItem";
            this.removeCurrentToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeCurrentToolStripMenuItem.Text = "Remove Current";
            // 
            // removeAllToolStripMenuItem
            // 
            this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeAllToolStripMenuItem.Text = "Reset";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewAPIReferenceToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // viewAPIReferenceToolStripMenuItem
            // 
            this.viewAPIReferenceToolStripMenuItem.Name = "viewAPIReferenceToolStripMenuItem";
            this.viewAPIReferenceToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewAPIReferenceToolStripMenuItem.Text = "View API Reference";
            this.viewAPIReferenceToolStripMenuItem.Click += new System.EventHandler(this.viewAPIReferenceToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // propertiesTabPage
            // 
            this.propertiesTabPage.Controls.Add(this.animationLengthLabel);
            this.propertiesTabPage.Controls.Add(this.propertyTypeTextLabel);
            this.propertiesTabPage.Controls.Add(this.propertyNameTextLabel);
            this.propertiesTabPage.Controls.Add(this.removeBtn);
            this.propertiesTabPage.Controls.Add(this.addBtn);
            this.propertiesTabPage.Controls.Add(this.valueListBox);
            this.propertiesTabPage.Controls.Add(this.valueLabel);
            this.propertiesTabPage.Controls.Add(this.propertyTypeLabel);
            this.propertiesTabPage.Controls.Add(this.propertyNameLabel);
            this.propertiesTabPage.Controls.Add(this.keyframeLabel);
            this.propertiesTabPage.Controls.Add(this.keyframeListBox);
            this.propertiesTabPage.Location = new System.Drawing.Point(4, 24);
            this.propertiesTabPage.Name = "propertiesTabPage";
            this.propertiesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.propertiesTabPage.Size = new System.Drawing.Size(721, 373);
            this.propertiesTabPage.TabIndex = 0;
            this.propertiesTabPage.Text = "Placeholder";
            this.propertiesTabPage.UseVisualStyleBackColor = true;
            // 
            // animationLengthLabel
            // 
            this.animationLengthLabel.AutoSize = true;
            this.animationLengthLabel.Location = new System.Drawing.Point(550, 355);
            this.animationLengthLabel.Name = "animationLengthLabel";
            this.animationLengthLabel.Size = new System.Drawing.Size(171, 15);
            this.animationLengthLabel.TabIndex = 2;
            this.animationLengthLabel.Text = "Animation Length: Placeholder";
            this.animationLengthLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // propertyTypeTextLabel
            // 
            this.propertyTypeTextLabel.AutoSize = true;
            this.propertyTypeTextLabel.Location = new System.Drawing.Point(357, 65);
            this.propertyTypeTextLabel.Name = "propertyTypeTextLabel";
            this.propertyTypeTextLabel.Size = new System.Drawing.Size(69, 15);
            this.propertyTypeTextLabel.TabIndex = 10;
            this.propertyTypeTextLabel.Text = "Placeholder";
            // 
            // propertyNameTextLabel
            // 
            this.propertyNameTextLabel.AutoSize = true;
            this.propertyNameTextLabel.Location = new System.Drawing.Point(357, 35);
            this.propertyNameTextLabel.Name = "propertyNameTextLabel";
            this.propertyNameTextLabel.Size = new System.Drawing.Size(69, 15);
            this.propertyNameTextLabel.TabIndex = 9;
            this.propertyNameTextLabel.Text = "Placeholder";
            // 
            // removeBtn
            // 
            this.removeBtn.Location = new System.Drawing.Point(98, 319);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(86, 23);
            this.removeBtn.TabIndex = 7;
            this.removeBtn.Text = "Remove";
            this.removeBtn.UseVisualStyleBackColor = true;
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(6, 319);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(86, 23);
            this.addBtn.TabIndex = 6;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            // 
            // valueListBox
            // 
            this.valueListBox.Location = new System.Drawing.Point(357, 94);
            this.valueListBox.Name = "valueListBox";
            this.valueListBox.Size = new System.Drawing.Size(358, 23);
            this.valueListBox.TabIndex = 5;
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(262, 97);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(38, 15);
            this.valueLabel.TabIndex = 4;
            this.valueLabel.Text = "Value:";
            // 
            // propertyTypeLabel
            // 
            this.propertyTypeLabel.AutoSize = true;
            this.propertyTypeLabel.Location = new System.Drawing.Point(262, 65);
            this.propertyTypeLabel.Name = "propertyTypeLabel";
            this.propertyTypeLabel.Size = new System.Drawing.Size(85, 15);
            this.propertyTypeLabel.TabIndex = 3;
            this.propertyTypeLabel.Text = "Property Type: ";
            // 
            // propertyNameLabel
            // 
            this.propertyNameLabel.AutoSize = true;
            this.propertyNameLabel.Location = new System.Drawing.Point(262, 35);
            this.propertyNameLabel.Name = "propertyNameLabel";
            this.propertyNameLabel.Size = new System.Drawing.Size(90, 15);
            this.propertyNameLabel.TabIndex = 2;
            this.propertyNameLabel.Text = "Property Name:";
            // 
            // keyframeLabel
            // 
            this.keyframeLabel.AutoSize = true;
            this.keyframeLabel.Location = new System.Drawing.Point(6, 6);
            this.keyframeLabel.Name = "keyframeLabel";
            this.keyframeLabel.Size = new System.Drawing.Size(65, 15);
            this.keyframeLabel.TabIndex = 1;
            this.keyframeLabel.Text = "Keyframes:";
            // 
            // keyframeListBox
            // 
            this.keyframeListBox.FormattingEnabled = true;
            this.keyframeListBox.ItemHeight = 15;
            this.keyframeListBox.Location = new System.Drawing.Point(6, 24);
            this.keyframeListBox.Name = "keyframeListBox";
            this.keyframeListBox.Size = new System.Drawing.Size(178, 289);
            this.keyframeListBox.TabIndex = 0;
            // 
            // propertiesTabControl
            // 
            this.propertiesTabControl.Controls.Add(this.propertiesTabPage);
            this.propertiesTabControl.Location = new System.Drawing.Point(30, 27);
            this.propertiesTabControl.Name = "propertiesTabControl";
            this.propertiesTabControl.SelectedIndex = 0;
            this.propertiesTabControl.Size = new System.Drawing.Size(729, 401);
            this.propertiesTabControl.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.propertiesTabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Lightning Animation Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.propertiesTabPage.ResumeLayout(false);
            this.propertiesTabPage.PerformLayout();
            this.propertiesTabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadJSONToolStripMenuItem;
        private ToolStripMenuItem exportJSONToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem animationToolStripMenuItem;
        private ToolStripMenuItem addNewToolStripMenuItem;
        private ToolStripMenuItem propertyToolStripMenuItem;
        private ToolStripMenuItem keyframeToolStripMenuItem;
        private ToolStripMenuItem removeCurrentToolStripMenuItem;
        private ToolStripMenuItem removeAllToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem viewAPIReferenceToolStripMenuItem;
        private TabPage propertiesTabPage;
        private Label keyframeLabel;
        private ListBox keyframeListBox;
        private TabControl propertiesTabControl;
        private Button removeBtn;
        private Button addBtn;
        private TextBox valueListBox;
        private Label valueLabel;
        private Label propertyTypeLabel;
        private Label propertyNameLabel;
        private ToolStripMenuItem setLengthToolStripMenuItem;
        private Label animationLengthLabel;
        private Label propertyTypeTextLabel;
        private Label propertyNameTextLabel;
    }
}