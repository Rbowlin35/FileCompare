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

        public List<string> Both;
        public List<string> LeftOnly;
        public List<string> RightOnly;
        public List<string> Differ;


        public FileCompareCore(string f1, string f2)
        {
            path1 = f1;
            path2 = f2;

            Both = new List<string>();
            LeftOnly = new List<string>();
            RightOnly = new List<string>();
            Differ = new List<string>();
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

            var display1 = Files1.Select(f => f.Name);
            var display2 = Files2.Select(f => f.Name);
            var both = Files1.Where(f => display2.Contains(f.Name)).ToList();

            var t3 = Task.Factory.StartNew(() =>
            {
                Both = both.Select(d => d.Display()).ToList();
                LeftOnly = Files1.Where(f => !display2.Contains(f.Name)).Select(d => d.Display()).ToList();
                RightOnly = Files2.Where(f => !display1.Contains(f.Name)).Select(d => d.Display()).ToList();
            });

            var t4 = Task.Factory.StartNew(() =>
            {
                Differ = new List<string>();
                foreach (var b in Files1.Where(f => both.Select(d => d.Name).ToList().Contains(f.Name)))
                {
                    var b2 = Files2.Single(f => f.Name == b.Name);
                    if (!b.Equals(b2))
                        Differ.Add(b.Display());
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
