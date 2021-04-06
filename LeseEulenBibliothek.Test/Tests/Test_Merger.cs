using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeseEulenBibliothek.Data;
using LeseEulenBibliothek.Logic;
using NUnit;
using NUnit.Framework;

namespace LeseEulenBibliothek.Tests
{
    [TestFixture]
    public class Test_Merger
    {
        private FolderCollection CreateBasicTestSetup(DateTime fileChangeTime)
        {
            var fileExtension = ".mp3";
            var result = new FolderCollection();
            var folder1 = new FolderData() { IndexNumber = 1, FolderName = "TestFolder1", FolderPath = "test/folder1" };
            folder1.EntryCollection.Add(new FolderDataEntry() { IndexNumber = 1 , OriginalTitle = "file1", FileTime = fileChangeTime, OriginalExtension = fileExtension });
            folder1.EntryCollection.Add(new FolderDataEntry() { IndexNumber = 2 , OriginalTitle = "file2", FileTime = fileChangeTime, OriginalExtension = fileExtension });
            folder1.EntryCollection.Add(new FolderDataEntry() { IndexNumber = 3 , OriginalTitle = "file3", FileTime = fileChangeTime, OriginalExtension = fileExtension });
            folder1.EntryCollection.Add(new FolderDataEntry() { IndexNumber = 4 , OriginalTitle = "file4", FileTime = fileChangeTime, OriginalExtension = fileExtension });
            folder1.EntryCollection.Add(new FolderDataEntry() { IndexNumber = 6 , OriginalTitle = "file6", FileTime = fileChangeTime, OriginalExtension = fileExtension });
            folder1.EntryCollection.Add(new FolderDataEntry() { IndexNumber = 7, OriginalTitle = "_file7_", FileTime = fileChangeTime, OriginalExtension = fileExtension });
            folder1.EntryCollection.Add(new FolderDataEntry() { IndexNumber = 8, OriginalTitle = "a_file8", FileTime = fileChangeTime, OriginalExtension = fileExtension });
            var folder3 = new FolderData() { IndexNumber = 3, FolderName = "TestFolder3", FolderPath = "test/folder3" };
            folder3.EntryCollection.Add(new FolderDataEntry() { IndexNumber = 3, OriginalTitle = "file3", FileTime = fileChangeTime, OriginalExtension = fileExtension });
            folder3.EntryCollection.Add(new FolderDataEntry() { IndexNumber = 4, OriginalTitle = "file4", FileTime = fileChangeTime, OriginalExtension = fileExtension });
            result.FolderEntries.Add(folder1);
            result.FolderEntries.Add(folder3);
            return result;
        }

        private FolderData CreateNewFolderData(int index, string fileNamePrefix, string extension, DateTime fileChangeTime, params int[] newFiles)
        {
            var result = new FolderData() { IndexNumber = index };            
            foreach (var entry in CreateNewFolderDataEntries(fileNamePrefix, extension, fileChangeTime, newFiles))            
                result.EntryCollection.Add(entry);            
            return result;
        }

        private IEnumerable<FolderDataEntry> CreateNewFolderDataEntries(string fileNamePrefix, string extension, DateTime fileChangeTime, params int[] newFiles)
        {
            var result = new List<FolderDataEntry>();
            foreach (var file in newFiles)
                result.Add(new FolderDataEntry() { IndexNumber = file, OriginalTitle = $"{fileNamePrefix}{file}", FileTime = fileChangeTime, OriginalExtension = extension });
            return result;
        }

        [Test]
        public void Test_MergeFolder()
        {
            var createTime = DateTime.Now - new TimeSpan(10000);
            var mainData = CreateBasicTestSetup(createTime);

            var folder1 = mainData.FolderEntries.FirstOrDefault(f => f.IndexNumber == 1);
            Assert.IsNotNull(folder1);
            Assert.AreEqual(7, folder1.EntryCollection.Count);
            Assert.AreEqual(2, mainData.FolderEntries.Count);

            var newData = CreateNewFolderData(1, "a_new_file", ".mp3", DateTime.Now, 1, 2, 3, 4, 5, 6);
            var newData1a = CreateNewFolderDataEntries("file", ".mp3", createTime, 1, 2, 3, 4, 5, 6);
            foreach (var entry in newData1a)
            {
                entry.IndexNumber = newData.FileEntries.Count;
                newData.FileEntries.Add(entry);
            }
            newData.FolderPath = "test/folder1";

            var newData2 = CreateNewFolderData(3, "file", ".mp3", DateTime.Now, 1,2,3,4);
            newData2.FolderPath = "test/folder3";

            DataMerger.MergeData(mainData, new List<FolderData>() { newData, newData2 });
            Assert.AreEqual(2, mainData.FolderEntries.Count);
            folder1 = mainData.FolderEntries.FirstOrDefault(f => f.IndexNumber == 1);
            Assert.IsNotNull(folder1);
            Assert.AreEqual(12, folder1.EntryCollection.Count);
            var secondFile = folder1.FileEntries[1];
            Assert.AreEqual(2, secondFile.IndexNumber);
            Assert.AreEqual("file2", secondFile.OriginalTitle);
            Assert.AreEqual(createTime, secondFile.FileTime);
            var folder3 = mainData.FolderEntries.FirstOrDefault(f => f.IndexNumber == 3);
            Assert.IsNotNull(folder3);
            Assert.AreEqual(4, folder3.EntryCollection.Count);
        }

        [Test]
        public void Test_AddFolder()
        {
            var newData = new List<FolderData>();
            Assert.IsTrue(true);
        }

        [Test]
        public void Test_FileTimeChanges()
        {
            var mainData = CreateBasicTestSetup(DateTime.Now - new TimeSpan(10000));
            var newData2 = CreateNewFolderData(1, "file", ".mp3", DateTime.Now, 1, 2, 3, 4);
            var firstFolder = mainData.FolderEntries.FirstOrDefault();
            newData2.FolderName = firstFolder.FolderName;
            newData2.FolderPath = firstFolder.FolderPath;

            Assert.AreNotEqual(firstFolder.FileEntries.FirstOrDefault().FileTime, newData2.FileEntries.FirstOrDefault().FileTime);
            DataMerger.MergeData(mainData, new List<FolderData>() { newData2 });
            Assert.AreEqual(firstFolder.FileEntries.FirstOrDefault().FileTime, newData2.FileEntries.FirstOrDefault().FileTime);
        }

        [Test]
        public void Test_FileExtensionChanges()
        {
            var mainData = CreateBasicTestSetup(DateTime.Now - new TimeSpan(10000));
            var newData2 = CreateNewFolderData(1, "file", ".wav", DateTime.Now, 1, 2, 3, 4);
            var firstFolder = mainData.FolderEntries.FirstOrDefault();
            newData2.FolderName = firstFolder.FolderName;
            newData2.FolderPath = firstFolder.FolderPath;

            Assert.AreNotEqual(firstFolder.FileEntries.FirstOrDefault().FileTime, newData2.FileEntries.FirstOrDefault().FileTime);
            Assert.AreNotEqual(firstFolder.FileEntries.FirstOrDefault().OriginalExtension, newData2.FileEntries.FirstOrDefault().OriginalExtension);
            Assert.AreEqual(firstFolder.FileEntries.FirstOrDefault().IndexNumber, newData2.FileEntries.FirstOrDefault().IndexNumber);
            DataMerger.MergeData(mainData, new List<FolderData>() { newData2 });
            Assert.AreEqual(firstFolder.FileEntries.FirstOrDefault().FileTime, newData2.FileEntries.FirstOrDefault().FileTime);
            Assert.AreEqual(firstFolder.FileEntries.FirstOrDefault().OriginalExtension, newData2.FileEntries.FirstOrDefault().OriginalExtension);
            Assert.AreEqual(firstFolder.FileEntries.FirstOrDefault().IndexNumber, newData2.FileEntries.FirstOrDefault().IndexNumber);
        }
    }
}
