using CherkashinProject.Entity;
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

namespace CherkashinProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAllUser.xaml
    /// </summary>
    public partial class PageAllUser : Page
    {
        public PageAllUser()
        {
            InitializeComponent();
            var roles = AppData.Context.Role.ToList();
            roles.Insert(0, new Entity.Role
            {
                RoleName = "Все записи"
            });
            CBxSearch.ItemsSource = roles;
            CBxSearch.SelectedIndex = 0;
            UpdateUsers();
        }

        public void UpdateUsers()
        {
            var users = AppData.Context.Users.ToList();
            if (CBxSearch.SelectedIndex > 0)
            {
                users = users.Where(p => p.Role == CBxSearch.SelectedItem).ToList();
            }
            if (!TBxSearch.Text.Equals(""))
                users = users.Where(p => p.Name.ToLower().Contains(TBxSearch.Text.ToLower())).ToList();
            if (users.Count == 0)
            {
                TbNothing.Visibility = Visibility.Visible;
                DataGridAllUser.Visibility = Visibility.Hidden;
            }
            else
            {
                TbNothing.Visibility = Visibility.Hidden;
                DataGridAllUser.Visibility = Visibility.Visible;
            }
            DataGridAllUser.ItemsSource = users;
        }

        private void TBxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void CBxSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AppData.Context.Users.Remove(DataGridAllUser.SelectedItem as Users);
                AppData.Context.SaveChanges();
            }
            UpdateUsers();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            AppData.WindowAddEdit = new WindowAddEdit(new PageAddUser(DataGridAllUser.SelectedItem as Users));
            AppData.WindowAddEdit.ShowDialog();
            UpdateUsers();
        }
    }
}
