
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management; // need to add System.Management to your project references.
using System.Reflection.Metadata;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Devices.Display;
using Windows.Devices.Input;
using Windows.Devices.Usb;
using Windows.Devices.Portable;
using Windows.Devices;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Documents;
using Windows.Devices.Display.Core;
using Windows.Devices.Custom;
using Windows.Storage.Streams;
using Microsoft.VisualBasic;
using System.Net;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.Devices.Bluetooth.Advertisement;
using System.Threading;

namespace WidgetAdvSampleCS
{
    internal class DeviceFinder
    {
        public void DeviceAdded(DeviceWatcher e1, DeviceInformation e2)
        {
            var benicle = e1;
            var phyrus = e2;
            output_thing = e2;
            //beginReadingBattery(phyrus);
        }
        public void DeviceUpdated(DeviceWatcher e1, DeviceInformationUpdate e2)
        {
            var benicle = e1;
            var phyrus = e2;
        }
        public void DeviceRemoved(DeviceWatcher e1, DeviceInformationUpdate e2)
        {
            var benicle = e1;
            var phyrus = e2;
        }
        public void watcher_enum_complete(DeviceWatcher e1, object e2)
        {
            var benicle = e1;
            var phyrus = e2;
        }
        public void watcher_stopped(DeviceWatcher e1, object e2)
        {
            var benicle = e1;
            var phyrus = e2;
        }
        DeviceInformation output_thing = null;
        public DeviceInformation testDevices()
        {
            string s = BluetoothLEDevice.GetDeviceSelector();

            string[] requested_properties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };



            DeviceWatcher deviceWatcher = DeviceInformation.CreateWatcher(
                BluetoothLEDevice.GetDeviceSelectorFromPairingState(false),
                requested_properties,
                DeviceInformationKind.AssociationEndpoint);

            deviceWatcher.Added += DeviceAdded;
            deviceWatcher.Updated += DeviceUpdated;
            deviceWatcher.Removed += DeviceRemoved;

            deviceWatcher.EnumerationCompleted += watcher_enum_complete;
            deviceWatcher.Stopped += watcher_stopped;
            deviceWatcher.Start();

            while (output_thing == null)
            {
                Thread.Sleep(200);
            }

            return output_thing;
            //beginReadingBattery();
            string str = "";

            string controller_selector = BluetoothLEDevice.GetDeviceSelector();
            string bluetooth_selector = BluetoothDevice.GetDeviceSelector();
            string monitor_selector = DisplayMonitor.GetDeviceSelector();
            //string mouse_selector = CustomDevice.GetDeviceSelector(new Guid("378DE44C-56EF-11D1-BC8C-00A0C91405DD"));
            //string keyboard_selector = CustomDevice.GetDeviceSelector(new Guid("884b96c3-56ef-11d1-bc8c-00a0c91405dd"));
            string usb_selector = CustomDevice.GetDeviceSelector(new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"));

            DeviceInformationCollection deviceTest;
            string output = "Devices\n\n";

            //output += device_info_printed(await DeviceInformation.FindAllAsync(mouse_selector)); // mouse 

            //output += device_info_printed(deviceTest = await DeviceInformation.FindAllAsync(monitor_selector)); // monitors

            //output += device_info_printed(deviceTest = await DeviceInformation.FindAllAsync(keyboard_selector)); // keyboard

            //output += device_info_printed(deviceTest = await DeviceInformation.FindAllAsync(usb_selector)); // usb


            //output += device_info_printed(deviceTest = await DeviceInformation.FindAllAsync(controller_selector)); // controller

            //output += device_info_printed(deviceTest = await DeviceInformation.FindAllAsync(bluetooth_selector)); // bluetooth



            output += "END;";
        }

        public string device_info_printed(DeviceInformationCollection devices)
        {
            string output = "";
            foreach (var device in devices)
            {
                //var imgtest = await device.GetGlyphThumbnailAsync();
                output += device.Name + ": [";
                output += device.Id + "]\n{";
                output += "\n   " + device.Kind.ToString();
                // pairing stats yeild information useless
                //output += "\n   Can pair: " + device.Pairing.CanPair;
                //output += "\n   Is paired: " + device.Pairing.IsPaired;
                //output += "\n   Protection level: " + device.Pairing.ProtectionLevel;
                //output += "\n   Custom pairing: " + device.Pairing.Custom;
                if (device.EnclosureLocation != null)
                {
                    output += "\n      In dock: " + device.EnclosureLocation.InDock;
                    output += "\n      In lid: " + device.EnclosureLocation.InLid;
                    output += "\n      Panel: " + device.EnclosureLocation.Panel;
                    output += "\n      Rotation: " + device.EnclosureLocation.RotationAngleInDegreesClockwise;
                }

                foreach (var v in device.Properties)
                {
                    output += "\n    . " + v.Key + " : " + v.Value;
                }
                output += "\n}\n\n";
            }
            return output;
        }



        private static string Battery_Service_UUID = "0000180F-0000-1000-8000-00805f9b34fb";
        private static string Battery_Level_UUID = "00002a19-0000-1000-8000-00805f9b34fb";

        public async void beginReadingBattery(DeviceInformation found_device)
        {
            //string controller_selector = BluetoothLEDevice.GetDeviceSelector();
            //DeviceInformationCollection deviceTest = await DeviceInformation.FindAllAsync(controller_selector);
            //if (deviceTest.Count == 0) return;
            //await deviceTest[0].Pairing.UnpairAsync();
            //connect to BluetoothDevice
            //var device = await BluetoothLEDevice.FromBluetoothAddressAsync(0);
            var device1 = await BluetoothLEDevice.FromIdAsync(found_device.Id);
            //get UUID of Services
            var services = await device1.GetGattServicesAsync();
            if (services != null)
            {
                foreach (var servicesID in services.Services)
                {
                    //if there is a service thats same like the Battery Service
                    if (servicesID.Uuid.ToString() == Battery_Service_UUID)
                    {

                        var characteristics = await servicesID.GetCharacteristicsAsync();
                        foreach (var character in characteristics.Characteristics)
                        {
                            if (Battery_Level_UUID == character.Uuid.ToString())
                            {
                                GattReadResult result = await character.ReadValueAsync();
                                if (result.Status == GattCommunicationStatus.Success)
                                {
                                    var reader = DataReader.FromBuffer(result.Value);
                                    byte[] input = new byte[reader.UnconsumedBufferLength];
                                    reader.ReadBytes(input);
                                    System.Diagnostics.Debug.WriteLine(BitConverter.ToString(input));

                                }

                            }
                        }

                    }
                }
            }
        }


        
    }
}

