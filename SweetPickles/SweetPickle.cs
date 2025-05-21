using System.Collections.Generic;
using System.Linq;
using Gherkin.Ast;
using Gherkin.Pickles;

namespace FeatureFileParser.SweetPickles;

public class SweetPickle
{
    public string FeatureName { get; set; }
    public List<string> Tags { get; set; }
    public string Name { get; set; }
    public int ExampleNumber { get; set; }
    public List<SweetPickleStep> Steps { get; set; }

    public SweetPickle(Pickle pickle, GherkinDocument gherkinDocument)
    {
        FeatureName = gherkinDocument.Feature.Name;
        Tags = GetTags(pickle);
        Name = pickle.Name;
        ExampleNumber = GetExampleNumber(pickle, gherkinDocument);
        Steps = GetSweetPickleSteps(pickle, gherkinDocument);
    }

    private static List<string> GetTags(Pickle pickle) => [.. pickle.Tags.Select(tag => tag.Name)];

    private List<SweetPickleStep> GetSweetPickleSteps(Pickle pickle, GherkinDocument gherkinDocument)
    {
        var sweetPickleStepBuilder = new SweetPickleStepBuilder();
        return sweetPickleStepBuilder.BuildSteps(pickle, gherkinDocument);
    }

    private int GetExampleNumber(Pickle pickle, GherkinDocument gherkinDocument)
    {
        var PickleCompiler = new Compiler();
        var Pickles = PickleCompiler.Compile(gherkinDocument);
        var pickleComparer = new PickleComparer();
        var Examples = Pickles.Where(P => P.Name == pickle.Name);
        int exampleNumber = 0;
        if (Examples.Count() > 1)
        {
            int i = 1;
            foreach (var examplePickle in Examples)
            {
                if (pickleComparer.AreTheStepsTheSame(examplePickle, pickle))
                {
                    exampleNumber = i;
                    break;
                }
                i++;
            }
            return exampleNumber;
        }
        else
        {
            return 0;
        }
    }
}