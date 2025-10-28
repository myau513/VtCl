using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VeterinaryClinic.Core.Logic;
using VeterinaryClinic.Core.Essence;
using VeterinaryClinic.WinFormsUserInterface;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class PetForm : Form
    {
        private readonly ClinicService clinicService;
        private int selectedOwnerId;
        private List<Appointment> appointments = new List<Appointment>();

        /// <summary>
        /// Создает форму для работы с питомцами
        /// </summary>
        /// <param name="clinicService">Сервис клиники для работы с данными</param>
        public PetForm(ClinicService clinicService)
        {
            InitializeComponent();
            this.clinicService = clinicService;
            dataGridViewPets.AutoGenerateColumns = false;
            LoadOwnersComboBox();
        }

        /// <summary>
        /// Загружает список владельцев в выпадающий список
        /// </summary>
        private void LoadOwnersComboBox()
        {
            var owners = clinicService.GetAllOwners();
            comboBoxOwners.DataSource = owners;
            comboBoxOwners.DisplayMember = "FullName";
            comboBoxOwners.ValueMember = "Id";
        }

        /// <summary>
        /// Загружает список питомцев для выбранного владельца
        /// </summary>
        /// <param name="ownerId">ID владельца</param>
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

        /// <summary>
        /// Обрабатывает нажатие кнопки "Добавить питомца"
        /// Открывает форму добавления и сохраняет нового питомца
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
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

        /// <summary>
        /// Обрабатывает нажатие кнопки "Редактировать питомца"
        /// Открывает форму редактирования и обновляет данные питомца
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
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

        /// <summary>
        /// Обрабатывает нажатие кнопки "Удалить питомца"
        /// Удаляет питомца после подтверждения
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
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

        /// <summary>
        /// Обрабатывает изменение выбранного владельца в выпадающем списке
        /// Загружает питомцев выбранного владельца
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void comboBoxOwners_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOwners.SelectedValue != null &&
                comboBoxOwners.SelectedValue is int ownerId)
            {
                selectedOwnerId = ownerId;
                LoadPets(ownerId);
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Запись на прием"
        /// Открывает форму записи на прием для выбранного питомца
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void btnCreateAppointment_Click(object sender, EventArgs e)
        {
            if (dataGridViewPets.SelectedRows.Count > 0)
            {
                Pet selectedPet = (Pet)dataGridViewPets.SelectedRows[0].DataBoundItem;
                var appointmentForm = new AppointmentForm(clinicService, selectedPet);
                appointmentForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите питомца для записи на прием!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "История посещений"
        /// Открывает форму с историей посещений для выбранного питомца
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void buttonShowHistory_Click(object sender, EventArgs e)
        {
            if (dataGridViewPets.SelectedRows.Count > 0)
            {
                var selectedPet = (Pet)dataGridViewPets.SelectedRows[0].DataBoundItem;

                var petAppointments = clinicService.GetAppointmentsByPetId(selectedPet.Id);

                var historyForm = new PetHistoryForm(clinicService, selectedPet, petAppointments);
                historyForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите питомца для просмотра истории", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Назад"
        /// Закрывает текущую форму
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Обрабатывает событие загрузки формы
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void PetForm_Load(object sender, EventArgs e)
        {

        }
    }
}