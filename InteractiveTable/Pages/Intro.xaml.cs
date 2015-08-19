using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для Intro.xaml
    /// </summary>
    public partial class Intro : Page
    {
        public Intro()
        {
            InitializeComponent();

            App.LanguageChanged += LanguageChanged;
        }

        private void LanguageChanged(Object sender, EventArgs e)
        {
            // Смена языка приложения
            CultureInfo currLang = App.Language;
        }

        private void Rus_Button_Click(object sender, RoutedEventArgs e)
        {
            App.Language = CultureInfo.GetCultureInfo("ru-RU");
            this.NavigationService.Navigate(new Uri("Pages/MainMenu.xaml", UriKind.Relative));
        }

        private void Tat_Button_Click(object sender, RoutedEventArgs e)
        {
            App.Language = CultureInfo.GetCultureInfo("tt-RU");
            this.NavigationService.Navigate(new Uri("Pages/MainMenu.xaml", UriKind.Relative));
        }

        private void Eng_Button_Click(object sender, RoutedEventArgs e)
        {
            App.Language = CultureInfo.GetCultureInfo("en-US");
            this.NavigationService.Navigate(new Uri("Pages/MainMenu.xaml", UriKind.Relative));
        }
    }
}
