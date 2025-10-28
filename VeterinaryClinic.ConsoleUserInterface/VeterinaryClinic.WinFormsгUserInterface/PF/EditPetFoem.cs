using System;
using System.Windows.Forms;
using System.Xml.Linq;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class EditPetForm : Form
    {
        /// <summary>
        /// Кличка питомца (результат редактирования)
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Вид животного (результат редактирования)
        /// </summary>
        public string Species { get; private set; }

        /// <summary>
        /// Порода животного (результат редактирования)
        /// </summary>
        public string Breed { get; private set; }

        private readonly Pet pet;

        /// <summary>
        /// Создает форму для редактирования данных питомца
        /// </summary>
        /// <param name="petToEdit">Питомец для редактирования</param>
        public EditPetForm(Pet petToEdit)
        {
            InitializeComponent();
            pet = petToEdit ?? throw new ArgumentNullException(nameof(petToEdit));
        }

        /// <summary>
        /// Обрабатывает событие загрузки формы
        /// Заполняет поля формы текущими данными питомца
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void EditPetForm_Load(object sender, EventArgs e)
        {
            txtName.Text = pet.Name;
            txtSpecies.Text = pet.Species;
            txtBreed.Text = pet.Breed;
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Сохранить"
        /// Проверяет обязательные поля и сохраняет измененные данные
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
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

        /// <summary>
        /// Обрабатывает нажатие кнопки "Отмена"
        /// Закрывает форму без сохранения изменений
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Дополнительный обработчик загрузки формы (дублирующий)
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void EditPetForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}