using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PostureGuard.Systems
{
    public class UISystem : Form
    {
        private int _currentScore = 0;
        private Label _scoreLabel;

        public UISystem()
        {
            // Form Configuration
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.Size = new Size(180, 60);
            this.BackColor = Color.Black;
            this.StartPosition = FormStartPosition.Manual;
            this.Icon = new Icon("icon.ico");

            // Position: Bottom Right
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(workingArea.Right - this.Width - 20, workingArea.Bottom - this.Height - 20);

            // Score Label
            _scoreLabel = new Label
            {
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 12f),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Posture: 0%"
            };

            this.Controls.Add(_scoreLabel);
        }

        public void UpdateScore(int score)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateScore(score)));
                return;
            }

            _currentScore = score;
            _scoreLabel.Text = $"Posture: {_currentScore}%";

            // Visual feedback: Text turns gray as score increases (fade effect)
            _scoreLabel.ForeColor = score > 50 ? Color.FromArgb(255, 200, 200) : Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Professional Modern Style: Rounded border and subtle gradient
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 15;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
                path.CloseAllFigures();

                this.Region = new Region(path);

                using (Pen pen = new Pen(Color.FromArgb(60, 60, 60), 2))
                {
                    g.DrawPath(pen, path);
                }
            }
        }
    }
}