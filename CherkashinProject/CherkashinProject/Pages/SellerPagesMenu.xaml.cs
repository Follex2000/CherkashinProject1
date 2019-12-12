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
    /// Interaction logic for SellerPagesMenu.xaml
    /// </summary>
    public partial class SellerPagesMenu : Page
    {
        public SellerPagesMenu()
        {
            InitializeComponent();
        }

        private void BtnTovar_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new PageAllTovar());
        }

        private void BtnRashodnaya_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new PagePostTovar());
        }

        private void BtnAddRashodnaya_Click(object sender, RoutedEventArgs e)
        {
            AppData.WindowAddEdit = new WindowAddEdit();
            AppData.WindowAddEdit.ChangePage(new PageRashodnaya());
            AppData.WindowAddEdit.ShowDialog();
        }
    }
}
