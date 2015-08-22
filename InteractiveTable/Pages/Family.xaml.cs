using System;
using System.Windows;
using System.Windows.Controls;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для Family.xaml
    /// </summary>
    public partial class Family : Page
    {
        public Family()
        {
            InitializeComponent();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Father_Button_Click(object sender, RoutedEventArgs e)
        {
            new PopupWindow("Family", 1, 0).Show();
        }

        private void Mather_Button_Click(object sender, RoutedEventArgs e)
        {
            new PopupWindow("Family", 1, 1).Show();
        }

        private void Aunti_Button_Click(object sender, RoutedEventArgs e)
        {
            new PopupWindow("Family", 1, 2).Show();
        }

        private void Brother_Sister_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/BrothersSister.xaml", UriKind.Relative));
        }

        private void Wife_Children_Button_Click(object sender, RoutedEventArgs e)
        {
            new PopupWindow("Family", 1, 3).Show();
        }

    }
}
