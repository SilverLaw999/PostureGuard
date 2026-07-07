using System;

namespace PostureGuard.Systems
{
    public class AdjustmentSystem
    {
        private double _lastY = -1;
        private const double JumpThreshold = 60; // Pixels

        public bool IsAdjusting(double currentY)
        {
            if (_lastY == -1) { _lastY = currentY; return false; }

            double delta = Math.Abs(currentY - _lastY);
            _lastY = currentY;

            return delta > JumpThreshold;
        }
    }
}
