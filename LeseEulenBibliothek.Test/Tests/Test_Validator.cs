using LeseEulenBibliothek.Data;
using LeseEulenBibliothek.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeseEulenBibliothek.Tests
{
    [TestFixture]
    public class Test_Validator
    {
        [Test]
        public void Test_RepairFolder()
        {
            var collection = new FolderCollection();
            var folder1 = new FolderData() { FolderName = "fol1", IndexNumber = 1, FolderPath = "..." };
            var folder2 = new FolderData() { FolderName = "fol2", IndexNumber = 2, FolderPath = "..." };
            var folder3 = new FolderData() { FolderName = "fol3", IndexNumber = 1, FolderPath = "..." };
            var folder4 = new FolderData() { FolderName = "fol4", IndexNumber = 2, FolderPath = "..." };
            var folder5 = new FolderData() { FolderName = "fol5", IndexNumber = 4, FolderPath = "..." };
            collection.FolderEntries.Add(folder1);
            collection.FolderEntries.Add(folder2);
            collection.FolderEntries.Add(folder3);
            collection.FolderEntries.Add(folder4);
            collection.FolderEntries.Add(folder5);
            Assert.AreEqual(1, folder3.IndexNumber);
            Assert.AreEqual(2, folder4.IndexNumber);
            Assert.AreEqual(4, folder5.IndexNumber);
            DataValidator.ValidateCollection(collection);
            Assert.AreEqual(1, folder1.IndexNumber);
            Assert.AreEqual(2, folder2.IndexNumber);
            Assert.AreEqual(3, folder3.IndexNumber);
            Assert.AreEqual(5, folder4.IndexNumber);
            Assert.AreEqual(4, folder5.IndexNumber);
        }

        [Test]
        public void Test_RepairEntries()
        {
            var collection = new FolderCollection();
            var folder1 = new FolderData() { FolderName = "fol1", IndexNumber = 1, FolderPath = "..." };
            collection.FolderEntries.Add(folder1);

            var file1 = new FolderDataEntry() { IndexNumber = 1, OriginalTitle = "file1" };
            var file2 = new FolderDataEntry() { IndexNumber = 1, OriginalTitle = "file2" };
            var file3 = new FolderDataEntry() { IndexNumber = 2, OriginalTitle = "file3" };
            var file4 = new FolderDataEntry() { IndexNumber = 3, OriginalTitle = "file4" };
            var file5 = new FolderDataEntry() { IndexNumber = -1, OriginalTitle = "file5" };
            folder1.FileEntries.Add(file1);
            folder1.FileEntries.Add(file2);
            folder1.FileEntries.Add(file3);
            folder1.FileEntries.Add(file4);
            folder1.FileEntries.Add(file5);
            Assert.AreEqual(1, file1.IndexNumber);
            Assert.AreEqual(2, file3.IndexNumber);
            Assert.AreEqual(-1, file5.IndexNumber);
            DataValidator.ValidateCollection(collection);
            Assert.AreEqual(1, file1.IndexNumber);
            Assert.AreEqual(4, file2.IndexNumber);
            Assert.AreEqual(2, file3.IndexNumber);
            Assert.AreEqual(3, file4.IndexNumber);
            Assert.AreEqual(5, file5.IndexNumber);
        }
    }
}
