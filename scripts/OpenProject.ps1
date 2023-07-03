. .\UnityEditorHelpers.ps1

Write-Output "Using editor version $editorversion"
Write-Output "Editor is at $unityPath"
Write-Output "Launching editor, hold onto your butts!"

Start-Process -FilePath $unityPath -ArgumentList "-projectPath pig\"