using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WidgetSampleCS
{
    public sealed partial class dividerControl : UserControl
    {
        public dividerControl(string icon_path)
        {
            this.InitializeComponent();

            this.category_icon.Source = new BitmapImage(new Uri(icon_path));

        }

        bool IsHidden = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsHidden = !IsHidden;
            if (IsHidden)
            {
                devices_panel.Visibility = Visibility.Collapsed;
                visibility_button.Content = "v";
            }
            else
            {
                devices_panel.Visibility = Visibility.Visible;
                visibility_button.Content = "^";
            }
        }
        public void add_child(UIElement child)
        {
            devices_panel.Children.Add(child);
        }
    }
}
