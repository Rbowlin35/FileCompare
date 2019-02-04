using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace FileCompare.Core
{
    public class ZipFileObj : FileObject
    {
        internal List<FileObject> _contents;
        internal bool _loaded;

        public ZipFileObj(string path) : base(path)
        {
            _contents = new List<FileObject>();
            _loaded = false;
            Task.Factory.StartNew(() => LoadContents(path));
        }

        private void LoadContents(string path)
        {
            var newPath = GetExtractPath(path);
            using (ZipArchive archive = ZipFile.OpenRead(path))
            {
                var chain = new FileChain();
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    var newFile = Path.Combine(newPath, entry.Name);
                    entry.ExtractToFile(newFile, true);
                    _contents.Add(chain.GetFile(newFile));
                }
            }

            _loaded = true;
        }

        internal string GetExtractPath(string path)
        {
            var app = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var extractPath = Path.Combine(app, "FileCompare", Path.GetFileNameWithoutExtension(path), "extract");
            if (!Directory.Exists(extractPath))
                Directory.CreateDirectory(extractPath);

            return extractPath;
            //Todo: clean up folder on exit?
        }
        public override bool Compare(object obj)
        {
            var zObj = obj as ZipFileObj;
            foreach(var item in zObj._contents)
            {
                bool match = false;
                foreach (var item2 in _contents)
                {
                    match = CompareContents(item, item2);
                }

                if (!match)
                    return false;
            }

            return true;
        }

        public override bool CompareContents(object obj, object obj2)
        {
            return obj.Equals(obj2);
        }

        public override string ToString()
        {
            if (_loaded != false)
            {
                return string.Join(Environment.NewLine, _contents.Select(s => $"{Path.GetFileName(Name)}:{s.ToString()}").ToArray());
            }
            else
                return $"{Path.GetFileName(Name)}: Loading contents...";
        }
    }
}
