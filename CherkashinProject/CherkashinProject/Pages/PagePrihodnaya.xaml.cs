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
                        GetId = AppData.Context.Tovares.Max(p => p.TovarId) + 1,
                        Tovares = CBxTovar.SelectedItem as Tovares,
                        Sklad = CBxSklad.SelectedItem as Sklad,
                        Count = count,
                        Price = price,
                        Users = CBxManager.SelectedItem as Users,
                        DateOfGet = (DateTime)DPDateOfGet.SelectedDate
                    };
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
                AppData.Context.SaveChanges();
                AppData.WindowAddEdit.CloseDialog();

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
            var users = AppData.Context.Users.ToList();
            switch (AppData.currentUser.RoleId)
            {
                case 0:
                    {
                        CBxSklad.IsEditable = true;
                        tovares.Insert(0, new Tovares()
                        {
                            TovarName = Properties.Resources.CBxAddColor
                        });
                        sklads.Insert(0, new Sklad()
                        {
                            SkladName = Properties.Resources.CBxAddSklad
                        });
                        users.Insert(0, new Users()
                        {
                            Name = Properties.Resources.CBxAddUser
                        });
                        users = users.Where(p => p.RoleId == 1).ToList();
                        CBxTovar.SelectedIndex = 0;
                        CBxSklad.SelectedIndex = 0;
                        break;
                    }
                case 1:
                    {
                        CBxSklad.IsEditable = false;
                        users = users.Where(p => p.UserId == AppData.currentUser.UserId).ToList();
                        break;
                    }
            }
            CBxManager.SelectedIndex = 0;
        }

        private void CBxTovar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0)
            {

                if (((ComboBox)sender).Text.Equals(""))
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddColorEmpty, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = null;
                    return;
                }
                if (AppData.Context.TovarColor.Where(p => p.Color.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().Count != 0)
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddColorDuplicates, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = AppData.Context.TovarColor.Where(p => p.Color.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().FirstOrDefault();
                    return;
                }
                var color = new TovarColor()
                {
                    ColorId = AppData.Context.TovarColor.Max(p => p.ColorId) + 1,
                    Color = ((ComboBox)sender).Text
                };
                AppData.Context.TovarColor.Add(color);
                AppData.Context.SaveChanges();
                var newColor = new TovarColor()
                {
                    ColorId = -1,
                    Color = Properties.Resources.CBxAddColor
                };
                var colors = AppData.Context.TovarColor.ToList();
                colors.Insert(0, newColor);
                ((ComboBox)sender).ItemsSource = colors.OrderBy(p => p.ColorId);
                ((ComboBox)sender).SelectedItem = color;
            }
        }

        private void CBxSklad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0)
            {

                if (((ComboBox)sender).Text.Equals(""))
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddColorEmpty, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = null;
                    return;
                }
                if (AppData.Context.TovarColor.Where(p => p.Color.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().Count != 0)
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddColorDuplicates, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = AppData.Context.TovarColor.Where(p => p.Color.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().FirstOrDefault();
                    return;
                }
                var color = new TovarColor()
                {
                    ColorId = AppData.Context.TovarColor.Max(p => p.ColorId) + 1,
                    Color = ((ComboBox)sender).Text
                };
                AppData.Context.TovarColor.Add(color);
                AppData.Context.SaveChanges();
                var newColor = new TovarColor()
                {
                    ColorId = -1,
                    Color = Properties.Resources.CBxAddColor
                };
                var colors = AppData.Context.TovarColor.ToList();
                colors.Insert(0, newColor);
                ((ComboBox)sender).ItemsSource = colors.OrderBy(p => p.ColorId);
                ((ComboBox)sender).SelectedItem = color;
            }
        }

        private void CBxManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0)
            {

                if (((ComboBox)sender).Text.Equals(""))
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddColorEmpty, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = null;
                    return;
                }
                if (AppData.Context.TovarColor.Where(p => p.Color.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().Count != 0)
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddColorDuplicates, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = AppData.Context.TovarColor.Where(p => p.Color.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().FirstOrDefault();
                    return;
                }
                var color = new TovarColor()
                {
                    ColorId = AppData.Context.TovarColor.Max(p => p.ColorId) + 1,
                    Color = ((ComboBox)sender).Text
                };
                AppData.Context.TovarColor.Add(color);
                AppData.Context.SaveChanges();
                var newColor = new TovarColor()
                {
                    ColorId = -1,
                    Color = Properties.Resources.CBxAddColor
                };
                var colors = AppData.Context.TovarColor.ToList();
                colors.Insert(0, newColor);
                ((ComboBox)sender).ItemsSource = colors.OrderBy(p => p.ColorId);
                ((ComboBox)sender).SelectedItem = color;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_cgt!=null)
            {
                
            }
        }
    }
}
