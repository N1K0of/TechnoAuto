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
    /// Логика взаимодействия для TexSupportPage.xaml
    /// </summary>
    public partial class TexSupportPage : Page
    {

        TechnoAutoEntities autoEntities = new TechnoAutoEntities();

        public TexSupportPage()
        {
            InitializeComponent();
            LoadCategori();
            LoadPriority();
        }
        private void LoadCategori()
        {

            var categori = autoEntities.Categories.ToList();
            ChoseCategory.ItemsSource = categori;
            ChoseCategory.DisplayMemberPath = "name";
            ChoseCategory.SelectedValuePath = "category_id";
        }

        private void LoadPriority()
        {

            var priorety = autoEntities.Priorities.ToList();
            ChosePriority.ItemsSource = priorety;
            ChosePriority.DisplayMemberPath = "name";
            ChosePriority.SelectedValuePath = "priority_id";
        }

        private void ClearFields()
        {
            EnterProblems.Clear();
            EnterThemse.Clear();
            ChoseCategory.SelectedIndex = -1;
            ChosePriority.SelectedIndex = -1;
        }

        private void AddNewProblem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EnterProblems.Text == "" && EnterThemse.Text == "" && ChoseCategory.Text == "" && ChosePriority.Text == "")
                {
                    MessageBox.Show("Поля пустые, заполните все поля", "Внмание!", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return;
                }
                else
                {
                    var selectedCategory = (Categories)ChoseCategory.SelectedItem;
                    var selectedPriority = (Priorities)ChosePriority.SelectedItem;


                    Tickets tickets = new Tickets()
                    {
                        title = EnterThemse.Text,
                        description = EnterProblems.Text,
                        category_id = selectedCategory.category_id,
                        priority_id = selectedPriority.priority_id,
                        created_at = DateTime.Now,
                        creator_id = 1,
                        status_id = 1
                    };



                    autoEntities.Tickets.Add(tickets);
                    autoEntities.SaveChanges();

                    MessageBox.Show("Запрос успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении заявки: " + ex.Message + "\n" + ex.InnerException?.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
    }
}
