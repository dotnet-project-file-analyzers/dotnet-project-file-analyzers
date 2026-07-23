namespace DotNetProjectFile.Analyzers;

/// <summary>Communicates the project file and its analyzer type.</summary>
public readonly record struct AnalyzerFileInfo<TFile>(TFile File, AnalyzerType Type)
    where TFile : ProjectFile;
