using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeSERt
{
    public struct ProjectInfo
    {
        public static int MainVersion { get { return 1; } }
        public static string Status  { get { return "alpha"; } }
        public static int MinorVersion { get { return 5; } }
        public static string Name { get { return "DeSERt"; } }
        public static string OSversion { get { return Environment.OSVersion.VersionString; } }
        public static string Bitdepth { get { if (Environment.Is64BitOperatingSystem) { return "64 bit"; } else { return "32 bit"; } } }
        public static string FullVersion { get { return MainVersion + " " + Status + " " + MinorVersion; } }
        public static string FullName { get { return ProjectInfo.Name + " V" + ProjectInfo.MainVersion + " " + ProjectInfo.Status + " " + ProjectInfo.MinorVersion; } }
        public static int FileVersion { get { return 2; } }
    }
}
