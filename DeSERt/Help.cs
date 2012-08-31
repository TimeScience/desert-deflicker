using System;
using System.Xml;
using System.IO;
using Gtk;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace DeSERt
{
    public partial class Help : Gtk.Dialog
    {
        TreeStore table = new TreeStore(typeof(string));
        Dictionary<string, string> helptext = new Dictionary<string, string>();

        public Help()
        {
            this.Build();
			Init();
        }

        protected void OnButtonCancelClicked(object sender, EventArgs e)
        {
            this.Hide();
        }

        protected void OnHelpnodeCursorChanged(object sender, EventArgs e)
        {
            try
            {
                TreePath p;
                TreeViewColumn c;
                TreeIter iter;
                Helpnode.GetCursor(out p, out c);
                if (p != null)
                {
                    table.GetIter(out iter, Helpnode.Selection.GetSelectedRows()[0]);                    
                    string title = (string)table.GetValue(iter, 0);
                    string text;
                    if (helptext.TryGetValue(title, out text)) { Helptext.Buffer.Text = text; }
                    else { Helptext.Buffer.Text = "Sorry, couldn´t read helptext"; }
                }
            } catch (Exception ex) { ErrorReport.ReportError("Helpnode Cursor Changed", ex); }
        }

        private void Init()
        {
            try
            {
                TreeViewColumn Column = new TreeViewColumn();
                CellRendererText Cell = new CellRendererText();
                Column.Title = "Help";
                Column.PackStart(Cell, true);
                Helpnode.AppendColumn(Column);
                Column.AddAttribute(Cell, "text", 0);

                Helpnode.Model = table;

                TreeIter mainiter = new TreeIter();
                TreeIter subiter = new TreeIter();
                TreeIter subsubiter = new TreeIter();

                int id = 0;
                string attribute = String.Empty;

                FileStream stream = new FileStream(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Help/en.xml"), FileMode.Open);
                XmlTextReader reader = new XmlTextReader(stream);

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            id = Convert.ToInt32(reader.GetAttribute("id"));
                            if (id == 0) { /*do nothing*/}
                            else if (id == 1) { attribute = reader.GetAttribute("title"); mainiter = table.AppendValues(attribute); }
                            else if (id == 2) { attribute = reader.GetAttribute("title"); subiter = table.AppendValues(mainiter, attribute); }
                            else if (id == 3) { attribute = reader.GetAttribute("title"); subsubiter = table.AppendValues(subiter, attribute); }
                            break;
                        case XmlNodeType.Text:
                            if (id != 0) { helptext.Add(attribute, reader.Value); }
                            break;
                    }
                }
                reader.Close();
                stream.Close();
				
                TreeIter fIter;
                Helpnode.Model.GetIterFirst(out fIter);
                Helpnode.SetCursor(Helpnode.Model.GetPath(fIter), Helpnode.Columns[0], false);
			}
            catch (Exception ex) { ErrorReport.ReportError("Init (Help)", ex); }
        }

    }
}