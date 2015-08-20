using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для Pedigree.xaml
    /// </summary>
    public partial class Pedigree : Page
    {
        private string culture;
        private int personNumber = -1;

        public Pedigree()
        {
            InitializeComponent();
            culture = App.Language.Name;

            VisualBrush maglifier_brush = (VisualBrush)magnifierEllipse.Fill;
            maglifier_brush.Visual = mainUI;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            readMorePopup.IsOpen = false;
            this.NavigationService.GoBack();
        }

        private void PopupShow_Button_Enter(object sender, MouseEventArgs e)
        {
            int n = Convert.ToInt32((sender as Button).Name.Substring(1, 2));
            if (personNumber != n || !readMorePopup.IsOpen)
            {
                personNumber = n;
                ShowPopup(personNumber, culture);
            }
            e.Handled = true;
        }

        private void mainUI_MouseDown(object sender, MouseButtonEventArgs e)
        {
            readMorePopup.IsOpen = false;
            e.Handled = true;
        }

        private void Read_More_Button_Click(object sender, RoutedEventArgs e)
        {
            new PopupWindow("Pedigree", 1, personNumber).Show();
            readMorePopup.IsOpen = false;
        }

        private void OnMoveOverMainUI(object sender, MouseEventArgs e)
        {
            VisualBrush magnlifier_brush = (VisualBrush)magnifierEllipse.Fill;
            Point position = e.MouseDevice.GetPosition(mainUI);
            Rect viewBox = magnlifier_brush.Viewbox;
            double xoffset = viewBox.Width / 2.0;
            double yoffset = viewBox.Height / 2.0;
            viewBox.X = position.X - xoffset;
            viewBox.Y = position.Y - yoffset;
            magnlifier_brush.Viewbox = viewBox;
            Canvas.SetLeft(magnifierCanvas, position.X - magnifierEllipse.Width / 2);
            Canvas.SetTop(magnifierCanvas, position.Y - magnifierEllipse.Height / 2);
        }

        public void ShowPopup(int number, string culture)
        {
            readMorePopup.IsOpen = false;
            Uri pathImage = new Uri(String.Format("pack://application:,,,/Pedigree/img.{0}.png", number), UriKind.Absolute);
            try
            {
                photoImage.Source = new BitmapImage(pathImage);
            }
            catch (IOException)
            {
                photoImage.Source = null;
            }

            Uri pathPlate = new Uri(String.Format("/Pedigree/plate.{0}.{1}.xaml", number, culture), UriKind.Relative);
            try
            {
                FlowDocument doc = Application.LoadComponent(pathPlate) as FlowDocument;
                plateDocument.Document = doc;
            }
            catch (IOException)
            {
                plateDocument.Document = null;
            }
            readMorePopup.IsOpen = true;
        }
    }
}
