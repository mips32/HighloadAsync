using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using highload.Helpers;

namespace highload.FileSystem
{
    public class FileObject : IFileObject
    {
        public string Path { get; private set; }
        public long Size { get; private set; }
        
        public MemoryMappedFile MappedFile { get; private set; }
        public MemoryMappedViewAccessor Accessor { get; private set; }
        
        public bool isMapped { get; private set; }

        private FileObject()
        {
        }

        public FileObject(string path, long size) : this()
        {
            this.Path = path;
            this.Size = size;
        }

        public MemoryMappedFile MapFile()
        {
            try
            {
                this.MappedFile = MemoryMappedFile.CreateFromFile(this.Path, FileMode.Open);

                this.Accessor = this.MappedFile.CreateViewAccessor(0x0L, this.Size, MemoryMappedFileAccess.Read);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
                ((IDisposable) this.MappedFile)?.Dispose();
                ((IDisposable) this.Accessor)?.Dispose();

                isMapped = false;
            }

            return this.MappedFile;
        }

        public void UnmapFile()
        {
            ((IDisposable) this.MappedFile)?.Dispose();
            ((IDisposable) this.Accessor)?.Dispose();

            isMapped = false;
        }

        public string GetString()
        {
            return this.Accessor.ReadString(0);
        }
    }
    
}