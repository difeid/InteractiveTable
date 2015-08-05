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
    /// Логика взаимодействия для SubMenu.xaml
    /// </summary>
    public partial class SubMenu : Page
    {
        public SubMenu()
        {
            InitializeComponent();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Milestones_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Pedigree_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Family_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
