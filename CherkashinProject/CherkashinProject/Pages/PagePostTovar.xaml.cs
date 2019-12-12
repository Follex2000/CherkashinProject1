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
    /// Interaction logic for PagePostTovar.xaml
    /// </summary>
    public partial class PagePostTovar : Page
    {
        public PagePostTovar()
        {
            InitializeComponent();
            var tovares = AppData.Context.Tovares.ToList();
            tovares.Insert(0, new Entity.Tovares
            {
                TovarName = "Все товары"
            });
            CBxSearch.ItemsSource = tovares;
            CBxSearch.SelectedIndex = 0;
            UpdatePostTovares();
        }

        public void UpdatePostTovares()
        {
            var getTovares = AppData.Context.GetTovara.ToList();
            if (CBxSearch.SelectedIndex > 0)
            {
                getTovares = getTovares.Where(p => p.Tovares == CBxSearch.SelectedItem).ToList();
            }
            if (!TBxSearch.Text.Equals(""))
                getTovares = getTovares.Where(p => p.Users.Name.ToLower().Contains(TBxSearch.Text.ToLower())
                || p.Sklad.SkladName.ToLower().Contains(TBxSearch.Text.ToLower())).ToList();
            if (getTovares.Count == 0)
            {
                TbNothing.Visibility = Visibility.Visible;
                DataGridGetTovar.Visibility = Visibility.Hidden;
            }
            else
            {
                TbNothing.Visibility = Visibility.Hidden;
                DataGridGetTovar.Visibility = Visibility.Visible;
            }
            DataGridGetTovar.ItemsSource = getTovares;
        }

        private void TBxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePostTovares();
        }

        private void CBxSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePostTovares();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить эту расходную?", "Уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AppData.Context.GetTovara.Remove(DataGridGetTovar.SelectedItem as GetTovara);
                AppData.Context.SaveChanges();
            }
            UpdatePostTovares();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            AppData.WindowAddEdit = new WindowAddEdit();
            AppData.WindowAddEdit.ChangePage(new PagePrihodnaya(DataGridGetTovar.SelectedItem as GetTovara));
            AppData.WindowAddEdit.ShowDialog();
            UpdatePostTovares();
        }
    }
}
