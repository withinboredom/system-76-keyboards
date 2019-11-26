using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace keyboards.Monitors
{
    public class Display : Monitor
    {
        [DllImport("libX11")]
        private static extern IntPtr XOpenDisplay(string displayName);

        [DllImport("libXext.so.6")]
        private static extern bool DPMSQueryExtension(IntPtr display, out string dummy1, out string dummy2);

        [DllImport("libXext.so.6")]
        private static extern bool DPMSCapable(IntPtr display);

        [DllImport("libXext.so.6")]
        private static extern void DPMSInfo(IntPtr display, out State state, out bool onOff);

        enum State
        {
            Fail = -1,
            On = 0,
            Standby = 1,
            Suspend = 2,
            Off = 3,
            Disabled = 4
        }

        private Display(IControlContainer container) : base(container)
        {
            UpdateMode = Mode.PercentageSmooth;
            Container.RegisterActiveMonitor(this);
        }

        private static IMonitor _instance;
        public static IMonitor Instance(IControlContainer container) => _instance ??= new Display(container);

        protected override Task<double> GetReading()
        {
            var display = XOpenDisplay(":0");
            if (display == IntPtr.Zero) return Task.FromResult(100D);
            
            var ext = DPMSQueryExtension(display, out var dummy1, out var dummy2);
                
            if (!ext) return Task.FromResult(100D);
                
            var hasDpms = DPMSCapable(display);
                
            if (!hasDpms) return Task.FromResult(100D);
                    
            DPMSInfo(display, out var state, out var onOff);
            return onOff ? Task.FromResult(state == State.On ? 100D : 0D) : Task.FromResult(100D);
        }
    }
}