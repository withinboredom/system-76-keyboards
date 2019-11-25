using System;
using System.Collections.Generic;

namespace keyboards
{
    public class LedProvider
    {
        public enum Location
        {
            Left,
            Center,
            Right,
            Single,
        }

        public static IFile GetLedFor(IControlContainer container, Location led)
        {
            switch (led)
            {
                case Location.Left:
                    return container.File("/sys/class/leds/system76::kbd_backlight/color_left");
                case Location.Center:
                    return container.File("/sys/class/leds/system76::kbd_backlight/color_center");
                case Location.Right:
                    return container.File("/sys/class/leds/system76::kbd_backlight/color_right");
                case Location.Single:
                    return container.File("/sys/class/leds/system76_acpi::kbd_backlight/color");
                default:
                    throw new ArgumentOutOfRangeException(nameof(led), led, null);
            }
        }

        public static IEnumerable<IFile> SupportedConfiguration(IControlContainer container)
        {
            var next = GetLedFor(container, Location.Left);
            if (next.Exists && next.HasPermission) yield return next;
            next = GetLedFor(container, Location.Center);
            if (next.Exists && next.HasPermission) yield return next;
            next = GetLedFor(container, Location.Right);
            if (next.Exists && next.HasPermission) yield return next;
            next = GetLedFor(container, Location.Single);
            if (next.Exists && next.HasPermission) yield return next;
        }
    }
}