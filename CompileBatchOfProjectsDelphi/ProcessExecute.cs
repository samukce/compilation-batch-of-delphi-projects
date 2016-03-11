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
        }

        private void ProcessConsoleLog(object sender, DataReceivedEventArgs e) {
            Console.WriteLine(e.Data);
        }
    }
}