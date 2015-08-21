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
using System.Windows.Threading;

namespace InteractiveTable
{
    /// <summary>
    /// Логика взаимодействия для BookViewer.xaml
    /// </summary>
    public partial class BookViewer : Window
    {
        private string bookName;
        protected Point TouchStart;
        private bool AlreadySwiped;
        private string culture;

        public BookViewer(string bookName)
        {
            InitializeComponent();

            culture = App.Language.Name;
            this.bookName = bookName;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            timer.Tick += (s, ar) =>
            {
                timer.Stop();

                FlowDocument book = OpenBook(bookName, culture);
                if (book == null)
                {
                    if (culture != "ru-RU")
                    {
                        book = OpenBook(bookName, "ru-RU");
                    }
                }
                bookReader.Document = book;
                pleaseWaitPopup.IsOpen = false;
            };
            timer.Start();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Content_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationCommands.NextPage.Execute(null, bookReader);
        }

        private void Page_MouseDown(object sender, MouseEventArgs e)
        {
            TouchStart = e.GetPosition(this);
        }

        private void Page_MouseUp(object sender, MouseEventArgs e)
        {
            AlreadySwiped = false;
        }

        private void Page_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!AlreadySwiped)
                {
                    var Touch = e.GetPosition(this);

                    //right now a swipe is 200 pixels 

                    //Swipe Left

                    if (Touch.X < (TouchStart.X - 200) && TouchStart.X >1000)
                    {
                        //RunMyCustomCode();
                        if (bookReader.CanGoToNextPage)
                        {
                            NavigationCommands.NextPage.Execute(null, bookReader);
                        }
                        AlreadySwiped = true;

                    }

                    //Swipe Right
                    if (Touch.X > (TouchStart.X + 200) && TouchStart.X < 900)
                    {
                        //RunMyCustomCodeSwipeRight();
                        if (bookReader.CanGoToPreviousPage)
                        {
                            NavigationCommands.PreviousPage.Execute(null, bookReader);
                        }
                        AlreadySwiped = true;
                    }
                }
            }
            e.Handled = true;
        }

        public FlowDocument OpenBook(string bookName, string culture)
        {
            string path = String.Format("Contents/Book/{0}/book.{1}.rtf", bookName, culture);

            FlowDocument book = null;

            if (File.Exists(path))
            {
                book = new FlowDocument();
                TextRange tr = new TextRange(book.ContentStart, book.ContentEnd);
                try
                {
                    using (FileStream fs = File.Open(path, FileMode.Open))
                    {
                        tr.Load(fs, DataFormats.Rtf);
                    }
                }
                catch (IOException) { }

                book.ColumnWidth = 800;
                book.PagePadding = new Thickness(70);
            }
            return book;
        }
    }
}
