using System;
using PostureGuard.Libraries;

namespace PostureGuard.Systems
{
    public class MonitoringSystem
    {
        private VisionLibrary? _vision;
        private CalibrationSystem? _calibration;
        private UISystem? _ui;

        // The "Bucket" (0-100)
        private double _slouchScore = 0;

        // Settings (Tuned for 10s Testing)
        private const double SlouchIncrement = 10.0; // Fills in 10s
        private const double RecoveryDecrement = 12.0;

        // DETECTION CONSTANTS
        // We focus on Height (Depth) because tilting the screen doesn't change how close you are.
        private const double DepthSensitivity = 1.08; // 8% increase in head size triggers slouch
        private const double VerticalLeeway = 0.08;   // 8% drop in Y (very lenient to allow for screen tilt)

        public void Init(VisionLibrary vision, CalibrationSystem calibration, UISystem ui)
        {
            _vision = vision;
            _calibration = calibration;
            _ui = ui;
        }

        public void Update()
        {
            if (_calibration == null || !_calibration.IsCalibrated || _vision == null) return;

            FaceData current = _vision.GetFaceData();
            var config = _calibration.Config;

            if (!current.IsValid) return;

            // 1. Z-Axis Check (Depth): Did your head get bigger? 
            // This is the primary check for laptop users who tilt their screens.
            double hLimit = config.BaselineHeight * DepthSensitivity;
            bool isLeaningIn = current.Height > hLimit;

            // 2. Y-Axis Check (Vertical): Did your head drop relative to your own size?
            // We use BaselineHeight as a unit of measure to make it proportional.
            double yLimit = config.BaselineY + (config.BaselineHeight * VerticalLeeway);
            bool isDropping = current.Y > yLimit;

            // Logic: If you are leaning in OR dropping, the bucket fills.
            // This catches the slouch even if you've tilted the screen to "center" yourself.
            bool isSlouching = isLeaningIn || isDropping;

            if (isSlouching)
            {
                _slouchScore = Math.Min(100, _slouchScore + SlouchIncrement);
            }
            else
            {
                // Janitor: Slowly clear the score if posture is corrected
                _slouchScore = Math.Max(0, _slouchScore - RecoveryDecrement);
            }

            _ui?.UpdateScore((int)_slouchScore);

            //// debugging: showing you the Height (H) vs the Limit
            //int progress = (int)_slouchScore;
            //string status = isLeaningIn ? "LEANING" : (isDropping ? "DROPPING" : "GOOD");

            //Console.WriteLine($"[DEBUG] H: {Math.Round(current.Height)}/{Math.Round(hLimit)} | Y: {Math.Round(current.Y)}/{Math.Round(yLimit)} | State: {status} | Score: {progress}%");

            if (_slouchScore >= 100)
            {
                NotificationLibrary.Send();
                _slouchScore = 0;
            }
        }
    }
}