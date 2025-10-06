namespace VeterinaryClinic.WinFormsUserInterface
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnShowSchedule = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnManageOwners = new System.Windows.Forms.Button();
            this.btnManagePets = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnShowSchedule
            // 
            this.btnShowSchedule.Location = new System.Drawing.Point(307, 165);
            this.btnShowSchedule.Name = "btnShowSchedule";
            this.btnShowSchedule.Size = new System.Drawing.Size(184, 88);
            this.btnShowSchedule.TabIndex = 7;
            this.btnShowSchedule.Text = "Расписание ветеринаров";
            this.btnShowSchedule.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(332, 366);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(143, 36);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnManageOwners
            // 
            this.btnManageOwners.Location = new System.Drawing.Point(403, 49);
            this.btnManageOwners.Name = "btnManageOwners";
            this.btnManageOwners.Size = new System.Drawing.Size(210, 110);
            this.btnManageOwners.TabIndex = 5;
            this.btnManageOwners.Text = "Хозяева";
            this.btnManageOwners.UseVisualStyleBackColor = true;
            // 
            // btnManagePets
            // 
            this.btnManagePets.Location = new System.Drawing.Point(188, 49);
            this.btnManagePets.Name = "btnManagePets";
            this.btnManagePets.Size = new System.Drawing.Size(200, 110);
            this.btnManagePets.TabIndex = 4;
            this.btnManagePets.Text = "Питомцы";
            this.btnManagePets.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnShowSchedule);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnManageOwners);
            this.Controls.Add(this.btnManagePets);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnShowSchedule;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnManageOwners;
        private System.Windows.Forms.Button btnManagePets;
    }
}

