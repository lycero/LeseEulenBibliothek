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
        private readonly ProgressInfo m_ProgressInfo;
        private readonly Regex m_FileRegex = new Regex(@"(mp3|wav|ogg)$");

        public FolderScanner(string rootPath, ProgressInfo progressInfo)
        {
            this.m_RootPath = rootPath;
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
            result.FileEntries = new System.Collections.ObjectModel.ObservableCollection<FolderDataEntry>(files.Select(ScanFile));
            m_ProgressInfo.RemoveStep(folder);
            return result;
        }

        private bool FileMatch(string filename)
        {
            return m_FileRegex.IsMatch(filename);
        }

        private FolderDataEntry ScanFile(string filename, int index)
        {
            return new FolderDataEntry()
            {
                IndexNumber = index + 1,
                OriginalTitle = Path.GetFileNameWithoutExtension(filename),
                OriginalExtension = Path.GetExtension(filename),
                FileTime = File.GetLastWriteTime(filename)
            };
        }
    }
}
