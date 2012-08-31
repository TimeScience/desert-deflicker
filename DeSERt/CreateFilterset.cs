using System;
using Gtk;
using System.IO;

namespace DeSERt
{
    public partial class CreateFilterset : Gtk.Window
    {
        #region Variables

        private ListStore table = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));
        private Filterset fst = new Filterset();
        private bool saved = true;

        #endregion Variables

        #region Events

        public CreateFilterset() :
            base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            InitTable();
        }

        protected void OnRefImgChooserSelectionChanged(object sender, EventArgs e)
        {
            if (File.Exists(RefImgChooser.Filename))
            {
                fst.AddReferenceImage(RefImgChooser.Filename);
                UpdateTable();
                saved = false;
            }
        }

        protected void OnFilterImgChooserSelectionChanged(object sender, EventArgs e)
        {
            if (File.Exists(FilterImgChooser.Filename))
            {
                fst.AddFilterImage(FilterImgChooser.Filename);
                UpdateTable();
                saved = false;
            }
        }

        protected void OnValueTableKeyPressEvent(object o, KeyPressEventArgs args)
        {
            try
            {
                if (args.Event.Key == Gdk.Key.Delete)
                {
                    TreeIter iter;
                    table.GetIter(out iter, ValueTable.Selection.GetSelectedRows()[0]);
                    int index = table.GetPath(iter).Indices[0];

                    if (index == 0 && fst.HasReferenceImage) { fst.RemovReferenceImage(); }
                    else if (fst.HasReferenceImage) { fst.RemoveFilterImage(index - 1); }
                    else { fst.RemoveFilterImage(index); }

                    saved = false;
                    UpdateTable();
                }
            }
            catch (Exception ex) { ErrorReport.ReportError("ValueTableKeyPress (Create Filterset)", ex); }
        }

        protected void OnSaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                FileChooserDialog fc = new FileChooserDialog("Save Filterset", this, FileChooserAction.Save, "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept);

                FileFilter filter = new FileFilter();
                filter.Name = "DeSERt Filterset";
                filter.AddMimeType("Filterset/fis");
                filter.AddPattern("*.fis");
                fc.AddFilter(filter);
                fc.DoOverwriteConfirmation = true;
                fc.CurrentName = FiltersetnameEntry.Text.Replace(" ", "-");
                if (Directory.Exists(MySettings.LastFilterDir)) { fc.SetCurrentFolder(MySettings.LastFilterDir); }

                ResponseType res = (ResponseType)fc.Run();
                if (res == ResponseType.Ok || res == ResponseType.Close)
                {
                    string path;
                    if (!System.IO.Path.HasExtension(fc.Filename)) { path = fc.Filename + ".fis"; }
                    else { path = fc.Filename; }
                    MySettings.LastFilterDir = System.IO.Path.GetDirectoryName(fc.Filename);
                    MySettings.Save();
                    fst.Name = FiltersetnameEntry.Text;
                    Filterset.SaveFilterset(path, fst);
                    saved = true;
                }
                fc.Destroy();
                if (res == ResponseType.Close) { this.Destroy(); }
            }
            catch (Exception ex) { ErrorReport.ReportError("Save Button (Create Filterset)", ex); }
        }

        protected void OnCancelButtonClicked(object sender, EventArgs e)
        {
            this.Hide();
        }

        protected void OnHidden(object o, EventArgs args)
        {
            if (!saved)
            {
                ResponseType res = MessageBox.Show("The data isn´t saved yet." + Environment.NewLine + " Do you want to save now?", MessageType.Question, ButtonsType.YesNo);
                if (res == ResponseType.Yes) { OnSaveButtonClicked(o, args); }
            }
        }

        #endregion Events

        #region Subroutines

        private void UpdateTable()
        {
            try
            {
                table.Clear();
                if (fst.HasReferenceImage)
                {
                    table.AppendValues(System.IO.Path.GetFileName(RefImgChooser.Filename), "Reference Image", 0, 0, 0);
                }

                if (fst.FilterImages.Count > 0)
                {
                    for (int i = 0; i < fst.FilterImages.Count; i++)
                    {
                        table.AppendValues(System.IO.Path.GetFileName(FilterImgChooser.Filename), "Filter Image", fst.WBJumps[i], fst.TintJumps[i], fst.EVJumps[i]);
                    }
                }
            }
            catch (Exception ex) { ErrorReport.ReportError("Update Table (Create Filterset)", ex); }
        }

        private void InitTable()
        {
            try
            {
                TreeViewColumn ImgColumn = new TreeViewColumn();
                TreeViewColumn TypeColumn = new TreeViewColumn();
                TreeViewColumn WBColumn = new TreeViewColumn();
                TreeViewColumn TintColumn = new TreeViewColumn();
                TreeViewColumn EVColumn = new TreeViewColumn();

                CellRendererText ImgCell = new CellRendererText();
                CellRendererText TypeCell = new CellRendererText();
                CellRendererText WBCell = new CellRendererText();
                CellRendererText TintCell = new CellRendererText();
                CellRendererText EVCell = new CellRendererText();

                ImgColumn.Title = "Image";
                ImgColumn.MinWidth = 100;
                ImgColumn.PackStart(ImgCell, true);

                TypeColumn.Title = "Type";
                TypeColumn.MinWidth = 100;
                TypeColumn.PackStart(TypeCell, true);

                TypeColumn.Title = "WB Change";
                TypeColumn.MinWidth = 100;
                TypeColumn.PackStart(WBCell, true);

                TypeColumn.Title = "Tint Change";
                TypeColumn.MinWidth = 100;
                TypeColumn.PackStart(TintCell, true);

                TypeColumn.Title = "EV Change";
                TypeColumn.MinWidth = 100;
                TypeColumn.PackStart(EVCell, true);

                ValueTable.AppendColumn(ImgColumn);
                ValueTable.AppendColumn(TypeColumn);
                ValueTable.AppendColumn(WBColumn);
                ValueTable.AppendColumn(TintColumn);
                ValueTable.AppendColumn(EVColumn);

                ImgColumn.AddAttribute(ImgCell, "text", 0);
                TypeColumn.AddAttribute(TypeCell, "text", 1);
                ImgColumn.AddAttribute(WBCell, "text", 2);
                TypeColumn.AddAttribute(TintCell, "text", 3);
                ImgColumn.AddAttribute(EVCell, "text", 4);

                ValueTable.Model = table;
            }
            catch (Exception ex) { ErrorReport.ReportError("Init Table (Create Filterset)", ex); }
        }        

        #endregion Subroutines
    }
}

