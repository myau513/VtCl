using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VeterinaryClinic.Core.Logic;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class PetHistoryForm : Form
    {
        private readonly Pet selectedPet;
        private ClinicService clinicService;
        private List<Appointment> allAppointments;

        /// <summary>
        /// Создает форму для просмотра истории посещений питомца
        /// </summary>
        /// <param name="clinicService">Сервис клиники для работы с данными</param>
        /// <param name="pet">Питомец для просмотра истории</param>
        /// <param name="appointments">Список записей на прием (может быть устаревшим)</param>
        public PetHistoryForm(ClinicService clinicService, Pet pet, List<Appointment> appointments)
        {
            InitializeComponent(); 
            this.clinicService = clinicService;
            selectedPet = pet;
            allAppointments = appointments;
        }

        /// <summary>
        /// Обрабатывает событие загрузки формы
        /// Устанавливает имя питомца и загружает историю посещений
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void PetHistoryForm_Load(object sender, EventArgs e)
        {
            textBoxPetName.Text = selectedPet.Name;
            LoadHistoryData();
        }

        /// <summary>
        /// Загружает историю посещений для выбранного питомца
        /// Использует актуальные данные из сервиса, а не переданный список
        /// </summary>
        private void LoadHistoryData()
        {
            try
            {
                dataGridViewHistory.Rows.Clear();
                var petAppointments = clinicService.GetAppointmentsByPetId(selectedPet.Id);

                Console.WriteLine($"Найдено записей для питомца: {petAppointments.Count}");

                foreach (var appointment in petAppointments)
                {
                    dataGridViewHistory.Rows.Add(
                        appointment.AppointmentDate.ToString("dd.MM.yyyy"),
                        appointment.TimeSlot.Replace("-", ":"),
                        GetVeterinarianName(appointment.VeterinarianId),
                        appointment.Reason
                    );
                }

                if (dataGridViewHistory.Rows.Count == 0)
                {
                    MessageBox.Show("Для этого питомца нет записей!", "Информация",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Получает имя ветеринара по его ID
        /// </summary>
        /// <param name="veterinarianId">ID ветеринара</param>
        /// <returns>Имя ветеринара или "Неизвестный врач"</returns>
        private string GetVeterinarianName(int veterinarianId)
        {
            switch (veterinarianId)
            {
                case 1: return "Иванов И.И.";
                case 2: return "Петров П.П.";
                case 3: return "Сидоров С.С.";
                default: return "Неизвестный врач";
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Обновить"
        /// Перезагружает данные истории посещений
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadHistoryData();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Закрыть"
        /// Закрывает текущую форму
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обрабатывает двойной клик по ячейке таблицы истории
        /// Показывает детальную информацию о выбранной записи
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события (индекс строки и столбца)</param>
        private void dataGridViewHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridViewHistory.Rows[e.RowIndex].Cells[0].Value != null)
            {
                var selectedRow = dataGridViewHistory.Rows[e.RowIndex];

                string date = selectedRow.Cells[0].Value?.ToString() ?? "";
                string time = selectedRow.Cells[1].Value?.ToString() ?? "";
                string vet = selectedRow.Cells[2].Value?.ToString() ?? "";
                string reason = selectedRow.Cells[3].Value?.ToString() ?? "";

                MessageBox.Show($"Дата: {date}\nВремя: {time}\nВетеринар: {vet}\nПричина: {reason}",
                              "Детали записи", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Обрабатывает изменение текста в поле имени питомца
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void textBoxPetName_TextChanged(object sender, EventArgs e)
        {
        }
    }
}