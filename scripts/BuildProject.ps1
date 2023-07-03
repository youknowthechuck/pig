. .\UnityEditorHelpers.ps1

Write-Output "Using editor version $editorversion"
Write-Output "Editor is at $unityPath"

$unixTime = [int](Get-Date -UFormat %s -Millisecond 0)
$buildPath = "$repoRoot\build"
$logPath = "$repoRoot\build\logs\$unixTime.txt"

Write-Output "Building project at $projectPath"
Write-Output "Output is in $buildPath"
Write-Output "Logs are at $logPath"

Start-Process -Wait -FilePath $unityPath -ArgumentList "Unity -quit -batchmode -nographics -buildWindowsPlayer $buildPath -logFile $logPath -projectPath $projectPath"