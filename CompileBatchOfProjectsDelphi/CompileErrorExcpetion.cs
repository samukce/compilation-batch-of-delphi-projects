using System;

namespace CompileBatchOfProjectsDelphi {
    public class CompileErrorExcpetion : Exception {
        public CompileErrorExcpetion(Exception pee) : base(pee.Message, pee) {
        }
    }
}
