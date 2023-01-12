using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth;
using Windows.Devices.Custom;
using Windows.Devices.Display;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WidgetSampleCS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Widget1 : Page
    {
        public Widget1()
        {
            this.InitializeComponent();

            string controller_selector = BluetoothLEDevice.GetDeviceSelector();
            string bluetooth_selector = BluetoothDevice.GetDeviceSelector();
            string monitor_selector = DisplayMonitor.GetDeviceSelector();
            string usb_selector = CustomDevice.GetDeviceSelector(new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"));

            //string super_selector = "System.Devices.DevObjectType:=5 AND System.Devices.Aep.IsPaired:=System.StructuredQueryType.Boolean#True AND (System.Devices.Aep.ProtocolId:=\"{BB7BB05E-5972-42B5-94FC-76EAA7084D49}\" OR System.Devices.Aep.ProtocolId:=\"{E0CBF06C-CD8B-4647-BB8A-263B43F0F974}\") ";

            string currentAssemblyPath = Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            dividerControl parent1 = new dividerControl(currentAssemblyPath + "\\Assets\\custom\\monitor.png");
            device_panel.Children.Add(parent1);
            process_devices(monitor_selector, parent1);

            dividerControl parent2 = new dividerControl(currentAssemblyPath + "\\Assets\\custom\\usb.png");
            device_panel.Children.Add(parent2);
            process_devices(usb_selector, parent2);

            dividerControl parent3 = new dividerControl(currentAssemblyPath + "\\Assets\\custom\\bluetooth.png");
            device_panel.Children.Add(parent3);
            process_devices(controller_selector, parent3);
            process_devices(bluetooth_selector, parent3);
        }

        public async void process_devices(string selector, dividerControl parent)
        {
            DeviceInformationCollection resultingDevices = await DeviceInformation.FindAllAsync(selector);
            foreach (var device in resultingDevices)
            {
                var device_glyph = await device.GetGlyphThumbnailAsync();
                var device_thumb = await device.GetThumbnailAsync();
                string device_name = device.Name;
                string device_id = device.Id;

                deviceControl DC = new deviceControl(device_name, device_id, device_glyph, device_thumb);
                parent.add_child(DC);
            }

        }
         

    }
}
