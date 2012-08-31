using System;

namespace DeSERt
{
    public partial class MyAbout : Gtk.Dialog
    {
        public MyAbout()
        {
            this.Build();
			textview2.Buffer.Text = ProjectInfo.FullName + @"

Question?
Contact me at: bildstein.johannes@gmail.com

DeSERt uses:
#RawTherapee:		http://rawtherapee.com/
#Exiftool: 			http://www.sno.phy.queensu.ca/~phil/exiftool/
#SharpZipLib:		http://www.icsharpcode.net/opensource/sharpziplib/

© 2012 Johannes Bildstein";
        }

        protected void OnButtonCancelClicked(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}