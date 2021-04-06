using LeseEulenBibliothek.Core;
using LeseEulenBibliothek.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeseEulenBibliothek.Logic
{
    public class FormatConverter
    {
        private readonly ConfigData m_Config;
        private readonly ProgressInfo m_ProgressInfo;
        private readonly SemaphoreSlim m_ConvertGate;

        public FormatConverter(ConfigData config, Core.ProgressInfo progressInfo)
        {
            this.m_Config = config;
            this.m_ProgressInfo = progressInfo;
            m_ConvertGate = new SemaphoreSlim(config.MaxConverterCount, config.MaxConverterCount);
        }

        public async Task ConvertFilesAsync(FolderCollection data)
        {
            m_ProgressInfo.TaskText = TranslationService.GetTranslation(TranslationTexts.Progress.Step_Convert);
            foreach (var folder in data.FolderEntries)
                await ConvertFilesAsync(folder);
        }
        public async Task ConvertFilesAsync(FolderData data)
        {
            if (data == null)
                return;
            m_ProgressInfo.AddStep(data.FolderName);
            var tasks = new List<Task>();
            foreach (var file in data.EntryCollection)
                tasks.Add(Task.Run(() => ConvertFileAsync(Path.Combine(data.FolderPath, file.OriginalTitle), file.OriginalExtension)));
            await Task.WhenAll(tasks.ToArray());
            m_ProgressInfo.RemoveStep(data.FolderName);
        }

        private async Task ConvertFileAsync(string filePath, string originalExtension)
        {
            await m_ConvertGate.WaitAsync();
            m_ProgressInfo.AddStep(filePath);
            try
            {
                if (!m_Config.ExecuteConverterForTargetFormat && originalExtension.EndsWith("mp3"))
                    return;
                var fullFilePath = Path.Combine(m_Config.ArchiveFolder, filePath) + originalExtension;
                var newFileFullPath = Path.Combine(m_Config.ConvertFolder, filePath) + ".mp3";
                Directory.CreateDirectory(Path.GetDirectoryName(newFileFullPath));
                if (File.Exists(newFileFullPath) && File.GetLastWriteTime(fullFilePath) <= File.GetLastWriteTime(newFileFullPath))
                    return;
                string argument = string.Format(m_Config.ConverterArgument, fullFilePath, newFileFullPath);
                var startInfo = new ProcessStartInfo(m_Config.ConverterCommand, argument);
                startInfo.CreateNoWindow = true;
                var process = Process.Start(startInfo);
                await Task.Delay(100);
                if(!process.HasExited)
                    process.WaitForExit();
            }
            finally
            {
                m_ProgressInfo.RemoveStep(filePath);
                m_ConvertGate.Release();
            }
        }
    }
}
