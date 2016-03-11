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
                                                           .BinPath(folder);

            Assert.That(Directory.Exists(folder));
        }

        [Test]
        public void ShouldCallCompress() {
            const string fileExecutable = "Resources\\Project3\\bin\\Project1.exe";

            if (File.Exists(fileExecutable))
                File.Delete(fileExecutable);

            var compressExecutable = Substitute.For<ICompressExecutable>();

            new CompileDelphiProject(AppSettings["DELPHI"]).ProjectFile(Path.GetFullPath("Resources\\Project3\\Project1.dpr"))
                                                           .BinPath("Resources\\Project3\\bin\\")
                                                           .Build(compressExecutable);

            compressExecutable.Received(1)
                              .Do(fileExecutable);
        }
    }
}
