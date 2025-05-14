using BCrypt.Net;
using System.Linq;
using System.Windows;

namespace TechnoAuto2.MainWindows
{
    public partial class VostanovleniePass : Window
    {
        public VostanovleniePass()
        {
            InitializeComponent();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new TechnoAutoEntities())
            {
                // Получаем значения из полей ввода
                string email = EnterEmail.Text;
                string login = EnterLogin.Text;
                string newPassword = EnterNewPassword.Text; // Используем PasswordBox

                var user = context.Users
                    .FirstOrDefault(u => u.email == email && u.login == login);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден!");
                    return;
                }

                // Хешируем новый пароль
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

                // Обновляем пароль
                user.password_hash = hashedPassword;
                context.SaveChanges();

                MessageBox.Show("Пароль успешно изменен!");
                Autorezats autorezats = new Autorezats();
                autorezats.Show();
                this.Close();
            }
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Autorezats autorezats = new Autorezats();
            autorezats.Show();
            this.Close();
        }
    }
}
