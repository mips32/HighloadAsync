using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using highload.FileSystem;
using highload.Helpers;

namespace highload.Parser
{
    public class FileParser : IFileParser
    {
        private Regex rx;

        private IFileObject fileObject;
        
        private FileParser()
        {
        }

        public FileParser(IFileObject fileObject, int minimumWordsLength) : this()
        {
            string pattern = $"(\\w+){{{minimumWordsLength},}}";
//            this.rx = new Regex(@"(\w+){3,}");
            this.rx = new Regex(pattern);

            this.fileObject = fileObject;
        }

        public async Task<IParseResult> Parse()
        {
            this.fileObject.MapFile();
            string content = this.fileObject.GetString();
            
            Dictionary<string, int> words = new Dictionary<string, int>(1024);
            MatchCollection matches = await Task.Run(() => rx.Matches(content));
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                int currentCount = 0;
                words.TryGetValue(match.Value, out currentCount);

                currentCount++;
                words[match.Value] = currentCount;
            }
            
            IParseResult res = new ParseResult(words);
            return res;
        }
    }
}