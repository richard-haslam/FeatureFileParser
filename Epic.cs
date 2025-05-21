using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FeatureFileParser
{
    public class Epic
    {
        public string Name { get; set; }
        public List<Epic> Epics { get; set; }
        public List<FeatureFile> FeatureFiles { get; set; }

        public Epic(string path)
        {
            CheckDirectoryExists(path);
            Name = GetName(path);
            Epics = GetEpics(path);
            FeatureFiles = GetFeatureFiles(path);
        }

        private void CheckDirectoryExists(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Console.WriteLine("!! Directory does not exist: {0}", path);
                Environment.Exit(1);
            }
        }

        private string GetName(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            return directoryInfo.Name;
        }

        private List<Epic> GetEpics(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            return [.. from directory in directoryInfo.GetDirectories()
                    let epic = new Epic(directory.FullName)
                    select epic];
        }

        private List<FeatureFile> GetFeatureFiles(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            var featureFiles = new List<FeatureFile>();
            foreach (var file in directoryInfo.GetFiles("*.feature"))
            {
                var featureFile = new FeatureFile(file.FullName);
                featureFiles.Add(featureFile);
            }
            return featureFiles;
        }
    }
}