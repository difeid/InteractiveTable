using System.Windows;
using System.Windows.Controls;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для BrothersSister.xaml
    /// </summary>
    public partial class BrothersSister : Page
    {
        public BrothersSister()
        {
            InitializeComponent();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Nikola_Button_Click(object sender, RoutedEventArgs e)
        {
            new PopupWindow("BrothersSister", 4, 0).Show();
        }

        private void Sergey_Button_Click(object sender, RoutedEventArgs e)
        {
            new PopupWindow("BrothersSister", 4, 1).Show();
        }

        private void Dmitry_Button_Click(object sender, RoutedEventArgs e)
        {
            new PopupWindow("BrothersSister", 4, 2).Show();
        }

        private void Mary_Button_Click(object sender, RoutedEventArgs e)
        {
            new PopupWindow("BrothersSister", 4, 3).Show();
        }
    }
}
