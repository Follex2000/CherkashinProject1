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

namespace CherkashinProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            if(Properties.Settings.Default.CurrentCulture == "")
            {
                Properties.Settings.Default.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                Properties.Settings.Default.Save();
            }

            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(Properties.Settings.Default.CurrentCulture);


            InitializeComponent();

            if (Properties.Settings.Default.CurrentCulture == "ru-Ru")
            {
                ComboLang.SelectedIndex = 0;
            }
            else ComboLang.SelectedIndex = 1;

            AppData.MainFrame = MainFrame;
            AppData.MainFrame.Navigate(new Pages.AutoRizationPage());
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.GoBack();
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            var page = MainFrame.Content as Page;
            if (page.Title == "AutoRizationPage")
            {
                Btn_Back.Visibility = Visibility.Hidden;
            }
            else Btn_Back.Visibility = Visibility.Visible;

            Title = BlockHeader.Text = page.Title;
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            if (ComboLang.SelectedIndex == 0)
            {
                Properties.Settings.Default.CurrentCulture = "ru-Ru";
            }
            else
            {
                Properties.Settings.Default.CurrentCulture = "en-Us";
            }
            Properties.Settings.Default.Save();

            if (MessageBox.Show(Properties.Resources.TextSelect, Properties.Resources.TextQt, MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
                System.Diagnostics.Process.Start(App.ResourceAssembly.Location);
            }
        }
    }
}
