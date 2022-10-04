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
            this.SuspendLayout();
            // 
            // animationLengthLabel
            // 
            this.animationLengthLabel.AutoSize = true;
            this.animationLengthLabel.Location = new System.Drawing.Point(589, 358);
            this.animationLengthLabel.Name = "animationLengthLabel";
            this.animationLengthLabel.Size = new System.Drawing.Size(131, 15);
            this.animationLengthLabel.TabIndex = 13;
            this.animationLengthLabel.Text = "Animation Length: N/A";
            this.animationLengthLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // propertyTypeTextLabel
            // 
            this.propertyTypeTextLabel.AutoSize = true;
            this.propertyTypeTextLabel.Location = new System.Drawing.Point(384, 84);
            this.propertyTypeTextLabel.Name = "propertyTypeTextLabel";
            this.propertyTypeTextLabel.Size = new System.Drawing.Size(29, 15);
            this.propertyTypeTextLabel.TabIndex = 21;
            this.propertyTypeTextLabel.Text = "N/A";
            // 
            // propertyNameTextLabel
            // 
            this.propertyNameTextLabel.AutoSize = true;
            this.propertyNameTextLabel.Location = new System.Drawing.Point(384, 54);
            this.propertyNameTextLabel.Name = "propertyNameTextLabel";
            this.propertyNameTextLabel.Size = new System.Drawing.Size(29, 15);
            this.propertyNameTextLabel.TabIndex = 20;
            this.propertyNameTextLabel.Text = "N/A";
            // 
            // removeBtn
            // 
            this.removeBtn.Location = new System.Drawing.Point(125, 338);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(86, 23);
            this.removeBtn.TabIndex = 19;
            this.removeBtn.Text = "Remove";
            this.removeBtn.UseVisualStyleBackColor = true;
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(33, 338);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(86, 23);
            this.addBtn.TabIndex = 18;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            // 
            // valueListBox
            // 
            this.valueListBox.Location = new System.Drawing.Point(384, 113);
            this.valueListBox.Name = "valueListBox";
            this.valueListBox.Size = new System.Drawing.Size(336, 23);
            this.valueListBox.TabIndex = 17;
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(289, 116);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(38, 15);
            this.valueLabel.TabIndex = 16;
            this.valueLabel.Text = "Value:";
            // 
            // propertyTypeLabel
            // 
            this.propertyTypeLabel.AutoSize = true;
            this.propertyTypeLabel.Location = new System.Drawing.Point(289, 84);
            this.propertyTypeLabel.Name = "propertyTypeLabel";
            this.propertyTypeLabel.Size = new System.Drawing.Size(85, 15);
            this.propertyTypeLabel.TabIndex = 15;
            this.propertyTypeLabel.Text = "Property Type: ";
            // 
            // propertyNameLabel
            // 
            this.propertyNameLabel.AutoSize = true;
            this.propertyNameLabel.Location = new System.Drawing.Point(289, 54);
            this.propertyNameLabel.Name = "propertyNameLabel";
            this.propertyNameLabel.Size = new System.Drawing.Size(90, 15);
            this.propertyNameLabel.TabIndex = 14;
            this.propertyNameLabel.Text = "Property Name:";
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
            // 
            // TabContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.animationLengthLabel);
            this.Controls.Add(this.propertyTypeTextLabel);
            this.Controls.Add(this.propertyNameTextLabel);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.valueListBox);
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.propertyTypeLabel);
            this.Controls.Add(this.propertyNameLabel);
            this.Controls.Add(this.keyframeLabel);
            this.Controls.Add(this.keyframeListBox);
            this.Name = "TabContent";
            this.Size = new System.Drawing.Size(757, 399);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label animationLengthLabel;
        private Label propertyTypeTextLabel;
        private Label propertyNameTextLabel;
        private Button removeBtn;
        private Button addBtn;
        private TextBox valueListBox;
        private Label valueLabel;
        private Label propertyTypeLabel;
        private Label propertyNameLabel;
        private Label keyframeLabel;
        private ListBox keyframeListBox;
    }
}
