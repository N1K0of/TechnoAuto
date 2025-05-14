using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для HistoryTable.xaml
    /// </summary>
    public partial class HistoryTable : Page
    {

        TechnoAutoEntities autoEntities = new TechnoAutoEntities();

        public HistoryTable()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            autoEntities.TicketHistory.Load();
            DgHistory.ItemsSource = autoEntities.TicketHistory.Local.ToBindingList();
        }

    }
}
