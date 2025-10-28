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
        /// <summary>
        /// Кличка питомца (результат работы формы)
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Вид животного (результат работы формы)
        /// </summary>
        public string Species { get; private set; }

        /// <summary>
        /// Порода животного (результат работы формы)
        /// </summary>
        public string Breed { get; private set; }

        /// <summary>
        /// Создает форму для добавления нового питомца
        /// </summary>
        public AddPetForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Добавить"
        /// Проверяет обязательные поля и сохраняет данные питомца
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
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

        /// <summary>
        /// Обрабатывает нажатие кнопки "Отмена"
        /// Закрывает форму без сохранения данных
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Обрабатывает событие загрузки формы
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void AddPetForm_Load(object sender, EventArgs e)
        {

        }
    }
}