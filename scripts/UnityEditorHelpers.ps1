Write-Output $PSScriptRoot

$repoRoot = "$PSScriptRoot\.."
$projectPath = "$repoRoot\pig\"
$projectVersionFile = Get-Content "$projectPath\ProjectSettings\ProjectVersion.txt"

$versionDictionary = @{}
foreach($line in $projectVersionFile) {
    $key,$value = $line.Split(':')
    $versionDictionary[$key] = $value.Trim()
}

$editorVersion = $versionDictionary["m_EditorVersion"]
$unityPath = "C:\Program Files\Unity\Hub\Editor\$editorVersion\Editor\Unity.exe"