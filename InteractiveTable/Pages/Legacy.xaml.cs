using System;
using System.Windows;
using System.Windows.Controls;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для Legacy.xaml
    /// </summary>
    public partial class Legacy : Page
    {
        public Legacy()
        {
            InitializeComponent();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        public void OpenBook(string bookName, int partCount)
        {
            if (!App.BookOpen)
            {
                new BookViewer(bookName, partCount).Show();
            }
        }

        private void Rules_Of_Life_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenBook("RulesOfLife", 4);
        }

        private void Trilogy_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenBook("Trilogy", 4);
        }

        private void Diary_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenBook("Diary", 4);
        }

        private void Memories_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenBook("Memories", 4);
        }

        private void Morning_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenBook("Morning", 4);
        }

        private void Cossacks_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenBook("Cossacks", 4);
        }

        private void War_And_Peace_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenBook("WarAndPeace", 4);
        }

        private void After_Ball_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenBook("AfterBall", 4);
        }

        private void Yule_Night_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenBook("YuleNight", 4);
        }

        private void Oasis_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenBook("Oasis", 4);
        }
    }
}
