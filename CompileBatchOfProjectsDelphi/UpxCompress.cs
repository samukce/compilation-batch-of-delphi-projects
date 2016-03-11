using System;
using System.Diagnostics;

namespace CompileBatchOfProjectsDelphi {
    public class UpxCompress : ICompressExecutable {
        private const string UpxProcess = "upx.exe";

        public void Do(string workingDirectory, string pathExecutable) {
            var argumentsProcessCompile = $"\"{pathExecutable}\" ";

            var process = new Process {
                StartInfo = {
                    FileName = UpxProcess,
                    Arguments = argumentsProcessCompile,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory
                }
            };

            process.OutputDataReceived += ProcessConsoleLog;
            process.ErrorDataReceived += ProcessConsoleLog;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
        }

        private void ProcessConsoleLog(object sender, DataReceivedEventArgs e) {
            Console.WriteLine(e.Data);
        }
    }
}
