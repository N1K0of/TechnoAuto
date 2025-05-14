using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using System.Windows.Shapes;
using TechnoAuto2.MainPages;
using TechnoAuto2.Resource.Class;

namespace TechnoAuto2.MainWindows
{
    /// <summary>
    /// Логика взаимодействия для GlavnoeOkno.xaml
    /// </summary>
    public partial class GlavnoeOkno : Window
    {
        public GlavnoeOkno()
        {
            InitializeComponent();
            Show();
            switch (GlobalClass.RoleIDLoged)
            {
                case 1:
                    break;

                case 2:
                    Sotrudick.Visibility = Visibility.Collapsed;                    
                    User.Visibility = Visibility.Collapsed;
                    NameRole.Text = "Пользователь";
                    break;
                case 3:
                    Sotrudick.Visibility = Visibility.Collapsed;
                    NameRole.Text = "Поддержка";
                    break;
            }
        }
        private void Show()
        {
            using (var context = new TechnoAutoEntities())
            {
                // Получаем ID текущего пользователя
                int userId = GlobalClass.UserIDLoged;

                // Находим сотрудника в БД
                var user = context.Users.FirstOrDefault(e => e.user_id == userId);

                if (user != null)
                {
                    // Сохраняем ID сотрудника в глобальный класс (если нужно)
                    GlobalClass.UserIDLoged = user.user_id;

                    // Формируем ФИО и выводим в TextBlock
                    TbFullName.Text = $"{user.last_name} {user.first_name} {user.MiddleName}";
                }
                else
                {
                    TbFullName.Text = "Пользователь не найден";
                }
            }
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Autorezats autorezats = new Autorezats();
            autorezats.Show();
            this.Close();
        }

        private void User_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPages.UserPage());
        }

        private void TexSupport_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPages.TexSupportPage());
        }

        private void Sotrudick_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPages.EmployTablePage());
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPages.HistoryTable());
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = GlobalClass.GetCurrentUser();


            if (currentUser == null)
            {
                MessageBox.Show("Пользователь не найден.");
                return;
            }

            //MainFrame.Navigate(new MainPages.OrderPage(currentUser, Tickets.ticket_id));
            MainFrame.Navigate(new MainPages.OrderPage());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = GlobalClass.GetCurrentUser();
            if (currentUser == null)
            {
                MessageBox.Show("Пользователь не найден.");
                return;
            }
            MainFrame.Navigate(new AccountPage(currentUser));
        }
    }
}

