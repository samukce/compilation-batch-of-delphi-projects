namespace CompileBatchOfProjectsDelphi {
    public class UpxCompress : ICompressExecutable {
        private readonly ProcessExecute processExecute;
        private const string UpxProcess = "upx.exe";

        public UpxCompress() {
            processExecute = new ProcessExecute();
        }

        public void Do(string workingDirectory, string pathExecutable) {
            var argumentsProcessCompile = $"\"{pathExecutable}\" ";

            processExecute.ExecuteProcess(UpxProcess, argumentsProcessCompile, workingDirectory);
        }
    }
}
