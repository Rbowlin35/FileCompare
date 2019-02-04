using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompare.Core
{
    public class OtherFile : FileObject
    {
        public OtherFile(string path) : base(path) { }

        public override bool Compare(object obj)
        {
            return true;
        }

        public override bool CompareContents(object obj, object obj2)
        {
            throw new NotImplementedException();
        }

    }
}
