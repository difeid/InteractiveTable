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
    /// Логика взаимодействия для Photo.xaml
    /// </summary>
    public partial class Photo : UserControl
    {
        //Узнать истинный размер изображения
        //ImageSource imageSource = q.Source;
        //BitmapImage bitmapImage = (BitmapImage)imageSource;
        //int asd = bitmapImage.PixelHeight;

        public Photo(int index)
        {
            InitializeComponent();
            try
            {
                photoImage.Source = new BitmapImage(new Uri(String.Format("/Images/Photos/{0}.jpg", index), UriKind.Relative));
            }
            catch (UriFormatException)
            {
                photoImage.Source = new BitmapImage(new Uri("/Images/Background.png", UriKind.Relative));
            }
            
            //Поиск словаря
            ResourceDictionary dict = (from d in Application.Current.Resources.MergedDictionaries where d.Source != null &&
                                           d.Source.OriginalString.StartsWith("LanguageResources/lang.") select d).First();
            
            String[] title = (String[])dict["p_Title"];
            String[] paragraph = (String[])dict["p_Paragraph"];
            
            try
            {
                photoTextTitle.Text = title[index];
                photoTextParagraph.Text = paragraph[index];
            }
            catch (NullReferenceException)
            {
                photoTextTitle.Text = "?";
                photoTextParagraph.Text = "?";
            }
        }
    }
}
