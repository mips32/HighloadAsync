using System.Collections.Generic;
using System.Threading.Tasks;
using highload.FileSystem;

namespace highload.Parser
{
    public interface IFileParser
    {
        Task<IParseResult> Parse();
    }
}