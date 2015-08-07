﻿using System;
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
        private int count = 0;
        private int maxCountPage = 5;

        public PhotoAlbum()
        {
            InitializeComponent();
            count = 0;
            back_foto_button.Visibility = Visibility.Hidden;
            Photo newPage = new Photo(count);
            pageStack.Push(newPage);
            photoPage.ShowPage(newPage);
            //photoPage.TransitionType = (PageTransitionType)Enum.Parse(typeof(PageTransitionType), "Slide", true);
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Next_Foto_Button_Click(object sender, RoutedEventArgs e)
        {
            if (count < maxCountPage)
            {
                count++;
                Photo newPage = new Photo(count);
                pageStack.Push(newPage);
                photoPage.ShowPage(newPage);
                if (count == maxCountPage)
                {
                    next_foto_button.Visibility = Visibility.Hidden;
                }
            }
            if (count == 1)
            {
                back_foto_button.Visibility = Visibility.Visible;
            }
        }

        private void Back_Foto_Button_Click(object sender, RoutedEventArgs e)
        {
            if (count > 0)
            {
                count--;
                photoPage.ShowPage(pageStack.Pop());
                if (count == 0)
                {
                    back_foto_button.Visibility = Visibility.Hidden;
                }
            }
            if (count == maxCountPage-2)
            {
                next_foto_button.Visibility = Visibility.Visible;
            }
        }
    }
}
