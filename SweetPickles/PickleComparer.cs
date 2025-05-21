using System.Collections.Generic;
using Gherkin.Pickles;

namespace FeatureFileParser.SweetPickles;

public class PickleComparer
{
    private List<PickleStep> GetStepsAsList(Pickle pickle)
    {
        IEnumerable<PickleStep> steps = pickle.Steps;
        return steps as List<PickleStep>;
    }

    public bool AreTheStepsTheSame(Pickle firstPickle, Pickle secondPickle)
    {
        var isSame = true;

        if (AreTheStepCountsTheSame(firstPickle, secondPickle))
        {
            var firstStepsArray = GetStepsAsList(firstPickle);
            var secondStepsArray = GetStepsAsList(secondPickle);

            for (int i = 0; i < firstStepsArray.Count; i++)
            {
                if (firstStepsArray[i].Text != secondStepsArray[i].Text)
                {
                    isSame = false;
                    break;
                }
            }
        }
        return isSame;
    }

    public bool AreTheStepCountsTheSame(Pickle firstPickle, Pickle secondPickle)
    {
        var firstStepsArray = GetStepsAsList(firstPickle);
        var secondStepsArray = GetStepsAsList(secondPickle);

        return firstStepsArray.Count == secondStepsArray.Count;
    }
}