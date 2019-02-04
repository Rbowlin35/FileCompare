using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompare.Core
{
    public abstract class FileObject
    {
        public enum FileTypes { Zip, Txt, Cs, Other };

        internal string Name { get; set; }
        internal long Size { get; set; }
        internal FileTypes FileType { get; set; }

        public FileObject(string path)
        {
            Name = Path.GetFileName(path);

            var f = new FileInfo(path);
            Size = f.Length;
        }

        public override bool Equals(object obj)
        {
            var fObj = obj as FileObject;
                if (FileType == fObj.FileType
                    && Name == fObj.Name
                    && Size == fObj.Size)
                    return Compare(obj);

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public abstract bool Compare(object obj);
        public abstract bool CompareContents(object obj, object obj2);

        public virtual string Display()
        {
            return Name;
        }
    }
}
