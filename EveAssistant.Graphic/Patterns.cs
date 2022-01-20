using System.Collections.Generic;
using System.IO;
using EveAssistant.Common;

namespace EveAssistant.Graphic
{
    public class Patterns : IPatterns
    {
        private readonly Dictionary<string, IPatternsCollection> _patterns = new Dictionary<string, IPatternsCollection>();

        public Patterns(string directoryPath)
        {
            var rootDirectory = new DirectoryInfo(directoryPath);

            if (rootDirectory.Exists == false)
            {
                throw new DirectoryNotFoundException();
            }

            LoadDirectoryPatterns(rootDirectory, rootDirectory);
        }

        public void LoadDirectoryPatterns(DirectoryInfo currentDirectory, DirectoryInfo rootDirectory)
        {
            var directoryKey = currentDirectory.FullName.Replace(rootDirectory.FullName + @"\", "").Replace(@"\", "/");

            if(currentDirectory != rootDirectory) 
                _patterns.Add(directoryKey, new PatternsCollection(currentDirectory));

            foreach (var directoryInfo in currentDirectory.GetDirectories())
            {
                LoadDirectoryPatterns(directoryInfo, rootDirectory);
            }
        }

        public List<IPattern> Get(string key)
        {
            return _patterns[key].PatternEntity;
        }

        public IPatternsCollection GetPatterns(string key)
        {
            return _patterns.ContainsKey(key) ? _patterns[key] : new PatternsCollection();
        }
    }
}