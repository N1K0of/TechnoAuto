using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechnoAuto2.Resource.Class;

namespace TechnoAuto2.MainPages
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {

        TechnoAutoEntities autoEntities = new TechnoAutoEntities();
        private ObservableCollection<Users> _users;

        public UserPage()
        {
            InitializeComponent();
            LoadData();
            switch (GlobalClass.RoleIDLoged)
            {
                case 1:
                    break;

                case 2:                    
                    break;
                case 3:
                    AddNewUser.Visibility = Visibility.Collapsed;
                    use.Visibility = Visibility.Collapsed;                 
                    break;
            }
        }
     
        private void LoadData()
        {
            autoEntities.Users.Load();
            DgUser.ItemsSource = autoEntities.Users.Local.ToBindingList();
            autoEntities.Users.Include(u => u.Role).Load();
            _users = autoEntities.Users.Local;
            DgUser.ItemsSource = _users;
        }

        private void Search_KeyUp(object sender, KeyEventArgs e)
        {
            string query = SearchTextBox.Text.Trim().ToLower();
                DgUser.ItemsSource = autoEntities.Users.ToList();
           
            var result = autoEntities.Users
                .Where(emp =>
                    emp.first_name.ToLower().Contains(query) ||
                    emp.last_name.ToLower().Contains(query) ||
                    emp.MiddleName.ToLower().Contains(query) ||
                    emp.Role.name.ToLower().Contains(query) ||
                    emp.email.ToLower().Contains(query))
                .ToList();

            DgUser.ItemsSource = result;
        }

        private void AddNewUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNewUser());
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

            Button btn = sender as Button;
            if (btn == null) return;


            var user = btn.DataContext as Users;
            if (user == null) return;


            var result = MessageBox.Show($"Удалить пользователя {user.first_name} {user.last_name}?",
                                         "Подтверждение удаления",
                                         MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    autoEntities.Users.Remove(user);
                    autoEntities.SaveChanges();

                    DgUser.ItemsSource = null;
                    DgUser.ItemsSource = autoEntities.Users.Local.ToBindingList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении: " + ex.Message);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                autoEntities.SaveChanges();
                MessageBox.Show("Изменения успешно сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
