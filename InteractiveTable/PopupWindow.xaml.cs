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
        public PopupWindow(string year, ScrollViewer sv)
        {
            InitializeComponent();

            Uri path = new Uri(String.Format("/Articles/Timeline/{0}.xaml", year), UriKind.Relative);
            try
            {
                FlowDocument doc = Application.LoadComponent(path) as FlowDocument;
                documentPage.Document = doc;
            }
            catch (IOException)
            {
                MessageBox.Show("File not found");
            }
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
