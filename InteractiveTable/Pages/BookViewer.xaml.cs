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

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для BookViewer.xaml
    /// </summary>
    public partial class BookViewer : Page
    {
        private string pathBook;
        protected Point TouchStart;
        private bool AlreadySwiped;

        public BookViewer(string bookName)
        {
            InitializeComponent();

            string culture = App.Language.Name;
            pathBook = String.Format("Book/book.{0}.{1}.rtf", bookName, culture);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            timer.Tick += (s, ar) =>
            {
                timer.Stop();

                if (File.Exists(pathBook))
                {
                    FlowDocument book = new FlowDocument();
                    TextRange tr = new TextRange(book.ContentStart, book.ContentEnd);

                    using (FileStream fs = File.Open(pathBook, FileMode.Open))
                    {
                        tr.Load(fs, DataFormats.Rtf);
                    }
                    book.ColumnWidth = 900;
                    book.PagePadding = new Thickness(50);

                    bookReader.Document = book;
                }
                pleaseWaitPopup.IsOpen = false;
                backButton.IsEnabled = true;
                contentButton.IsEnabled = true;
            };
            timer.Start();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Content_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationCommands.NextPage.Execute(null, bookReader);
        }

        void Page_MouseDown(object sender, MouseEventArgs e)
        {
            TouchStart = e.GetPosition(this);
        }

        void Page_MouseUp(object sender, MouseEventArgs e)
        {
            AlreadySwiped = false;
        }

        void Page_MouseMove(object sender, MouseEventArgs e)
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
    }
}
