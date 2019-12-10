using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Threading;
using Shape = System.Windows.Shapes;

namespace CherkashinProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для AutoRizationPage.xaml
    /// </summary>
    public partial class AutoRizationPage : Page
    {
            private string _capchaText = "";
        public int CountEnter = 0;
        private DispatcherTimer _dispatchertimer = new DispatcherTimer();
        private DateTime _startDate = new DateTime();
        public AutoRizationPage()
        {
            InitializeComponent();
            _dispatchertimer.Interval = TimeSpan.FromSeconds(1);
            _dispatchertimer.Tick += Dispatchertimer_Tick;
            ImgCapcha.Source = Drawing(Convert.ToInt32(ImgCapcha.Width), Convert.ToInt32(ImgCapcha.Height));
        }

        private DrawingImage Drawing(int width, int height)
        {
            Random random = new Random();
            _capchaText = "";
            string allText = "1234567890QqWwEeRrTtYyUuIiOoPpAaSsDdFfGgHhJjKkLlZzXxCcVvBbNnMm";
            for (int i = 0; i < 6; i++)
            {
                _capchaText += allText[random.Next(allText.Length)];
            }
            byte[] bytes = new byte[width * height * 100];
            random.NextBytes(bytes);
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(BitmapSource.Create(width, height, 300, 300, PixelFormats.Rgb48, null, bytes, width * 30),
                    new Rect(0, 0, width, height));
                drawingContext.DrawText(new FormattedText(_capchaText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface("Arial"), (height / 4 + width / 4) / 2, Brushes.Black), new Point(width / 5, height / 4));
            }
            return new DrawingImage(drawingVisual.Drawing);
        }

        private void Dispatchertimer_Tick(object sender, EventArgs e)
        {
            var deff = DateTime.Now - _startDate;
            if(deff.TotalSeconds > 60)
            {
                BtnLogin.IsEnabled = true;
                _dispatchertimer.Stop();
                BtnLogin.Content = Properties.Resources.ActionLogin;
            }
            else
            {
                BtnLogin.Content = (60 - deff.TotalSeconds).ToString("N0");
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Entity.Users CurrentUser = AppData.Context.Users.ToList().Where(user => user.Login == TBoxLogin.Text && user.Password == PBoxPasswod.Password).FirstOrDefault();
            if (CurrentUser == null)
            {
                MessageBox.Show(Properties.Resources.WrongPass);
                if (CountEnter == 3)
                {
                    _startDate = DateTime.Now;
                    _dispatchertimer.Start();
                    CountEnter = 0;
                    BtnLogin.IsEnabled = false;
                }
                else CountEnter += 1;
            }
            else
            {
                if(_capchaText != TBCapt.Text)
                {
                    MessageBox.Show("Неверный текст капчи!", Properties.Resources.CaptionError, MessageBoxButton.OK, MessageBoxImage.Error);
                    ImgCapcha.Source = Drawing(Convert.ToInt32(ImgCapcha.Width), Convert.ToInt32(ImgCapcha.Height));
                    return;
                }
                AppData.currentUser = CurrentUser;
                switch (CurrentUser.RoleId)
                {
                    case 0:
                        {
                            AppData.MainFrame.Navigate(new Pages.AdminPagesMenu());
                            break;
                        }
                    case 1:
                        {
                            AppData.MainFrame.Navigate(new Pages.SellerPagesMenu());
                            break;
                        }
                    case 2:
                        {
                            AppData.MainFrame.Navigate(new Pages.ManagerPagesMenu());
                            break;
                        }
                    default:
                        break;
                }
            }
            ImgCapcha.Source = Drawing(Convert.ToInt32(ImgCapcha.Width), Convert.ToInt32(ImgCapcha.Height));
        }

        private void ImgCapcha_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ImgCapcha.Source = Drawing(Convert.ToInt32(ImgCapcha.Width), Convert.ToInt32(ImgCapcha.Height));
        }
    }
}
