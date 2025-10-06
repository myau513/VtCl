using System;
using System.Windows.Forms;
using System.Xml.Linq;
using VeterinaryClinic.Core.Models;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class EditPetForm : Form
    {
        public string Name { get; private set; }
        public string Species { get; private set; }
        public string Breed { get; private set; }

        private readonly Pet pet;

        public EditPetForm(Pet petToEdit)
        {
            InitializeComponent();
            pet = petToEdit ?? throw new ArgumentNullException(nameof(petToEdit));
        }

        private void EditPetForm_Load(object sender, EventArgs e)
        {
            // При загрузке формы заполняем поля текущими данными питомца
            txtName.Text = pet.Name;
            txtSpecies.Text = pet.Species;
            txtBreed.Text = pet.Breed;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите кличку питомца!");
                return;
            }

            Name = txtName.Text.Trim();
            Species = txtSpecies.Text.Trim();
            Breed = txtBreed.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void EditPetForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
