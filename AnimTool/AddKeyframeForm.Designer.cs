namespace AnimTool
{
    partial class AddKeyframeForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addKeyframeText = new System.Windows.Forms.Label();
            this.propertyTextBox = new System.Windows.Forms.ComboBox();
            this.addButton = new System.Windows.Forms.Button();
            this.propertyText = new System.Windows.Forms.Label();
            this.lengthTrackBar = new System.Windows.Forms.TrackBar();
            this.positionText = new System.Windows.Forms.Label();
            this.zeroMsText = new System.Windows.Forms.Label();
            this.lengthText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.lengthTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // addKeyframeText
            // 
            this.addKeyframeText.AutoSize = true;
            this.addKeyframeText.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.addKeyframeText.Location = new System.Drawing.Point(2, 9);
            this.addKeyframeText.Name = "addKeyframeText";
            this.addKeyframeText.Size = new System.Drawing.Size(325, 65);
            this.addKeyframeText.TabIndex = 0;
            this.addKeyframeText.Text = "Add Keyframe";
            // 
            // propertyTextBox
            // 
            this.propertyTextBox.FormattingEnabled = true;
            this.propertyTextBox.Location = new System.Drawing.Point(76, 97);
            this.propertyTextBox.Name = "propertyTextBox";
            this.propertyTextBox.Size = new System.Drawing.Size(270, 23);
            this.propertyTextBox.TabIndex = 1;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(242, 193);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // propertyText
            // 
            this.propertyText.AutoSize = true;
            this.propertyText.Location = new System.Drawing.Point(12, 100);
            this.propertyText.Name = "propertyText";
            this.propertyText.Size = new System.Drawing.Size(58, 15);
            this.propertyText.TabIndex = 4;
            this.propertyText.Text = "Property: ";
            // 
            // lengthTrackBar
            // 
            this.lengthTrackBar.Location = new System.Drawing.Point(91, 142);
            this.lengthTrackBar.Name = "lengthTrackBar";
            this.lengthTrackBar.Size = new System.Drawing.Size(226, 45);
            this.lengthTrackBar.TabIndex = 5;
            // 
            // positionText
            // 
            this.positionText.AutoSize = true;
            this.positionText.Location = new System.Drawing.Point(12, 142);
            this.positionText.Name = "positionText";
            this.positionText.Size = new System.Drawing.Size(53, 15);
            this.positionText.TabIndex = 6;
            this.positionText.Text = "Position:";
            // 
            // zeroMsText
            // 
            this.zeroMsText.AutoSize = true;
            this.zeroMsText.Location = new System.Drawing.Point(71, 142);
            this.zeroMsText.Name = "zeroMsText";
            this.zeroMsText.Size = new System.Drawing.Size(29, 15);
            this.zeroMsText.TabIndex = 7;
            this.zeroMsText.Text = "0ms";
            // 
            // lengthText
            // 
            this.lengthText.AutoSize = true;
            this.lengthText.Location = new System.Drawing.Point(312, 142);
            this.lengthText.Name = "lengthText";
            this.lengthText.Size = new System.Drawing.Size(44, 15);
            this.lengthText.TabIndex = 8;
            this.lengthText.Text = "Length";
            // 
            // AddKeyframeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 245);
            this.Controls.Add(this.lengthText);
            this.Controls.Add(this.zeroMsText);
            this.Controls.Add(this.positionText);
            this.Controls.Add(this.lengthTrackBar);
            this.Controls.Add(this.propertyText);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.propertyTextBox);
            this.Controls.Add(this.addKeyframeText);
            this.Name = "AddKeyframeForm";
            this.Text = "Add Keyframe";
            ((System.ComponentModel.ISupportInitialize)(this.lengthTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label addKeyframeText;
        private ComboBox propertyTextBox;
        private Button addButton;
        private Label propertyText;
        private TrackBar lengthTrackBar;
        private Label positionText;
        private Label zeroMsText;
        private Label lengthText;
    }
}