using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
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

        private bool openFlag = false;
        private DispatcherTimer timer;

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
            if (--number < 0)
            {
                number = maxNumber - 1;
            }
            maxNumberImage = WritePage(folder, number, culture);
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            if (++number >= maxNumber)
            {
                number = 0;
            }
            maxNumberImage = WritePage(folder, number, culture);
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close(); 
        }

        private void Zoom_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button)
            {
                string tag = (e.Source as Button).Tag.ToString();
                if (tag != null)
                {
                    if (!tag.Contains(":"))
                    {
                        if (Int32.TryParse(tag, out numberImage))
                        {
                            OpenZoomImage();
                        }
                    }
                    else
                    {
                        OpenBook(tag);
                    }
                }
            }
        }

        private void OpenZoomImage()
        {
            if (!openFlag)
            {
                this.Topmost = false;
                openFlag = true;
                try
                {
                    new ImageViewer(folder, number, numberImage, maxNumberImage).Show();
                }
                catch { }
            }
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            openFlag = false;
        }

        public void OpenBook(string bookName)
        {
            if (!App.BookOpen && bookName != null)
            {
                try
                {
                    new BookViewer(bookName).Show();
                }
                catch { }
            }
        }

        private void Popup_ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void Popup_ScrollViewer_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        private void Popup_ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void Init(int count)
        {
            App.PopupOpen = true;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 3, 0);
            timer.Tick += Timer_Tick;

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

        private void PopupWindow_Closed(object sender, EventArgs e)
        {
            App.PopupOpen = false;
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
            if (article != null)
            {
                Int32.TryParse(article.Tag.ToString(), out intTag);
                documentPage.Document = article; 
            }
            else
            {
                documentPage.Document.Blocks.Clear();
            }
            return intTag;
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
                catch { }
            }
            return content;
        }
    }
}
