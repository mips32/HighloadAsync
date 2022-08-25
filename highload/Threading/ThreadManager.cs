using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using highload.FileSystem;
using highload.Parser;
using highload.Settings;

namespace highload.Threading
{
    public class ThreadManager
    {

        private ICurrentSettings settings;

        private IFolderObject folder;

//        private IFileParser parser;

        private List<List<IFileObject>> files;

        private ThreadManager()
        {
        }

        public ThreadManager(ICurrentSettings settings, IFolderObject folder /*, IFileParser parser*/)
        {
            this.settings = settings;
            this.folder = folder;
//            this.parser = parser;

            this.files = new List<List<IFileObject>>(this.settings.NumberOfThreads);
        }

        private void SeparateFiles()
        {

            for (int i = 0; i < this.settings.NumberOfThreads; i++)
            {
                this.files.Add(new List<IFileObject>());
            }

            for (int i = 0; i < this.folder.DataFiles.Count; i+=this.settings.NumberOfThreads)
            {
                for (int j = 0; j < this.settings.NumberOfThreads; j++)
                {
                    if (i + j >= this.folder.DataFiles.Count)
                        break;

                    this.files[j].Add(this.folder.DataFiles[i + j]);
                }
            }
            
        }
        
        public async Task<List<KeyValuePair<string, int>>> Go()
        {
            // Separate files
            this.SeparateFiles();
            // Parse files
            Task<List<IParseResult>>[] tasks = new Task<List<IParseResult>>[this.settings.NumberOfThreads];

            for (int i = 0; i < this.settings.NumberOfThreads; i++)
                tasks[i] =  Worker(files[i], this.settings.MinimumWordLength);

            List<IParseResult>[] results =  await Task.WhenAll(tasks);

            // Group results
            Dictionary<string, int> total = new Dictionary<string, int>(results.Length);
            foreach (List<IParseResult> parseResult in results)
            {
                foreach (IParseResult localResult in parseResult)
                {
                    foreach (KeyValuePair<string, int> wordToFreq in localResult.Result)
                    {
                        if (!total.ContainsKey(wordToFreq.Key))
                        {
                            total.Add(wordToFreq.Key, wordToFreq.Value);
                        }
                        else
                        {
                            total[wordToFreq.Key] += wordToFreq.Value;
                        }
                    }
                }
            }
            
            // Sort results
            IOrderedEnumerable<KeyValuePair<string, int>> sortedDict = from entry in total orderby entry.Value descending select entry;
            List<KeyValuePair<string, int>> list = sortedDict.ToList();
            
            return list;

        }
        
        private static async Task<List<IParseResult>> Worker(List<IFileObject> files, int minimumWordLength)
        {
            List<IParseResult> result = new List<IParseResult>(files.Count);
            for (int i = 0; i < files.Count; i++)
            {
                IFileParser parser = new FileParser(files[i], minimumWordLength);

                IParseResult res = await parser.Parse();
                    
                result.Add(res);
            }

            return result;
        }

    }
    
}