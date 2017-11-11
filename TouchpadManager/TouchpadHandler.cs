using System;
using System.Collections.Generic;
using System.Management;
using log4net;

namespace TouchpadManager
{
    /// <summary>
    /// Auto enable-disable the touchpad.
    /// </summary>
    public class TouchpadHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TouchpadHandler));
        private static readonly Guid MouseClassGuid = new Guid("{4d36e96f-e325-11ce-bfc1-08002be10318}");
        private readonly HashSet<string> _touchpadDeviceNames = new HashSet<string> { "Asus Support Device" };
        private ManagementEventWatcher _watcher;
        private string _touchpadId;

        /// <summary>
        /// Starts handling the touchpad.
        /// </summary>
        public void Start()
        {
            Synchronize();
        }

        /// <summary>
        /// Ceases handling the touchpad.
        /// </summary>
        public void Stop()
        {
            CleanUp();
            if (_touchpadId != null)
            {
                Log.Info("Re-enabling touchpad before leaving.");
                DeviceHelper.SetDeviceEnabled(MouseClassGuid, _touchpadId, true);
            }
        }

        // Check if touchpad should be disable or not, and synchronize its state.
        private void Synchronize()
        {
            CleanUp();

            var otherPointingDeviceAvailable = false;
            // WMI does seems to allow querying disable devices as pointing device.
            using (var searcher = new ManagementObjectSearcher(@"select * from Win32_PointingDevice"))
            using (var pointingDevices = searcher.Get())
            {
                foreach (var device in pointingDevices)
                {
                    // https://msdn.microsoft.com/en-us/library/aa394356(VS.85).aspx
                    switch (device.Properties["PointingType"].Value as int?)
                    {
                        // Other
                        case 1:
                        // Unknwonw
                        case 2:
                        case null:
                            if (_touchpadDeviceNames.Contains(device.Properties["Name"].Value as string))
                            {
                                SetTouchpad(device);
                            }
                            else
                            {
                                otherPointingDeviceAvailable = true;
                            }
                            break;

                        // Touch Pad
                        case 7:
                            SetTouchpad(device);
                            break;
                        default:
                            otherPointingDeviceAvailable = true;
                            break;
                    }
                    device.Dispose();
                }
            }

            if (_touchpadId == null)
            {
                SearchDisabledTouchpad();
                if (_touchpadId == null)
                {
                    Log.Warn("No touchpad found, doing nothing.");
                    return;
                }
            }

            Log.InfoFormat("Found touchpad {0}; other pointing device: {1}.", _touchpadId, otherPointingDeviceAvailable);
            DeviceHelper.SetDeviceEnabled(MouseClassGuid, _touchpadId, !otherPointingDeviceAvailable);

            _watcher = new ManagementEventWatcher("");
        }

        private void SetTouchpad(ManagementBaseObject device)
        {
            var touchpadId = GetDeviceId(device);
            if (_touchpadId == null)
                _touchpadId = touchpadId;
            else if (_touchpadId != touchpadId)
                Log.WarnFormat("Found another touchpad: {0}. It will be ignored.", touchpadId);
        }

        private string GetDeviceId(ManagementBaseObject device)
        {
            return device.Properties["DeviceID"].Value as string;
        }

        private void SearchDisabledTouchpad()
        {
            
        }

        private void CleanUp()
        {
            _watcher?.Stop();
            _watcher?.Dispose();
            _watcher = null;
        }
    }
}
