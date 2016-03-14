# Compilation Batch of Delphi projects

Utility to compile Delphi 7 projects in batch.

## Call by command line on Jenkins


 - Add CompileBatchOfProjectsDelphi.exe and UPX.exe to path of Windows;
 - In folder where have *.dpr files, execute the command: 
   - CompileBatchOfProjectsDelphi.exe %DELPHI% "c:\temp" "C:\Library;" "c:\executablefolder"
   - Example: DELPHI="C:\Program Files (x86)\Borland\Delphi7\Bin\dcc32.exe"

## Download

See the list of [available releases](https://github.com/samukce/compilation-batch-of-delphi-projects/releases).
  
## Requirements to use

- .NET Framework 4.6
- Delphi 7
- UPX.exe

## License

MIT License
