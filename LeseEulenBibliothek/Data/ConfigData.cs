using LeseEulenBibliothek.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LeseEulenBibliothek.Data
{
    public class ConfigData : ObservableBase
    {
        private string m_DataFile = "data\\eulenbib.json";
        private string m_OutputFolder = "output";
        private string m_ConvertFolder = "data\\convert";
        private int m_MaxConverterCount = 2;
        private string m_ArchiveFolder = "archive";
        private string m_ConverterArgument = @"-I dummy --no-repeat ""{0}"" --sout=#transcode{{acodec=mp3,ab=128,vcodec=dummy}}:std{{access=file,mux=mp3,dst=""{1}""}} vlc://quit";
        private string m_ConverterCommand = "data\\Converter\\vlc.exe";
        private bool m_ExecuteConverterForTargetFormat = false;
        private string m_Language = "";

        [JsonPropertyName("datafile")]
        public string DataFile { get => m_DataFile; set => Set(ref m_DataFile, value); }
        [JsonPropertyName("output")]
        public string OutputFolder { get => m_OutputFolder; set => Set(ref m_OutputFolder, value); }
        [JsonPropertyName("convert")]
        public string ConvertFolder { get => m_ConvertFolder; set => Set(ref m_ConvertFolder, value); }
        [JsonPropertyName("maxconvertercount")]
        public int MaxConverterCount { get => m_MaxConverterCount; set => Set(ref m_MaxConverterCount, value); }
        [JsonPropertyName("executeconverterfortargetformat")]
        public bool ExecuteConverterForTargetFormat { get => m_ExecuteConverterForTargetFormat; set => Set(ref m_ExecuteConverterForTargetFormat, value); }
        [JsonPropertyName("archive")]
        public string ArchiveFolder { get => m_ArchiveFolder; set => Set(ref m_ArchiveFolder, value); }
        [JsonPropertyName("converterargument")]
        public string ConverterArgument { get => m_ConverterArgument; set => Set(ref m_ConverterArgument, value); }
        [JsonPropertyName("convertercommand")]
        public string ConverterCommand { get => m_ConverterCommand; set => Set(ref m_ConverterCommand, value); }
        [JsonPropertyName("language")]
        public string Language { get => m_Language; set => Set(ref m_Language, value); }
    }
}
