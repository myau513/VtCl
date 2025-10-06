namespace VeterinaryClinic.WinFormsUserInterface
{
    partial class PetForm
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
            this.buttonShowHistory = new System.Windows.Forms.Button();
            this.btnCreateAppointment = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxOwners = new System.Windows.Forms.ComboBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnDeletePet = new System.Windows.Forms.Button();
            this.btnEditPet = new System.Windows.Forms.Button();
            this.btnAddPet = new System.Windows.Forms.Button();
            this.dataGridViewPets = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPets)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonShowHistory
            // 
            this.buttonShowHistory.Location = new System.Drawing.Point(598, 207);
            this.buttonShowHistory.Name = "buttonShowHistory";
            this.buttonShowHistory.Size = new System.Drawing.Size(149, 55);
            this.buttonShowHistory.TabIndex = 24;
            this.buttonShowHistory.Text = "История записей питомца";
            this.buttonShowHistory.UseVisualStyleBackColor = true;
            // 
            // btnCreateAppointment
            // 
            this.btnCreateAppointment.Location = new System.Drawing.Point(598, 117);
            this.btnCreateAppointment.Name = "btnCreateAppointment";
            this.btnCreateAppointment.Size = new System.Drawing.Size(149, 58);
            this.btnCreateAppointment.TabIndex = 23;
            this.btnCreateAppointment.Text = "Записать на прием";
            this.btnCreateAppointment.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(243, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Питомцы:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(253, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Хозяин:";
            // 
            // comboBoxOwners
            // 
            this.comboBoxOwners.FormattingEnabled = true;
            this.comboBoxOwners.Location = new System.Drawing.Point(129, 52);
            this.comboBoxOwners.Name = "comboBoxOwners";
            this.comboBoxOwners.Size = new System.Drawing.Size(284, 21);
            this.comboBoxOwners.TabIndex = 20;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(356, 381);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(182, 36);
            this.btnBack.TabIndex = 19;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = true;
            // 
            // btnDeletePet
            // 
            this.btnDeletePet.BackColor = System.Drawing.Color.Coral;
            this.btnDeletePet.Location = new System.Drawing.Point(339, 282);
            this.btnDeletePet.Name = "btnDeletePet";
            this.btnDeletePet.Size = new System.Drawing.Size(95, 41);
            this.btnDeletePet.TabIndex = 18;
            this.btnDeletePet.Text = "Удалить";
            this.btnDeletePet.UseVisualStyleBackColor = false;
            // 
            // btnEditPet
            // 
            this.btnEditPet.BackColor = System.Drawing.Color.Yellow;
            this.btnEditPet.Location = new System.Drawing.Point(237, 282);
            this.btnEditPet.Name = "btnEditPet";
            this.btnEditPet.Size = new System.Drawing.Size(96, 41);
            this.btnEditPet.TabIndex = 17;
            this.btnEditPet.Text = "Изменить";
            this.btnEditPet.UseVisualStyleBackColor = false;
            // 
            // btnAddPet
            // 
            this.btnAddPet.BackColor = System.Drawing.Color.Chartreuse;
            this.btnAddPet.Location = new System.Drawing.Point(133, 282);
            this.btnAddPet.Name = "btnAddPet";
            this.btnAddPet.Size = new System.Drawing.Size(98, 41);
            this.btnAddPet.TabIndex = 16;
            this.btnAddPet.Text = "Добавить";
            this.btnAddPet.UseVisualStyleBackColor = false;
            // 
            // dataGridViewPets
            // 
            this.dataGridViewPets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column4,
            this.Column2});
            this.dataGridViewPets.Location = new System.Drawing.Point(54, 117);
            this.dataGridViewPets.Name = "dataGridViewPets";
            this.dataGridViewPets.Size = new System.Drawing.Size(443, 145);
            this.dataGridViewPets.TabIndex = 15;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Id";
            this.Column1.HeaderText = "ID";
            this.Column1.Name = "Column1";
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "Species";
            this.Column3.HeaderText = "Вид";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "Breed";
            this.Column4.HeaderText = "Порода";
            this.Column4.Name = "Column4";
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Name";
            this.Column2.HeaderText = "Кличка";
            this.Column2.Name = "Column2";
            // 
            // PetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonShowHistory);
            this.Controls.Add(this.btnCreateAppointment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxOwners);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnDeletePet);
            this.Controls.Add(this.btnEditPet);
            this.Controls.Add(this.btnAddPet);
            this.Controls.Add(this.dataGridViewPets);
            this.Name = "PetForm";
            this.Text = "PetForm";
            this.Load += new System.EventHandler(this.PetForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonShowHistory;
        private System.Windows.Forms.Button btnCreateAppointment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxOwners;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnDeletePet;
        private System.Windows.Forms.Button btnEditPet;
        private System.Windows.Forms.Button btnAddPet;
        private System.Windows.Forms.DataGridView dataGridViewPets;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}