using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompare.Core
{
    public class TextFile : FileObject
    {
        public TextFile(string path) : base(path)  { }

        public override bool CompareContents(object obj, object obj2)
        {
            return string.Compare((obj as string), (obj2 as string), true) == 0;
        }

        public override bool Compare(object obj)
        {
            var file = obj as FileObject;

            using (var sr = new StreamReader(file.Name))
            using (var sr2 = new StreamReader(this.Name))
            {
                while (sr.Peek() > -1 && sr2.Peek() > -1)
                {
                    var line1 = sr.ReadLine();
                    var line2 = sr2.ReadLine();
                    if (!CompareContents(line1, line2))
                        return false;
                }
            }

            return true;
        }
    }
}
