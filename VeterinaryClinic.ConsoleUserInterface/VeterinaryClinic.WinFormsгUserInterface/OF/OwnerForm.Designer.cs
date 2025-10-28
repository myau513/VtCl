namespace VeterinaryClinic.WinFormsUserInterface
{
    partial class OwnerForm
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
            this.btnBack = new System.Windows.Forms.Button();
            this.btnDeleteOwner = new System.Windows.Forms.Button();
            this.btnEditOwner = new System.Windows.Forms.Button();
            this.btnAddOwner = new System.Windows.Forms.Button();
            this.dataGridViewOwners = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOwners)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(314, 369);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(182, 36);
            this.btnBack.TabIndex = 9;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = true;
            // 
            // btnDeleteOwner
            // 
            this.btnDeleteOwner.BackColor = System.Drawing.Color.Coral;
            this.btnDeleteOwner.Location = new System.Drawing.Point(533, 284);
            this.btnDeleteOwner.Name = "btnDeleteOwner";
            this.btnDeleteOwner.Size = new System.Drawing.Size(166, 41);
            this.btnDeleteOwner.TabIndex = 8;
            this.btnDeleteOwner.Text = "Удалить";
            this.btnDeleteOwner.UseVisualStyleBackColor = false;
            // 
            // btnEditOwner
            // 
            this.btnEditOwner.BackColor = System.Drawing.Color.Yellow;
            this.btnEditOwner.Location = new System.Drawing.Point(314, 284);
            this.btnEditOwner.Name = "btnEditOwner";
            this.btnEditOwner.Size = new System.Drawing.Size(182, 41);
            this.btnEditOwner.TabIndex = 7;
            this.btnEditOwner.Text = "Изменить";
            this.btnEditOwner.UseVisualStyleBackColor = false;
            // 
            // btnAddOwner
            // 
            this.btnAddOwner.BackColor = System.Drawing.Color.Chartreuse;
            this.btnAddOwner.Location = new System.Drawing.Point(101, 284);
            this.btnAddOwner.Name = "btnAddOwner";
            this.btnAddOwner.Size = new System.Drawing.Size(161, 41);
            this.btnAddOwner.TabIndex = 6;
            this.btnAddOwner.Text = "Добавить";
            this.btnAddOwner.UseVisualStyleBackColor = false;
            // 
            // dataGridViewOwners
            // 
            this.dataGridViewOwners.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOwners.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dataGridViewOwners.Location = new System.Drawing.Point(101, 46);
            this.dataGridViewOwners.Name = "dataGridViewOwners";
            this.dataGridViewOwners.Size = new System.Drawing.Size(598, 176);
            this.dataGridViewOwners.TabIndex = 5;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Id";
            this.Column1.HeaderText = "ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "FullName";
            this.Column2.HeaderText = "ФИО";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "PhoneNumber";
            this.Column3.HeaderText = "Номер телефона";
            this.Column3.Name = "Column3";
            // 
            // OwnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnDeleteOwner);
            this.Controls.Add(this.btnEditOwner);
            this.Controls.Add(this.btnAddOwner);
            this.Controls.Add(this.dataGridViewOwners);
            this.Name = "OwnerForm";
            this.Text = "OwnerForm";
            this.Load += new System.EventHandler(this.OwnerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOwners)).EndInit();
            this.ResumeLayout(false);

            this.btnBack.Click += btnBack_Click;
            this.btnDeleteOwner.Click += btnDeleteOwner_Click;
            this.btnEditOwner.Click += btnEditOwner_Click;
            this.btnAddOwner.Click += btnAddOwner_Click;
        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnDeleteOwner;
        private System.Windows.Forms.Button btnEditOwner;
        private System.Windows.Forms.Button btnAddOwner;
        private System.Windows.Forms.DataGridView dataGridViewOwners;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}