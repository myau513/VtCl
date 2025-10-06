using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class AddPetForm : Form
    {
        public string Name { get; private set; }
        public string Species { get; private set; }
        public string Breed { get; private set; }
        public AddPetForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите кличку питомца!");
                return;
            }

            Name = txtName.Text;
            Species = txtSpecies.Text;
            Breed = txtBreed.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AddPetForm_Load(object sender, EventArgs e)
        {

        }
    }
}
