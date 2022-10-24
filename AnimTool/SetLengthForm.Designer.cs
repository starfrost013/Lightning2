namespace AnimTool
{
    partial class SetLengthForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetLengthForm));
            this.setAnimationLengthText = new System.Windows.Forms.Label();
            this.lengthTextBox = new System.Windows.Forms.TextBox();
            this.lengthMsText = new System.Windows.Forms.Label();
            this.changeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // setAnimationLengthText
            // 
            this.setAnimationLengthText.AutoSize = true;
            this.setAnimationLengthText.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.setAnimationLengthText.Location = new System.Drawing.Point(6, 9);
            this.setAnimationLengthText.Name = "setAnimationLengthText";
            this.setAnimationLengthText.Size = new System.Drawing.Size(485, 65);
            this.setAnimationLengthText.TabIndex = 0;
            this.setAnimationLengthText.Text = "Set Animation Length";
            // 
            // lengthTextBox
            // 
            this.lengthTextBox.Location = new System.Drawing.Point(104, 86);
            this.lengthTextBox.Name = "lengthTextBox";
            this.lengthTextBox.Size = new System.Drawing.Size(367, 23);
            this.lengthTextBox.TabIndex = 1;
            // 
            // lengthMsText
            // 
            this.lengthMsText.AutoSize = true;
            this.lengthMsText.Location = new System.Drawing.Point(12, 89);
            this.lengthMsText.Name = "lengthMsText";
            this.lengthMsText.Size = new System.Drawing.Size(74, 15);
            this.lengthMsText.TabIndex = 2;
            this.lengthMsText.Text = "Length (ms):";
            // 
            // changeButton
            // 
            this.changeButton.Location = new System.Drawing.Point(396, 127);
            this.changeButton.Name = "changeButton";
            this.changeButton.Size = new System.Drawing.Size(75, 23);
            this.changeButton.TabIndex = 3;
            this.changeButton.Text = "Change";
            this.changeButton.UseVisualStyleBackColor = true;
            this.changeButton.Click += new System.EventHandler(this.changeButton_Click);
            // 
            // SetLengthForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 167);
            this.Controls.Add(this.changeButton);
            this.Controls.Add(this.lengthMsText);
            this.Controls.Add(this.lengthTextBox);
            this.Controls.Add(this.setAnimationLengthText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetLengthForm";
            this.Text = "Set Animation Length";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label setAnimationLengthText;
        private TextBox lengthTextBox;
        private Label lengthMsText;
        private Button changeButton;
    }
}