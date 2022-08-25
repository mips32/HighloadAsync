using System.Collections.Generic;

namespace highload.FileSystem
{
    public interface IFolderObject
    {
        string SourcePath { get; }

        List<IFileObject> DataFiles { get; }
    }
}