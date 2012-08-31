using System;
using Gtk;

namespace DeSERt
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			DeSERtMain win = new DeSERtMain ();
			win.Show ();
			Application.Run ();
		}
	}
}
