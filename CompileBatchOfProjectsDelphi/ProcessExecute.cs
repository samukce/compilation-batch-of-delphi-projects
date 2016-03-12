using System;
using System.Diagnostics;

namespace CompileBatchOfProjectsDelphi {
    public class ProcessExecute {
        public ProcessExecute() {
        }

        public void ExecuteProcess(string fileExecute, string argumentsProcessCompile, string workingDirectory) {
            var process = new Process {
                StartInfo = {
                    FileName = fileExecute,
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

            if (process.ExitCode == 0) return;

            throw new ProcessErrorExecuteException(fileExecute, argumentsProcessCompile);
        }

        private void ProcessConsoleLog(object sender, DataReceivedEventArgs e) {
            Console.WriteLine(e.Data);
        }
    }
}