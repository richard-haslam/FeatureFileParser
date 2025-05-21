# FeatureFileParser
This package provides an easy to use data object of a feature file. The class ```FeatureFile``` requires the path to an actual featurefile to allow to data object to be constructed.

Example:
```
string pathToFeatureFile = @"Features\featureFile.feature";
FeatureFile myFeature = new FeatureFile(pathToFeatureFile);
```