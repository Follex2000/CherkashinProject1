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
    /// Логика взаимодействия для AdminPagesMenu.xaml
    /// </summary>
    public partial class AdminPagesMenu : Page
    {
        public AdminPagesMenu()
        {
            InitializeComponent();
        }

        private void ButtonUsers_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new Pages.PageAllUser());
        }

        private void BtnTovar_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.Navigate(new Pages.PageAllTovar());
        }
    }
}
