using System.IO;
using CompileBatchOfProjectsDelphi;
using NSubstitute;
using NUnit.Framework;
using static System.Configuration.ConfigurationManager;

namespace Test.CompileBatchOfProjectsDelphi {
    [TestFixture]
    public class CompileExecutableTest {

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ThrowFileNotFouldWhenProjectFileNotExist() {
            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile("Resources\\Project1\\Project_NOT_EXIST.dpr");
        }

        [Test]
        public void ShouldBuildExecutableFile() {
            const string fileExecutable = "Resources\\Project1\\Project1.exe";

            if (File.Exists(fileExecutable))
                File.Delete(fileExecutable);

            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile(Path.GetFullPath("Resources\\Project1\\Project1.dpr"))
                                                           .Build();
            Assert.That(File.Exists(fileExecutable));
        }

        [Test]
        public void ShouldBuildExecutableFileUsingSearchPath() {
            const string fileExecutable = "Resources\\Project2\\Project1.exe";

            if (File.Exists(fileExecutable))
                File.Delete(fileExecutable);

            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile(Path.GetFullPath("Resources\\Project2\\Project1.dpr"))
                                                           .SearchPath("FolderSearch")
                                                           .Build();
            Assert.That(File.Exists(fileExecutable));
        }

        [Test]
        public void ShouldUseTempFolderSpecified() {
            const string tempfolder = "TempFolder";

            if (Directory.Exists(tempfolder)) {
                Directory.Delete(tempfolder, true);
            }

            Directory.CreateDirectory(tempfolder);

            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile(Path.GetFullPath("Resources\\Project1\\Project1.dpr"))
                                                           .TempDirectory(Path.GetFullPath(tempfolder))
                                                           .Build();

            Assert.That(Directory.GetFiles(tempfolder).Length > 0);
        }

        [Test]
        public void IfTempFolderNotexistShouldCreateIt() {
            const string tempfolder = "TempFolderNotExist";

            if (Directory.Exists(tempfolder)) {
                Directory.Delete(tempfolder, true);
            }

            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile(Path.GetFullPath("Resources\\Project1\\Project1.dpr"))
                                                           .TempDirectory(Path.GetFullPath(tempfolder))
                                                           .Build();

            Assert.That(Directory.Exists(tempfolder));
        }

        [Test]
        public void ShouldDeleteFolderBeforeExecuteBuild() {
            const string tempfolder = "TempFolderNewBuild";

            if (Directory.Exists(tempfolder)) {
                Directory.Delete(tempfolder, true);
            }

            Directory.CreateDirectory(tempfolder);

            var dateBefore = Directory.GetCreationTime(tempfolder);

            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile(Path.GetFullPath("Resources\\Project1\\Project1.dpr"))
                                                           .TempDirectory(Path.GetFullPath(tempfolder))
                                                           .Build();

            var dateAfter = Directory.GetCreationTime(tempfolder);

            Assert.That(dateAfter, Is.GreaterThan(dateBefore));
        }

        [Test]
        public void ShouldBuildExecutableFileToOutpurPathInProject() {
            const string fileExecutable = "Resources\\Project3\\bin\\Project1.exe";

            if (File.Exists(fileExecutable))
                File.Delete(fileExecutable);

            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile(Path.GetFullPath("Resources\\Project3\\Project1.dpr"))
                                                           .Build();
            Assert.That(File.Exists(fileExecutable));
        }

        [Test]
        public void ShouldCreateBinFolderIfNotExist() {
            const string folder = "NEW_FODLER_BIN";

            if (Directory.Exists(folder))
                Directory.Delete(folder, true);

            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile(Path.GetFullPath("Resources\\Project3\\Project1.dpr"))
                                                           .BinPath(folder)
                                                           .Build();

            Assert.That(Directory.Exists("Resources\\Project3\\NEW_FODLER_BIN"));
        }

        [Test]
        public void ShouldCallCompress() {
            const string fileExecutable = "Resources\\Project1\\Project1.exe";

            if (File.Exists(fileExecutable))
                File.Delete(fileExecutable);

            var compressExecutable = Substitute.For<ICompressExecutable>();

            var fileDprProject = Path.GetFullPath("Resources\\Project1\\Project1.dpr");

            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile(fileDprProject)
                                                           .Build(compressExecutable);

            compressExecutable.Received(1)
                              .Do(Path.GetFullPath( Path.GetDirectoryName(fileExecutable)), "Project1.exe");
        }

        [Test]
        public void ShouldCallCompressWithBinPath() {
            const string fileExecutable = "Resources\\Project3\\bin\\Project1.exe";

            if (File.Exists(fileExecutable))
                File.Delete(fileExecutable);

            var compressExecutable = Substitute.For<ICompressExecutable>();

            var fileDprProject = Path.GetFullPath("Resources\\Project3\\Project1.dpr");

            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile(fileDprProject)
                                                           .BinPath("bin")
                                                           .Build(compressExecutable);

            compressExecutable.Received(1)
                              .Do(Path.GetFullPath("Resources\\Project3\\bin"), "Project1.exe");
        }

        [Test]
        public void ShouldBuildExecutableFileWithVersionInFileName() {
            const string fileExecutable = "Resources\\Project1\\Project1.exe";
            const string executableWithVersion = "Resources\\Project1\\Project1 v1.2.3.4.exe";

            if (File.Exists(fileExecutable))
                File.Delete(fileExecutable);

            if (File.Exists(executableWithVersion))
                File.Delete(executableWithVersion);

            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile(Path.GetFullPath("Resources\\Project1\\Project1.dpr"))
                                                           .MakeCopyFileWithVersion()
                                                           .Build();

            Assert.That(File.Exists(executableWithVersion));
        }

        [Test]
        public void ShouldBuildExecutableFileWithVersionInFileNameInBinPath() {
            const string fileExecutable = "Resources\\bin\\Project1.exe";
            const string executableWithVersion = "Resources\\bin\\Project1 v1.2.3.4.exe";

            if (File.Exists(fileExecutable))
                File.Delete(fileExecutable);

            if (File.Exists(executableWithVersion))
                File.Delete(executableWithVersion);

            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile(Path.GetFullPath("Resources\\Project1\\Project1.dpr"))
                                                           .MakeCopyFileWithVersion()
                                                           .BinPath("..\\bin")
                                                           .Build();

            Assert.That(File.Exists(executableWithVersion));
        }
    }
}
