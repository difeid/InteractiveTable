using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для Legacy.xaml
    /// </summary>
    public partial class Legacy : Page
    {
        //private Thread thread;

        public Legacy()
        {
            InitializeComponent();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        //void ShowWindow(object bookNumber)
        //{
        //    BookViewer bookWindow = new BookViewer(bookNumber.ToString());
        //    bookWindow.Show();
        //    Dispatcher.Run();
        //}

        private void Book_Button_Click(object sender, RoutedEventArgs e)
        {
            int bookNumber = Convert.ToInt32((sender as Button).Name.Substring(1, 1));
            new BookViewer(bookNumber.ToString()).Show();
            //thread = new Thread(ShowWindow);
            //thread.Name = "BookViewer";
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.IsBackground = false;
            //thread.Start(bookNumber);
        }
    }
}
