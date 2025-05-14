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

namespace TechnoAuto2.MainPages
{
    /// <summary>
    /// Логика взаимодействия для EmployTablePage.xaml
    /// </summary>
    public partial class EmployTablePage : Page
    {

        TechnoAutoEntities autoEntities = new TechnoAutoEntities();
        private ObservableCollection<Employees> _users;

        public EmployTablePage()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            autoEntities.Employees.Load();
            DgEmploy.ItemsSource = autoEntities.Employees.Local.ToBindingList();
            autoEntities.Employees.Include(u => u.Role).Load();
            _users = autoEntities.Employees.Local;
            DgEmploy.ItemsSource = _users;
        }

        private void AddNewEmploy_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNewEmploy());
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Если текст пустой, показываем водяной знак, иначе скрываем
            WatermarkTextBlock.Visibility =
                string.IsNullOrEmpty(SearchTextBox.Text) ?
                Visibility.Visible :
                Visibility.Collapsed;
        }
        private void Search_KeyUp(object sender, KeyEventArgs e)
        {
            string query = SearchTextBox.Text.Trim().ToLower();


            if (string.IsNullOrEmpty(query))
            {
                DgEmploy.ItemsSource = autoEntities.Employees.ToList();
                return;
            }


            var result = autoEntities.Employees
                .Where(emp =>
                    emp.first_name.ToLower().Contains(query) ||
                    emp.last_name.ToLower().Contains(query) ||
                    emp.MiddleName.ToLower().Contains(query) ||
                    emp.Role.name.ToLower().Contains(query) ||
                    emp.email.ToLower().Contains(query))
                .ToList();

            DgEmploy.ItemsSource = result;
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;


            var user = btn.DataContext as Employees;
            if (user == null) return;


            var result = MessageBox.Show($"Удалить пользователя {user.first_name} {user.last_name}?",
                                         "Подтверждение удаления",
                                         MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {

                    autoEntities.Employees.Remove(user);
                    autoEntities.SaveChanges();


                    DgEmploy.ItemsSource = null;
                    DgEmploy.ItemsSource = autoEntities.Employees.Local.ToBindingList();
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
