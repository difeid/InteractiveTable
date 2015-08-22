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

        private void OpenPopup(int number)
        {
            if (!App.PopupOpen)
            {
                new PopupWindow("Family", 4, number).Show();
            }
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Father_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenPopup(0);
        }

        private void Mather_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenPopup(1);
        }

        private void Aunti_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenPopup(2);
        }

        private void Brother_Sister_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/BrothersSister.xaml", UriKind.Relative));
        }

        private void Wife_Children_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenPopup(3);
        }

    }
}
