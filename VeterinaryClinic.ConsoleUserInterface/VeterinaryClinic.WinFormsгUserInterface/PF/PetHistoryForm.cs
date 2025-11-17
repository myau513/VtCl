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
        private readonly Pet _selectedPet;
        private readonly ClinicService _clinicService;
        private List<VisitHistory> _visitHistory;

        /// <summary>
        /// Создает форму для просмотра истории посещений питомца
        /// </summary>
        /// <param name="clinicService">Сервис клиники для работы с данными</param>
        /// <param name="pet">Питомец для просмотра истории</param>
        /// <param name="visitHistory">Список истории посещений</param>
        public PetHistoryForm(ClinicService clinicService, Pet pet, List<VisitHistory> visitHistory)
        {
            InitializeComponent();
            _clinicService = clinicService;
            _selectedPet = pet;
            _visitHistory = visitHistory ?? new List<VisitHistory>();
        }

        /// <summary>
        /// Обрабатывает событие загрузки формы
        /// </summary>
        private void PetHistoryForm_Load(object sender, EventArgs e)
        {
            textBoxPetName.Text = _selectedPet.Name;
            LoadHistoryData();
        }

        /// <summary>
        /// Загружает историю посещений для выбранного питомца
        /// </summary>
        private void LoadHistoryData()
        {
            try
            {
                dataGridViewHistory.Rows.Clear();

                // Сортируем историю по дате (от новых к старым)
                var sortedHistory = _visitHistory
                    .OrderByDescending(h => h.VisitDate)
                    .ToList();

                Console.WriteLine($"Найдено записей в истории: {sortedHistory.Count}");

                foreach (var visit in sortedHistory)
                {
                    dataGridViewHistory.Rows.Add(
                        visit.VisitDate.ToString("dd.MM.yyyy HH:mm"),
                        visit.VeterinarianName,
                        visit.Reason,
                        visit.Diagnosis ?? "",
                        visit.Treatment ?? "",
                        visit.Notes ?? ""
                    );
                }

                if (dataGridViewHistory.Rows.Count == 0)
                {
                    MessageBox.Show("Для этого питомца нет записей в истории посещений!", "Информация",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Обновить"
        /// </summary>
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                // Обновляем данные через сервис
                var owner = _clinicService.FindOwnersByName("").Find(o => o.Id == _selectedPet.OwnerId);
                if (owner != null)
                {
                    var history = _clinicService.GetVisitHistoryByOwner(owner.FullName);
                    _visitHistory = history.Where(h => h.PetId == _selectedPet.Id).ToList();
                    LoadHistoryData();
                    MessageBox.Show("Данные обновлены!", "Обновление",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Закрыть"
        /// </summary>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обрабатывает двойной клик по ячейке таблицы истории
        /// </summary>
        private void dataGridViewHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridViewHistory.Rows[e.RowIndex].Cells[0].Value != null)
            {
                var selectedRow = dataGridViewHistory.Rows[e.RowIndex];

                string date = selectedRow.Cells[0].Value?.ToString() ?? "";
                string vet = selectedRow.Cells[1].Value?.ToString() ?? "";
                string reason = selectedRow.Cells[2].Value?.ToString() ?? "";
                string diagnosis = selectedRow.Cells[3].Value?.ToString() ?? "";
                string treatment = selectedRow.Cells[4].Value?.ToString() ?? "";
                string notes = selectedRow.Cells[5].Value?.ToString() ?? "";

                string details = $"Дата и время: {date}\n" +
                               $"Ветеринар: {vet}\n" +
                               $"Причина: {reason}";

                if (!string.IsNullOrEmpty(diagnosis))
                    details += $"\nДиагноз: {diagnosis}";
                if (!string.IsNullOrEmpty(treatment))
                    details += $"\nЛечение: {treatment}";
                if (!string.IsNullOrEmpty(notes))
                    details += $"\nЗаметки: {notes}";

                MessageBox.Show(details, "Детали визита",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Обрабатывает изменение текста в поле имени питомца
        /// </summary>
        private void textBoxPetName_TextChanged(object sender, EventArgs e)
        {
            // Не нужно изменять имя питомца
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Экспорт в текст"
        /// </summary>
        private void buttonExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (_visitHistory.Count == 0)
                {
                    MessageBox.Show("Нет данных для экспорта!", "Информация",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string exportText = $"История посещений питомца: {_selectedPet.Name}\n";
                exportText += $"Порода: {_selectedPet.Breed}\n";
                exportText += $"Всего визитов: {_visitHistory.Count}\n\n";

                foreach (var visit in _visitHistory.OrderByDescending(h => h.VisitDate))
                {
                    exportText += $"Дата: {visit.VisitDate:dd.MM.yyyy HH:mm}\n";
                    exportText += $"Ветеринар: {visit.VeterinarianName}\n";
                    exportText += $"Причина: {visit.Reason}\n";

                    if (!string.IsNullOrEmpty(visit.Diagnosis))
                        exportText += $"Диагноз: {visit.Diagnosis}\n";
                    if (!string.IsNullOrEmpty(visit.Treatment))
                        exportText += $"Лечение: {visit.Treatment}\n";
                    if (!string.IsNullOrEmpty(visit.Notes))
                        exportText += $"Заметки: {visit.Notes}\n";

                    exportText += new string('-', 40) + "\n";
                }

                Clipboard.SetText(exportText);
                MessageBox.Show("Данные скопированы в буфер обмена!", "Экспорт",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}