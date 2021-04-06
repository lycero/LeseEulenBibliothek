using System;
using System.Collections.Generic;
using System.Text;

namespace LeseEulenBibliothek.Core
{
    public static class TranslationTexts
    {
        public const string Title = "application_title|Reading owl library";
        public const string SaveDataButton = "btn_save_data|Save Data";
        public const string SaveDataTooltip = "tt_save_data|Save current data";
        public const string ScanButton = "btn_scan_archive|Scan";
        public const string ScanTooltip = "tt_scan_archive|Scan the archive for new files";
        public const string UpdateButton = "btn_update|Update";
        public const string UpdateTooltip = "tt_update|Full update from archive to output";
        public const string ConvertButton = "btn_convert|Convert";
        public const string ConvertTooltip = "tt_convert|Convert the files in the current selected folder";
        public const string ValidateButton = "btn_validate|Validate";
        public const string ValidateTooltip = "tt_validate|Validates & repairs indexes";
        public const string OutputButton = "btn_output|Output";
        public const string OutputTooltip = "tt_output|Fills the output folder";
        public const string ConfigShowTooltip = "tt_config_show|Show the configuration";
        public const string ConfigHideTooltip = "tt_config_hide|Hide the configuration";
        public const string SaveConfigButton = "btn_configsave|Save Config";
        public const string SaveConfigTooltip = "tt_configsave|Persist changes of the configuration";
        public const string ResetConfigButton = "btn_config_reset|Restore Defaults";
        public const string ResetConfigTooltip = "tt_config_reset|Reset configuration to application defaults";


        public const string ReorderFilesTooltip = "tt_reoder_files|Reoder files and fill gaps";
        public const string FileListLabel = "label_filelist|Files:";
        public const string PathLabel = "label_path|Path:";
        public const string IndexLabel = "label_index|Index:";
        public const string NameLabel = "label_name|Name:";


        public static class Progress
        {
            public const string Step_Scan = "progress_step_scan|Scan";
            public const string Step_Output = "progress_step_output|Create Output";
            public const string Step_Convert = "progress_step_convert|Convert";
        }

        public static class Config
        {
            public const string FileChooserTooltip = "config_tt_filechooser|Open file dialog to select file path";
            public const string FolderChooserTooltip = "config_tt_folderchooser|Open folder dialog to select path";
            public const string ArchiveFolderLabel = "config_label_archive|Archive folder";
            public const string ArchiveFolderTooltip = "config_tt_archive|Archive folder";
            public const string OutputFolderLabel = "config_label_output|Output folder";
            public const string OutputFolderTooltip = "config_tt_output|Output folder";
            public const string LanguageLabel = "config_label_language|Language";
            public const string LanguageTooltip = "config_tt_language|Culture used for translation (de-DE, en-GB, ...)";
            public const string DatafileLabel = "config_label_datafile|Data file";
            public const string DatafileTooltip = "config_tt_datafile|Path to the file used to store file & folder data";
            public const string ConverterBinaryTooltip = "config_tt_converterbinary|Path to the executable used as converter";
            public const string ConverterBinaryLabel= "config_label_converterbinary|Converter executeable";
            public const string ConverterFolderLabel= "config_label_converterfolder|Converted files folder";
            public const string ConverterFolderTooltip= "config_tt_converterfolder|Folder to store converted files";
            public const string ConverterArgumentLabel= "config_label_converterargument|Converter command";
            public const string ConverterArgumentTooltip= "config_tt_converterargument|Argument passed to converter binary, placeholder will be filled with source and target file";
            public const string ConverterCountLabel= "config_label_convertercount|Converter count";
            public const string ConverterCountTooltip= "config_tt_convertercount|Max number of concurrent running converters";
            public const string ConverterConvertAllLabel= "config_label_convertall|Convert *.mp3 files";
            public const string ConverterConvertAllTooltip= "config_tt_convertall|Execute converter for *.mp3 files";
        }
    }
}
