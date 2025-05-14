using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BCrypt.Net;
using System.Data.Entity;

namespace TechnoAuto2.MainPages
{
    /// <summary>
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        private Users _user;
        private Role _role;
        private Departments _departments;

        public AccountPage(Users user)
        {
            InitializeComponent();
            _user = user;
            this.DataContext = _user;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (!string.IsNullOrEmpty(newPassword))
            {
                if (newPassword != confirmPassword)
                {
                    MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                _user.password_hash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            }

            try
            {
                using (var db = new TechnoAutoEntities())
                {
                    var userInDb = db.Users.FirstOrDefault(u => u.user_id == _user.user_id);

                    if (userInDb != null)
                    {
                        // Копируем изменения из `_user` в `userInDb`
                        userInDb.first_name = FirstNameTextBox.Text; // Пример для TextBox
                        userInDb.last_name = LastNameTextBox.Text;
                        userInDb.MiddleName = MiddleNameTextBox.Text;
                        userInDb.login = LoginTextBox.Text;

                        if (!string.IsNullOrEmpty(newPassword))
                        {
                            userInDb.password_hash = _user.password_hash;
                        }

                        db.SaveChanges();

                        // Обновляем DataContext, чтобы интерфейс увидел изменения
                        this.DataContext = null;
                        this.DataContext = _user;

                        MessageBox.Show("Данные сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        NewPasswordBox.Password = "";
                        ConfirmPasswordBox.Password = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
