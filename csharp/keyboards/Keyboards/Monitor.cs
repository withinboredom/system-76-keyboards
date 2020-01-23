using System;
using System.Collections.Generic;
using System.Linq;
using keyboards.Monitors;
using keyboards.Sides;
using Microsoft.Extensions.Configuration;

namespace keyboards.Keyboards
{
    public class Monitor : Keyboard
    {
        private readonly MonitorConfiguration _monitorConfiguration;

        public Monitor(IControlContainer container, IConfiguration configuration) : base(container)
        {
            _monitorConfiguration = configuration == null
                ? new MonitorConfiguration()
                : configuration.GetSection("Monitor").Get<MonitorConfiguration>();
            Init(container);
        }

        private static IMonitor MonitorFromBreakpoint(IControlContainer container, Breakpoints p)
        {
            return p.Name switch
            {
                Name.Cpu => Cpu.Instance(container),
                Name.Memory => Memory.Instance(container),
                Name.Disk => Disk.Instance(container),
                _ => throw new Exception($"Unable to location monitor for {p.Name}")
            };
        }

        private Side SideFromBreakpoint(IControlContainer container, Breakpoints p)
        {
            return new MonitorSide(MonitorFromBreakpoint(container, p), p.Red, p.Yellow, p.Green);
        }

        private void Init(IControlContainer container)
        {
            var breakpoints = new List<Breakpoints>
                {_monitorConfiguration.Cpu, _monitorConfiguration.Hdd, _monitorConfiguration.Memory};

            var left = breakpoints.FirstOrDefault(x => x.Position == Position.LeftOrSingle);
            var center = breakpoints.FirstOrDefault(x => x.Position == Position.Center);
            var right = breakpoints.FirstOrDefault(x => x.Position == Position.Right);

            if (left == default(Breakpoints) || center == default(Breakpoints) || right == default(Breakpoints))
                throw new Exception("All sides must be specified even if they aren't used");

            Sides = new[]
            {
                SideFromBreakpoint(container, left),
                SideFromBreakpoint(container, center),
                SideFromBreakpoint(container, right)
            };
        }

        private class MonitorConfiguration
        {
            public Breakpoints Cpu { get; } = new Breakpoints
            {
                Red = 95,
                Yellow = 75,
                Green = 50,
                Position = Position.LeftOrSingle,
                Name = Name.Cpu
            };

            public Breakpoints Memory { get; } = new Breakpoints
            {
                Red = 85,
                Yellow = 45,
                Green = 15,
                Position = Position.Center,
                Name = Name.Memory
            };

            public Breakpoints Hdd { get; } = new Breakpoints
            {
                Red = 90,
                Yellow = 45,
                Green = 30,
                Position = Position.Right,
                Name = Name.Disk
            };
        }

        private class Breakpoints
        {
            public double Red { get; set; }
            public double Yellow { get; set; }
            public double Green { get; set; }
            public Position Position { get; set; }

            public Name Name { get; set; }
        }

        private enum Position
        {
            LeftOrSingle,
            Center,
            Right
        }

        private enum Name
        {
            Cpu,
            Memory,
            Disk
        }
    }
}