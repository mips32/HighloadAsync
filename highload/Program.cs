using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using highload.FileSystem;
using highload.Helpers;
using highload.Parser;
using highload.Settings;
using highload.Threading;

namespace highload
{
    static class Program
    {
        private const int NUMBER_OF_WORDS = 10;

        static void Main(string[] args)
        {
            // Read settings
            CurrentSettings settings = CurrentSettings.ParseSettings(args);
            if (settings == null)
            {
                return;
            }
            
            Console.WriteLine("Path: \"{0}\"", settings.DataPath);
            Console.WriteLine("Number of threads: \"{0}\"", settings.NumberOfThreads);
            Console.WriteLine("Minimum word length: \"{0}\"", settings.MinimumWordLength);

            // Read folder info
            IFolderObject folder = FolderObject.ReadFilesInfo(settings.DataPath);

            ThreadManager tm = new ThreadManager(settings, folder);
            //tm.Go().Wait();

            Task<List<KeyValuePair<string, int>>> task = tm.Go();
            List<KeyValuePair<string, int>> res = task.Result;

            for (int i = 0; i < res.Count() && i < NUMBER_OF_WORDS; i++)
            {
                Console.WriteLine("{0} - {1}", res[i].Key, res[i].Value);
            }

        }
        
    }

}