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
        private bool makeCopyFileWithVersion;

        public CompileDelphiProject(string delphiPath) {
            this.delphiPath = delphiPath;
            processExecute = new ProcessExecute();
        }

        public ICompileDelphiProject MakeCopyFileWithVersion() {
            makeCopyFileWithVersion = true;
            return this;
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
            var workingDirectory = Path.GetDirectoryName(fileDprProject);

            var argumentOutPut = string.Empty;
            if (binPath != null)
                argumentOutPut = $"-E\"{binPath}\"";

            if (binPath == null) {
                binPath = workingDirectory;
            } else {
                var pathCombineBinAndWorkDirectory = Path.Combine(workingDirectory, binPath);

                if (!Directory.Exists(pathCombineBinAndWorkDirectory)) {
                    Directory.CreateDirectory(pathCombineBinAndWorkDirectory);
                }
                binPath = pathCombineBinAndWorkDirectory;
            }

            var argumentsProcessCompile = $"\"{fileNameProject}\" -u\"{searchPath}\" -N\"{tempDirectory}\" -Q  {argumentOutPut}";

            try {
                processExecute.ExecuteProcess(delphiPath, argumentsProcessCompile, workingDirectory);
            } catch (ProcessErrorExecuteException) {

                throw new CompileErrorExcpetion();
            }

            var pathExecutable = CompressExecutable(compressExecutable, fileNameProject, binPath);

            CopyFileWithVersion(pathExecutable, fileNameProject);
        }

        private void CopyFileWithVersion(string pathExecutable, string fileNameProject) {
            if (!makeCopyFileWithVersion) return;

            var versionInfo = FileVersionInfo.GetVersionInfo(pathExecutable);
            var newNameExecutable = string.Format("{0} v{1}.{2}.{3}.{4}.exe",
                Path.GetFileNameWithoutExtension(fileNameProject),
                versionInfo.FileMajorPart,
                versionInfo.FileMinorPart,
                versionInfo.FileBuildPart,
                versionInfo.FilePrivatePart);

            File.Copy(pathExecutable, Path.Combine(binPath, newNameExecutable), true);
        }

        private string CompressExecutable(ICompressExecutable compressExecutable, string fileNameProject, string directoryOutput) {
            var executeName = Path.ChangeExtension(fileNameProject, "exe");

            compressExecutable?.Do(directoryOutput, executeName);

            return Path.Combine(directoryOutput, executeName);
        }
    }
}
