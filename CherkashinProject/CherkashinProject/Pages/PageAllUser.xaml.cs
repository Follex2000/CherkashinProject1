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
            var user = DataGridAllUser.SelectedItem as Users;
            if (user.RoleId==0)
            {
                if (user==AppData.currentUser)
                {
                    if (MessageBox.Show("Вы уверены, что хотите удалить сами себя?", "Уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        AppData.Context.Users.Remove(user);
                        AppData.Context.SaveChanges();
                        AppData.MainFrame.Navigate(new AutoRizationPage());
                    }
                }
                else
                {
                    MessageBox.Show("Вы не можете удалить администратора", Properties.Resources.CaptionError, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    AppData.Context.Users.Remove(user);
                    AppData.Context.SaveChanges();
                }
            }
            UpdateUsers();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var user = DataGridAllUser.SelectedItem as Users;
            if (user.RoleId == 0)
            {
                if (user != AppData.currentUser)
                {
                    MessageBox.Show("Вы не можете удалить администратора", Properties.Resources.CaptionError, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            AppData.WindowAddEdit = new WindowAddEdit();
            AppData.WindowAddEdit.ChangePage(new PageAddUser(user));
            AppData.WindowAddEdit.ShowDialog();
            UpdateUsers();
        }
    }
}
