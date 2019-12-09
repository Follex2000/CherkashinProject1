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
using System.Windows.Shapes;

namespace CherkashinProject
{
    /// <summary>
    /// Interaction logic for WindowAddEdit.xaml
    /// </summary>
    public partial class WindowAddEdit : Window
    {
        public WindowAddEdit(Page page)
        {
            InitializeComponent();
            AddWindowFrame.Navigate(page);
        }

        public void CloseDialog()
        {
            DialogResult = true;
        }

        public void ChangePage(Page page)
        {
            AddWindowFrame.Navigate(page);
        }
    }
}
