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
using System.IO;
using System.Windows.Markup;
using System.Globalization;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для MemoryViewer.xaml
    /// </summary>
    public partial class MemoryViewer : Page
    {
        public MemoryViewer(string item, int number)
        {
            InitializeComponent();

            Uri pathMain = new Uri(String.Format("/MemoryViewer/{0}/main.{1}.jpg", item, number), UriKind.Relative);
            Uri pathSub = new Uri(String.Format("/MemoryViewer/{0}/sub.{1}.jpg", item, number), UriKind.Relative);
            try
            {
                mainPhoto.Source = new BitmapImage(pathMain);
                subPhoto.Fill = new ImageBrush(new BitmapImage(pathSub));
            }
            catch (IOException) { }

            //Опредяляем текущий язык
            string culture = App.Language.Name;

            Uri pathDisc = new Uri(String.Format("/MemoryViewer/{0}/disc.{1}.{3}.xaml", item, number, culture), UriKind.Relative);
            try
            {
                FlowDocument doc = Application.LoadComponent(pathDisc) as FlowDocument;
                documentDiscription.Document = doc;
            }
            catch (IOException) { }
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Next_View_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Back_View_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
