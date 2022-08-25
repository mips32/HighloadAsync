using System.Collections.Generic;
using System.IO;

namespace highload.FileSystem
{
    public class FolderObject : IFolderObject
    {
        public string SourcePath { get; private set; }

        public List<IFileObject> DataFiles { get; private set; }

        private FolderObject()
        {
            this.DataFiles = new List<IFileObject>(1024);
        }

        private FolderObject(string sourcePath) : this()
        {
            this.SourcePath = sourcePath;
        }

        public static IFolderObject ReadFilesInfo(string sourcePath)
        {
            DirectoryInfo dir = new DirectoryInfo(sourcePath);
            FileInfo[] files = dir.GetFiles("*.txt");
            
            FolderObject folder = new FolderObject(sourcePath);

            foreach (FileInfo file in files)
            {
                IFileObject fileObject = new FileObject(file.FullName, file.Length);
                folder.DataFiles.Add(fileObject);
            }

            return folder;
        }

    }
}