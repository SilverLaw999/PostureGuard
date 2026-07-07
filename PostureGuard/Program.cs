using System;
using System.Threading;
using System.Linq;
using System.Windows.Forms;
using PostureGuard.Systems;
using PostureGuard.Libraries;

namespace PostureGuard
{
    static class Program
    {
        // System/Library Definitions
        private static VisionLibrary _vision = new VisionLibrary();
        private static CalibrationSystem _calibration = new CalibrationSystem();
        private static MonitoringSystem _monitor = new MonitoringSystem();
        private static PersistenceSystem _persistence = new PersistenceSystem();

        [STAThread]
        static void Main(string[] args)
        {
            // Init Phase: Module Loading
            _persistence.Init();
            _vision.Init();
            _calibration.Init();

            UISystem ui = new UISystem();
            _monitor.Init(_vision, _calibration, ui);

            // Command Handling (Calibration Phase)
            if (args.Contains("--calibrate"))
            {
                Thread.Sleep(2000);

                // Updated to use the FaceData class instead of a tuple
                FaceData data = _vision.GetFaceData();

                if (data.IsValid)
                {
                    _calibration.SetBaseline(data.Y, data.Height);
                    MessageBox.Show("Calibration Saved to AppData!", "PostureGuard");
                }
                else
                {
                    MessageBox.Show("Calibration Failed: No face detected.", "PostureGuard Error");
                }

                // Janitor: Cleanup hardware before exit
                _vision.Dispose();
                return;
            }

            // Start Phase: Background Loop
            Thread monitorThread = new Thread(() =>
            {
                while (true)
                {
                    _monitor.Update();
                    Thread.Sleep(1000);
                }
            });
            monitorThread.IsBackground = true;
            monitorThread.Start();

            Application.Run(ui);
        }
    }
}