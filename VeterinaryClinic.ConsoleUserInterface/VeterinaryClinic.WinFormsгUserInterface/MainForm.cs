using System;
using System.Windows.Forms;
using VeterinaryClinic.Core.Logic;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class MainForm : Form
    {
        private ClinicService _clinicService;

        /// <summary>
        /// Создает главную форму приложения ветеринарной клиники
        /// </summary>
        public MainForm(ClinicService clinicService)
        {
            InitializeComponent();
            _clinicService = clinicService;
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Управление питомцами"
        /// </summary>
        private void btnManagePets_Click(object sender, EventArgs e)
        {
            PetForm petsForm = new PetForm(_clinicService);
            petsForm.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Управление владельцами"
        /// </summary>
        private void btnManageOwners_Click(object sender, EventArgs e)
        {
            OwnerForm ownersForm = new OwnerForm(_clinicService);
            ownersForm.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Показать расписание"
        /// </summary>
        private void btnShowSchedule_Click(object sender, EventArgs e)
        {
            VeterinarianScheduleForm scheduleForm = new VeterinarianScheduleForm(_clinicService);
            scheduleForm.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Выход"
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void MainForm_Load_1(object sender, EventArgs e)
        {
        }
    }
}