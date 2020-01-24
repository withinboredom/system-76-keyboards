using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace keyboards.Monitors
{
    public class Display : Monitor
    {
        private static IMonitor? _instance;
        private double _lastReading;

        private DateTime _lastUpdate;

        private Display(IControlContainer container) : base(container)
        {
            UpdateMode = Mode.PercentageSmooth;
            Container.RegisterActiveMonitor(this);
            _lastUpdate = DateTime.Now;
            _lastReading = 100D;
        }

        [DllImport("libX11")]
        private static extern IntPtr XOpenDisplay(string displayName);

        [DllImport("libX11")]
        private static extern void XCloseDisplay(IntPtr display);

        [DllImport("libXext.so.6")]
        private static extern bool DPMSQueryExtension(IntPtr display, out string dummy1, out string dummy2);

        [DllImport("libXext.so.6")]
        private static extern bool DPMSCapable(IntPtr display);

        [DllImport("libXext.so.6")]
        private static extern void DPMSInfo(IntPtr display, out State state, out bool onOff);

        public static IMonitor Instance(IControlContainer container)
        {
            return _instance ??= new Display(container);
        }

        protected override Task<double> GetReading()
        {
            var update = DateTime.Now;
            if (update - _lastUpdate < TimeSpan.FromSeconds(5)) return Task.FromResult(_lastReading);
            _lastUpdate = update;

            var display = XOpenDisplay(Installer.ActiveDisplay);
            if (display == IntPtr.Zero)
            {
                _lastReading = 100D;
                return Task.FromResult(100D);
            }


            if (DPMSQueryExtension(display, out var dummy1, out var dummy2))
                if (DPMSCapable(display))
                {
                    DPMSInfo(display, out var state, out var onOff);

                    if (onOff) _lastReading = state == State.On || state == State.Fail ? 100D : 0D;
                }

            XCloseDisplay(display);
            return Task.FromResult(_lastReading);
        }

        private enum State
        {
            Fail = -1,
            On = 0,
            Standby = 1,
            Suspend = 2,
            Off = 3,
            Disabled = 4
        }
    }
}