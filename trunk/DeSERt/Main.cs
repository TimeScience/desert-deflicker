using System;
using Gtk;

namespace DeSERt
{
    internal class MainClass
    {
        public static void Main(string[] args)
        {
            try
            {
                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

                Application.Init();
                DeSERtMain win = new DeSERtMain();
                win.Show();
                Application.Run();
            }
            catch (Exception ex) { MessageBox.Show("Fatal Error!" + Environment.NewLine + ex.Message, MessageType.Error, ButtonsType.Ok); }
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
            MessageBox.Show("Unhandled Error!" + Environment.NewLine + ex.Message, MessageType.Error, ButtonsType.Ok);
        }

    }
}
