using LeseEulenBibliothek.Core;
using LeseEulenBibliothek.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeseEulenBibliothek.Logic
{
    public class OutputCreator
    {
        private readonly ConfigData m_Config;
        private readonly ProgressInfo m_ProgressInfo;

        public OutputCreator(ConfigData config, Core.ProgressInfo progressInfo)
        {
            this.m_Config = config;
            this.m_ProgressInfo = progressInfo;
        }

        public async Task CreateOutputAsync(FolderCollection collection)
        {
            m_ProgressInfo.TaskText = "Create Output";

            Directory.CreateDirectory(m_Config.OutputFolder);
            var existingDirs = Directory.GetDirectories(m_Config.OutputFolder).ToList();

            foreach (var folder in collection.FolderEntries)
            {
                m_ProgressInfo.AddStep(folder.FolderName);
                var path = CreateFolder(folder);
                await SyncFilesAsync(path, folder);
                m_ProgressInfo.RemoveStep(folder.FolderName);
                existingDirs.Remove(path);
            }

            foreach (var oldFolder in existingDirs)
            {
                if (oldFolder.EndsWith("mp3", StringComparison.OrdinalIgnoreCase))
                    continue;
                if (oldFolder.EndsWith("advert", StringComparison.OrdinalIgnoreCase))
                    continue;

                var files = Directory.GetFiles(oldFolder);
                foreach (var file in files)                
                    File.Delete(file);                 
            }        
        }

        private async Task SyncFilesAsync(string targetFolderPath, FolderData folder)
        {
            await Task.Run(() =>
            {
                var oldFiles = Directory.GetFiles(targetFolderPath).ToList();
                foreach (var file in folder.FileEntries)
                {
                    try
                    {
                        m_ProgressInfo.AddStep(file.OriginalTitle);
                        var targetPath = Path.Combine(targetFolderPath, GetFileName(file.IndexNumber));
                        var sourcePath = GetSourcePath(file, folder);

                        var fiTarget = new FileInfo(targetPath);
                        var fiSource = new FileInfo(sourcePath);
                        oldFiles.Remove(targetPath);
                        if (fiTarget.Exists && fiSource.Exists && fiTarget.Length == fiSource.Length && fiSource.LastWriteTime <= fiTarget.LastWriteTime)
                            continue;
                        if (fiSource.Exists)
                            File.Copy(sourcePath, targetPath, true);
                    }
                    finally
                    {
                        m_ProgressInfo.RemoveStep(file.OriginalTitle);
                    }
                }
                foreach (var file in oldFiles)
                    File.Delete(file);
            });
        }

        private string GetSourcePath(FolderDataEntry file, FolderData folder)
        {
            if (!m_Config.ExecuteConverterForTargetFormat && file.OriginalExtension == ".mp3")
                return Path.Combine(m_Config.ArchiveFolder, folder.FolderPath, file.OriginalTitle + file.OriginalExtension);
            return Path.Combine(m_Config.ConvertFolder, folder.FolderPath, file.OriginalTitle + ".mp3"); 
        }

        private string GetFileName(int indexNumber)
        {
            return $"{indexNumber:D3}.mp3";
        }

        private string CreateFolder(FolderData folder)
        {
            var path = Path.Combine(m_Config.OutputFolder, GetFolderName(folder.IndexNumber));
            Directory.CreateDirectory(path);
            return path;
        }

        private string GetFolderName(int indexNumber)
        {
            return $"{indexNumber:D2}";
        }
    }
}
