using System;
using System.Diagnostics;
using System.IO;

namespace CompileBatchOfProjectsDelphi {
    public class CompileDelphiProject : ICompileDelphiProject {
        private readonly string delphiPath;

        private string fileDprProject;
        private string searchPath;
        private string tempDirectory;
        private string binPath;

        public CompileDelphiProject(string delphiPath) {
            this.delphiPath = delphiPath;
        }

        public ICompileDelphiProject ProjectFile(string fileName) {
            if (!File.Exists(fileName))
                throw new FileNotFoundException(fileName);

            fileDprProject = fileName;
            return this;
        }

        public ICompileDelphiProject SearchPath(string path) {
            searchPath = path;
            return this;
        }

        public ICompileDelphiProject BinPath(string path) {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            binPath = path;
            return this;
        }

        public ICompileDelphiProject TempDirectory(string directory) {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);

            Directory.CreateDirectory(directory);

            tempDirectory = directory;
            return this;
        }

        public void Build(ICompressExecutable compressExecutable = null) {
            var fileNameProject = Path.GetFileName(fileDprProject);
            var argumentsProcessCompile = $"\"{fileNameProject}\" -u\"{searchPath}\" -N\"{tempDirectory}\" -Q";

            var process = new Process {
                StartInfo = {
                    FileName = delphiPath,
                    Arguments = argumentsProcessCompile,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(fileDprProject)
                }
            };

            process.OutputDataReceived += ProcessConsoleLog;
            process.ErrorDataReceived += ProcessConsoleLog;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            compressExecutable?.Do(Path.Combine(binPath, Path.ChangeExtension(fileNameProject, "exe")));
        }

        private void ProcessConsoleLog(object sender, DataReceivedEventArgs e) {
            Console.WriteLine(e.Data);
        }
    }
}
