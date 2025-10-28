namespace VeterinaryClinic.WinFormsUserInterface
{
    partial class AddOwnerForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnAddOwner = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(279, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Номер телефона";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(279, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "ФИО";
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.Location = new System.Drawing.Point(282, 189);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(240, 20);
            this.txtPhoneNumber.TabIndex = 9;
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(282, 145);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(240, 20);
            this.txtFullName.TabIndex = 8;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(424, 284);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(98, 37);
            this.BtnCancel.TabIndex = 7;
            this.BtnCancel.Text = "Отмена";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnAddOwner
            // 
            this.BtnAddOwner.Location = new System.Drawing.Point(282, 284);
            this.BtnAddOwner.Name = "BtnAddOwner";
            this.BtnAddOwner.Size = new System.Drawing.Size(105, 37);
            this.BtnAddOwner.TabIndex = 6;
            this.BtnAddOwner.Text = "Добавить";
            this.BtnAddOwner.UseVisualStyleBackColor = true;
            // 
            // AddOwnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPhoneNumber);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnAddOwner);
            this.Name = "AddOwnerForm";
            this.Text = "AddOwnerForm";
            this.Load += new System.EventHandler(this.AddOwnerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

            this.BtnAddOwner.Click += BtnAddOwner_Click;
            this.BtnCancel.Click += BtnCancel_Click;
            

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnAddOwner;
    }
}