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
using System.Windows.Shapes;
using System.IO;
using System.Windows.Markup;

namespace InteractiveTable
{
    /// <summary>
    /// Логика взаимодействия для PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        private string folder;
        private int number;
        private int maxNumber;
        private Image[] arrayImage;
        /// <summary>
        /// Вывод статей
        /// </summary>
        /// <param name="folder">Папка с содержимым</param>
        /// <param name="Count">Количество статей</param>
        public PopupWindow(string folder, int Count)
        {
            InitializeComponent();
            Init();
            maxNumber = Count;
            WritePage(this.folder = folder, this.number = 0);
        }

        /// <summary>
        /// Вывод статей
        /// </summary>
        /// <param name="folder">Папка с содержимым</param>
        /// <param name="Count">Количество статей</param>
        /// <param name="number">С какой по счету начинать, отсчет с нуля</param>
        public PopupWindow(string folder, int Count, int number)
        {
            InitializeComponent();
            Init();
            maxNumber = Count;

            if (number < maxNumber)
                this.number = number;
            else
                this.number = 0;

            WritePage(this.folder = folder, this.number);
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            if (--number < 0)
            {
                number = maxNumber - 1;
            }
            WritePage(folder, number);
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            if (++number >= maxNumber)
            {
                number = 0;
            }
            WritePage(folder, number);
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Zoom_Click(object sender, RoutedEventArgs e)
        {
            int imageNumber = Convert.ToInt32((sender as Button).Name.Substring(4));
            //Zoom(imageNumber);
            MessageBox.Show(imageNumber.ToString());
        }

        private void Init()
        {
            arrayImage = new[] { image0, image1, image2, image3, image4, image5, image6, image7, image8 };
        }

        private void WritePage(string folder, int number)
        {
            //Опредяляем текущий язык
            string culture = App.Language.Name;
            int countImg = 0;
            int maxImg = 0;

            //Печатаем статью
            Uri pathDisc = new Uri(String.Format("/PopupWindow/{0}/article.{1}.{2}.xaml", folder, number, culture), UriKind.Relative);
            try
            {
                FlowDocument doc = Application.LoadComponent(pathDisc) as FlowDocument;
                documentPage.Document = doc;
                maxImg = Convert.ToInt32(doc.Tag);
            }
            catch (IOException)
            {
                documentPage.Document = null;
            }

            //Вставляем изображения
            if (maxImg > 9) maxImg = 0;

            while (countImg < maxImg)
            {
                Uri pathImage = new Uri(String.Format("pack://application:,,,/PopupWindow/{0}/img.{1}.{2}.jpg", folder, number, countImg), UriKind.Absolute);
                try
                {
                    arrayImage[countImg].Source = new BitmapImage(pathImage);
                    arrayImage[countImg].Visibility = Visibility.Visible;
                }
                catch (IOException)
                {
                    arrayImage[countImg].Source = null;
                    arrayImage[countImg].Visibility = Visibility.Hidden;
                    break;
                }
                countImg++;
            }
            while (countImg < 9)
            {
                arrayImage[countImg].Source = null;
                arrayImage[countImg].Visibility = Visibility.Hidden;
                countImg++;
            }
        }
    }
}
