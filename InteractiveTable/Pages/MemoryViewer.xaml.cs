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
using System.IO;
using System.Windows.Markup;
using System.Globalization;

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
        /// <summary>
        /// Фотоальбом с описанием
        /// </summary>
        /// <param name="folder">Папка с содержимым</param>
        /// <param name="Count">Количество фото</param>
        public MemoryViewer(string folder, int Count)
        {
            InitializeComponent();
            maxNumber = Count;
            WritePage(this.folder = folder, this.number = 0);
        }

        /// <summary>
        /// Фотоальбом с описанием
        /// </summary>
        /// <param name="folder">Папка с содержимым</param>
        /// <param name="Count">Количество фото</param>
        /// <param name="number">С какого начинать</param>
        public MemoryViewer(string folder, int Count, int number)
        {
            InitializeComponent();
            maxNumber = Count;

            if (number < maxNumber)
                this.number = number;
            else
                this.number = 0;

            WritePage(this.folder = folder, this.number);
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
            WritePage(folder, number);
        }

        private void Back_View_Button_Click(object sender, RoutedEventArgs e)
        {
            if (--number < 0)
            {
                number = maxNumber-1;
            }
            WritePage(folder, number);
        }

        private void WritePage(string folder, int number)
        {
            Uri pathMain = new Uri(String.Format("pack://application:,,,/MemoryViewer/{0}/main.{1}.jpg", folder, number), UriKind.Absolute);
            Uri pathSub = new Uri(String.Format("pack://application:,,,/MemoryViewer/{0}/sub.{1}.jpg", folder, number), UriKind.Absolute);
            try
            {
                mainPhoto.Source = new BitmapImage(pathMain);
                subPhoto.Fill = new ImageBrush(new BitmapImage(pathSub));
            }
            catch (IOException)
            {
                mainPhoto.Source = null;
                subPhoto.Fill = null;
            }

            //Опредяляем текущий язык
            string culture = App.Language.Name;

            Uri pathDisc = new Uri(String.Format("/MemoryViewer/{0}/disc.{1}.{2}.xaml", folder, number, culture), UriKind.Relative);
            try
            {
                FlowDocument doc = Application.LoadComponent(pathDisc) as FlowDocument;
                documentDiscription.Document = doc;
            }
            catch (IOException)
            {
                documentDiscription.Document = null;
            }
        }
    }
}
