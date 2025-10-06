using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VeterinaryClinic.Core.Logic;
using VeterinaryClinic.Core.Models;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class AppointmentForm : Form
    {
        private readonly ClinicService clinicService;
        private readonly Pet selectedPet;

        private readonly string[] timeIntervals = { "10", "11", "12", "13", "14", "15", "16", "17", "18" };
        private readonly string[] daysOfWeek = { "pn", "vt", "sr", "cht", "pt", "sb" };

        private List<Veterinarian> veterinarians;

        public string Reason { get; private set; }
        public DateTime SelectedDate { get; private set; }
        public string SelectedTime { get; private set; }
        public int SelectedVeterinarianId { get; private set; }

        public AppointmentForm(ClinicService clinicService, Pet pet)
        {
            InitializeComponent();
            this.clinicService = clinicService;
            selectedPet = pet;
            InitializeVeterinarians();
            InitializeForm();
            SelectedTime = null;
        }

        private void InitializeVeterinarians()
        {
            // Получаем всех ветеринаров из сервиса
            veterinarians = clinicService.GetAllVeterinarians();
        }

        private void InitializeForm()
        {
            textBoxPetName.Text = selectedPet.Name;
            textBoxPetName.ReadOnly = true;

            textBoxBreed.Text = selectedPet.Breed;
            textBoxBreed.ReadOnly = true;

            SubscribeToEvents();
            UpdateSlotsAvailability();
        }

        private void SubscribeToEvents()
        {
            var checkBoxes = GetAllCheckBoxes(this);
            foreach (var cb in checkBoxes)
            {
                cb.CheckedChanged += CheckBox_CheckedChanged;
            }

            dateTimePickerWeek.ValueChanged += dateTimePickerWeek_ValueChanged;
            textBoxReason.TextChanged += textBoxReason_TextChanged;
            buttonConfirm.Click += buttonConfirm_Click;
            buttonCancel.Click += buttonCancel_Click;
        }

        private List<CheckBox> GetAllCheckBoxes(Control control)
        {
            var checkBoxes = new List<CheckBox>();

            foreach (Control child in control.Controls)
            {
                if (child is CheckBox checkBox)
                {
                    checkBoxes.Add(checkBox);
                }
                else
                {
                    checkBoxes.AddRange(GetAllCheckBoxes(child));
                }
            }

            return checkBoxes;
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;

            if (checkBox.Checked)
            {
                UncheckOtherCheckBoxes(checkBox);
                ProcessSelectedCheckbox(checkBox);
            }
            else
            {
                UpdateSelectedTimeFromCheckboxes();
            }
        }

        private void UncheckOtherCheckBoxes(CheckBox currentCheckBox)
        {
            var allCheckBoxes = GetAllCheckBoxes(this);
            foreach (var cb in allCheckBoxes)
            {
                if (cb != currentCheckBox)
                    cb.Checked = false;
            }
        }

        private void ProcessSelectedCheckbox(CheckBox checkBox)
        {
            string[] parts = checkBox.Name.Split('_');
            if (parts.Length >= 3)
            {
                string day = parts[1];
                string time = parts[2];

                DateTime baseDate = dateTimePickerWeek.Value;
                DateTime selectedDate = GetNextWeekday(baseDate, GetDayIndex(day));

                SelectedDate = selectedDate;
                SelectedTime = time + "-00";
                SelectedVeterinarianId = GetVeterinarianByDay(GetDayIndex(day));

                Console.WriteLine($" Выбрано: {day}, время {time}:00, дата {selectedDate:dd.MM.yyyy}");
            }
        }

        private void UpdateSelectedTimeFromCheckboxes()
        {
            var allCheckBoxes = GetAllCheckBoxes(this);
            var selectedCheckbox = allCheckBoxes.FirstOrDefault(cb => cb.Checked);

            if (selectedCheckbox != null)
            {
                ProcessSelectedCheckbox(selectedCheckbox);
            }
            else
            {
                SelectedTime = null;
                SelectedDate = DateTime.MinValue;
                SelectedVeterinarianId = 0;
                Console.WriteLine("❌ Время не выбрано");
            }
        }

        private void dateTimePickerWeek_ValueChanged(object sender, EventArgs e)
        {
            UpdateSlotsAvailability();
            SelectedTime = null;
            SelectedDate = DateTime.MinValue;
            UncheckAllCheckBoxes();
        }

        private void UncheckAllCheckBoxes()
        {
            foreach (var cb in GetAllCheckBoxes(this))
            {
                cb.Checked = false;
            }
        }

        private void textBoxReason_TextChanged(object sender, EventArgs e)
        {
            // Логика при изменении причины (если нужна)
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedTime))
            {
                MessageBox.Show("Выберите время приема!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxReason.Text))
            {
                MessageBox.Show("Введите причину визита!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Reason = textBoxReason.Text.Trim();

            try
            {
                clinicService.CreateAppointment(
                    selectedPet.Id,
                    SelectedVeterinarianId,
                    SelectedDate,
                    SelectedTime,
                    Reason,
                    selectedPet.Breed);

                ShowSuccessMessage();

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании записи: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowSuccessMessage()
        {
            string formattedDate = SelectedDate.ToString("dd.MM.yyyy");
            string formattedTime = SelectedTime.Replace("-", ":");
            string veterinarianName = GetVeterinarianName(SelectedVeterinarianId);

            MessageBox.Show($"Запись на прием создана успешно!\n\n" +
                            $"Питомец: {selectedPet.Name}\n" +
                            $"Порода: {selectedPet.Breed}\n" +
                            $"Дата: {formattedDate}\n" +
                            $"Время: {formattedTime}\n" +
                            $"Ветеринар: {veterinarianName}\n" +
                            $"Причина: {Reason}",
                            "Успешная запись",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void UpdateSlotsAvailability()
        {
            DateTime baseDate = dateTimePickerWeek.Value;
            var allCheckBoxes = GetAllCheckBoxes(this);

            foreach (string day in daysOfWeek)
            {
                int dayIdx = GetDayIndex(day);
                DateTime slotDate = GetNextWeekday(baseDate, dayIdx);
                int vetId = GetVeterinarianByDay(dayIdx);

                var availableSlots = clinicService.GetAvailableTimeSlots(vetId, slotDate);
                foreach (string time in timeIntervals)
                {
                    string checkBoxName = $"checkBox_{day}_{time}";
                    var checkBox = allCheckBoxes.FirstOrDefault(cb => cb.Name == checkBoxName);
                    if (checkBox != null)
                    {
                        string timeSlot = time + "-00";
                        bool isBooked = !availableSlots.Any(slot =>
                            slot.Hour == int.Parse(time) &&
                            slot.Date == slotDate.Date);

                        checkBox.Enabled = !isBooked;
                        checkBox.BackColor = isBooked ? Color.LightGray : SystemColors.Control;
                        checkBox.ForeColor = isBooked ? Color.DarkGray : SystemColors.ControlText;
                        checkBox.Text = isBooked ? " Занято" : " Свободно";

                        if (isBooked && checkBox.Checked)
                            checkBox.Checked = false;
                    }
                }
            }
        }

        private Veterinarian GetVeterinarianById(int id)
        {
            return veterinarians.FirstOrDefault(v => v.Id == id);
        }

        private DateTime GetNextWeekday(DateTime start, int dayIndex)
        {
            int currentDay = (int)start.DayOfWeek;
            int targetDay = dayIndex == 5 ? 6 : dayIndex + 1;

            int daysToAdd = targetDay - currentDay;
            if (daysToAdd < 0) daysToAdd += 7;

            return start.AddDays(daysToAdd);
        }

        private int GetDayIndex(string day)
        {
            switch (day)
            {
                case "pn": return 0;  // Понедельник
                case "vt": return 1;  // Вторник
                case "sr": return 2;  // Среда
                case "cht": return 3; // Четверг
                case "pt": return 4;  // Пятница
                case "sb": return 5;  // Суббота
                default: return 0;
            }
        }


        private int GetVeterinarianByDay(int dayIndex)
        {
            DayOfWeek targetDay;

            switch (dayIndex)
            {
                case 0: targetDay = DayOfWeek.Monday; break;
                case 1: targetDay = DayOfWeek.Tuesday; break;
                case 2: targetDay = DayOfWeek.Wednesday; break;
                case 3: targetDay = DayOfWeek.Thursday; break;
                case 4: targetDay = DayOfWeek.Friday; break;
                case 5: targetDay = DayOfWeek.Saturday; break;
                default: targetDay = DayOfWeek.Monday; break;
            }

            foreach (var vet in veterinarians)
            {
                if (vet.WorkDays.Contains(targetDay))
                    return vet.Id;
            }

            return 1; // По умолчанию Иванов
        }


        private string GetVeterinarianName(int veterinarianId)
        {
            var vet = GetVeterinarianById(veterinarianId);
            return vet != null ? vet.FullName : "Неизвестно";
        }

        private void AppointmentForm_Load(object sender, EventArgs e)
        {
            // Дополнительная инициализация если нужно
        }
    }
}
