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
    /// Логика взаимодействия для PageAllTovar.xaml
    /// </summary>
    public partial class PageAllTovar : Page
    {
        public PageAllTovar()
        {
            InitializeComponent();
            var countries = AppData.Context.Country.ToList();
            countries.Insert(0, new Entity.Country
            {
                CountryName = "Все записи"
            });
            CBxSearch.ItemsSource = countries;
            CBxSearch.SelectedIndex = 0;
            UpdateTovares();
        }

        public void UpdateTovares()
        {
            var tovares = AppData.Context.Tovares.ToList();
            if (CBxSearch.SelectedIndex > 0)
            {
                tovares = tovares.Where(p => p.Country == CBxSearch.SelectedItem).ToList();
            }
            if (!TBxSearch.Text.Equals(""))
                tovares = tovares.Where(p => p.TovarName.ToLower().Contains(TBxSearch.Text.ToLower())).ToList();
            if (tovares.Count == 0)
            {
                TbNothing.Visibility = Visibility.Visible;
                DataGridTovar.Visibility = Visibility.Hidden;
            }
            else
            {
                TbNothing.Visibility = Visibility.Hidden;
                DataGridTovar.Visibility = Visibility.Visible;
            }
            DataGridTovar.ItemsSource = tovares;
        }

        private void TBxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTovares();
        }

        private void CBxSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTovares();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (AppData.currentUser.RoleId==1|| AppData.currentUser.RoleId == 2)
            {
                MessageBox.Show("У вас недостаточно прав!", Properties.Resources.CaptionError, MessageBoxButton.OK, MessageBoxImage.Error;
                return;
            }
            if (MessageBox.Show("Вы уверены, что хотите удалить этот товар?", "Уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AppData.Context.Tovares.Remove(DataGridTovar.SelectedItem as Tovares);
                AppData.Context.SaveChanges();
            }
            UpdateTovares();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (AppData.currentUser.RoleId == 1AppData.currentUser.RoleId == 2)
            {
                MessageBox.Show("У вас недостаточно прав!", Properties.Resources.CaptionError, MessageBoxButton.OK, MessageBoxImage.Error;
                return;
            }
            AppData.WindowAddEdit = new WindowAddEdit();
            AppData.WindowAddEdit.ChangePage(new PageAddTovar(DataGridTovar.SelectedItem as Tovares));
            AppData.WindowAddEdit.ShowDialog();
            UpdateTovares();
        }
    }
}
