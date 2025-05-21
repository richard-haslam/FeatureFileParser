using System;
using System.Collections.Generic;
using System.IO;
using FeatureFileParser.SweetPickles;
using Gherkin;
using Gherkin.Ast;
using Gherkin.Pickles;

namespace FeatureFileParser;

public class FeatureFile
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public List<SweetPickle> Scenarios { get; set; }

    public FeatureFile(string path)
    {
        CheckFileExists(path);
        var gherkinDocument = ReadFeatureFile(path);
        var pickles = GetPickles(gherkinDocument);

        Scenarios = GetScenarios(pickles, gherkinDocument);
        Name = gherkinDocument.Feature.Name;
        Description = gherkinDocument.Feature.Description;
        Tags = GetTags(gherkinDocument);
    }

    private void CheckFileExists(string path)
    {
        if (File.Exists(path) == false)
        {
            Console.WriteLine("!! File does not exist: {0}", path);
            Environment.Exit(1);
        }
    }

    private GherkinDocument ReadFeatureFile(string path)
    {
        var parser = new Parser();
        var gherkinDocument = parser.Parse(path);

        if (parser.StopAtFirstError == true)
        {
            Console.WriteLine("File '{0}' has invalid format!", path);
        }
        return gherkinDocument;
    }
    private List<Pickle> GetPickles(GherkinDocument gherkinDocument)
    {
        var PickleCompiler = new Compiler();
        return PickleCompiler.Compile(gherkinDocument);
    }
    private List<SweetPickle> GetScenarios(List<Pickle> pickles, GherkinDocument gherkinDocument)
    {
        var sweetPickles = new List<SweetPickle>();
        foreach (var pickle in pickles)
        {
            var sweetPickle = new SweetPickle(pickle, gherkinDocument);
            sweetPickles.Add(sweetPickle);
        }
        return sweetPickles;
    }
    private List<string> GetTags(GherkinDocument gherkinDocument)
    {
        var tags = new List<string>();
        foreach (var tag in gherkinDocument.Feature.Tags)
        {
            tags.Add(tag.Name);
        }
        return tags;
    }
}