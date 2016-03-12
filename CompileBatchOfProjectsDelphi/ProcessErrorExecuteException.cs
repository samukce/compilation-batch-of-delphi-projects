using System;

namespace CompileBatchOfProjectsDelphi {
    public class ProcessErrorExecuteException : Exception {
        public ProcessErrorExecuteException(string command, string arguments)
            : base($"Error command [{command} {arguments}]") {
        }
    }
}
