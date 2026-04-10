param($installPath, $toolsPath, $package, $project)

if ($project.Object.AnalyzerReferences -eq $null) {
    throw 'This package cannot be installed without an analyzer reference.'
}

$analyzersPath = Split-Path -Path $toolsPath -Parent
$analyzersPath = Join-Path $toolsPath "analyzers"
$analyzerFilePath = Join-Path $analyzersPath "DotNetProjectFile.Analyzers.dll"
$project.Object.AnalyzerReferences.Add($analyzerFilePath)
