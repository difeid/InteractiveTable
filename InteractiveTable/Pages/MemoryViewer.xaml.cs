using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Markup;

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

            if (number < maxNumber)
                this.number = number;
            else
                this.number = 0;

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
            WriteImage(folder, number);

            if (!WriteDisc(folder, number, culture))
            {
                if (culture != "ru-RU")
                {
                    WriteDisc(folder, number, "ru-RU");
                }
            }
        }

        private void WriteImage(string folder, int number)
        {
            Uri pathMain = new Uri(String.Format("pack://siteoforigin:,,,/Contents/MemoryViewer/{0}/main.{1}.jpg", folder, number), UriKind.Absolute);
            Uri pathSub = new Uri(String.Format("pack://siteoforigin:,,,/Contents/MemoryViewer/{0}/sub.{1}.jpg", folder, number), UriKind.Absolute);
            try
            {
                mainPhoto.Source = new BitmapImage(pathMain);
                subPhoto.Source = new BitmapImage(pathSub);
            }
            catch (IOException)
            {
                mainPhoto.Source = null;
                subPhoto.Source = null;
            }
        }

        private bool WriteDisc(string folder, int number, string culture)
        {
            string pathDisc = String.Format("Contents/MemoryViewer/{0}/disc.{1}.{2}.xaml", folder, number, culture);

            if (File.Exists(pathDisc))
            {
                try
                {
                    using (FileStream fs = File.Open(pathDisc, FileMode.Open))
                    {
                        FlowDocument content = XamlReader.Load(fs) as FlowDocument;

                        documentDiscription.Document = content; 
                    }
                }
                catch (IOException) {}
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
