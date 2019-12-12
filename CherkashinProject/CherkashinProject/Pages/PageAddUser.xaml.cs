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
        private bool isCurrentUserEdit = false;
        private Page _pg = null;
        private Users _cu = null;
        public PageAddUser(Users currentUser)
        {
            InitializeComponent();
            BtnAdd.Content = Properties.Resources.BtnEdit;
            _cu = currentUser;
            if (currentUser == AppData.currentUser)
                isCurrentUserEdit = true;
        }

        void UpdateComboBoxes()
        {
            var roles = AppData.Context.Role.ToList();
            roles.Insert(0, new Role()
            {
                RoleName = Properties.Resources.CBxAddRole
            });   
            CBxRole.ItemsSource = roles;
        }

        public PageAddUser()
        {
            InitializeComponent();
            BtnAdd.Content = Properties.Resources.BtnAdd;
        }

        public PageAddUser(Page page)
        {
            InitializeComponent();
            _pg = page;
            BtnAdd.Content = Properties.Resources.BtnAdd;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PBxPassword.IsEnabled = true;
            UpdateComboBoxes();
            if (_pg==null)
            {
                AppData.WindowAddEdit.HideBtnBack();
            }
            else
            {
                AppData.WindowAddEdit.ShowBtnBack();
            }
            if (_cu != null)
            {
                if (!isCurrentUserEdit)
                {
                    PBxPassword.IsEnabled = false;
                }
                TBxName.Text = _cu.Name;
                TBxLogin.Text = _cu.Login;
                PBxPassword.Password = _cu.Password;
                CBxRole.SelectedItem = _cu.Role;
            }
            
        }

        private void CBxRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0)
            {

                if (((ComboBox)sender).Text.Equals(""))
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddRoleEmpty, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = null;
                    return;
                }
                if (AppData.Context.Sklad.Where(p => p.SkladName.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().Count != 0)
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddRoleDuplicates, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = AppData.Context.Sklad.Where(p => p.SkladName.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().FirstOrDefault();
                    return;
                }
                var sklad = new Sklad()
                {
                    SkladId = AppData.Context.Sklad.Max(p => p.SkladId) + 1,
                    SkladName = ((ComboBox)sender).Text
                };
                AppData.Context.Sklad.Add(sklad);
                AppData.Context.SaveChanges();
                UpdateComboBoxes();
                ((ComboBox)sender).SelectedItem = sklad;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();
            if (string.IsNullOrWhiteSpace(TBxName.Text))
                error.AppendLine(Properties.Resources.ErrorName);
            if (string.IsNullOrWhiteSpace(TBxLogin.Text))
                error.AppendLine(Properties.Resources.ErrorLogin);
            if (string.IsNullOrWhiteSpace(PBxPassword.Password))
                error.AppendLine(Properties.Resources.ErrorPassword);
            if (!(CBxRole.SelectedItem is Role))
                error.AppendLine(Properties.Resources.ErrorRole);
            if (!error.ToString().Equals(""))
            {
                System.Windows.MessageBox.Show(Properties.Resources.ErrorSomethingWrong + "\n\n" + error, Properties.Resources.CaptionError,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (_cu == null)
                {
                    Users user = new Users()
                    {
                        UserId = AppData.Context.Users.Max(p => p.UserId) + 1,
                        Login = TBxLogin.Text,
                        Name = TBxName.Text,
                        Password = PBxPassword.Password,
                        Role = CBxRole.SelectedItem as Role,
                    };
                    AppData.Context.Users.Add(user);
                    System.Windows.MessageBox.Show(Properties.Resources.MessageSuccessfullAdd, Properties.Resources.CaptionSuccessfully,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _cu.Login = TBxLogin.Text;
                    _cu.Name = TBxName.Text;
                    _cu.Password = PBxPassword.Password;
                    _cu.Role = CBxRole.SelectedItem as Role;
                    System.Windows.MessageBox.Show(Properties.Resources.MessageSuccessfullEdit, Properties.Resources.CaptionSuccessfully,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                AppData.Context.SaveChanges();
                AppData.WindowAddEdit.CloseDialog();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(Properties.Resources.ErrorUnspecified + ex.Message, Properties.Resources.CaptionError,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

    }
}
