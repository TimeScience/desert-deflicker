using GLib;
using Gtk;

namespace DeSERt
{
    public class PulseBar
    {
        public bool Enabled { get { return working; } }
        public double Step { get; set; }

        private TimeoutHandler handler;
        private ProgressBar PrBar;
        private bool working = false;

        public PulseBar(ProgressBar ProgressTarget)
        {
            handler = new TimeoutHandler(Timer_Tick);
            PrBar = ProgressTarget;
            Step = 0.05;
        }

        public void Start()
        {
            if (!working)
            {
                GLib.Timeout.Add(80, handler);
                working = true;
            }
        }

        public void Stop()
        {
            working = false;
        }

        private bool Timer_Tick()
        {
            if (this.Enabled)
            {
                PrBar.PulseStep = Step;
                Application.Invoke(delegate { PrBar.Pulse(); });
            }
            return this.Enabled;
        }

    }
}
