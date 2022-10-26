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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddKeyframeForm));
            this.addKeyframeText = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.lengthTrackBar = new System.Windows.Forms.TrackBar();
            this.positionText = new System.Windows.Forms.Label();
            this.zeroMsText = new System.Windows.Forms.Label();
            this.lengthText = new System.Windows.Forms.Label();
            this.initialValueLabel = new System.Windows.Forms.Label();
            this.initialValueTextBox = new System.Windows.Forms.TextBox();
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
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(363, 162);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // lengthTrackBar
            // 
            this.lengthTrackBar.Location = new System.Drawing.Point(91, 98);
            this.lengthTrackBar.Name = "lengthTrackBar";
            this.lengthTrackBar.Size = new System.Drawing.Size(215, 45);
            this.lengthTrackBar.TabIndex = 5;
            // 
            // positionText
            // 
            this.positionText.AutoSize = true;
            this.positionText.Location = new System.Drawing.Point(12, 98);
            this.positionText.Name = "positionText";
            this.positionText.Size = new System.Drawing.Size(53, 15);
            this.positionText.TabIndex = 6;
            this.positionText.Text = "Position:";
            // 
            // zeroMsText
            // 
            this.zeroMsText.AutoSize = true;
            this.zeroMsText.Location = new System.Drawing.Point(71, 98);
            this.zeroMsText.Name = "zeroMsText";
            this.zeroMsText.Size = new System.Drawing.Size(29, 15);
            this.zeroMsText.TabIndex = 7;
            this.zeroMsText.Text = "0ms";
            // 
            // lengthText
            // 
            this.lengthText.AutoSize = true;
            this.lengthText.Location = new System.Drawing.Point(301, 98);
            this.lengthText.Name = "lengthText";
            this.lengthText.Size = new System.Drawing.Size(44, 15);
            this.lengthText.TabIndex = 8;
            this.lengthText.Text = "Length";
            // 
            // initialValueLabel
            // 
            this.initialValueLabel.AutoSize = true;
            this.initialValueLabel.Location = new System.Drawing.Point(12, 134);
            this.initialValueLabel.Name = "initialValueLabel";
            this.initialValueLabel.Size = new System.Drawing.Size(70, 15);
            this.initialValueLabel.TabIndex = 9;
            this.initialValueLabel.Text = "Initial Value:";
            // 
            // initialValueTextBox
            // 
            this.initialValueTextBox.Location = new System.Drawing.Point(91, 131);
            this.initialValueTextBox.Name = "initialValueTextBox";
            this.initialValueTextBox.Size = new System.Drawing.Size(254, 23);
            this.initialValueTextBox.TabIndex = 10;
            // 
            // AddKeyframeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(450, 197);
            this.ControlBox = false;
            this.Controls.Add(this.initialValueTextBox);
            this.Controls.Add(this.initialValueLabel);
            this.Controls.Add(this.lengthText);
            this.Controls.Add(this.zeroMsText);
            this.Controls.Add(this.positionText);
            this.Controls.Add(this.lengthTrackBar);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.addKeyframeText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddKeyframeForm";
            this.Text = "Add Keyframe";
            ((System.ComponentModel.ISupportInitialize)(this.lengthTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label addKeyframeText;
        private Button addButton;
        private TrackBar lengthTrackBar;
        private Label positionText;
        private Label zeroMsText;
        private Label lengthText;
        private Label initialValueLabel;
        private TextBox initialValueTextBox;
    }
}