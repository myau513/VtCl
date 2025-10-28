using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VeterinaryClinic.Core.Logic;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.WinFormsUserInterface
{
    public partial class AddOwnerForm : Form
    {
        /// <summary>
        /// Полное имя владельца (результат работы формы)
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Номер телефона владельца (результат работы формы)
        /// </summary>
        public string PhoneNumber { get; private set; }

        /// <summary>
        /// Создает форму для добавления нового владельца
        /// </summary>
        public AddOwnerForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Добавить владельца"
        /// Проверяет обязательные поля и сохраняет данные
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные события</param>
        private void BtnAddOwner_Click(object sender, EventArgs e)
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
        /// Закрывает форму без сохранения данных
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
        private void AddOwnerForm_Load(object sender, EventArgs e)
        {
        }
    }
}