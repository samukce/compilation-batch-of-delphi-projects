using System;
using System.Diagnostics;
using System.IO;

namespace CompileBatchOfProjectsDelphi {
    public class CompileDelphiProject : ICompileDelphiProject {
        private readonly string delphiPath;
        private readonly ProcessExecute processExecute;

        private string fileDprProject;
        private string searchPath;
        private string tempDirectory;
        private string binPath;


        public CompileDelphiProject(string delphiPath) {
            this.delphiPath = delphiPath;
            processExecute = new ProcessExecute();
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

            var workingDirectory = Path.GetDirectoryName(fileDprProject);

            processExecute.ExecuteProcess(delphiPath, argumentsProcessCompile, workingDirectory);

            compressExecutable?.Do(workingDirectory, Path.Combine(binPath, Path.ChangeExtension(fileNameProject, "exe")));
        }
    }
}
