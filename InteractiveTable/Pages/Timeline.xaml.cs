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
using System.Windows.Controls.Primitives;

namespace InteractiveTable.Pages
{
    /// <summary>
    /// Логика взаимодействия для Timeline.xaml
    /// </summary>
    public partial class Timeline : Page
    {
        public Timeline()
        {
            InitializeComponent();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Year_Button_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Name.Substring(1, 4);
            MessageBox.Show(name);
        }
    }

    /// <summary>
    /// An extended scrollbar that can be bound to an external scrollviewer.
    /// </summary>
    public class BindableScrollBar : ScrollBar
    {
        public ScrollViewer BoundScrollViewer
        {
            get { return (ScrollViewer)GetValue(BoundScrollViewerProperty); }
            set { SetValue(BoundScrollViewerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BoundScrollViewer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoundScrollViewerProperty =
            DependencyProperty.Register("BoundScrollViewer", typeof(ScrollViewer), typeof(BindableScrollBar), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnBoundScrollViewerPropertyChanged)));

        private static void OnBoundScrollViewerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BindableScrollBar sender = d as BindableScrollBar;
            if (sender != null && e.NewValue != null)
            {
                sender.UpdateBindings();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableScrollBar"/> class.
        /// </summary>
        /// <param name="scrollViewer">The scroll viewer.</param>
        /// <param name="o">The o.</param>
        public BindableScrollBar(ScrollViewer scrollViewer, Orientation o)
            : base()
        {
            this.Orientation = o;
            BoundScrollViewer = scrollViewer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableScrollBar"/> class.
        /// </summary>
        public BindableScrollBar() : base() { }

        private void UpdateBindings()
        {
            AddHandler(ScrollBar.ScrollEvent, new ScrollEventHandler(OnScroll));
            BoundScrollViewer.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(BoundScrollChanged));
            Minimum = 0;
            if (Orientation == Orientation.Horizontal)
            {
                SetBinding(ScrollBar.MaximumProperty, (new Binding("ScrollableWidth") { Source = BoundScrollViewer, Mode = BindingMode.OneWay }));
                SetBinding(ScrollBar.ViewportSizeProperty, (new Binding("ViewportWidth") { Source = BoundScrollViewer, Mode = BindingMode.OneWay }));
            }
            else
            {
                this.SetBinding(ScrollBar.MaximumProperty, (new Binding("ScrollableHeight") { Source = BoundScrollViewer, Mode = BindingMode.OneWay }));
                this.SetBinding(ScrollBar.ViewportSizeProperty, (new Binding("ViewportHeight") { Source = BoundScrollViewer, Mode = BindingMode.OneWay }));
            }
            LargeChange = 0.1 * Maximum;
            SmallChange = 0.02 * Maximum;
        }

        private void BoundScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            switch (this.Orientation)
            {
                case Orientation.Horizontal:
                    this.Value = e.HorizontalOffset;
                    break;
                case Orientation.Vertical:
                    this.Value = e.VerticalOffset;
                    break;
                default:
                    break;
            }
        }

        private void OnScroll(object sender, ScrollEventArgs e)
        {
            switch (this.Orientation)
            {
                case Orientation.Horizontal:
                    this.BoundScrollViewer.ScrollToHorizontalOffset(e.NewValue);
                    break;
                case Orientation.Vertical:
                    this.BoundScrollViewer.ScrollToVerticalOffset(e.NewValue);
                    break;
                default:
                    break;
            }
        }
    }
}
