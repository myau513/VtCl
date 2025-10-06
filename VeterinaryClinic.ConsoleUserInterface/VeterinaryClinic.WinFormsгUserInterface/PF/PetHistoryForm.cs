using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VeterinaryClinic.Core.Logic;
using VeterinaryClinic.Core.Models;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class PetHistoryForm : Form
    {
        private readonly PetManager petManager;
        private readonly Pet selectedPet;
        private List<Appointment> allAppointments;

        public PetHistoryForm(PetManager petMgr, Pet pet, List<Appointment> appointments)
        {
            InitializeComponent(); // ЭТО ВАЖНО - должен быть первым!
            petManager = petMgr;
            selectedPet = pet;
            allAppointments = appointments;
        }

        private void PetHistoryForm_Load(object sender, EventArgs e)
        {
            // Устанавливаем кличку питомца
            textBoxPetName.Text = selectedPet.Name;

            // Загружаем данные
            LoadHistoryData();
        }

        private void LoadHistoryData()
        {
            try
            {
                // Очищаем таблицу
                dataGridViewHistory.Rows.Clear();

                // ПРОВЕРКА ДАННЫХ - ВЫВОДИМ ВСЕ В КОНСОЛЬ
                Console.WriteLine("=== ОТЛАДКА ===");
                Console.WriteLine($"allAppointments: {allAppointments != null}");
                Console.WriteLine($"Количество записей: {allAppointments?.Count ?? 0}");
                Console.WriteLine($"ID питомца: {selectedPet.Id}");
                Console.WriteLine($"Имя питомца: {selectedPet.Name}");

                if (allAppointments == null || allAppointments.Count == 0)
                {
                    MessageBox.Show("Нет данных о записях!", "Информация",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ВЫВОДИМ ВСЕ ЗАПИСИ ДЛЯ ОТЛАДКИ
                Console.WriteLine("ВСЕ ЗАПИСИ:");
                foreach (var app in allAppointments)
                {
                    Console.WriteLine($"PetId: {app.PetId}, Date: {app.AppointmentDate}, Time: {app.TimeSlot}, Reason: {app.Reason}");
                }

                // Получаем записи текущего питомца
                var petAppointments = allAppointments
                    .Where(a => a.PetId == selectedPet.Id)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();

                Console.WriteLine($"Найдено записей для питомца: {petAppointments.Count}");

                // Заполняем таблицу
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

        // СОБЫТИЯ КНОПОК - ДОЛЖНЫ БЫТЬ ПРИВЯЗАНЫ В ДИЗАЙНЕРЕ!
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadHistoryData();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close(); // ЭТО ДОЛЖНО РАБОТАТЬ!
        }

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

        private void textBoxPetName_TextChanged(object sender, EventArgs e)
        {
            // Пустой метод
        }
    }
}