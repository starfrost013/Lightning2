namespace AnimTool
{
    partial class ChangeSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeSettingsForm));
            this.changeAnimationSettingsText = new System.Windows.Forms.Label();
            this.repeatCount = new System.Windows.Forms.NumericUpDown();
            this.repeatCountText = new System.Windows.Forms.Label();
            this.repeatInfinitely = new System.Windows.Forms.CheckBox();
            this.reverseAnimation = new System.Windows.Forms.CheckBox();
            this.okBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.repeatCount)).BeginInit();
            this.SuspendLayout();
            // 
            // changeAnimationSettingsText
            // 
            this.changeAnimationSettingsText.AutoSize = true;
            this.changeAnimationSettingsText.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.changeAnimationSettingsText.Location = new System.Drawing.Point(7, 9);
            this.changeAnimationSettingsText.Name = "changeAnimationSettingsText";
            this.changeAnimationSettingsText.Size = new System.Drawing.Size(603, 65);
            this.changeAnimationSettingsText.TabIndex = 0;
            this.changeAnimationSettingsText.Text = "Change Animation Settings";
            // 
            // repeatCount
            // 
            this.repeatCount.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.repeatCount.Location = new System.Drawing.Point(121, 79);
            this.repeatCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.repeatCount.Name = "repeatCount";
            this.repeatCount.Size = new System.Drawing.Size(88, 23);
            this.repeatCount.TabIndex = 1;
            this.repeatCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // repeatCountText
            // 
            this.repeatCountText.AutoSize = true;
            this.repeatCountText.Location = new System.Drawing.Point(33, 81);
            this.repeatCountText.Name = "repeatCountText";
            this.repeatCountText.Size = new System.Drawing.Size(82, 15);
            this.repeatCountText.TabIndex = 2;
            this.repeatCountText.Text = "Repeat Count:";
            // 
            // repeatInfinitely
            // 
            this.repeatInfinitely.AutoSize = true;
            this.repeatInfinitely.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.repeatInfinitely.Location = new System.Drawing.Point(21, 106);
            this.repeatInfinitely.Name = "repeatInfinitely";
            this.repeatInfinitely.Size = new System.Drawing.Size(114, 19);
            this.repeatInfinitely.TabIndex = 4;
            this.repeatInfinitely.Text = "Repeat Infinitely:";
            this.repeatInfinitely.UseVisualStyleBackColor = true;
            this.repeatInfinitely.CheckedChanged += new System.EventHandler(this.repeatInfinitely_CheckedChanged);
            // 
            // reverseAnimation
            // 
            this.reverseAnimation.AutoSize = true;
            this.reverseAnimation.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.reverseAnimation.Location = new System.Drawing.Point(7, 131);
            this.reverseAnimation.Name = "reverseAnimation";
            this.reverseAnimation.Size = new System.Drawing.Size(128, 19);
            this.reverseAnimation.TabIndex = 5;
            this.reverseAnimation.Text = "Reverse Animation:";
            this.reverseAnimation.UseVisualStyleBackColor = true;
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(7, 156);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 6;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // ChangeSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 191);
            this.ControlBox = false;
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.reverseAnimation);
            this.Controls.Add(this.repeatInfinitely);
            this.Controls.Add(this.repeatCountText);
            this.Controls.Add(this.repeatCount);
            this.Controls.Add(this.changeAnimationSettingsText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChangeSettingsForm";
            this.Text = "Change Settings";
            this.Load += new System.EventHandler(this.ChangeSettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.repeatCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label changeAnimationSettingsText;
        private NumericUpDown repeatCount;
        private Label repeatCountText;
        private CheckBox repeatInfinitely;
        private CheckBox reverseAnimation;
        private Button okBtn;
    }
}