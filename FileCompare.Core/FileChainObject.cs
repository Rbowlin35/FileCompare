using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompare.Core
{
    public abstract class FileChainObject
    {
        internal FileChainObject NextType { get; set; }

        public FileChainObject()
        {
            NextType = null;
        }

        public abstract FileObject GetFile(string path);
    }

    public class TxtFileType : FileChainObject
    {

        public override FileObject GetFile(string path)
        {
            if (Path.GetExtension(path).ToUpper() == ".TXT" || NextType == null)
                return new TextFile(path);
            else return NextType.GetFile(path);
        }

    }

    public class CsFileType : FileChainObject
    {
        public override FileObject GetFile(string path)
        {
            if (Path.GetExtension(path).ToUpper() == ".CS" || NextType == null)
                return new CSFile(path);
            else return NextType.GetFile(path);
        }

    }

    public class ZipFileType : FileChainObject
    {
        public override FileObject GetFile(string path)
        {
            if (Path.GetExtension(path).ToUpper() == ".ZIP" || NextType == null)
                return new ZipFileObj(path);
            else return NextType.GetFile(path);
        }
    }

    public class OtherFileType : FileChainObject
    {
        public override FileObject GetFile(string path)
        {
            return new OtherFile(path);
        }
    }
}
