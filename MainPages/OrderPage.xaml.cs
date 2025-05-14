using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        private Users _currentUser;

        TechnoAutoEntities autoEntities = new TechnoAutoEntities();

            public List<TicketStatuses> Statuses { get; set; }

        public OrderPage()
        {
            InitializeComponent();
            LoadData();
            LoadStatuses();
            switch (GlobalClass.RoleIDLoged)
            {
                case 1:
                    break;

                case 2:
                    complete.Visibility= Visibility.Collapsed;
                    break;
                case 3:                    
                    break;
            }
        }

        private void LoadStatuses()
        {
            Statuses = autoEntities.TicketStatuses.ToList();
        }

        private void LoadData()
        {
            autoEntities.Tickets.Load();

            if (GlobalClass.RoleIDLoged == 3) // роль "пользователь"
            {
                int currentUserId = GlobalClass.UserIDLoged;

                // Показываем только заявки текущего пользователя
                var userTickets = autoEntities.Tickets.Local.Where(t => t.creator_id == currentUserId).ToList();
                DgOrder.ItemsSource = userTickets;
            }
            else if (GlobalClass.RoleIDLoged == 1 || GlobalClass.RoleIDLoged == 2)
            {
                // Роли 1 и 2 видят все заявки
                DgOrder.ItemsSource = autoEntities.Tickets.Local.ToBindingList();
            }
            else
            {
                // На всякий случай, если есть другие роли - можно скрыть все заявки или показать пустой список
                DgOrder.ItemsSource = new List<Tickets>();
            }
        }              

        private void finish_Click(object sender, RoutedEventArgs e)
        {

            Button btn = sender as Button;
            if (btn == null) return;


            var user = btn.DataContext as Tickets;
            if (user == null) return;


            var result = MessageBox.Show("Завершить запрос ?",
                                         "Подтверждение удаления",
                                         MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {

                    autoEntities.Tickets.Remove(user);
                    autoEntities.SaveChanges();


                    DgOrder.ItemsSource = null;
                    DgOrder.ItemsSource = autoEntities.Users.Local.ToBindingList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении: " + ex.Message);
                }
            }

            //var button = sender as Button;
            //if (button == null) return;

            //// Получаем DataContext строки (выбранный тикет)
            //var ticket = button.DataContext as Tickets;
            //if (ticket == null) return;

            //// Находим ComboBox в той же строке
            //var stackPanel = button.Parent as StackPanel;
            //if (stackPanel == null) return;

            //var comboBox = stackPanel.Children.OfType<ComboBox>().FirstOrDefault();
            //if (comboBox == null) return;

            //var selectedStatus = comboBox.SelectedItem as TicketStatuses;
            //if (selectedStatus == null) return;

            //// Обновляем тикет
            //ticket.status_id = selectedStatus.status_id; // или как у вас хранится статус
            //ticket.closed_at = DateTime.Now;
            //try { 
            //// Сохраняем изменения в базе
            //autoEntities.SaveChanges();
            //var lastId = autoEntities.TicketHistory
            //            .OrderByDescending(x => x.history_id)
            //            .Select(x => x.history_id)
            //            .FirstOrDefault();
            //GlobalClass.LastIdHistory = lastId;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка при сохранении данных инцидента: {ex.Message}");
            //    return;
            //}
            //var employee = autoEntities.Employees.FirstOrDefault(x => x.employee_id == GlobalClass.UserIDLoged);
            //if (employee != null)
            //{


            //    // Получение NewHistoryId из GlobalClass
            //    int historyId = GlobalClass.LastIdHistory;

            //    // Поиск записи в истории по NewHistoryId
            //    var historyRecord = autoEntities.TicketHistory.FirstOrDefault(h => h.history_id == historyId);
            //    if (historyRecord != null)
            //    {
            //        // Объединение FirstName и LastName
            //        string fullName = $"{employee.first_name} {employee.last_name}";
            //        // Обновляем поле EmployeeUpdate в записи истории
            //        historyRecord.EmployUpdate = fullName;

            //        try
            //        {
            //            autoEntities.SaveChanges();
            //            LoadData(); // Перезагрузка инцидентов для отражения изменений
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show($"Ошибка при сохранении данных истории: {ex.Message}");
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Запись в истории не найдена.");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Сотрудник не найден.");
            //}
            //// Обновляем DataGrid
            //DgOrder.Items.Refresh();

            //var history = new TicketHistory
            //{
            //    ticket_id = ticket.ticket_id,
            //    change_type = "Завершён",
            //    change_date = DateTime.Now,
            //    new_value = selectedStatus.name
            //};

            //autoEntities.TicketHistory.Add(history);
            //autoEntities.SaveChanges();
        }

        private void commenent_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Comment(_currentUser));
            NavigationService.Navigate(new Comment());
        }
    }
}
