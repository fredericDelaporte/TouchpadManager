using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Management.Infrastructure;

namespace TouchpadManager
{
    /// <summary>
    /// Auto enable-disable the touchpad.
    /// </summary>
    public class TouchpadHandler
    {
        private Thread _handlingThread;
        private volatile bool _stopRequested;
        private bool _disabled;
        private bool _otherPointingDeviceAvailable;
        private CimInstance _touchpad;
        private readonly HashSet<string> _touchpadDeviceNames = new HashSet<string>{ "Asus Support Device" };

        /// <summary>
        /// Starts handling the touchpad.
        /// </summary>
        public void Start()
        {
            if (_handlingThread != null)
                throw new InvalidOperationException("Already started.");

            _handlingThread = new Thread(HandleTouchad);
            _handlingThread.Start();
            _stopRequested = false;
        }

        /// <summary>
        /// Ceases handling the touchpad.
        /// </summary>
        public void Stop()
        {
            _stopRequested = true;
            _handlingThread?.Join();
            _handlingThread = null;
        }

        private void HandleTouchad()
        {
            Synchronize();
            while (!_stopRequested)
            {
                Thread.Sleep(2000);
            }
            CleanUp();
        }

        // Check if touchpad should be disable or not, and synchronize its state.
        private void Synchronize()
        {
            using (var session = CimSession.Create(null))
            {
                var result = session.QueryInstances("root/cimv2", "WQL", "select * from Win32_PointingDevice");

                _otherPointingDeviceAvailable = false;
                foreach (var device in result)
                {
                    // https://msdn.microsoft.com/en-us/library/aa394356(VS.85).aspx
                    switch (device.CimInstanceProperties["PointingType"]?.Value as int?)
                    {
                        // Other
                        case 1:
                        // Unknwonw
                        case 2:
                        case null:
                            if (_touchpadDeviceNames.Contains(device.CimInstanceProperties["Name"]?.Value as string))
                            {
                                SetTouchpad(device);
                                continue;
                            }
                            break;

                        // Touch Pad
                        case 7:
                            SetTouchpad(device);
                            continue;
                    }
                    _otherPointingDeviceAvailable = true;
                    device.Dispose();
                }
            }

            if (_touchpad == null || !_otherPointingDeviceAvailable)
                return;

            //_touchpad.CimInstanceProperties
        }

        private void SetTouchpad(CimInstance device)
        {
            if (_touchpad != null)
                throw new InvalidOperationException("A touchpad is already found, cannot handle multiple touchpad.");
            _touchpad = device;
        }

        private void CleanUp()
        {
            _touchpad?.Dispose();
            _touchpad = null;
        }
    }
}
