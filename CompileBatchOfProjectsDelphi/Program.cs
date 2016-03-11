using System;
using System.IO;

namespace CompileBatchOfProjectsDelphi {
    class Program {
        static void Main(string[] args) {
            var delphiFile = args[0];
            var tempFolder = args.Length > 0 ? args[1] : string.Empty;
            var seacrhPathFolder = args.Length > 1 ? args[2] : string.Empty;
            var binPath = args.Length > 2 ? args[3] : string.Empty;

            CompileAllProjectFiles(delphiFile, tempFolder, seacrhPathFolder, binPath);
        }

        private static void CompileAllProjectFiles(string delphiFile, string tempFolder, string seacrhPathFolder, string binPath) {
            var filePaths = new DirectoryInfo(".").EnumerateFiles("*.dpr", SearchOption.AllDirectories);

            foreach (var filePath in filePaths) {
                try {
                    Console.WriteLine(filePath.FullName);

                    new CompileDelphiProject(delphiFile).ProjectFile(filePath.FullName)
                                                        .TempDirectory(tempFolder)
                                                        .SearchPath(seacrhPathFolder)
                                                        .BinPath(binPath)
                                                        .Build(new UpxCompress());
                } catch (Exception exception) {
                    Console.WriteLine("Error >>>>> " + exception.Message);
                }
            }
        }
    }
}
