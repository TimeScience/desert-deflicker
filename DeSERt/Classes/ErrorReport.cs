using System;
using System.Text;
using System.IO;
using Gtk;

namespace DeSERt
{
    public struct ErrorReport
    {
        public static bool ReportError(string name, Exception exception)
        {
            try
            {
                string Text = string.Empty;

                if (File.Exists("ErrorLog.txt")) { Text = File.ReadAllText("ErrorLog.txt") + Environment.NewLine; }

                StreamWriter writer = new StreamWriter("ErrorLog.txt");

                if (System.Text.ASCIIEncoding.Unicode.GetByteCount(Text) > 10000)
                {
                    Text.Substring(Text.IndexOf('#', Text.Length / 2));
                }

                writer.Write(Text);
                writer.WriteLine("#" + DateTime.Now);
                writer.WriteLine("System Information:");
                writer.WriteLine("  " + ProjectInfo.FullName);
                writer.WriteLine("  OS:   " + ProjectInfo.OSversion);
                writer.WriteLine("  Bit:  " + ProjectInfo.Bitdepth);
                writer.WriteLine("  CLI:  " + Environment.Version);
                writer.WriteLine("Message:" + Environment.NewLine + "  " + exception.Message);
                writer.WriteLine("Stacktrace:" + Environment.NewLine + exception.StackTrace);
                writer.Close();
                writer.Dispose();

                MessageBox.Show(name + ":" + Environment.NewLine + exception.Message, MessageType.Error, ButtonsType.Ok);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error!" + Environment.NewLine + ex.Message, MessageType.Error, ButtonsType.Ok);
                return false;
            }
        }
    }
}
