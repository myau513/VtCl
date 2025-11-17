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
using VeterinaryClinic.Core.Essence;
using VeterinaryClinic.WinFormsUserInterface;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class MainForm : Form
    {
        private readonly OwnerManager ownerManager;
        private readonly PetManager petManager;
        private readonly VeterinarianManager vetManager;
        private readonly AppointmentManager appointmentManager;
        private readonly VisitHistoryManager visitHistoryManager;

        /// <summary>
        /// Создает главную форму приложения ветеринарной клиники
        /// Инициализирует менеджеры данных и сервисы
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            ownerManager = new OwnerManager();
            vetManager = new VeterinarianManager();
            appointmentManager = new AppointmentManager();
            visitHistoryManager = new VisitHistoryManager();
            petManager = new PetManager(ownerManager);
        }

        private ClinicService clinicService;

        /// <summary>
        /// Инициализирует сервис клиники, если он еще не создан
        /// Обеспечивает единый экземпляр сервиса для всех форм
        /// </summary>
        private void InitializeClinicService()
        {
            if (clinicService == null)
            {
                clinicService = new ClinicService(
                    ownerManager,
                    petManager,
                    vetManager,
                    appointmentManager,
                    visitHistoryManager
                );
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Управление питомцами"
        /// Открывает форму для работы с питомцами
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void btnManagePets_Click(object sender, EventArgs e)
        {
            InitializeClinicService();  // обеспечиваем инициализацию
            PetForm petsForm = new PetForm(clinicService);
            petsForm.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Управление владельцами"
        /// Открывает форму для работы с владельцами животных
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void btnManageOwners_Click(object sender, EventArgs e)
        {
            InitializeClinicService();
            OwnerForm ownersForm = new OwnerForm(clinicService);
            ownersForm.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Выход"
        /// Завершает работу приложения
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Обрабатывает событие загрузки главной формы
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Показать расписание"
        /// Открывает форму с расписанием ветеринаров
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void btnShowSchedule_Click(object sender, EventArgs e)
        {
            InitializeClinicService();
            var scheduleForm = new VeterinarianScheduleForm(clinicService);
            scheduleForm.ShowDialog();
        }

        /// <summary>
        /// Дополнительный обработчик загрузки формы (дублирующий)
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void MainForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}