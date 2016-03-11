using System;
using System.IO;

namespace CompileBatchOfProjectsDelphi {
    class Program {
        static void Main(string[] args) {
            var delphiFile = args[0];
            var tempFolder = args.Length > 0 ? args[1] : string.Empty;
            var seacrhPathFolder = args.Length > 1 ? args[2] : string.Empty;

            CompileAllProjectFiles(delphiFile, tempFolder, seacrhPathFolder);
        }

        private static void CompileAllProjectFiles(string delphiFile, string tempFolder, string seacrhPathFolder) {
            var filePaths = new DirectoryInfo(".").EnumerateFiles("*.dpr", SearchOption.AllDirectories);

            foreach (var filePath in filePaths) {
                try {
                    Console.WriteLine(filePath.FullName);

                    new CompileDelphiProject(delphiFile).ProjectFile(filePath.FullName)
                                                        .TempDirectory(tempFolder)
                                                        .SearchPath(seacrhPathFolder)
                                                        .Build();
                } catch (Exception exception) {
                    Console.WriteLine("Error>>>>> " + exception.Message);
                }
            }
        }
    }
}
