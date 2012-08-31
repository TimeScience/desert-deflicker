using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DeSERt
{
    [Serializable()]
    public class Filterset
    {
        public List<int> WBJumps { get { return WBJmp; } }
        public List<double> TintJumps { get { return TintJmp; } }
        public List<double> EVJumps { get { return EVJmp; } }
        public List<string> FilterImages { get { return FilterImgs; } }
        public string ReferenceImage { get { return RefImg; } }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool HasReferenceImage { get { return HasRefImg; } }

        private string RefImg = null;
        private bool HasRefImg = false;
        private List<string> FilterImgs = new List<string>();
        private List<int> WBJmp = new List<int>();
        private List<double> TintJmp = new List<double>();
        private List<double> EVJmp = new List<double>();

        public Filterset()
        {
            Path = null;
            Name = "Unnamed Filterset";
        }

        public Filterset(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
        }

        public Filterset(string path, string name)
        {
            Path = path;
            Name = name;
        }


        public void AddReferenceImage(string path)
        {
            RefImg = path;
            HasRefImg = true;
        }

        public void RemovReferenceImage()
        {
            RefImg = null;
            HasRefImg = false;
        }

        public void AddFilterImage(string path)
        {
            FilterImgs.Add(path);
            CalcJumps();
        }

        public void RemoveFilterImage(int index)
        {
            FilterImgs.RemoveAt(index);
            WBJmp.RemoveAt(index);
            TintJmp.RemoveAt(index);
            EVJmp.RemoveAt(index);
        }

        public static Filterset OpenFilterset(string Path)
        {
            try
            {
                if (File.Exists(Path))
                {
                    Filterset fst = new Filterset(Path);

                    FileStream stream = new FileStream(Path, FileMode.Open);
                    BinaryReader reader = new BinaryReader(stream);

                    fst.Path = Path;

                    fst.Name = reader.ReadString();
                    if (reader.ReadBoolean()) { fst.AddReferenceImage(reader.ReadString()); }
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        fst.SetWBJump(i, reader.ReadInt32());
                        fst.SetTintJump(i, reader.ReadDouble());
                        fst.SetEVJump(i, reader.ReadDouble());
                    }

                    reader.Close();
                    stream.Close();

                    return new Filterset(Path);
                }
                else { return null; }
            }
            catch (Exception ex) { ErrorReport.ReportError("Open Filterset", ex); return null; }
        }

        public static void SaveFilterset(string Path, Filterset fst)
        {
            try
            {
                FileStream str = new FileStream(Path, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(str);

                writer.Write(fst.Name);
                writer.Write(fst.HasReferenceImage);
                if (fst.HasReferenceImage) { writer.Write(fst.ReferenceImage); }
                writer.Write(fst.FilterImages.Count);
                for (int i = 0; i < fst.FilterImages.Count; i++)
                {
                    writer.Write(fst.FilterImages[i]);
                    writer.Write(fst.WBJumps[i]);
                    writer.Write(fst.TintJumps[i]);
                    writer.Write(fst.EVJumps[i]);
                }

                writer.Close();
                str.Close();
            }
            catch (Exception ex) { ErrorReport.ReportError("Save Filterset", ex); }
        }

        private void SetWBJump(int index, int WBJump)
        {
            this.WBJmp[index] = WBJump;
        }

        private void SetTintJump(int index, double TintJump)
        {
            this.TintJmp[index] = TintJump;
        }

        private void SetEVJump(int index, double EVJump)
        {
            this.EVJmp[index] = EVJump;
        }

        //not written yet
        private void CalcJumps()
        {

        }


    }
}
