using System;
using PostureGuard.Libraries;

namespace PostureGuard.Systems
{
    public class PostureConfig
    {
        public double BaselineY { get; set; } = -1;
        public double BaselineHeight { get; set; } = -1;
        public double Sensitivity { get; set; } = 0.25;
    }

    public class CalibrationSystem
    {
        public PostureConfig Config { get; private set; } = new PostureConfig();
        public bool IsCalibrated => Config.BaselineY != -1;

        public void Init()
        {
            // Load existing data from AppData
            var saved = DataLibrary.Load<PostureConfig>();
            if (saved != null)
            {
                Config = saved;
                Console.WriteLine("[CalibrationSystem]: Loaded saved baseline.");
            }
        }

        public void SetBaseline(double y, double height)
        {
            Config.BaselineY = y;
            Config.BaselineHeight = height;
            DataLibrary.Save(Config); // Persist immediately
        }
    }
}