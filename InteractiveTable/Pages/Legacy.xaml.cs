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
            //new BookViewer(bookNumber.ToString(),).Show();
        }

        private void Rules_Of_Life_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Trilogy_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Diary_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Memories_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Morning_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Cossacks_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void War_And_Peace_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void After_Ball_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Yule_Night_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Oasis_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
