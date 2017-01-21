@ECHO OFF
"..\Aci.X.DalGen\bin\Debug\Aci.X.DalGen.Exe" DalSprocs.cs .\bin\Debug\Aci.X.Database.dll Aci.X.Database DalSqlDb MyStoredProc
IF ERRORLEVEL 0 GOTO end
ECHO REM DalGenfailed.
DEL DalSprocs.cs
:end