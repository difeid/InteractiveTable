using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для MemoryViewer.xaml
    /// </summary>
    public partial class MemoryViewer : Page
    {
        private string folder;
        private int number;
        private int maxNumber;
        private string culture;

        protected Point TouchStart;
        private bool AlreadySwiped;

        /// <summary>
        /// Фотоальбом с описанием
        /// </summary>
        /// <param name="folder">Папка с содержимым</param>
        /// <param name="count">Количество фото</param>
        public MemoryViewer(string folder)
        {
            InitializeComponent();
            Init(1);
            maxNumber = 1;
            WritePage(this.folder = folder, this.number = 0, this.culture);
        }

        /// <summary>
        /// Фотоальбом с описанием
        /// </summary>
        /// <param name="folder">Папка с содержимым</param>
        /// <param name="count">Количество фото</param>
        public MemoryViewer(string folder, int count)
        {
            InitializeComponent();
            Init(count);

            maxNumber = count;
            WritePage(this.folder = folder, this.number = 0, this.culture);
        }

        /// <summary>
        /// Фотоальбом с описанием
        /// </summary>
        /// <param name="folder">Папка с содержимым</param>
        /// <param name="count">Количество фото</param>
        /// <param name="number">С какого начинать</param>
        public MemoryViewer(string folder, int count, int number)
        {
            InitializeComponent();
            Init(count);

            maxNumber = count;
            this.number = number;
            WritePage(this.folder = folder, this.number, this.culture);
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Next_View_Button_Click(object sender, RoutedEventArgs e)
        {
            if (++number >= maxNumber)
            {
                number = 0;
            }
            WritePage(folder, number, culture);
        }

        private void Back_View_Button_Click(object sender, RoutedEventArgs e)
        {
            if (--number < 0)
            {
                number = maxNumber-1;
            }
            WritePage(folder, number, culture);
        }

        private void Memory_ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void Init(int count)
        {
            culture = App.Language.Name;

            if (count > 1)
            {
                next_view_button.Visibility = Visibility.Visible;
                back_view_button.Visibility = Visibility.Visible;
            }
            else
            {
                next_view_button.Visibility = Visibility.Hidden;
                back_view_button.Visibility = Visibility.Hidden;
            }
        }

        public void WritePage(string folder, int number, string culture)
        {
            scrollContent.ScrollToVerticalOffset(0);

            OpenImage(folder, number);

            FlowDocument disc = OpenDisc(folder, number, culture);
            if (disc == null)
            {
                if (culture != "ru-RU")
                {
                    disc = OpenDisc(folder, number, "ru-RU");
                }
            }
            if (disc != null)
            {
                documentDiscription.Document = disc;
            }
            else
            {
                documentDiscription.Document.Blocks.Clear();
            }
        }

        private void OpenImage(string folder, int number)
        {
            Uri pathMain = new Uri(String.Format("pack://siteoforigin:,,,/Contents/Photo/{0}/{1}/main.jpg", folder, number), UriKind.Absolute);
            Uri pathSub = new Uri(String.Format("pack://siteoforigin:,,,/Contents/Photo/{0}/{1}/sub.jpg", folder, number), UriKind.Absolute);
            try
            {
                mainPhoto.Source = new BitmapImage(pathMain);
                subPhoto.Source = new BitmapImage(pathSub);
            }
            catch
            {
                mainPhoto.Source = null;
                subPhoto.Source = null;
            }
        }

        private FlowDocument OpenDisc(string folder, int number, string culture)
        {
            string pathDisc = String.Format("Contents/Photo/{0}/{1}/disc.{2}.xaml", folder, number, culture);

            FlowDocument content = null;

            if (File.Exists(pathDisc))
            {
                try
                {
                    using (FileStream fs = File.Open(pathDisc, FileMode.Open))
                    {
                        content = XamlReader.Load(fs) as FlowDocument;
                    }
                }
                catch {}
            }
            return content;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TouchStart = e.GetPosition(this);
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AlreadySwiped = false;
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!AlreadySwiped)
                {
                    var Touch = e.GetPosition(this);

                    //right now a swipe is 200 pixels 

                    //Swipe Left

                    if (Touch.X < (TouchStart.X - 200))
                    {
                        //RunMyCustomCode();
                        if (++number >= maxNumber)
                        {
                            number = 0;
                        }
                        WritePage(folder, number, culture);

                        AlreadySwiped = true;

                    }

                    //Swipe Right
                    if (Touch.X > (TouchStart.X + 200))
                    {
                        //RunMyCustomCodeSwipeRight();
                        if (--number < 0)
                        {
                            number = maxNumber - 1;
                        }
                        WritePage(folder, number, culture);

                        AlreadySwiped = true;
                    }
                }
            }
            e.Handled = true;
        }
    }
}
