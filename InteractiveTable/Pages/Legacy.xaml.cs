using System;
using System.Windows;
using System.Windows.Controls;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для Legacy.xaml
    /// </summary>
    public partial class Legacy : Page
    {
        public Legacy()
        {
            InitializeComponent();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Book_Button_Click(object sender, RoutedEventArgs e)
        {
            int bookNumber = Convert.ToInt32((sender as Button).Name.Substring(1, 1));
            new BookViewer(bookNumber.ToString()).Show();
        }
    }
}
