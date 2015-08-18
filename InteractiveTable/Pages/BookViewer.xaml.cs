﻿using System;
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
        string pathBook;

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

                    pleaseWaitPopup.IsOpen = false;
                    backButton.IsEnabled = true;
                    contentButton.IsEnabled = true;
                }
            };
            timer.Start();
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
