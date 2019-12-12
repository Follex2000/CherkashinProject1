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
    /// Логика взаимодействия для PageGetTovar.xaml
    /// </summary>
    public partial class PageGetTovar : Page
    {
        public PageGetTovar()
        {
            InitializeComponent();
            var tovares = AppData.Context.Tovares.ToList();
            tovares.Insert(0, new Entity.Tovares
            {
                TovarName = "Все товары"
            });
            CBxSearch.ItemsSource = tovares;
            CBxSearch.SelectedIndex = 0;
            UpdateGetTovares();
        }

        public void UpdateGetTovares()
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
            UpdateGetTovares();
        }

        private void CBxSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGetTovares();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить эту приходную?", "Уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AppData.Context.GetTovara.Remove(DataGridGetTovar.SelectedItem as GetTovara);
                AppData.Context.SaveChanges();
            }
            UpdateGetTovares();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            AppData.WindowAddEdit = new WindowAddEdit();
            AppData.WindowAddEdit.ChangePage(new PagePrihodnaya(DataGridGetTovar.SelectedItem as GetTovara));
            AppData.WindowAddEdit.ShowDialog();
            UpdateGetTovares();
        }
    }
}
