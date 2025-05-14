using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    /// Логика взаимодействия для Comment.xaml
    /// </summary>
    public partial class Comment : Page
    {
        //private Users _currentUser;
        //private int _ticketId;

        public Comment()
        {
            InitializeComponent();
            //_currentUser = currentUser;
            //LoadMessages();
            //_ticketId = ticketId;
        }

        //private void LoadMessages()
        //{
        //    using (var context = new TechnoAutoEntities())
        //    {
        //        var messages = context.Comments.OrderBy(m => m.created_at).ToList();
        //        MessagesListBox.ItemsSource = messages.Select(m =>
        //        {
        //            var user = context.Users.FirstOrDefault(u => u.user_id == m.author_id);
        //            return $"{user?.first_name ?? "Unknown"}: {m.comment_text}";
        //        }).ToList();
        //    }
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    string messageContent = MessageTextBox.Text.Trim();

        //    if (string.IsNullOrEmpty(messageContent))
        //    {
        //        MessageBox.Show("Введите сначала сообщение.", "Внмание", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    using (var context = new TechnoAutoEntities())
        //    {
                  
        //        //var message = new Comments
        //        //{
        //        //    ticket_id = _ticketId,                   
        //        //    comment_text = messageContent,
        //        //    author_id = context.TicketHistory.,
        //        //    created_at = DateTime.Now
        //        //};

        //        context.Comments.Add(message);
        //        context.SaveChanges();
        //    }

        //    MessageTextBox.Clear();
        //    LoadMessages();
        //}
    }
}
