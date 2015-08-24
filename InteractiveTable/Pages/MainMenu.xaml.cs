using System;
using System.Windows;
using System.Windows.Controls;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Memory_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/Memory.xaml", UriKind.Relative));
        }

        private void Student_Task_Button_Click(object sender, RoutedEventArgs e)
        {
            Test t = new Test(10);
            this.NavigationService.Navigate(t);
        }

        private void Motiv_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/Legacy.xaml", UriKind.Relative));
        }

        private void Biography_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/SubMenu.xaml", UriKind.Relative));
        }
    }
}
