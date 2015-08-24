using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для TestResult.xaml
    /// </summary>
    public partial class TestResult : Page
    {
        public TestResult(int result)
        {
            InitializeComponent();

            resultText.Text = result.ToString();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/MainMenu.xaml", UriKind.Relative));
        }

        private void Repeat_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pages/Test.xaml", UriKind.Relative));
        }
    }
}
