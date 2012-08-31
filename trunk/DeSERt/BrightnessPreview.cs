using System;
using Gdk;
using System.IO;
using Gtk;
using System.Collections.Generic;

namespace DeSERt
{
    public partial class BrightnessPreview : Gtk.Window
    {
        List<Pixbuf> Imgs = new List<Pixbuf>();
        List<string> Names = new List<string>();
        bool GotError = false;

        public BrightnessPreview() : base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "preview");
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, "*.jpg");

                PrevImgSlide.Adjustment.Upper = files.Length;

                if (files.Length > 1)
                {
                    try
                    {
                        for (int i = 0; i < files.Length; i++) { Imgs.Add(new Pixbuf(files[i])); Names.Add(System.IO.Path.GetFileName(files[i])); }
                    }
                    catch { MessageBox.Show("Couldn´t open image!", Gtk.MessageType.Error, Gtk.ButtonsType.Ok); GotError = true; }

                    PreviewImage.Pixbuf = Imgs[0];
                    this.Title = "Brightness Preview - " + Names[0];
                }
                else { MessageBox.Show("No images available!", Gtk.MessageType.Error, Gtk.ButtonsType.Ok); GotError = true; }
            }
            else { MessageBox.Show("Directory does not exist!", Gtk.MessageType.Error, Gtk.ButtonsType.Ok); GotError = true; }
        }

        protected void OnDeleteEvent(object sender, DeleteEventArgs a)
        {
            if (PreviewImage.Pixbuf != null) { PreviewImage.Pixbuf.Dispose(); }
            for (int i = 0; i < Imgs.Count; i++) { Imgs[i].Dispose(); }
        }

        protected void OnPrevImgSlideValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!GotError)
                {
                    PreviewImage.Pixbuf = Imgs[(int)PrevImgSlide.Value - 1];
                    this.Title = "Brightness Preview - " + Names[(int)PrevImgSlide.Value - 1];
                }
            }
            catch { MessageBox.Show("Couldn´t set image!", Gtk.MessageType.Error, Gtk.ButtonsType.Ok); GotError = true; }
        }

    }
}

