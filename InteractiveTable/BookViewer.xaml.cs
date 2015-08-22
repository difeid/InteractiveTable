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
using System.Windows.Media.Animation;

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

        private bool IsToggle;
        DoubleAnimation da = new DoubleAnimation();

        public BookViewer(string bookName)
        {
            InitializeComponent();

            slidePanel.Width = 0;
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
                OpenContents(bookName, culture);
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
            if (!IsToggle)
            {
                da.To = 500;
                da.Duration = TimeSpan.FromMilliseconds(300);
                slidePanel.BeginAnimation(Grid.WidthProperty, da);
                IsToggle = true;
            }
        }

        private void Page_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsToggle)
            {
                da.To = 0;
                da.Duration = TimeSpan.FromMilliseconds(300);
                slidePanel.BeginAnimation(Grid.WidthProperty, da);
                IsToggle = false;
                AlreadySwiped = true;
            }
            else
            {
                TouchStart = e.GetPosition(this);
            }
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

        private void Memory_ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
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
                book.PagePadding = new Thickness(80,170,80,170);
            }
            return book;
        }

        public void OpenContents(string bookName, string culture)
        {
            string path = String.Format("Contents/Book/{0}/contents.{1}.txt", bookName, culture);

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                int count = lines.Length;

                ResourceDictionary dict = new ResourceDictionary();
                dict.Source = new Uri("/InteractiveTable;component/Resource/ResourceDictionaryTemplate.xaml",UriKind.RelativeOrAbsolute);

                //<Button Width="360" Height="50" Margin="5" Template="{StaticResource HiddenButtonTemplate}" Tag="Лев Николаевич Толстой" FontSize="16"/>
                Button b = new Button();
                b.Width = 360;
                b.Height = 50;
                b.Margin = new Thickness(5);
                b.Template = dict["HiddenButtonTemplate"] as ControlTemplate;
                b.Tag = lines[0];
                b.FontSize = 16;
                stack.Children.Add(b);

                //<Line Stroke="Gray" StrokeThickness="0.5" Width="360" X1="0" Y1="0" X2="360" Y2="0"/>
                Line l = new Line();
                l.X1 = 0;
                l.X2 = 360;
                l.Y1 = 0;
                l.Y2 = 0;
                l.Width = 360;
                l.StrokeThickness = 0.5;
                l.Stroke = Brushes.Gray;
                stack.Children.Add(l);
            }
        }
    }
}
