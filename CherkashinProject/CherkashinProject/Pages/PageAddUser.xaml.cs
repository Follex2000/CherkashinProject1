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
    /// Interaction logic for PageAddUser.xaml
    /// </summary>
    public partial class PageAddUser : Page
    {
        Users _cu = null;
        public PageAddUser(Users currentUser)
        {
            InitializeComponent();
            _cu = currentUser;
            
        }
    }
}
