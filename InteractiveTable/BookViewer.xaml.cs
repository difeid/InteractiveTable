using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace InteractiveTable
{
    /// <summary>
    /// Логика взаимодействия для BookViewer.xaml
    /// </summary>
    public partial class BookViewer : Window
    {
        private string bookName;
        private int partCount;
        private int currentPart = 0;
        private int part;

        protected Point TouchStart;
        private bool AlreadySwiped;
        private string culture;

        private bool IsToggle;
        private DoubleAnimation da = new DoubleAnimation();
        private bool IsLocked;

        private Button[] but;
        private Line[] line;
        private int countLines = 0;

        private DispatcherTimer timer = new DispatcherTimer();

        public BookViewer(string bookName, int partCount)
        {
            InitializeComponent();

            App.BookOpen = true;
            slidePanel.Width = 0;

            culture = App.Language.Name;
            this.bookName = bookName;
            this.partCount = partCount;

            but = new Button[partCount];
            line = new Line[partCount];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!OpenContents(bookName, culture))
            {
                if (culture != "ru-RU")
                {
                    OpenContents(bookName, culture);
                }
            }
            OpenBook(currentPart);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.BookOpen = false;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsLocked)
            {
                pleaseWaitPopup.IsOpen = false;
                this.Close();
            }
            e.Handled = true;
        }

        private void Content_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsLocked)
            {
                AnimationPanel(true);
            }
            e.Handled = true;
        }

        private void Point_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsLocked)
            {
                int part = Convert.ToInt32((sender as Button).Name.Substring(3));
                if (part != currentPart)
                {
                    OpenBook(part);
                }
            }
            e.Handled = true;
        }

        private void OpenBook(int part)
        {
            if (part >= 0 && part < partCount)
            {
                IsLocked = true;
                HighlightingButton(part, currentPart);
                pleaseWaitPopup.IsOpen = true;
                bookReader.Document.Blocks.Clear();
                this.part = part;

                timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
                timer.Tick += Timer_Tick1;
                timer.Start();
            }
        }

        private void Timer_Tick1(object sender, EventArgs e)
        {
            timer.Stop();

            FlowDocument book = OpenPartBook(bookName, part, culture);
            if (book == null)
            {
                if (culture != "ru-RU")
                {
                    book = OpenPartBook(bookName, part, "ru-RU");
                }
            }
            if (book != null)
            {
                bookReader.Document = book;
            }
            else
            {
                bookReader.Document.Blocks.Clear();
            }
            currentPart = part;
            AnimationPanel(false);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick -= Timer_Tick1;
            timer.Tick += Timer_Tick2;
            timer.Start();
        }

        private void Timer_Tick2(object sender, EventArgs e)
        {
            timer.Stop();
            pleaseWaitPopup.IsOpen = false;
            IsLocked = false;
            timer.Tick -= Timer_Tick2;
        }

        private void HighlightingButton( int selectPart, int canselSelectPart)
        {
            if (selectPart < countLines && canselSelectPart < countLines && but[canselSelectPart] != null && but[selectPart] != null)
            {
                but[canselSelectPart].FontWeight = FontWeights.Normal;
                but[canselSelectPart].IsEnabled = true;
                but[selectPart].FontWeight = FontWeights.Bold;
                but[selectPart].IsEnabled = false;
            }
        }

        public FlowDocument OpenPartBook(string bookName, int part, string culture)
        {
            string path = String.Format("Contents/Book/{0}/book.{1}.{2}.rtf", bookName, part, culture);

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
                catch { }

                book.ColumnWidth = 800;
                book.PagePadding = new Thickness(60,70,60,100);
            }
            return book;
        }

        private bool OpenContents(string bookName, string culture)
        {
            string path = String.Format("Contents/Book/{0}/contents.{1}.txt", bookName, culture);

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                countLines = lines.Length;
                int num = 0;

                ResourceDictionary dict = new ResourceDictionary();
                dict.Source = new Uri("/InteractiveTable;component/Resource/ResourceDictionaryTemplate.xaml", UriKind.RelativeOrAbsolute);

                //<Line Stroke="Gray" StrokeThickness="0.5" Width="360" X1="0" Y1="0" X2="360" Y2="0"/>
                Line l = new Line();
                l.X1 = 0;
                l.X2 = 360;
                l.Y1 = 0;
                l.Y2 = 0;
                l.Width = 360;
                l.StrokeThickness = 1;
                l.Stroke = Brushes.Gray;
                stack.Children.Add(l);

                while (num < countLines)
                {
                    //<Button Width="360" Height="50" Margin="5" Template="{StaticResource HiddenButtonTemplate}" Tag="Лев Николаевич Толстой" FontSize="16"/>
                    but[num] = new Button();
                    but[num].Width = 360;
                    but[num].Height = 50;
                    but[num].Margin = new Thickness(5);
                    but[num].Template = dict["HiddenButtonTemplate"] as ControlTemplate;
                    but[num].Tag = lines[num];
                    but[num].FontSize = 16;
                    but[num].FocusVisualStyle = null;
                    but[num].Name = String.Concat("but", num.ToString());
                    but[num].Click += Point_Button_Click;
                    stack.Children.Add(but[num]);

                    line[num] = new Line();
                    line[num].X1 = 0; line[num].X2 = 360;
                    line[num].Y1 = 0; line[num].Y2 = 0;
                    line[num].Width = 360;
                    line[num].StrokeThickness = 0.5;
                    line[num].Stroke = Brushes.Gray;
                    stack.Children.Add(line[num]);

                    num++;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Page_MouseDown(object sender, MouseEventArgs e)
        {
            if (!IsLocked)
            {
                AnimationPanel(false);
                TouchStart = e.GetPosition(this);
            }
        }

        private void Page_MouseUp(object sender, MouseEventArgs e)
        {
            AlreadySwiped = false;
        }

        private void Page_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !IsLocked)
            {
                if (!AlreadySwiped)
                {
                    var Touch = e.GetPosition(this);

                    //right now a swipe is 200 pixels 

                    //Swipe Left

                    if (Touch.X < (TouchStart.X - 200) && TouchStart.X > 1000)
                    {
                        //RunMyCustomCode();
                        if (bookReader.CanGoToNextPage)
                        {
                            NavigationCommands.NextPage.Execute(null, bookReader);
                        }
                        else
                        {
                            OpenBook(currentPart + 1);
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
                        else
                        {
                            OpenBook(currentPart - 1);
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

        public void AnimationPanel(bool enabled)
        {
            if (!IsToggle && enabled)
            {
                da.To = 500;
                da.Duration = TimeSpan.FromMilliseconds(300);
                slidePanel.BeginAnimation(Grid.WidthProperty, da);
                IsToggle = true;
            }
            else if (IsToggle && !enabled)
            {
                da.To = 0;
                da.Duration = TimeSpan.FromMilliseconds(300);
                slidePanel.BeginAnimation(Grid.WidthProperty, da);
                IsToggle = false;
            }
        }
    }
}
