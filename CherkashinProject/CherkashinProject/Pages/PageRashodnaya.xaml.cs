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
    /// Interaction logic for PageRashodnaya.xaml
    /// </summary>
    public partial class PageRashodnaya : Page
    {
        private PostTovara _cpt = null;
        public PageRashodnaya()
        {
            InitializeComponent();
            BtnAdd.Content = Properties.Resources.BtnAdd;
        }

        public PageRashodnaya(PostTovara postTovara)
        {
            InitializeComponent();
            _cpt = postTovara;
            BtnAdd.Content = Properties.Resources.BtnEdit;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppData.WindowAddEdit.HideBtnBack();
            UpdateComboBoxes();
            if (_cpt!=null)
            {
                CBxTovar.SelectedItem = _cpt.Tovares;
                CBxKontragent.SelectedItem = _cpt.Kontragent;
                CBxManager.SelectedItem = _cpt.Users;
                TBxCount.Text = _cpt.Count.ToString();
                TBxPrice.Text = _cpt.Price.ToString();
                DPDateOfSale.SelectedDate = _cpt.DateOfPost;
            }
        }

        void UpdateComboBoxes()
        {
            var tovares = AppData.Context.Tovares.Where(p=>p.Count>0).ToList();
            var kontragents = AppData.Context.Kontragent.ToList();
            var users = AppData.Context.Users.ToList();
            switch (AppData.currentUser.RoleId)
            {
                case 0:
                    {
                        CBxKontragent.IsEditable = true;
                        tovares.Insert(0, new Tovares()
                        {
                            TovarName = Properties.Resources.CBxAddColor
                        });
                        kontragents.Insert(0, new Kontragent()
                        {
                            KontragentName = Properties.Resources.CBxAddSklad
                        });
                        users = users.Where(p => p.RoleId == 2 || p.RoleId == 1).ToList();
                        users.Insert(0, new Users()
                        {
                            Name = Properties.Resources.CBxAddUser
                        });
                        break;
                    }
                case 1:
                    {
                        CBxKontragent.IsEditable = false;
                        users = users.Where(p => p.UserId == AppData.currentUser.UserId).ToList();
                        CBxManager.SelectedIndex = 0;
                        break;
                    }
                case 2:
                    {
                        CBxKontragent.IsEditable = false;
                        users = users.Where(p => p.UserId == AppData.currentUser.UserId || p.RoleId == 1).ToList();
                        CBxManager.SelectedItem = AppData.currentUser;
                        break;
                    }
            }
            CBxTovar.ItemsSource = tovares;
            CBxKontragent.ItemsSource = kontragents;
            CBxManager.ItemsSource = users;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();
            int count = 0;
            decimal price = 0;
            if (!(CBxTovar.SelectedItem is Tovares))
                error.AppendLine(Properties.Resources.ErrorTovar);
            if (!(CBxKontragent.SelectedItem is Kontragent))
                error.AppendLine(Properties.Resources.ErrorSklad);
            if (!(CBxManager.SelectedItem is Users))
                error.AppendLine(Properties.Resources.ErrorManager);
            if (string.IsNullOrWhiteSpace(TBxCount.Text))
                error.AppendLine(Properties.Resources.ErrorCountEmpty);
            else
            if (!int.TryParse(TBxCount.Text, out count))
                error.AppendLine(Properties.Resources.ErrorCountFormat);
            else if (count > ((Tovares)CBxTovar.SelectedItem).Count)
            {
                error.AppendLine(Properties.Resources.ErrorCountFormat);
            }
            if (string.IsNullOrWhiteSpace(TBxPrice.Text))
                error.AppendLine(Properties.Resources.ErrorPriceEmpty);
            else
                if (!decimal.TryParse(TBxPrice.Text, out price))
                error.AppendLine(Properties.Resources.ErrorPriceFormat);
            if (!error.ToString().Equals(""))
            {
                System.Windows.MessageBox.Show(Properties.Resources.ErrorSomethingWrong + "\n\n" + error, Properties.Resources.CaptionError,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {

                if (_cpt == null)
                {
                    PostTovara postTovara = new PostTovara()
                    {
                        PostId = AppData.Context.PostTovara.Max(p => p.PostId) + 1,
                        Tovares = CBxTovar.SelectedItem as Tovares,
                        Kontragent = CBxKontragent.SelectedItem as Kontragent,
                        Count = count,
                        Price = price,
                        Users = CBxManager.SelectedItem as Users,
                        DateOfPost = (DateTime)DPDateOfSale.SelectedDate
                    };
                    postTovara.Tovares.Count = postTovara.Tovares.Count - count;
                    AppData.Context.PostTovara.Add(postTovara);
                    System.Windows.MessageBox.Show(Properties.Resources.MessageSuccessfullAdd, Properties.Resources.CaptionSuccessfully,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _cpt.Tovares = CBxTovar.SelectedItem as Tovares;
                    _cpt.Kontragent = CBxKontragent.SelectedItem as Kontragent;
                    _cpt.Count = count;
                    _cpt.Price = price;
                    _cpt.Users = CBxManager.SelectedItem as Users;
                    _cpt.DateOfPost = (DateTime)DPDateOfSale.SelectedDate;
                    System.Windows.MessageBox.Show(Properties.Resources.MessageSuccessfullEdit, Properties.Resources.CaptionSuccessfully,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                AppData.WindowAddEdit.CloseDialog();
                AppData.Context.SaveChanges();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(Properties.Resources.ErrorUnspecified + ex.Message, Properties.Resources.CaptionError,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CBxTovar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppData.currentUser.RoleId != 0)
            {
                return;
            }
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                AppData.WindowAddEdit.ChangePage(new PageAddTovar(this));
            }
            UpdateComboBoxes();
        }

        private void CBxKontragent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppData.currentUser.RoleId != 0)
            {
                return;
            }
            if (((ComboBox)sender).SelectedIndex == 0)
            {

                if (((ComboBox)sender).Text.Equals(""))
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddKontragentEmpty, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = null;
                    return;
                }
                if (AppData.Context.Kontragent.Where(p => p.KontragentName.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().Count != 0)
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddKontragentDuplicates, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = AppData.Context.Kontragent.Where(p => p.KontragentName.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().FirstOrDefault();
                    return;
                }
                var kontragent = new Kontragent()
                {
                    KontragentId = AppData.Context.Kontragent.Max(p => p.KontragentId) + 1,
                    KontragentName = ((ComboBox)sender).Text
                };
                AppData.Context.Kontragent.Add(kontragent);
                AppData.Context.SaveChanges();
                UpdateComboBoxes();
                ((ComboBox)sender).SelectedItem = kontragent;
            }
        }
        private void CBxManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppData.currentUser.RoleId!=0)
            {
                return;
            }
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                AppData.WindowAddEdit.ChangePage(new PageAddUser(this));
            }
            UpdateComboBoxes();
        }

        private void TBxCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            string buf = "";
            char[] array = (sender as TextBox).Text.ToCharArray();
            foreach (var item in array)
            {
                if (Char.IsDigit(item))
                    buf += item;
            }
            (sender as TextBox).Text = buf;
            (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
        }

        private void TBxPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            string buf = "";
            bool dot = true;
            char[] array = (sender as TextBox).Text.ToCharArray();
            foreach (var item in array)
            {
                if (Char.IsDigit(item))
                    buf += item;
                if (item == '.' && dot)
                {
                    buf += item;
                    dot = false;
                }
            }
            (sender as TextBox).Text = buf;
            (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
        }
    }
}
