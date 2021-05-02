using LeseEulenBibliothek.Core;
using LeseEulenBibliothek.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeseEulenBibliothek.Logic
{
    public class FolderScanner
    {
        public const string FileFilter = "*.*";
        private readonly string m_RootPath;
        private readonly Regex? m_IndexRecognitionRegex;
        private readonly ProgressInfo m_ProgressInfo;
        private readonly Regex m_FileRegex = new Regex(@"(mp3|wav|ogg)$");

        public FolderScanner(string rootPath, string indexRecognitionPattern, ProgressInfo progressInfo)
        {
            this.m_RootPath = rootPath;
            this.m_IndexRecognitionRegex = string.IsNullOrEmpty(indexRecognitionPattern) ? null : new Regex(indexRecognitionPattern);
            this.m_ProgressInfo = progressInfo;
        }

        public async Task<List<FolderData>> ScanFolderAsync()
        {
            if (!Directory.Exists(m_RootPath))
                return new List<FolderData>();
            m_ProgressInfo.TaskText = TranslationService.GetTranslation(TranslationTexts.Progress.Step_Scan);
            m_ProgressInfo.AddStep(m_RootPath);
            var result = await Task.Run(() =>
            {
                return Directory.EnumerateDirectories(m_RootPath, "*", new EnumerationOptions() { RecurseSubdirectories = true, IgnoreInaccessible = true }).Select(ScanSubFolder).Where(f => f.FileEntries.Count > 0).ToList();
            });
            m_ProgressInfo.RemoveStep(m_RootPath);
            return result;
        }

        private FolderData ScanSubFolder(string folder)
        {
            m_ProgressInfo.AddStep(folder);
            var files = Directory.GetFiles(folder, FileFilter, SearchOption.TopDirectoryOnly).Where(FileMatch);
            if (!files.Any())
                return new FolderData();
            var result = new FolderData();
            result.FolderPath = Path.GetRelativePath(m_RootPath, folder);
            result.FolderName = Path.GetFileName(folder) ?? "";
            result.IndexNumber = GetNextIndex(result.FolderName, new HashSet<int>());
            var indexSet = new HashSet<int>();
            result.FileEntries = new System.Collections.ObjectModel.ObservableCollection<FolderDataEntry>(files.Select(f => ScanFile(f,indexSet)));
            UpdateIndices(result.FileEntries, indexSet);
            m_ProgressInfo.RemoveStep(folder);
            return result;
        }

        private void UpdateIndices(IEnumerable<FolderDataEntry> entries, HashSet<int> usedIndices)
        {
            var nextIndex = 1;
            foreach (var entry in entries)
            {
                if (entry.IndexNumber > -1)
                    continue;
                entry.IndexNumber = GetNextFreeIndex(nextIndex, usedIndices);
                nextIndex = entry.IndexNumber + 1;
            }
        }

        private int GetNextFreeIndex(int currentIndex, HashSet<int> usedIndices)
        {
            var index = currentIndex;
            while (!usedIndices.Add(index))
                index++;
            return index;
        }

        private bool FileMatch(string filename)
        {
            return m_FileRegex.IsMatch(filename);
        }

        private FolderDataEntry ScanFile(string filename, HashSet<int> indices)
        {
            var title = Path.GetFileNameWithoutExtension(filename);
            return new FolderDataEntry()
            {
                OriginalTitle = title,
                IndexNumber = GetNextIndex(title, indices),
                OriginalExtension = Path.GetExtension(filename),
                FileTime = File.GetLastWriteTime(filename)
            };
        }
        private int GetNextIndex(string name, HashSet<int> indices)
        {
            if (m_IndexRecognitionRegex == null)
                return -1;
            var match = m_IndexRecognitionRegex.Match(name);
            if (!match.Success || match.Captures.Count < 1)
                return -1;
            if (!int.TryParse(match.Captures[0].Value, out var nexIndex))
                return -1;
            if (!indices.Add(nexIndex))
                return -1;
            return nexIndex;            
        }
    }
}
