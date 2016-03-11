namespace CompileBatchOfProjectsDelphi {
    public interface ICompileDelphiProject {
        void Build();
        ICompileDelphiProject ProjectFile(string fileName);
        ICompileDelphiProject SearchPath(string path);
        ICompileDelphiProject TempDirectory(string directory);
    }
}