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

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для BookViewer.xaml
    /// </summary>
    public partial class BookViewer : Page
    {

        public BookViewer(string bookName)
        {
            InitializeComponent();

            string culture = App.Language.Name;
            string pathBook = String.Format("Book/book.{0}.{1}.rtf", bookName, culture);

            TextRange tr = new TextRange(bookReader.Document.ContentStart, bookReader.Document.ContentEnd);

            using (FileStream fs = File.Open(pathBook, FileMode.Open))
            {
                tr.Load(fs, DataFormats.Rtf);
            }
            bookReader.Document.ColumnWidth = 900;
            bookReader.Document.PagePadding = new Thickness(50);
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
