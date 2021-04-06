using LeseEulenBibliothek.Core;
using LeseEulenBibliothek.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using LeseEulenBibliothek.Logic;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

namespace LeseEulenBibliothek.ViewModel
{
    public class MainViewModel : ObservableBase
    {
        private FolderCollection m_Data = new FolderCollection();
        private FolderData m_CurrentSelectedFolder = new FolderData();
        private bool m_ShowConfigWindow = false;

        public ProgressInfo ProgressInfo { get; } = new ProgressInfo();
#if DEBUG
        public Visibility VisibleInDebug  => Visibility.Visible;
#else
        public Visibility VisibleInDebug  => Visibility.Hidden;
#endif
        public FolderCollection Data { get => m_Data; set => Set(ref m_Data, value); }
        public bool IsShowConfig { get => m_ShowConfigWindow; set => Set(ref m_ShowConfigWindow, value); }
        
        public ConfigViewModel ConfigView { get; }

        public FolderData CurrentSelectedFolder { get => m_CurrentSelectedFolder; set => Set(ref m_CurrentSelectedFolder, value); }

        public ICommand SaveDataCommand { get; }
        public ICommand LoadDataCommand { get; }
        public ICommand ScanFolderCommand { get; }
        public ICommand ConvertFolderCommand { get; }
        public ICommand ValidateCollectionCommand { get; }
        public ICommand CreateOutputCommand { get; }
        public ICommand UpdateAllCommand { get; }

        public MainViewModel(string configFilePath)
        {
            ConfigView = new ConfigViewModel(configFilePath);
            SaveDataCommand = new CommandHandler(SaveData);
            LoadDataCommand = new CommandHandler(LoadData);
            ScanFolderCommand = new CommandHandler(ScanFolder);
            ConvertFolderCommand = new CommandHandler(ConvertFolder);
            ValidateCollectionCommand = new CommandHandler(ValidateCollection);
            CreateOutputCommand = new CommandHandler(CreateOutput);
            UpdateAllCommand = new CommandHandler(UpdateAll);
            LoadData(new object());
        }

        private ConfigData Config => ConfigView.Data;

        private void UpdateAll(object obj)
        {
            Task.Run(UpdateAllAsync);
        }
        private async Task UpdateAllAsync()
        {
            ProgressInfo.StartTask(TranslationService.GetTranslation(TranslationTexts.Progress.Step_Output));
            await ScanFolderAsync();
            ProgressInfo.ClearSteps();
            DataValidator.ValidateCollection(Data);
            SerializationHelper.SaveFileData(Config.DataFile, Data);
            var converter = new FormatConverter(Config, ProgressInfo);
            await converter.ConvertFilesAsync(Data);
            ProgressInfo.ClearSteps();
            await CreateOutputAsync();
            ProgressInfo.EndTask();
        }

        private void CreateOutput(object obj)
        {
            Task.Run(async () =>
            {
                ProgressInfo.StartTask(TranslationService.GetTranslation(TranslationTexts.Progress.Step_Output));
                await CreateOutputAsync();
                ProgressInfo.EndTask();
            });
        }

        private async Task CreateOutputAsync()
        {
            var creator = new OutputCreator(Config, ProgressInfo);
            await creator.CreateOutputAsync(Data);
        }

        private void ValidateCollection(object obj)
        {
            DataValidator.ValidateCollection(Data);
        }

        private void ConvertFolder(object obj)
        {
            Task.Run(async () =>
            {
                ProgressInfo.StartTask(TranslationService.GetTranslation(TranslationTexts.Progress.Step_Convert));
                await ConvertFolderAsync();
                ProgressInfo.EndTask();
            });
        }

        private async Task ConvertFolderAsync()
        {
            var converter = new FormatConverter(Config, ProgressInfo);
            await converter.ConvertFilesAsync(CurrentSelectedFolder);
        }

        private void ScanFolder(object obj)
        {
            Task.Run(async () =>
            {
                ProgressInfo.StartTask(TranslationService.GetTranslation(TranslationTexts.Progress.Step_Scan));
                await ScanFolderAsync();
                ProgressInfo.EndTask();
            });
        }
        private async Task ScanFolderAsync()
        {
            var scanner = new FolderScanner(Config.ArchiveFolder, ProgressInfo);
            var data = await scanner.ScanFolderAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                DataMerger.MergeData(Data, data);
            });
        }

        private void LoadData(object obj)
        {
            if (!File.Exists(Config.DataFile))
                return;
            Data = SerializationHelper.LoadFileData<FolderCollection>(Config.DataFile);
            CurrentSelectedFolder = Data.FolderEntries.FirstOrDefault();
        }

        private void SaveData(object obj)
        {
            SerializationHelper.SaveFileData(Config.DataFile, Data);
        }
    }
}
