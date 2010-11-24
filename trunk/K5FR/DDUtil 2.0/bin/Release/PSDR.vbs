Set wsh = CreateObject("WScript.Shell")
Set fso = CreateObject("Scripting.FileSystemObject")
Set wshEnv = wsh.Environment("PROCESS")
sPgmFiles = wshEnv("ProgramFiles")
If sPgmFiles = "" Then
  sPgmFiles = wsh.RegRead _
     ("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\ProgramFilesDir")
End If
If fso.FolderExists(sPgmFiles & "\FlexRadio Systems\PowerSDR v2.0.8") Then
    Wscript.Echo "Location is: " + sPgmFiles
Else
    Wscript.Echo "Folder does not exist."
End If
