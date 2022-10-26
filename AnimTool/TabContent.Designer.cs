namespace AnimTool
{
    partial class TabContent
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.animationLengthText = new System.Windows.Forms.Label();
            this.propertyTypeValueText = new System.Windows.Forms.Label();
            this.propertyPositionValueText = new System.Windows.Forms.Label();
            this.removeBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.valueText = new System.Windows.Forms.Label();
            this.propertyTypeText = new System.Windows.Forms.Label();
            this.propertyNameText = new System.Windows.Forms.Label();
            this.keyframeLabel = new System.Windows.Forms.Label();
            this.keyframeListBox = new System.Windows.Forms.ListBox();
            this.valueValueText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // animationLengthText
            // 
            this.animationLengthText.AutoSize = true;
            this.animationLengthText.Location = new System.Drawing.Point(577, 360);
            this.animationLengthText.Name = "animationLengthText";
            this.animationLengthText.Size = new System.Drawing.Size(131, 15);
            this.animationLengthText.TabIndex = 13;
            this.animationLengthText.Text = "Animation Length: N/A";
            this.animationLengthText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // propertyTypeValueText
            // 
            this.propertyTypeValueText.AutoSize = true;
            this.propertyTypeValueText.Location = new System.Drawing.Point(384, 84);
            this.propertyTypeValueText.Name = "propertyTypeValueText";
            this.propertyTypeValueText.Size = new System.Drawing.Size(46, 15);
            this.propertyTypeValueText.TabIndex = 21;
            this.propertyTypeValueText.Text = "Not Set";
            // 
            // propertyPositionValueText
            // 
            this.propertyPositionValueText.AutoSize = true;
            this.propertyPositionValueText.Location = new System.Drawing.Point(384, 54);
            this.propertyPositionValueText.Name = "propertyPositionValueText";
            this.propertyPositionValueText.Size = new System.Drawing.Size(46, 15);
            this.propertyPositionValueText.TabIndex = 20;
            this.propertyPositionValueText.Text = "Not Set";
            // 
            // removeBtn
            // 
            this.removeBtn.Location = new System.Drawing.Point(125, 338);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(86, 23);
            this.removeBtn.TabIndex = 19;
            this.removeBtn.Text = "Remove";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new System.EventHandler(this.removeBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(33, 338);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(86, 23);
            this.addBtn.TabIndex = 18;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // valueText
            // 
            this.valueText.AutoSize = true;
            this.valueText.Location = new System.Drawing.Point(289, 115);
            this.valueText.Name = "valueText";
            this.valueText.Size = new System.Drawing.Size(38, 15);
            this.valueText.TabIndex = 16;
            this.valueText.Text = "Value:";
            // 
            // propertyTypeText
            // 
            this.propertyTypeText.AutoSize = true;
            this.propertyTypeText.Location = new System.Drawing.Point(289, 84);
            this.propertyTypeText.Name = "propertyTypeText";
            this.propertyTypeText.Size = new System.Drawing.Size(85, 15);
            this.propertyTypeText.TabIndex = 15;
            this.propertyTypeText.Text = "Property Type: ";
            // 
            // propertyNameText
            // 
            this.propertyNameText.AutoSize = true;
            this.propertyNameText.Location = new System.Drawing.Point(289, 54);
            this.propertyNameText.Name = "propertyNameText";
            this.propertyNameText.Size = new System.Drawing.Size(53, 15);
            this.propertyNameText.TabIndex = 14;
            this.propertyNameText.Text = "Position:";
            // 
            // keyframeLabel
            // 
            this.keyframeLabel.AutoSize = true;
            this.keyframeLabel.Location = new System.Drawing.Point(33, 25);
            this.keyframeLabel.Name = "keyframeLabel";
            this.keyframeLabel.Size = new System.Drawing.Size(65, 15);
            this.keyframeLabel.TabIndex = 12;
            this.keyframeLabel.Text = "Keyframes:";
            // 
            // keyframeListBox
            // 
            this.keyframeListBox.FormattingEnabled = true;
            this.keyframeListBox.ItemHeight = 15;
            this.keyframeListBox.Location = new System.Drawing.Point(33, 43);
            this.keyframeListBox.Name = "keyframeListBox";
            this.keyframeListBox.Size = new System.Drawing.Size(178, 289);
            this.keyframeListBox.TabIndex = 11;
            this.keyframeListBox.SelectedIndexChanged += new System.EventHandler(this.keyframeListBox_SelectedIndexChanged);
            // 
            // valueValueText
            // 
            this.valueValueText.AutoSize = true;
            this.valueValueText.Location = new System.Drawing.Point(384, 115);
            this.valueValueText.Name = "valueValueText";
            this.valueValueText.Size = new System.Drawing.Size(46, 15);
            this.valueValueText.TabIndex = 22;
            this.valueValueText.Text = "Not Set";
            // 
            // TabContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.valueValueText);
            this.Controls.Add(this.animationLengthText);
            this.Controls.Add(this.propertyTypeValueText);
            this.Controls.Add(this.propertyPositionValueText);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.valueText);
            this.Controls.Add(this.propertyTypeText);
            this.Controls.Add(this.propertyNameText);
            this.Controls.Add(this.keyframeLabel);
            this.Controls.Add(this.keyframeListBox);
            this.Name = "TabContent";
            this.Size = new System.Drawing.Size(757, 399);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label animationLengthText;
        private Label propertyTypeValueText;
        private Label propertyPositionValueText;
        private Button removeBtn;
        private Button addBtn;
        private Label valueText;
        private Label propertyTypeText;
        private Label propertyNameText;
        private Label keyframeLabel;
        private ListBox keyframeListBox;
        private Label valueValueText;
    }
}
