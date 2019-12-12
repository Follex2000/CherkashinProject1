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
    /// Interaction logic for PageAddTovar.xaml
    /// </summary>
    public partial class PageAddTovar : Page
    {
        private Page _pg = null;
        Tovares _ct = null;
        public PageAddTovar(Tovares currentTovares)
        {
            InitializeComponent();
            _ct = currentTovares;
            BtnAddEdit.Content = Properties.Resources.BtnEdit;
        }

        public PageAddTovar()
        {
            InitializeComponent();
            BtnAddEdit.Content = Properties.Resources.BtnAdd;
        }

        public PageAddTovar(Page page)
        {
            InitializeComponent();
            _pg = page;
            BtnAddEdit.Content = Properties.Resources.BtnAdd;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_pg==null)
            {
                AppData.WindowAddEdit.HideBtnBack();
            }
            else
            {
                AppData.WindowAddEdit.ShowBtnBack();
            }
            UpdateComboBoxes();
            if (_ct != null)
            {
                TBxTovarName.Text = _ct.TovarName;
                TBxArticle.Text = _ct.TovarArticle.ToString();
                CBxColor.SelectedItem = _ct.TovarColor;
                CBxCountry.SelectedItem = _ct.Country;
            }
        }

        void UpdateComboBoxes()
        {
            var colors = AppData.Context.TovarColor.ToList();
            colors.Insert(0, new TovarColor()
            {
                ColorId = -1,
                Color = Properties.Resources.CBxAddColor
            });
            CBxColor.ItemsSource = colors;
            var countries = AppData.Context.Country.ToList();
            countries.Insert(0, new Country()
            {
                CountryId = -1,
                CountryName = Properties.Resources.CBxAddCountry
            });
            CBxCountry.ItemsSource = countries;
        }

        private void CBxColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                UpdateComboBoxes();
                ((ComboBox)sender).SelectedItem = color;
            }
        }

        private void CBxCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                if (((ComboBox)sender).Text.Equals(""))
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddCountryEmpty, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = null;
                    return;
                }
                if (AppData.Context.Country.Where(p => p.CountryName.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().Count != 0)
                {
                    System.Windows.MessageBox.Show(Properties.Resources.ErrorAddCountryDuplicates, Properties.Resources.CaptionError,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    ((ComboBox)sender).SelectedItem = AppData.Context.Country.Where(p => p.CountryName.ToLower() == ((ComboBox)sender).Text.ToLower()).ToList().FirstOrDefault();
                    return;
                }
                var country = new Country()
                {
                    CountryId = AppData.Context.Country.Max(p => p.CountryId) + 1,
                    CountryName = ((ComboBox)sender).Text
                };
                AppData.Context.Country.Add(country);
                UpdateComboBoxes();
                ((ComboBox)sender).SelectedItem = country;
            }
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();
            int article = 0;
            if (string.IsNullOrWhiteSpace(TBxTovarName.Text))
                error.AppendLine(Properties.Resources.ErrorTovarName);
            if (string.IsNullOrWhiteSpace(TBxArticle.Text))
                error.AppendLine(Properties.Resources.ErrorArticleEmpty);
            else
                if (!int.TryParse(TBxArticle.Text, out article))
                error.AppendLine(Properties.Resources.ErrorArcticleFormat);
            if (!(CBxCountry.SelectedItem is Country))
                error.AppendLine(Properties.Resources.ErrorCountry);
            if (!(CBxColor.SelectedItem is TovarColor))
                error.AppendLine(Properties.Resources.ErrorColor);
            if (!error.ToString().Equals(""))
            {
                System.Windows.MessageBox.Show(Properties.Resources.ErrorSomethingWrong + "\n\n" + error, Properties.Resources.CaptionError,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {

                if (_ct == null)
                {
                    Tovares tovar = new Tovares()
                    {
                        TovarId = AppData.Context.Tovares.Max(p => p.TovarId) + 1,
                        TovarName = TBxTovarName.Text,
                        TovarArticle = article,
                        Country = CBxCountry.SelectedItem as Country,
                        TovarColor = CBxColor.SelectedItem as TovarColor,
                        Count = 0,
                    };
                    AppData.Context.Tovares.Add(tovar);
                    System.Windows.MessageBox.Show(Properties.Resources.MessageSuccessfullAdd, Properties.Resources.CaptionSuccessfully,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _ct.TovarName = TBxTovarName.Text;
                    _ct.TovarArticle = article;
                    _ct.Country = CBxCountry.SelectedItem as Country;
                    _ct.TovarColor = CBxColor.SelectedItem as TovarColor;
                    _ct.Count = 0;
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

        private void TBxTovarName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckForInteger.CheckForInt(sender);
        }

        private void TBxArticle_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckForInteger.CheckForInt(sender);
        }

        
    }
}
