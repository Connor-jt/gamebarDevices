using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
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
    public sealed partial class deviceControl : UserControl
    {
        public deviceControl(string name, string id, DeviceThumbnail glyph, DeviceThumbnail thumb)
        {
            this.InitializeComponent();

            this.name_text.Text = name;
            //this.name_text.text = id;

            var regular = new BitmapImage();
            regular.SetSource(glyph);
            this.glyphIMG.Source = regular;

            var regular1 = new BitmapImage();
            regular1.SetSource(thumb);
            this.typeIMG.Source = regular1;

            //setup_the_images(glyph, thumb);
        }
        private async void setup_the_images(DeviceThumbnail glyph, DeviceThumbnail thumb)
        {

            //this.glyphIMG.Source = glyph;
            //this.typeIMG.Source = thumb;

        }

        
    }
}
