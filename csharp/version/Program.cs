using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace version
{
    class Program
    {
        static void Main(string[] args)
        {
            var version = File.ReadAllText("VERSION");
            
            // update versions in keyboards
            var keyboards = File.ReadLines("csharp/keyboards/keyboards.csproj");
            var newLines = keyboards.Select(line => line.Replace("_version_", version)).ToList();

            File.WriteAllLines("csharp/keyboards/keyboards.csproj", newLines);
            
            // update deb version
            var deb = File.ReadAllText("deb/DEBIAN/control");
            File.WriteAllText("deb/DEBIAN/control", deb.Replace("_version_", version));
        }
    }
}