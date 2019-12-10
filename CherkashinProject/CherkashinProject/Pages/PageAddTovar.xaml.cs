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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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
                AppData.Context.SaveChanges();
                var newCountry = new Country()
                {
                    CountryId = 0,
                    CountryName = Properties.Resources.CBxAddCountry
                };
                var climates = AppData.Context.Country.ToList();
                climates.Insert(0, newCountry);
                ((ComboBox)sender).ItemsSource = climates.OrderBy(p => p.CountryId);
                ((ComboBox)sender).SelectedItem = country;
            }
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();
            int article = 0;
            if (string.IsNullOrWhiteSpace(TBxHotelName.Text))
                error.AppendLine(Properties.Resources.ErrorHotelName);
            if (string.IsNullOrWhiteSpace(TBxAddress.Text))
                error.AppendLine(Properties.Resources.ErrorAddress);
            if (!(CBxCountry.SelectedItem is Country))
                error.AppendLine(Properties.Resources.ErrorCountry);
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
                    Tovares hotel = new Tovares()
                    {
                        TovarId = AppData.Context.Tovares.Max(p => p.TovarId) + 1,
                        TovarName = TBxTovarName.Text,
                        TovarArticle = article,
                        Country = CBxCountry.SelectedItem as Country,
                        Stars = int.Parse(CBxRating.SelectedValue.ToString()),
                        Address = TBxAddress.Text
                    };
                    AppData.context.Hotels.Add(hotel);
                    System.Windows.MessageBox.Show(Properties.Resources.MessageSuccessfullAdd, Properties.Resources.CaptionSuccessfully,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _ct.NameHotel = TBxHotelName.Text;
                    _ct.Country = CBxCountry.SelectedItem as Country;
                    _ct.Stars = int.Parse(CBxRating.SelectedValue.ToString());
                    _ct.Address = TBxAddress.Text;
                    System.Windows.MessageBox.Show(Properties.Resources.MessageSuccessfullEdit, Properties.Resources.CaptionSuccessfully,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                AppData.context.SaveChanges();
                AppData.WindowAdd.CloseDialog();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(Properties.Resources.ErrorUnspecified + ex.Message, Properties.Resources.CaptionError,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
