using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TechnoAuto2.MainWindows
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {

        TechnoAutoEntities technoAuto = new TechnoAutoEntities();
        Users users = new Users();
        Role roles = new Role();

        public Register()
        {
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Autorezats autorezats = new Autorezats();
            autorezats.Show();
            this.Close();
        }

        private string GenerateUniqueLogin(TechnoAutoEntities db, int length = 6)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            string login;

            do
            {
                login = new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (db.Users.Any(u => u.login == login));

            return login;
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            if (EnterEmail.Text == "")
            {
                MessageBox.Show("Поле почты пустое!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (EnterPassword.Password == "")
            {
                MessageBox.Show("Поле пароля пустое!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (EnterPhoneNumber.Text == "")
            {
                MessageBox.Show("Поле номера телефона пустое!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!Regex.IsMatch(EnterPhoneNumber.Text, @"^(7|8)\d{10}$"))
            {
                MessageBox.Show("Некорректный номер телефона! Формат: 7XXXXXXXXXX или 8XXXXXXXXXX", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (EnterFirstName.Text == "")
            {
                MessageBox.Show("Поле имя пустое!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (EnterLastName.Text == "")
            {
                MessageBox.Show("Поле фамилия пустое!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (EnterMidleName.Text == "")
            {
                MessageBox.Show("Поле отчество пустое!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string generatedLogin = GenerateUniqueLogin(technoAuto, 6);

            var user = new Users
            {
                login = generatedLogin,
                email = EnterEmail.Text,
                password_hash = BCrypt.Net.BCrypt.HashPassword(EnterPassword.Password),
                phone = EnterPhoneNumber.Text,
                first_name = EnterFirstName.Text,
                last_name = EnterLastName.Text,
                MiddleName = EnterMidleName.Text,
                role_id = 2,
                created_at = DateTime.Now
            };

            try
            {
                technoAuto.Users.Add(user);
                technoAuto.SaveChanges();
                MessageBox.Show($"Регистрация прошла успешно!\nВаш логин: {generatedLogin}", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                Autorezats autorezats = new Autorezats();
                autorezats.Show();
                this.Close();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string errors = "";
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errors += $"Свойство: {validationError.PropertyName} - Ошибка: {validationError.ErrorMessage}\n";
                    }
                }
                MessageBox.Show(errors, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
