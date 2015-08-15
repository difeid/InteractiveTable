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
    /// Логика взаимодействия для Pedigree.xaml
    /// </summary>
    public partial class Pedigree : Page
    {
        public Pedigree()
        {
            InitializeComponent();

            VisualBrush maglifier_brush = (VisualBrush)magnifierEllipse.Fill;
            maglifier_brush.Visual = mainUI;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Read_More_Button_Click(object sender, RoutedEventArgs e)
        {
            
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
    }
}
