namespace AnimTool
{
    partial class AddPropertyForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.propertiesText = new System.Windows.Forms.Label();
            this.propertiesComboBox = new System.Windows.Forms.ComboBox();
            this.addBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 65);
            this.label1.TabIndex = 0;
            this.label1.Text = "Add Property";
            // 
            // propertiesText
            // 
            this.propertiesText.AutoSize = true;
            this.propertiesText.Location = new System.Drawing.Point(22, 89);
            this.propertiesText.Name = "propertiesText";
            this.propertiesText.Size = new System.Drawing.Size(55, 15);
            this.propertiesText.TabIndex = 1;
            this.propertiesText.Text = "Property:";
            // 
            // propertiesComboBox
            // 
            this.propertiesComboBox.FormattingEnabled = true;
            this.propertiesComboBox.Location = new System.Drawing.Point(83, 86);
            this.propertiesComboBox.Name = "propertiesComboBox";
            this.propertiesComboBox.Size = new System.Drawing.Size(221, 23);
            this.propertiesComboBox.TabIndex = 2;
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(229, 115);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 23);
            this.addBtn.TabIndex = 3;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            // 
            // AddPropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 152);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.propertiesComboBox);
            this.Controls.Add(this.propertiesText);
            this.Controls.Add(this.label1);
            this.Name = "AddPropertyForm";
            this.Text = "Add Property";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label propertiesText;
        private ComboBox propertiesComboBox;
        private Button addBtn;
    }
}