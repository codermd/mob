using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.UITest;
using Xamarin.UITest.Configuration;

namespace Mxp.UITests.CrossPlatform.Helpers
{
    public static class iOSSimulatorsHelpers
    {
        public static iOSAppConfigurator SetDeviceByName(this iOSAppConfigurator configurator, string simulatorName)
        {
            var deviceId = GetDeviceID(simulatorName);
            return configurator.DeviceIdentifier(deviceId);
        }

        static string GetDeviceID(string simulatorName)
        {
            if (!TestEnvironment.Platform.Equals(TestPlatform.Local))
            {
                return string.Empty;
            }

            // See below for the InstrumentsRunner class.
            IEnumerable<iOSSimulator> simulators = new InstrumentsRunner().GetListOfSimulators();

            var simulator = (from sim in simulators
                             where sim.Name.Equals(simulatorName)
                             select sim).FirstOrDefault();

            if (simulator == null)
            {
                throw new ArgumentException("Could not find a device identifier for '" + simulatorName + "'.", "simulatorName");
            }
            else
            {
                return simulator.GUID;
            }
        }

        class InstrumentsRunner
        {
            static string[] GetInstrumentsOutput()
            {
                const string cmd = "/usr/bin/xcrun";

                var startInfo = new ProcessStartInfo
                {
                    FileName = cmd,
                    Arguments = "instruments -s devices",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    StandardOutputEncoding = Encoding.UTF8
                };

                var proc = new Process
                {
                    StartInfo = startInfo
                };
                proc.Start();
                var result = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();

                var lines = result.Split('\n');
                return lines;
            }

            public iOSSimulator[] GetListOfSimulators()
            {
                var simulators = new List<iOSSimulator>();
                var lines = GetInstrumentsOutput();

                foreach (var line in lines)
                {
                    var sim = new iOSSimulator(line);
                    if (sim.IsValid())
                    {
                        simulators.Add(sim);
                    }
                }

                return simulators.ToArray();
            }
        }
    }


}
