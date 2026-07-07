using System;
using System.Windows.Forms;
using PostureGuard.Libraries;

// Explicitly define which Timer to use to resolve ambiguity
using Timer = System.Windows.Forms.Timer;

namespace PostureGuard.Systems
{
    public class PostureSystem
    {
        private Timer? _timer;
        private const int Interval = 10 * 60 * 1000; // 10 Minutes

        public void Init()
        {
            _timer = new Timer();
            _timer.Interval = Interval;
            _timer.Tick += (s, e) => NotificationLibrary.Send();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void ForceTrigger()
        {
            NotificationLibrary.Send();
        }
    }
}