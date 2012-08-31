using Gtk;

namespace DeSERt
{
    public struct MessageBox
    {
        public static ResponseType Show(string message)
        {
            MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Other, ButtonsType.Ok, message);
            md.Title = "Message";
			md.WindowPosition = WindowPosition.CenterOnParent;
            ResponseType result = (ResponseType)md.Run();
            md.Destroy();
            return result;
        }

        public static ResponseType Show(string message, MessageType type)
        {
            MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, type, ButtonsType.Ok, message);
            switch (type)
            {
                case MessageType.Error:
                    md.Title = "Error";
                    break;

                case MessageType.Info:
                    md.Title = "Info";
                    break;

                case MessageType.Other:
                    md.Title = "Message";
                    break;

                case MessageType.Question:
                    md.Title = "Question";
                    break;

                case MessageType.Warning:
                    md.Title = "Warning";
                    break;
            }
			md.WindowPosition = WindowPosition.CenterOnParent;
            ResponseType result = (ResponseType)md.Run();
            md.Destroy();
            return result;
        }

        public static ResponseType Show(string message, MessageType type, ButtonsType bType)
        {
            MessageDialog md = new MessageDialog(null, DialogFlags.Modal, type, bType, message);
            switch (type)
            {
                case MessageType.Error:
                    md.Title = "Error";
                    break;

                case MessageType.Info:
                    md.Title = "Info";
                    break;

                case MessageType.Other:
                    md.Title = "Message";
                    break;

                case MessageType.Question:
                    md.Title = "Question";
                    break;

                case MessageType.Warning:
                    md.Title = "Warning";
                    break;
            }
			md.WindowPosition = WindowPosition.CenterOnParent;
			ResponseType result = (ResponseType)md.Run();
            md.Destroy();
            return result;
        }

        public static ResponseType Show(string message, MessageType type, ButtonsType bType, bool AddCancel)
        {
            MessageDialog md = new MessageDialog(null, DialogFlags.Modal, type, bType, message);
            switch (type)
            {
                case MessageType.Error:
                    md.Title = "Error";
                    break;

                case MessageType.Info:
                    md.Title = "Info";
                    break;

                case MessageType.Other:
                    md.Title = "Message";
                    break;

                case MessageType.Question:
                    md.Title = "Question";
                    break;

                case MessageType.Warning:
                    md.Title = "Warning";
                    break;
            }
            if (AddCancel) { md.AddButton("Cancel", ResponseType.Cancel); }
			md.WindowPosition = WindowPosition.CenterOnParent;
            ResponseType result = (ResponseType)md.Run();
            md.Destroy();
            return result;
        }

        public static ResponseType Show(string message, string title, MessageType type, ButtonsType bType)
        {
            MessageDialog md = new MessageDialog(null, DialogFlags.Modal, type, bType, message);
            md.Title = title;
			md.WindowPosition = WindowPosition.CenterOnParent;
            ResponseType result = (ResponseType)md.Run();
            md.Destroy();
            return result;
        }

        public static ResponseType Show(string message, string title, MessageType type)
        {
            MessageDialog md = new MessageDialog(null, DialogFlags.Modal, type, ButtonsType.Ok, message);
            md.Title = title;
			md.WindowPosition = WindowPosition.CenterOnParent;
            ResponseType result = (ResponseType)md.Run();
            md.Destroy();
            return result;
        }

        public static ResponseType Show(string message, string title, MessageType type, ButtonsType bType, bool AddCancel)
        {
            MessageDialog md = new MessageDialog(null, DialogFlags.Modal, type, bType, message);
            md.Title = title;
			md.WindowPosition = WindowPosition.CenterOnParent;
            if (AddCancel) { md.AddButton("Cancel", ResponseType.Cancel); }
            ResponseType result = (ResponseType)md.Run();
            md.Destroy();
            return result;
        }
    }
}