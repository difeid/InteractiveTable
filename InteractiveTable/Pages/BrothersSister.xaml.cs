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

        private void OpenPopup(int number)
        {
            if (!App.PopupOpen)
            {
                new PopupWindow("BrothersSister", 4, number).Show();
            }
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Nikola_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenPopup(0);
        }

        private void Sergey_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenPopup(1);
        }

        private void Dmitry_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenPopup(2);
        }

        private void Mary_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenPopup(3);
        }
    }
}
