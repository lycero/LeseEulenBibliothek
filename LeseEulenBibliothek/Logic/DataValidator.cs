using LeseEulenBibliothek.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeseEulenBibliothek.Logic
{
    public static class DataValidator
    {
        public static void ValidateCollection(FolderCollection data)
        {
            ValidateIndices(data.Collection);
            foreach (var folder in data.Collection)
                ValidateIndices(folder.FileEntries);
        }

        private static void ValidateIndices<T>(IEnumerable<T> entries) where T : IIndexedEntry
        {
            var usedNumbers = new HashSet<int>();
            var repairEntries = new List<T>();
            foreach (var entry in entries)
            {
                if (!entry.IsValidIndex() || !usedNumbers.Add(entry.IndexNumber))
                    repairEntries.Add(entry);
            }

            int GetNextFreeNumber(T entry)
            {
                for (int i = entry.LowerLimit; i <= entry.UpperLimit; i++)
                    if (usedNumbers.Add(i))
                        return i;
                return -1;
            }

            foreach (var entry in repairEntries)
                entry.IndexNumber = GetNextFreeNumber(entry);
        }
    }
}
