using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LeseEulenBibliothek.Data;
using LeseEulenBibliothek.Logic;
using NUnit;
using NUnit.Framework;

namespace LeseEulenBibliothek.Tests
{
    [TestFixture]
    public class Test_Scanner
    {
        private string m_MainFolder;
        private string m_Folder;

        [OneTimeSetUp]
        public void SetupFiles()
        {
            m_MainFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            m_Folder = Path.Combine(m_MainFolder, "Test03");
            Directory.CreateDirectory(m_Folder);
            File.Create(Path.Combine(m_Folder, "A_01.ogg"), 1, FileOptions.Asynchronous);
            File.Create(Path.Combine(m_Folder, "B_05.ogg"), 1, FileOptions.Asynchronous);
            File.Create(Path.Combine(m_Folder, "C_02.ogg"), 1, FileOptions.Asynchronous);
            File.Create(Path.Combine(m_Folder, "D_006.ogg"), 1, FileOptions.Asynchronous);
            File.Create(Path.Combine(m_Folder, "E_05.ogg"), 1, FileOptions.Asynchronous);
            File.Create(Path.Combine(m_Folder, "F_15.ogg"), 1, FileOptions.Asynchronous);
            File.Create(Path.Combine(m_Folder, "G_8.ogg"), 1, FileOptions.Asynchronous);
        }

        [OneTimeTearDown]
        public void DeleteFiles()
        {
            Directory.Delete(m_MainFolder, true);
        }

        [Test, Timeout(1000)]
        public async Task TestScanWithRegex()
        {
            var config = new ConfigData();
            var scanner = new FolderScanner(m_MainFolder, config.IndexRecognitionRegex, new Core.ProgressInfo());
            var result = await scanner.ScanFolderAsync();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3, result[0].IndexNumber);
            Assert.AreEqual(7, result[0].FileEntries.Count);
            Assert.AreEqual(1, result[0].FileEntries[0].IndexNumber);
            Assert.AreEqual(5, result[0].FileEntries[1].IndexNumber);
            Assert.AreEqual(2, result[0].FileEntries[2].IndexNumber);
            Assert.AreEqual(6, result[0].FileEntries[3].IndexNumber);
            Assert.AreEqual(3, result[0].FileEntries[4].IndexNumber);
            Assert.AreEqual(15, result[0].FileEntries[5].IndexNumber);
            Assert.AreEqual(4, result[0].FileEntries[6].IndexNumber);
        }

        [Test, Timeout(1000)]
        public async Task TestScanNormal()
        {
            var scanner = new FolderScanner(m_MainFolder, "", new Core.ProgressInfo());
            var result = await scanner.ScanFolderAsync();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(-1, result[0].IndexNumber);
            Assert.AreEqual(7, result[0].FileEntries.Count);
            Assert.AreEqual(1, result[0].FileEntries[0].IndexNumber);
            Assert.AreEqual(2, result[0].FileEntries[1].IndexNumber);
            Assert.AreEqual(3, result[0].FileEntries[2].IndexNumber);
            Assert.AreEqual(4, result[0].FileEntries[3].IndexNumber);
            Assert.AreEqual(5, result[0].FileEntries[4].IndexNumber);
            Assert.AreEqual(6, result[0].FileEntries[5].IndexNumber);
            Assert.AreEqual(7, result[0].FileEntries[6].IndexNumber);
        }
    }
}
