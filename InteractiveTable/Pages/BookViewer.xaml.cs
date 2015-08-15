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
using MoonPdfLib;
using MoonPdfLib.MuPdf;
using System.IO;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для BookViewer.xaml
    /// </summary>
    public partial class BookViewer : Page
    {
        public BookViewer(int bookNumber)
        {
            InitializeComponent();

            string culture = App.Language.Name;
            string pathBook = String.Format("Book/book.{0}.{1}.pdf", bookNumber, culture);
            moonPdfPanel.OpenFile(pathBook);
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Content_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
