using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompare.Core
{
    public class FileCompareCore
    {
        internal string path1;
        internal string path2;

        public List<FileObject> Both;
        public List<FileObject> LeftOnly;
        public List<FileObject> RightOnly;
        public List<FileObject> Differ;


        public FileCompareCore(string f1, string f2)
        {
            path1 = f1;
            path2 = f2;

            Both = new List<FileObject>();
            LeftOnly = new List<FileObject>();
            RightOnly = new List<FileObject>();
            Differ = new List<FileObject>();
        }

        public void RunCompare()
        { 
            var Files1 = new List<FileObject>();
            var Files2 = new List<FileObject>();

            var chain = new FileChain();

            var t1 = Task.Factory.StartNew(() =>
            {
                LoadFiles(Files1, path1, chain);
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                LoadFiles(Files2, path2, chain);
            });

            Task.WaitAll(t1, t2);

            var display1 = Files1.Select(f => f.ToString()).ToList();
            var display2 = Files2.Select(f => f.ToString()).ToList();
            Both = Files1.Where(f => display2.Contains(f.ToString())).ToList();

            var t3 = Task.Factory.StartNew(() =>
            {
                LeftOnly = Files1.Where(f => !display2.Contains(f.ToString())).ToList();
                RightOnly = Files2.Where(f => !display1.Contains(f.ToString())).ToList();
            });

            var t4 = Task.Factory.StartNew(() =>
            {
                Differ = new List<FileObject>();
                foreach (var b in Files1.Where(f => Both.Select(d => d.ToString()).ToList().Contains(f.ToString())))
                {
                    var b2 = Files2.Single(f => f.ToString() == b.ToString());
                    if (!b.Equals(b2))
                        Differ.Add(b);
                }
            });

            Task.WaitAll(t3, t4);
        }

        private void LoadFiles(List<FileObject> files, string path, FileChain chain)
        {
            foreach( var file in Directory.EnumerateFiles(path))
                files.Add(chain.GetFile(file));
        }

    }
}
