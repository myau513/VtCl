using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VeterinaryClinic.Core.Logic;
using VeterinaryClinic.Core.Models;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class OwnerForm : Form
    {
        private readonly ClinicService clinicService;

        public OwnerForm(ClinicService clinicService)
        {
            InitializeComponent();
            this.clinicService = clinicService;
            dataGridViewOwners.AutoGenerateColumns = false;
            LoadOwners();
        }

        private void LoadOwners()
        {
            try
            {
                var owners = clinicService.GetAllOwners();

                MessageBox.Show($"Из базы получено {owners.Count} владельцев");

                foreach (var owner in owners)
                {
                    Console.WriteLine($"{owner.Id}: {owner.FullName}, {owner.PhoneNumber}");
                }

                dataGridViewOwners.DataSource = null;
                dataGridViewOwners.DataSource = owners;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void btnAddOwner_Click(object sender, EventArgs e)
        {
            var addForm = new AddOwnerForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Owner newOwner = clinicService.CreateOwner(addForm.FullName, addForm.PhoneNumber);
                    if (newOwner != null)
                    {
                        MessageBox.Show($"Владелец {newOwner.FullName} добавлен с ID: {newOwner.Id}");
                        LoadOwners();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось добавить владельца");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении: " + ex.Message);
                }
            }
        }

        private void btnEditOwner_Click(object sender, EventArgs e)
        {
            if (dataGridViewOwners.CurrentRow?.DataBoundItem is Owner selectedOwner)
            {
                var editForm = new EditOwnerForm(selectedOwner);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        selectedOwner.FullName = editForm.FullName;
                        selectedOwner.PhoneNumber = editForm.PhoneNumber;
                        clinicService.UpdateOwner(selectedOwner);
                        LoadOwners();
                        MessageBox.Show("Данные обновлены!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите владельца для редактирования!");
            }
        }

        private void btnDeleteOwner_Click(object sender, EventArgs e)
        {
            if (dataGridViewOwners.CurrentRow?.DataBoundItem is Owner selectedOwner)
            {
                try
                {
                    var pets = clinicService.GetPetsByOwnerId(selectedOwner.Id);
                    if (pets.Count > 0)
                    {
                        MessageBox.Show("Нельзя удалить владельца с питомцами!");
                        return;
                    }

                    if (MessageBox.Show($"Удалить владельца {selectedOwner.FullName}?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        bool deleted = clinicService.DeleteOwner(selectedOwner.Id);
                        if (deleted)
                        {
                            LoadOwners();
                            MessageBox.Show("Владелец удален!");
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при удалении владельца");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите владельца для удаления!");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OwnerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
