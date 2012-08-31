using GLib;

namespace DeSERt
{
    public class MyTimer
    {
        public uint Time { get; set; }
        public bool Enabled { get { return working; } }

        private TimeoutHandler handler;
        private bool working = false;

        public MyTimer(TimeoutHandler Tick_Handler)
        {
            handler = Tick_Handler;
            Time = 1000;
        }

        public void Start()
        {
            if (!working)
            {
                Timeout.Add(Time, handler);
                working = true;
            }
        }

        public void Stop()
        {
            working = false;
        }
    }
}
