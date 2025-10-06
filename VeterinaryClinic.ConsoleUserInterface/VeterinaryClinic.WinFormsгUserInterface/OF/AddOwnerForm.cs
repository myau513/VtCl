using System;
using System.Collections.Generic;
using System.ComponentModel;
// AddOwnerForm.cs
using System;
using System.Windows.Forms;

namespace VeterinaryClinic.WinForms.Forms
{
    public partial class AddOwnerForm : Form
    {
        public string FullName { get; private set; }
        public string PhoneNumber { get; private set; }

        public AddOwnerForm()
        {
            InitializeComponent();
        }

        private void BtnAddOwner_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО владельца!");
                return;
            }

            FullName = txtFullName.Text.Trim();
            PhoneNumber = txtPhoneNumber.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AddOwnerForm_Load(object sender, EventArgs e)
        {
        }
    }
}

