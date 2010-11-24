Dim Wsh, Dir
Set Wsh = CreateObject("WScript.Shell")
'Dir = Wsh.ExpandEnvironmentStrings("%AppData%") & "\DDUtil\vspMgrSetup.msi"
'MsgBox Dir
Wsh.Exec("msiexec /a C:\Windows\Temp\DDUtil\vspMgrSetup.msi")
'Wsh.Exec("msiexec /a ["%APPDATA"]\DDUtil\vspMgrSetup.msi")

