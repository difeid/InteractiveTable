using System;
using System.Windows;
using System.Windows.Controls;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для SubMenu.xaml
    /// </summary>
    public partial class SubMenu : Page
    {
        public SubMenu()
        {
            InitializeComponent();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Milestones_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/Timeline.xaml", UriKind.Relative));
        }

        private void Pedigree_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/Pedigree.xaml", UriKind.Relative));
        }

        private void Family_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/Family.xaml", UriKind.Relative));
        }

        
    }
}
