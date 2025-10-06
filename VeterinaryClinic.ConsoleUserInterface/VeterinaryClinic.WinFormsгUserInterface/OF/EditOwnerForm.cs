// EditOwnerForm.cs
using System;
using System.Windows.Forms;
using VeterinaryClinic.Core.Models;

namespace VeterinaryClinic.WinForms.Forms.OwnerForms
{
    public partial class EditOwnerForm : Form
    {
        public string FullName { get; private set; }
        public string PhoneNumber { get; private set; }

        public EditOwnerForm(Owner owner)
        {
            InitializeComponent();
            txtFullName.Text = owner.FullName;
            txtPhoneNumber.Text = owner.PhoneNumber;
        }

        private void BtnSave_Click(object sender, EventArgs e)
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

        private void EditOwnerForm_Load(object sender, EventArgs e)
        {
        }
    }
}
