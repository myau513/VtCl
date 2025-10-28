using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VeterinaryClinic.Core.Logic;
using VeterinaryClinic.WinFormsUserInterface;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class VeterinarianScheduleForm : Form
    {
        private readonly ClinicService clinicService;

        /// <summary>
        /// Создает форму для просмотра расписания ветеринаров
        /// </summary>
        /// <param name="clinicService">Сервис клиники для работы с данными</param>
        public VeterinarianScheduleForm(ClinicService clinicService)
        {
            InitializeComponent();
            this.clinicService = clinicService;
        }

        /// <summary>
        /// Обрабатывает событие загрузки формы
        /// Устанавливает текущую дату и обновляет расписание
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void VeterinarianScheduleForm_Load(object sender, EventArgs e)
        {
            dateTimePickerDate.Value = DateTime.Today;
            UpdateSchedule();
        }

        /// <summary>
        /// Обрабатывает изменение даты в элементе выбора даты
        /// Обновляет расписание для выбранной даты
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void dateTimePickerDate_ValueChanged(object sender, EventArgs e)
        {
            UpdateSchedule();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Обновить"
        /// Перезагружает данные расписания
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            UpdateSchedule();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Закрыть"
        /// Закрывает текущую форму
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обновляет отображение расписания ветеринаров на выбранную дату
        /// Создает таблицу с временными слотами и информацией о записях
        /// </summary>
        private void UpdateSchedule()
        {
            panelSchedule.Controls.Clear();

            var vets = clinicService.GetAllVeterinarians()
                .Where(v => v.WorkDays.Contains(dateTimePickerDate.Value.DayOfWeek))
                .ToList();

            int rowHeight = 30;
            int colWidth = 150;

            int rows = 10;
            int cols = vets.Count + 1;

            panelSchedule.Width = cols * colWidth + 20;
            panelSchedule.Height = rows * rowHeight + 20;

            panelSchedule.AutoScroll = true;

            for (int col = 0; col < cols; col++)
            {
                Label lbl = new Label
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(col * colWidth, 0),
                    Size = new Size(colWidth, rowHeight),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                };

                if (col == 0)
                    lbl.Text = "Время";
                else
                    lbl.Text = vets[col - 1].FullName;

                panelSchedule.Controls.Add(lbl);
            }

            for (int row = 1; row < rows; row++)
            {
                int hour = 9 + row; 

                Label lblTime = new Label
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, row * rowHeight),
                    Size = new Size(colWidth, rowHeight),
                    Text = $"{hour}:00"
                };
                panelSchedule.Controls.Add(lblTime);

                DateTime slotDateTime = new DateTime(dateTimePickerDate.Value.Year,
                                                     dateTimePickerDate.Value.Month,
                                                     dateTimePickerDate.Value.Day,
                                                     hour, 0, 0);
                for (int col = 1; col < cols; col++)
                {
                    var vet = vets[col - 1];

                    var appointments = clinicService.GetAppointmentsByDate(dateTimePickerDate.Value);

                    var appointment = appointments.FirstOrDefault(a =>
                        a.VeterinarianId == vet.Id &&
                        a.AppointmentDate == slotDateTime);

                    Label lblData = new Label
                    {
                        BorderStyle = BorderStyle.FixedSingle,
                        Location = new Point(col * colWidth, row * rowHeight),
                        Size = new Size(colWidth, rowHeight),
                        TextAlign = ContentAlignment.MiddleCenter,
                        AutoEllipsis = true
                    };

                    if (appointment != null)
                    {
                        var pet = clinicService.GetAllPets().FirstOrDefault(p => p.Id == appointment.PetId);
                        string petName = pet != null ? pet.Name : "Питомец";
                        lblData.Text = $"{petName}\n({appointment.Reason})";
                        lblData.BackColor = Color.LightCoral;
                        lblData.ForeColor = Color.White;
                    }
                    else
                    {
                        lblData.Text = "Свободно";
                        lblData.BackColor = Color.LightGreen;
                    }
                    panelSchedule.Controls.Add(lblData);
                }
            }
        }
    }
}