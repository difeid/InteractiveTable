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
using WpfPageTransitions;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для PhotoAlbum.xaml
    /// </summary>
    public partial class PhotoAlbum : Page
    {
        private Stack<UserControl> pageStack = new Stack<UserControl>();
        private Photo currentPage;
        private Photo nextPage;
        System.Windows.Threading.DispatcherTimer timer;
        private int count = 0;
        private int maxCountPage = 3; //ВНИМАНИЕ! Следить за наличием картинок. Нет проверки.

        public PhotoAlbum()
        {
            InitializeComponent();
            // Таймер
            timer = new System.Windows.Threading.DispatcherTimer(); 
            timer.Tick += new EventHandler(timer_Stop);
            timer.Interval = TimeSpan.FromSeconds(1);
            back_foto_button.Visibility = Visibility.Hidden;
            currentPage = new Photo(count);
            photoPage.ShowPage(currentPage);
            timer_Start();
            pageStack.Push(currentPage);
            nextPage = new Photo(count + 1);
        }

        private void timer_Start()
        {
            next_foto_button.IsEnabled = false;
            back_foto_button.IsEnabled = false;
            timer.Start();
        }

        private void timer_Stop(object sender, EventArgs e)
        {
            next_foto_button.IsEnabled = true;
            back_foto_button.IsEnabled = true;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Next_Foto_Button_Click(object sender, RoutedEventArgs e)
        {
            if (count < maxCountPage - 1)
            {
                count++;
                currentPage = nextPage;
                photoPage.TransitionType = PageTransitionType.Slide;
                photoPage.ShowPage(currentPage);
                timer_Start();
                pageStack.Push(currentPage);
                if (count == maxCountPage - 1)
                {
                    next_foto_button.Visibility = Visibility.Hidden;
                }
            }
            if (count > 0)
            {
                back_foto_button.Visibility = Visibility.Visible;
            }
            if (count < maxCountPage - 1)
            { 
                nextPage = new Photo(count + 1); 
            }
        }

        private void Back_Foto_Button_Click(object sender, RoutedEventArgs e)
        {
            if (count > 0)
            {
                count--;
                nextPage = (Photo)pageStack.Pop();
                currentPage = (Photo)pageStack.Peek();
                photoPage.TransitionType = PageTransitionType.SlideBack;
                photoPage.ShowPage(currentPage);
                timer_Start();
                if (count == 0)
                {
                    back_foto_button.Visibility = Visibility.Hidden;
                }
            }
            if (count < maxCountPage)
            {
                next_foto_button.Visibility = Visibility.Visible;
            }
        }
    }
}
