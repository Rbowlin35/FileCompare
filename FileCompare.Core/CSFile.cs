using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompare.Core
{
    public class CSFile : TextFile
    {
        public CSFile(string path) : base(path) { }

        public override bool CompareContents(object obj, object obj2)
        {
            var s1 = obj as string;
            var s2 = obj as string;

            if (s1.Contains("//") && s2.Contains("//"))
                return true;

            return base.CompareContents(obj, obj2);
        }
    }
}
