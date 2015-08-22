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
    /// Логика взаимодействия для ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : Window
    {
        private string folder;
        private int folderNumber;
        private int imageNumber;
        private int maxImageNumber;
        private string culture;

        private DateTime downTime;
        private object downSender;

        public ImageViewer(string folder, int folderNumber)
        {
            InitializeComponent();
            this.imageNumber = 0;
            this.maxImageNumber = 1;
            culture = App.Language.Name;

            ImageViewerShow(this.folder = folder, this.folderNumber = folderNumber, this.imageNumber, culture);
        }

        public ImageViewer(string folder, int folderNumber, int imageNumber)
        {
            InitializeComponent();
            this.maxImageNumber = 1;
            culture = App.Language.Name;

            ImageViewerShow(this.folder = folder, this.folderNumber = folderNumber, this.imageNumber = imageNumber, culture);
        }

        public ImageViewer(string folder, int folderNumber, int imageNumber, int maxImageNumber)
        {
            InitializeComponent();
            this.maxImageNumber = maxImageNumber;
            culture = App.Language.Name;

            ImageViewerShow(this.folder = folder, this.folderNumber = folderNumber, this.imageNumber = imageNumber, culture);
            
        }

        private void ChangeText(string folder, int folderNumber, int imageNumber, string culture)
        {
            FlowDocument signature = OpenSignature(folder, folderNumber, imageNumber,  culture);
            if (signature == null)
            {
                if (culture != "ru-RU")
                {
                    signature = OpenSignature(folder, folderNumber, imageNumber, "ru-RU");
                }
            }
            zoomImageText.Document = signature;
        }

        private bool ChangeSource(string folder, int folderNumber, int imageNumber)
        {
            Uri pathBigImage = new Uri(String.Format("pack://siteoforigin:,,,/Contents/Article/{0}/{1}/big.{2}.jpg", folder, folderNumber, imageNumber), UriKind.Absolute);
            try
            {
                BitmapImage bi = new BitmapImage(pathBigImage);
                if (zoomImage.Source != bi)
                {
                    zoomImage.Source = bi;
                    if (bi.Width > zoomImage.MaxWidth || bi.Height > zoomImage.MaxHeight)
                    {
                        zoomImage.Stretch = Stretch.Uniform;
                    }
                    else
                    {
                        zoomImage.Stretch = Stretch.None;
                    }
                }
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        private FlowDocument OpenSignature(string folder, int folderNumber, int imageNumber, string culture)
        {
            string path = String.Format("Contents/Article/{0}/{1}/signature.{2}.{3}.xaml", folder, folderNumber, imageNumber,  culture);

            FlowDocument content = null;

            if (File.Exists(path))
            {
                try
                {
                    using (FileStream fs = File.Open(path, FileMode.Open))
                    {
                        content = XamlReader.Load(fs) as FlowDocument;
                    }
                }
                catch (IOException) { }
            }
            return content;
        }

        public void ImageViewerShow(string folder, int folderNumber, int imageNumber, string culture)
        {
            if (!ChangeSource(folder, folderNumber, imageNumber))
            {
                this.Close();
            }
            ChangeText(folder, folderNumber, imageNumber, culture);
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Viewer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is Window)
            {
                this.Close();
            }
            e.Handled = true;
        }

        private void Popup_Down(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.downSender = sender;
                this.downTime = DateTime.Now;
            }
            e.Handled = true;
        }

        private void Popup_Up(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released &&
                sender == this.downSender)
            {
                TimeSpan timeSinceDown = DateTime.Now - this.downTime;
                if (timeSinceDown.TotalMilliseconds < 500)
                {
                    if (++imageNumber < maxImageNumber)
                    {
                        ImageViewerShow(folder, folderNumber, imageNumber, culture);
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            e.Handled = true;
        }
    }
}
