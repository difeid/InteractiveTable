using System.Windows;
using System.Windows.Controls;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для Memory.xaml
    /// </summary>
    public partial class Memory : Page
    {
        public Memory()
        {
            InitializeComponent();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Monument_Button_Click(object sender, RoutedEventArgs e)
        {
            MemoryViewer mv = new MemoryViewer("Monuments", 5);
            this.NavigationService.Navigate(mv);
        }

        private void Musuem_Button_Click(object sender, RoutedEventArgs e)
        {
            MemoryViewer mv = new MemoryViewer("Museum", 5);
            this.NavigationService.Navigate(mv);
        }

        private void Theatre_Button_Click(object sender, RoutedEventArgs e)
        {
            MemoryViewer mv = new MemoryViewer("Theatre", 5);
            this.NavigationService.Navigate(mv);
        }

        private void TolstoyHistory_Button_Click(object sender, RoutedEventArgs e)
        {
            MemoryViewer mv = new MemoryViewer("Tolstoy", 5);
            this.NavigationService.Navigate(mv);
        }
    }
}
