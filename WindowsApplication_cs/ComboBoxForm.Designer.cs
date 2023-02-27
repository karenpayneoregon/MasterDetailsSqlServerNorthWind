namespace WindowsApplication_cs
{
    partial class ComboBoxForm
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
            this.CustomerComboBox = new System.Windows.Forms.ComboBox();
            this.OrdersComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // CustomerComboBox
            // 
            this.CustomerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CustomerComboBox.FormattingEnabled = true;
            this.CustomerComboBox.Location = new System.Drawing.Point(25, 28);
            this.CustomerComboBox.Name = "CustomerComboBox";
            this.CustomerComboBox.Size = new System.Drawing.Size(237, 24);
            this.CustomerComboBox.TabIndex = 0;
            // 
            // OrdersComboBox
            // 
            this.OrdersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OrdersComboBox.FormattingEnabled = true;
            this.OrdersComboBox.Location = new System.Drawing.Point(296, 28);
            this.OrdersComboBox.Name = "OrdersComboBox";
            this.OrdersComboBox.Size = new System.Drawing.Size(237, 24);
            this.OrdersComboBox.TabIndex = 1;
            // 
            // ComboBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.OrdersComboBox);
            this.Controls.Add(this.CustomerComboBox);
            this.Name = "ComboBoxForm";
            this.Text = "ComboBoxForm";
            this.Load += new System.EventHandler(this.ComboBoxForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox CustomerComboBox;
        private System.Windows.Forms.ComboBox OrdersComboBox;
    }
}