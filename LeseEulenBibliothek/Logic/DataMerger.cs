using LeseEulenBibliothek.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeseEulenBibliothek.Logic
{
    public static class DataMerger
    {
        public static void MergeData(FolderCollection mainData, List<FolderData> mergeData)
        {
            var existingFolders = mainData.Collection.Select(f => f.FolderPath).ToHashSet();
            var newFolders = mergeData.Select(f => f.FolderPath).ToHashSet();
            foreach (var data in mergeData.Where(f => !existingFolders.Contains(f.FolderPath)))
                mainData.Collection.Add(data);
            foreach (var data in mergeData.Where(f => existingFolders.Contains(f.FolderPath)))            
                MergeFolderData(mainData.Collection.FirstOrDefault(f => f.FolderPath == data.FolderPath), data);
            foreach (var removedFolder in existingFolders.Where(f => !newFolders.Contains(f)))
                mainData.Collection.Remove(mainData.Collection.FirstOrDefault(f => f.FolderPath == removedFolder));
        }

        private static void MergeFolderData(FolderData mainData, FolderData mergeData)
        {
            var existingFiles = mainData.FileEntries.Select(f => f.OriginalTitle).ToHashSet();
            var usedIndices = mainData.FileEntries.Select(f => f.IndexNumber).ToHashSet();
            var newFiles = mergeData.FileEntries.Select(f => f.OriginalTitle).ToHashSet();
            foreach (var data in mergeData.FileEntries.Where(f => !existingFiles.Contains(f.OriginalTitle)))
            {
                if(!usedIndices.Add(data.IndexNumber))
                    data.IndexNumber = -1;
                mainData.FileEntries.Add(data);
            }
            foreach (var removedFile in existingFiles.Where(f => !newFiles.Contains(f)))
                mainData.FileEntries.Remove(mainData.FileEntries.FirstOrDefault(f => f.OriginalTitle == removedFile));
            foreach (var data in mergeData.FileEntries.Where(f => existingFiles.Contains(f.OriginalTitle)))
            {
                var oldEntry = mainData.FileEntries.FirstOrDefault(f => f.OriginalTitle == data.OriginalTitle);
                if (oldEntry == null)
                    continue; // should not happen

                oldEntry.OriginalExtension = data.OriginalExtension;
                oldEntry.FileTime = data.FileTime;
            }
        }
    }
}
