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
    /// Interaction logic for PagePrihodnaya.xaml
    /// </summary>
    public partial class PagePrihodnaya : Page
    {
        GetTovara _cgt = null;
        public PagePrihodnaya(GetTovara currentGetTovara)
        {
            InitializeComponent();
            _cgt = currentGetTovara;
            BtnAdd.Content = Properties.Resources.BtnEdit;
        }

        public PagePrihodnaya()
        {
            InitializeComponent();
            BtnAdd.Content = Properties.Resources.BtnAdd;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateComboBoxes();
            if (_cgt!=null)
            {
                CBxTovar.SelectedItem = _cgt.Tovares;
                CBxSklad.SelectedItem = _cgt.Sklad;
                CBxManager.SelectedItem = _cgt.Users;
                TBxCount.Text = _cgt.Count.ToString();
                TBxPrice.Text = _cgt.Price.ToString();
                DPDateOfGet.SelectedDate = _cgt.DateOfGet;
            }
            UpdateComboBoxes();
            AppData.WindowAddEdit.HideBtnBack();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();
            int count = 0;
            decimal price = 0;
            if (!(CBxTovar.SelectedItem is Tovares))
                error.AppendLine(Properties.Resources.ErrorTovar);
            if (!(CBxSklad.SelectedItem is Sklad))
                error.AppendLine(Properties.Resources.ErrorSklad);
            if (!(CBxManager.SelectedItem is Users))
                error.AppendLine(Properties.Resources.ErrorManager);
            if (string.IsNullOrWhiteSpace(TBxCount.Text))
                error.AppendLine(Properties.Resources.ErrorCountEmpty);
            else
                if (!int.TryParse(TBxCount.Text, out count))
                error.AppendLine(Properties.Resources.ErrorCountFormat);
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

                if (_cgt == null)
                {
                    GetTovara getTovar = new GetTovara()
                    {
                        GetId = AppData.Context.GetTovara.Max(p => p.GetId) + 1,
                        Tovares = CBxTovar.SelectedItem as Tovares,
                        Sklad = CBxSklad.SelectedItem as Sklad,
                        Count = count,
                        Price = price,
                        Users = CBxManager.SelectedItem as Users,
                        DateOfGet = (DateTime)DPDateOfGet.SelectedDate
                    };
                    getTovar.Tovares.Count = getTovar.Tovares.Count + count;
                    AppData.Context.GetTovara.Add(getTovar);
                    System.Windows.MessageBox.Show(Properties.Resources.MessageSuccessfullAdd, Properties.Resources.CaptionSuccessfully,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _cgt.Tovares = CBxTovar.SelectedItem as Tovares;
                    _cgt.Sklad = CBxSklad.SelectedItem as Sklad;
                    _cgt.Count = count;
                    _cgt.Price = price;
                    _cgt.Users = CBxManager.SelectedItem as Users;
                    _cgt.DateOfGet = (DateTime)DPDateOfGet.SelectedDate;
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

        void UpdateComboBoxes()
        {
            var tovares = AppData.Context.Tovares.ToList();
            var sklads = AppData.Context.Sklad.ToList();
            var managers = AppData.Context.Users.ToList();
            switch (AppData.currentUser.RoleId)
            {
                case 0:
                    {
                        CBxSklad.IsEditable = true;
                        managers = managers.Where(p => p.RoleId == 2).ToList();
                        tovares.Insert(0, new Tovares()
                        {
                            TovarName = Properties.Resources.CBxAddTovar
                        });
                        sklads.Insert(0, new Sklad()
                        {
                            SkladName = Properties.Resources.CBxAddSklad
                        });
                        managers.Insert(0, new Users()
                        {
                            Name = Properties.Resources.CBxAddUser
                        });
                        break;
                    }
                case 2:
                    {
                        CBxSklad.IsEditable = false;
                        managers = managers.Where(p => p.UserId == AppData.currentUser.UserId).ToList();
                        CBxManager.SelectedIndex = 0;
                        break;
                    }
            }
            CBxTovar.ItemsSource = tovares;
            CBxSklad.ItemsSource = sklads;
            CBxManager.ItemsSource = managers;
        }

        private void CBxTovar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                AppData.WindowAddEdit.ChangePage(new PageAddTovar(this));
            }
        }

        private void CBxSklad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0)
            {

                if (((ComboBox)sender).Text.Equals(""))
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddSkladEmpty, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = null;
                    return;
                }
                if (AppData.Context.Sklad.Where(p => p.SkladName.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().Count != 0)
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddSkladDuplicates, Properties.Resources.CaptionError,
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

        private void CBxManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                AppData.WindowAddEdit.ChangePage(new PageAddUser(this));
            }
        }

    }
}
