using System;
using System.Collections.Generic;

namespace FileCompare.Core
{
    internal class FileChain
    {
        internal FileChainObject firstObject;

        public FileChain()
        {
            firstObject = new ZipFileType();
            firstObject.NextType = new TxtFileType();
            firstObject.NextType.NextType = new CsFileType();
            firstObject.NextType.NextType.NextType = new OtherFileType();
        }

        internal FileObject GetFile(string file)
        {
            return firstObject.GetFile(file);
        }
    }
}