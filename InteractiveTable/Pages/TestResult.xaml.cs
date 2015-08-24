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

            ResourceDictionary dict = (from d in Application.Current.Resources.MergedDictionaries
                                       where d.Source != null && d.Source.OriginalString.StartsWith("LanguageResources/lang.")
                                       select d).First();

            if (result == 0)
            {
                 text.Text = dict["t_Answer2"] as String;
            }
            else if (result == 1)
            {
                text.Text = dict["t_Answer0"] as String;
            }
            else if (result <= 4)
            {
                text.Text = dict["t_Answer1"] as String;
            }
            else
            {
                text.Text = dict["t_Answer2"] as String;
            }
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
