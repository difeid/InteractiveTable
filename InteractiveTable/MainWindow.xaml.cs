using System;
using System.Windows;

namespace InteractiveTable
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            App.IdleTimeOut += App_IdleTimeOut;
        }

        private void App_IdleTimeOut(object sender, System.EventArgs e)
        {
            if (main_frame.Source.ToString() != "Pages/Intro.xaml")
            {
                main_frame.Navigate(new Uri("Pages/Intro.xaml", UriKind.Relative));
            }
        }
    }
}
