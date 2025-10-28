using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VeterinaryClinic.Core.Logic;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class EditOwnerForm : Form
    {
        /// <summary>
        /// Полное имя владельца (результат редактирования)
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Номер телефона владельца (результат редактирования)
        /// </summary>
        public string PhoneNumber { get; private set; }

        /// <summary>
        /// Создает форму для редактирования данных владельца
        /// </summary>
        /// <param name="owner">Владелец для редактирования (заполняет поля формы)</param>
        public EditOwnerForm(Owner owner)
        {
            InitializeComponent();
            txtFullName.Text = owner.FullName;
            txtPhoneNumber.Text = owner.PhoneNumber;
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Сохранить"
        /// Проверяет обязательные поля и сохраняет измененные данные
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО владельца!");
                return;
            }

            FullName = txtFullName.Text.Trim();
            PhoneNumber = txtPhoneNumber.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Отмена"
        /// Закрывает форму без сохранения изменений
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Обрабатывает событие загрузки формы
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void EditOwnerForm_Load(object sender, EventArgs e)
        {
        }
    }
}