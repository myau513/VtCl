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
    public partial class MainForm : Form
    {
        private readonly OwnerManager ownerManager;
        private readonly PetManager petManager;
        public MainForm()
        {
            InitializeComponent();
            ownerManager = new OwnerManager();
            petManager = new PetManager(ownerManager);
        }
        private void btnManagePets_Click(object sender, EventArgs e)
        {
            PetForm petsForm = new PetForm(ownerManager, petManager);
            petsForm.ShowDialog();
        }

        private void btnManageOwners_Click(object sender, EventArgs e)
        {
            OwnerForm ownersForm = new OwnerForm(ownerManager, petManager);
            ownersForm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnShowSchedule_Click(object sender, EventArgs e)
        {
            var clinicService = new ClinicService(ownerManager, petManager, new VeterinarianManager());
            var scheduleForm = new VeterinaryClinic.WinForms.Forms.VetForm.VeterinarianScheduleForm(clinicService);
            scheduleForm.ShowDialog();
        }
    }
}
