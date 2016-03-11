namespace CompileBatchOfProjectsDelphi {
    public interface ICompileDelphiProject {
        void Build(ICompressExecutable compressExecutable = null);
        ICompileDelphiProject ProjectFile(string fileName);
        ICompileDelphiProject SearchPath(string path);
        ICompileDelphiProject TempDirectory(string directory);
        ICompileDelphiProject BinPath(string path);
    }
}