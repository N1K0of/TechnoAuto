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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TechnoAuto2.MainPages
{
    /// <summary>
    /// Логика взаимодействия для AddNewEmploy.xaml
    /// </summary>
    public partial class AddNewEmploy : Page
    {

        TechnoAutoEntities technoAuto = new TechnoAutoEntities();

        public AddNewEmploy()
        {
            InitializeComponent();
            LoadDepartments();
            LoadRoles();
        }
        private void LoadDepartments()
        {

            var departments = technoAuto.Departments.ToList();
            ChooseDepartment.ItemsSource = departments;
            ChooseDepartment.DisplayMemberPath = "name";
            ChooseDepartment.SelectedValuePath = "department_id";
        }

        private void LoadRoles()
        {

            var roles = technoAuto.Role.ToList();
            ChooseRole.ItemsSource = roles;
            ChooseRole.DisplayMemberPath = "name";
            ChooseRole.SelectedValuePath = "role_id";
        }

        private bool IsValidPhone(string phone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\d{10,15}$");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EnterFirstName.Text) ||
                string.IsNullOrEmpty(EnterLastName.Text) ||               
                string.IsNullOrEmpty(EnterPhone.Text) ||
                string.IsNullOrEmpty(EnterEmail.Text) ||
                string.IsNullOrEmpty(EnterAdress.Text) ||
                EnterDateOfBirth.SelectedDate == null ||
                EnterDatePriem.SelectedDate == null ||
                ChooseDepartment.SelectedItem == null ||
                ChooseRole.SelectedItem == null)
            {
                MessageBox.Show("Заполните все обязательные поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidEmail(EnterEmail.Text))
            {
                MessageBox.Show("Некорректный формат e-mail!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPhone(EnterPhone.Text))
            {
                MessageBox.Show("Некорректный формат номера телефона! Введите только цифры, длина 10-15 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var selectedDepartment = (Departments)ChooseDepartment.SelectedItem;
                var selectedRole = (Role)ChooseRole.SelectedItem;
                bool isSupport = selectedDepartment.name.Trim().ToLower() == "поддержка";

                Employees newEmployee = new Employees()
                {
                    first_name = EnterFirstName.Text,
                    last_name = EnterLastName.Text,
                    MiddleName = EnterMiddleName.Text,
                    email = EnterEmail.Text,
                    adress = EnterAdress.Text,
                    phone = EnterPhone.Text,
                    birth_date = EnterDateOfBirth.SelectedDate.Value,
                    hire_date = EnterDatePriem.SelectedDate.Value,
                    department_id = selectedDepartment.department_id,
                    role_id = selectedRole.role_id,
                    is_support = isSupport
                };

                technoAuto.Employees.Add(newEmployee);
                technoAuto.SaveChanges();

                MessageBox.Show("Сотрудник успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении сотрудника: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
