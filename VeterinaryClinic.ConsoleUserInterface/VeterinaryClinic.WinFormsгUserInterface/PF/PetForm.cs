using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VeterinaryClinic.Core.Essence;
using VeterinaryClinic.Core.Logic;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class PetForm : Form
    {
        private readonly ClinicService _clinicService;
        private Guid _selectedOwnerId;
        private List<Appointment> _appointments = new List<Appointment>();

        /// <summary>
        /// Создает форму для работы с питомцами
        /// </summary>
        /// <param name="clinicService">Сервис клиники для работы с данными</param>
        public PetForm(ClinicService clinicService)
        {
            InitializeComponent();
            _clinicService = clinicService;
            dataGridViewPets.AutoGenerateColumns = false;
            LoadOwnersComboBox();
        }

        /// <summary>
        /// Загружает список владельцев в выпадающий список
        /// </summary>
        private void LoadOwnersComboBox()
        {
            try
            {
                var owners = _clinicService.GetAllOwners();
                comboBoxOwners.DataSource = owners;
                comboBoxOwners.DisplayMember = "FullName";
                comboBoxOwners.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки владельцев: " + ex.Message);
            }
        }

        /// <summary>
        /// Загружает список питомцев для выбранного владельца
        /// </summary>
        /// <param name="ownerId">ID владельца</param>
        private void LoadPets(Guid ownerId)
        {
            try
            {
                var pets = _clinicService.GetPetsByOwnerId(ownerId);
                dataGridViewPets.DataSource = pets;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки питомцев: " + ex.Message);
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Добавить питомца"
        /// </summary>
        private void btnAddPet_Click(object sender, EventArgs e)
        {
            if (_selectedOwnerId == Guid.Empty)
            {
                MessageBox.Show("Выберите владельца!");
                return;
            }

            var addForm = new AddPetForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _clinicService.CreatePet(addForm.Name, addForm.Species, addForm.Breed, _selectedOwnerId);
                    LoadPets(_selectedOwnerId);
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
        /// </summary>
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

                        _clinicService.UpdatePet(updatedPet);
                        LoadPets(_selectedOwnerId);
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
        /// </summary>
        private void btnDeletePet_Click(object sender, EventArgs e)
        {
            if (dataGridViewPets.CurrentRow?.DataBoundItem is Pet selectedPet)
            {
                if (MessageBox.Show($"Удалить питомца {selectedPet.Name}?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        if (_clinicService.DeletePet(selectedPet.Id))
                        {
                            LoadPets(_selectedOwnerId);
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
        /// </summary>
        private void comboBoxOwners_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOwners.SelectedValue != null && comboBoxOwners.SelectedValue is Guid ownerId)
            {
                _selectedOwnerId = ownerId;
                LoadPets(ownerId);
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Запись на прием"
        /// </summary>
        private void btnCreateAppointment_Click(object sender, EventArgs e)
        {
            if (dataGridViewPets.SelectedRows.Count > 0 && dataGridViewPets.SelectedRows[0].DataBoundItem is Pet selectedPet)
            {
                var appointmentForm = new AppointmentForm(_clinicService, selectedPet);
                appointmentForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите питомца для записи на прием!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "История посещений"
        /// </summary>
        /// <summary>
        /// Обрабатывает нажатие кнопки "История посещений"
        /// </summary>
        private void buttonShowHistory_Click(object sender, EventArgs e)
        {
            if (dataGridViewPets.SelectedRows.Count > 0 && dataGridViewPets.SelectedRows[0].DataBoundItem is Pet selectedPet)
            {
                try
                {
                    // Получаем историю через владельца (как в консольной версии)
                    var owner = _clinicService.FindOwnersByName("").Find(o => o.Id == selectedPet.OwnerId);
                    if (owner != null)
                    {
                        var history = _clinicService.GetVisitHistoryByOwner(owner.FullName);
                        var petHistory = history.Where(h => h.PetId == selectedPet.Id).ToList();

                        // Создаем форму с историей посещений (VisitHistory)
                        var historyForm = new PetHistoryForm(_clinicService, selectedPet, petHistory);
                        historyForm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки истории: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите питомца для просмотра истории", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Назад"
        /// </summary>
        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Обрабатывает событие загрузки формы
        /// </summary>
        private void PetForm_Load(object sender, EventArgs e)
        {
            // Дополнительная инициализация если нужно
        }
    }
}