using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TechnoAuto2.Resource.Class;

namespace TechnoAuto2.MainWindows
{
    /// <summary>
    /// Логика взаимодействия для Autorezats.xaml
    /// </summary>
    public partial class Autorezats : Window
    {
        public Autorezats()
        {
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Close();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string LoginUser = EnterEmail.Text.Trim();
                string PasswordUser = EnterPassword.Password.Trim();

                if (string.IsNullOrEmpty(LoginUser) || string.IsNullOrEmpty(PasswordUser))
                {
                    MessageBox.Show("Поля логина/почты и пароля не могут быть пустыми!",
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }


                bool isEmail = LoginUser.Contains("@");

                if (isEmail)
                {

                    if (!LoginUser.Contains("."))
                    {
                        MessageBox.Show("Некорректный формат почты!",
                                        "Ошибка",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {

                    if (LoginUser.Length < 3)
                    {
                        MessageBox.Show("Логин должен содержать минимум 3 символа!",
                                        "Ошибка",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                        return;
                    }
                }

                if (PasswordUser.Length < 1)
                {
                    MessageBox.Show("Пароль должен содержать минимум 1 символ!",
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                using (TechnoAutoEntities technoAuto = new TechnoAutoEntities())
                {

                    var user = technoAuto.Users.FirstOrDefault(x =>
                        (isEmail && x.email == LoginUser) ||
                        (!isEmail && x.login == LoginUser)
                    );

                    if (user == null || !BCrypt.Net.BCrypt.Verify(PasswordUser, user.password_hash))
                    {
                        MessageBox.Show("Неверная почта/логин или пароль!",
                                        "Ошибка",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                        return;
                    }

                    GlobalClass.RoleIDLoged = (int)user.role_id;
                    GlobalClass.UserIDLoged = (int)user.user_id;
                    MessageBox.Show("Авторизация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    GlavnoeOkno okno = new GlavnoeOkno();
                    okno.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                                "Ошибка системы",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            VostanovleniePass vostanovleniePass = new VostanovleniePass();
            vostanovleniePass.Show();
            this.Close();
        }
    }
}
