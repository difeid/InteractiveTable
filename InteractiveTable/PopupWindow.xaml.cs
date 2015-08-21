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
using System.IO;
using System.Windows.Markup;
using System.Windows.Threading;

namespace InteractiveTable
{
    /// <summary>
    /// Логика взаимодействия для PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        private string folder;
        private int number;
        private int maxNumber;
        private int numberImage;
        private int maxNumberImage;
        private string culture;

        private DateTime downTime;
        private object downSender;

        /// <summary>
        /// Вывод статей
        /// </summary>
        /// <param name="folder">Папка с содержимым</param>
        public PopupWindow(string folder)
        {
            InitializeComponent();
            Init(1);
            maxNumber = 1;
            maxNumberImage = WritePage(this.folder = folder, this.number = 0, this.culture);
        }
        
        /// <summary>
        /// Вывод статей
        /// </summary>
        /// <param name="folder">Папка с содержимым</param>
        /// <param name="count">Количество статей</param>
        public PopupWindow(string folder, int count)
        {
            InitializeComponent();
            Init(count);
            maxNumber = count;
            maxNumberImage = WritePage(this.folder = folder, this.number = 0, this.culture);
        }

        /// <summary>
        /// Вывод статей
        /// </summary>
        /// <param name="folder">Папка с содержимым</param>
        /// <param name="count">Количество статей</param>
        /// <param name="number">С какой по счету начинать, отсчет с нуля</param>
        public PopupWindow(string folder, int count, int number)
        {
            InitializeComponent();
            Init(count);

            maxNumber = count;
            this.number = number;
            maxNumberImage = WritePage(this.folder = folder, this.number, this.culture);
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            zoomPopup.IsOpen = false;

            if (--number < 0)
            {
                number = maxNumber - 1;
            }
            maxNumberImage = WritePage(folder, number, culture);
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            zoomPopup.IsOpen = false;

            if (++number >= maxNumber)
            {
                number = 0;
            }
            maxNumberImage = WritePage(folder, number, culture);
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            zoomPopup.IsOpen = false;

            this.Close();
        }

        private void Popup_Down(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.downSender = sender;
                this.downTime = DateTime.Now;
            }
        }

        private void Popup_Up(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released &&
                sender == this.downSender)
            {
                TimeSpan timeSinceDown = DateTime.Now - this.downTime;
                if (timeSinceDown.TotalMilliseconds < 500)
                {
                    if (++numberImage < maxNumberImage)
                    {
                        ShowPopup(folder, number, numberImage);
                    }
                    else
                    {
                        numberImage = 0;
                        zoomPopup.IsOpen = false;
                    }
                }
            }
        }

        private void Zoom_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button)
            {
                numberImage = Convert.ToInt32((e.Source as Button).Tag);
                ShowPopup(folder, number, numberImage);
            }
        }

        private void Popup_ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void Init(int count)
        {
            numberImage = 0;
            culture = App.Language.Name;

            if (count > 1)
            {
                next_button.Visibility = Visibility.Visible;
                back_button.Visibility = Visibility.Visible;
            }
            else
            {
                next_button.Visibility = Visibility.Hidden;
                back_button.Visibility = Visibility.Hidden;
            }
        }

        public int WritePage(string folder, int number, string culture)
        {
            int intTag = 0;

            //Печатаем статью
            FlowDocument article = OpenArticle(folder, number, culture);
            if (article == null)
            {
                if (culture != "ru-RU")
                {
                    article = OpenArticle(folder, number, "ru-RU");
                }
            }
            if (article == null)
            {
                intTag = 0;
            }
            else
            {
                intTag = Convert.ToInt32(article.Tag);
            }

            documentPage.Document = article;

            return intTag;
        }

        public void ShowPopup(string folder, int number, int numberBigImage)
        {
            if (zoomPopup.IsOpen)
            {
                zoomPopup.IsOpen = false;
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                timer.Tick += (s, ar) =>
                {
                    timer.Stop();
                    if (ChangeSource(folder, number, numberImage))
                    {
                        zoomPopup.IsOpen = true;
                    }
                };
                timer.Start();
            }
            else
            {
                if (ChangeSource(folder, number, numberImage))
                {
                    zoomPopup.IsOpen = true;
                }
            }
        }

        private bool ChangeSource (string folder, int number, int numberBigImage)
        {
            Uri pathBigImage = new Uri(String.Format("pack://siteoforigin:,,,/Contents/Article/{0}/{1}/big.{2}.jpg", folder, number, numberBigImage), UriKind.Absolute);
            try
            {
                BitmapImage bi = new BitmapImage(pathBigImage);
                if (zoomImage.Source != bi)
                {
                    zoomImage.Source = bi;
                }
                return true;
            }
            catch (IOException) 
            {
                return false;
            }
        }


        private FlowDocument OpenArticle(string folder, int number, string culture)
        {
            string path = String.Format("Contents/Article/{0}/{1}/article.{2}.xaml", folder, number, culture);

            FlowDocument content = null;

            if (File.Exists(path))
            {
                try
                {
                    using (FileStream fs = File.Open(path, FileMode.Open))
                    {
                        content = XamlReader.Load(fs) as FlowDocument;
                    }
                }
                catch (IOException) { }
            }
            return content;
        }
    }
}
