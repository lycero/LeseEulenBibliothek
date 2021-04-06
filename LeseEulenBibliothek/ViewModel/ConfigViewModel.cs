using LeseEulenBibliothek.Core;
using LeseEulenBibliothek.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace LeseEulenBibliothek.ViewModel
{
    public static class ConfigConstants
    {
        public const string Datafile = "datafile";
        public const string Converter = "converter";
        public const string ConvertedFiles = "convertedfiles";
        public const string ArchiveFolder = "archivefolder";
        public const string OutputFolder = "outputfolder";
    }
    public class ConfigViewModel : ObservableBase
    {
        public ConfigData Data { get => m_Data; private set => Set(ref m_Data, value); }
        public ICommand SaveConfigCommand { get; }
        public ICommand ResetConfigCommand { get; }
        public ICommand OpenFileChooserCommand { get; }

        private bool m_DataChanged = false;
        private ConfigData m_Data;
        private readonly string m_ConfigFilePath;

        public ConfigViewModel(string configFilePath)
        {
            m_Data = LoadOrCreateData(configFilePath);

            this.m_ConfigFilePath = configFilePath;
            Data.PropertyChanged += OnDataChanged;
            SaveConfigCommand = new CommandHandler(SaveConfig, CanSaveConfig);
            ResetConfigCommand = new CommandHandler(ResetConfig);
            OpenFileChooserCommand = new CommandHandler(OpenFileChooser);
        }

        private void OpenFileChooser(object obj)
        {
            if (obj is string chooserType)
                OpenFileChooserFromType(chooserType);
        }

        private void OpenFileChooserFromType(string chooserType)
        {            
            if (ChooseFile(chooserType))
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = GetFilter(chooserType);
                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                        return;
                    
                    var relativePath = System.IO.Path.GetRelativePath(System.IO.Directory.GetCurrentDirectory(), openFileDialog.FileName);
                    UpdatePathSetting(chooserType, relativePath);                    
                }
            }
            else
            {
                using(var openFolderDialog = new FolderBrowserDialog())
                {
                    if (openFolderDialog.ShowDialog() != DialogResult.OK)
                        return;

                    var relativePath = System.IO.Path.GetRelativePath(System.IO.Directory.GetCurrentDirectory(), openFolderDialog.SelectedPath);
                    UpdatePathSetting(chooserType, relativePath);
                }
            }
        }

        private bool ChooseFile(string chooserType)
        {
            if (chooserType == ConfigConstants.Datafile)
                return true;
            if (chooserType == ConfigConstants.Converter)
                return true;

            return false;
        }

        private string GetFilter(string chooserType)
        {
            if (chooserType == ConfigConstants.Converter)
                return "(*.*)|*.*";
            return "Json (*.json)|*.json";
        }

        private void UpdatePathSetting(string chooserType, string value)
        {
            switch (chooserType)
            {
                case ConfigConstants.Datafile:
                    m_Data.DataFile = value;
                    break;
                case ConfigConstants.Converter:
                    m_Data.ConverterCommand = value;
                    break;
                case ConfigConstants.OutputFolder:
                    m_Data.OutputFolder = value;
                    break;
                case ConfigConstants.ArchiveFolder:
                    m_Data.ArchiveFolder = value;
                    break;
                case ConfigConstants.ConvertedFiles:
                    m_Data.ConvertFolder = value;
                    break;
                default:
                    break;
            }
        }

        private void ResetConfig(object obj)
        {
            Data = new ConfigData();
            m_DataChanged = true;
        }

        private void OnDataChanged(object sender, PropertyChangedEventArgs e)
        {
            m_DataChanged = true;
            if(SaveConfigCommand is CommandHandler cmdHandler)
                cmdHandler.InvokeCanExecuteChanged();
        }

        private bool CanSaveConfig(object arg)
        {
            return m_DataChanged;
        }

        private void SaveConfig(object obj)
        {
            m_DataChanged = false;
            SerializationHelper.SaveFileData(m_ConfigFilePath, Data);
            if (SaveConfigCommand is CommandHandler cmdHandler)
                cmdHandler.InvokeCanExecuteChanged();
        }

        private ConfigData LoadOrCreateData(string configFilePath)
        {
            if (System.IO.File.Exists(configFilePath))
                return LoadConfig(configFilePath);
            return new ConfigData();
        }

        private ConfigData LoadConfig(string configFilePath)
        {
            return SerializationHelper.LoadFileData<ConfigData>(configFilePath);
        }
    }
}
