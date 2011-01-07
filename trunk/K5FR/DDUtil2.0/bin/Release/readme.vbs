Dim WshShell
Set WshShell = CreateObject("WScript.Shell")
WshShell.Exec("notepad %APPDATA%\DDUtil\readme.txt")
