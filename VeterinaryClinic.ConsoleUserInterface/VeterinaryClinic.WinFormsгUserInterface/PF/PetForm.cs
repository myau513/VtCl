using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VeterinaryClinic.Core.Logic;
using VeterinaryClinic.Core.Models;
using VeterinaryClinic.WinForms.Forms.OwnerForms;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class PetForm : Form
    {
        private readonly ClinicService clinicService;
        private int selectedOwnerId;
        private List<Appointment> appointments = new List<Appointment>();

        public PetForm(ClinicService clinicService)
        {
            InitializeComponent();
            this.clinicService = clinicService;
            dataGridViewPets.AutoGenerateColumns = false;
            LoadOwnersComboBox();
        }

        private void LoadOwnersComboBox()
        {
            var owners = clinicService.GetAllOwners();
            comboBoxOwners.DataSource = owners;
            comboBoxOwners.DisplayMember = "FullName";
            comboBoxOwners.ValueMember = "Id";
        }

        private void LoadPets(int ownerId)
        {
            try
            {
                var pets = clinicService.GetPetsByOwnerId(ownerId);
                dataGridViewPets.DataSource = pets;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки питомцев: " + ex.Message);
            }
        }

        private void btnAddPet_Click(object sender, EventArgs e)
        {
            if (selectedOwnerId == 0)
            {
                MessageBox.Show("Выберите владельца!");
                return;
            }

            var addForm = new AddPetForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    clinicService.CreatePet(addForm.Name, addForm.Species, addForm.Breed, selectedOwnerId);
                    LoadPets(selectedOwnerId);
                    MessageBox.Show("Питомец добавлен!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }


        private void btnEditPet_Click(object sender, EventArgs e)
        {
            if (dataGridViewPets.CurrentRow?.DataBoundItem is Pet selectedPet)
            {
                var editForm = new EditPetForm(selectedPet);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var updatedPet = new Pet(
                            selectedPet.Id,
                            editForm.Name,
                            editForm.Species,
                            editForm.Breed,
                            selectedPet.OwnerId);

                        clinicService.UpdatePet(updatedPet);
                        LoadPets(selectedOwnerId);
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
                MessageBox.Show("Выберите питомца для редактирования!");
            }
        }

        private void btnDeletePet_Click(object sender, EventArgs e)
        {
            if (dataGridViewPets.CurrentRow?.DataBoundItem is Pet selectedPet)
            {
                if (MessageBox.Show($"Удалить питомца {selectedPet.Name}?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        if (clinicService.DeletePet(selectedPet.Id))
                        {
                            LoadPets(selectedOwnerId);
                            MessageBox.Show("Питомец удален!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите питомца для удаления!");
            }
        }

        private void comboBoxOwners_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOwners.SelectedValue != null &&
                comboBoxOwners.SelectedValue is int ownerId)
            {
                selectedOwnerId = ownerId;
                LoadPets(ownerId);
            }
        }

        private void btnCreateAppointment_Click(object sender, EventArgs e)
        {
            if (dataGridViewPets.CurrentRow?.DataBoundItem is Pet selectedPet)
            {
                var appointmentForm = new AppointmentForm(clinicService, selectedPet, appointments);
                if (appointmentForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Запись на прием создана успешно!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Console.WriteLine($"Всего записей после создания: {appointments.Count}");
                }
            }
            else
            {
                MessageBox.Show("Выберите питомца для записи на прием!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonShowHistory_Click(object sender, EventArgs e)
        {
            if (dataGridViewPets.SelectedRows.Count > 0)
            {
                var selectedPet = (Pet)dataGridViewPets.SelectedRows[0].DataBoundItem;
                var historyForm = new PetHistoryForm(clinicService, selectedPet, appointments);
                historyForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите питомца для просмотра истории", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PetForm_Load(object sender, EventArgs e)
        {

        }
    }
}
