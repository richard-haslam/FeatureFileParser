using System.Collections.Generic;
using System.Linq;
using Gherkin.Ast;
using Gherkin.Pickles;

namespace FeatureFileParser.SweetPickles;

public class SweetPickleStepBuilder
{
    public List<SweetPickleStep> BuildSteps(Pickle pickle, GherkinDocument gherkinDocument)
    {
        List<SweetPickleStep> steps = new List<SweetPickleStep>();
        var scenarioSteps = GetScenarioStepsAsArray(pickle, gherkinDocument);
        int numberOfBackgroundSteps = CalculateBackgroundStepCount(pickle, gherkinDocument);

        int currentStepNumber = 0;
        foreach (var step in pickle.Steps)
        {
            var SweetPickleStep = new SweetPickleStep();
            if (IsStepABackgroundStep(currentStepNumber, pickle, gherkinDocument))
            {
                SweetPickleStep.IsBackGroundStep = true;
                SweetPickleStep.Keyword = "Given";
            }
            else
            {
                SweetPickleStep.IsBackGroundStep = false;
                SweetPickleStep.Keyword = scenarioSteps[currentStepNumber - numberOfBackgroundSteps].Keyword;
            }
            SweetPickleStep.Description = step.Text;
            steps.Add(SweetPickleStep);
            currentStepNumber++;
        }
        return steps;
    }

    private Step[] GetScenarioStepsAsArray(Pickle pickle, GherkinDocument gherkinDocument)
    {
        var scenario = GetScenarioFromGherkinDocument(pickle, gherkinDocument);
        IEnumerable<Step> scenarioSteps = scenario.Steps;
        return scenarioSteps as Step[];
    }
    private int CalculateBackgroundStepCount(Pickle pickle, GherkinDocument gherkinDocument)
    {
        ScenarioDefinition scenario = GetScenarioFromGherkinDocument(pickle, gherkinDocument);
        int numberOfPickleSteps = pickle.Steps.Count();
        int numberOfScenarioSteps = scenario.Steps.Count();
        return numberOfPickleSteps - numberOfScenarioSteps;
    }
    private ScenarioDefinition GetScenarioFromGherkinDocument(Pickle pickle, GherkinDocument gherkinDocument)
    {
        return gherkinDocument.Feature.Children.Where(Child => Child.Name == pickle.Name).First();
    }
    private bool IsStepABackgroundStep(int stepNumber, Pickle pickle, GherkinDocument gherkinDocument)
    {
        int numberOfBackgroundSteps = CalculateBackgroundStepCount(pickle, gherkinDocument);
        if (stepNumber >= numberOfBackgroundSteps)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}