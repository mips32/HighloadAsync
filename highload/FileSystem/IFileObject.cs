using System.IO.MemoryMappedFiles;

namespace highload.FileSystem
{
    public interface IFileObject
    {
        string Path { get; }
        long Size { get; }
        
        MemoryMappedFile MappedFile { get; }
        MemoryMappedViewAccessor Accessor { get; }
        
        bool isMapped { get; }
        
        MemoryMappedFile MapFile();

        void UnmapFile();

        string GetString();

    }
}