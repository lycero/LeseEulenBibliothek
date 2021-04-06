using LeseEulenBibliothek.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LeseEulenBibliothek.Data
{
    public class FolderDataEntry : ObservableBase, IIndexedEntry
    {
        private int m_IndexNumber;
        private string m_OriginalTitle = "";
        private string m_OriginalExtension = "";
        private DateTime m_FileTime;

        [JsonIgnore]
        public int LowerLimit => 1;
        [JsonIgnore]
        public int UpperLimit => 255;

        [JsonPropertyName("index")]
        public int IndexNumber { get => m_IndexNumber; set => Set(ref m_IndexNumber, value); }

        [JsonPropertyName("title")]
        public string OriginalTitle { get => m_OriginalTitle; set => Set(ref m_OriginalTitle, value); }

        [JsonPropertyName("ext")]
        public string OriginalExtension { get => m_OriginalExtension; set => Set(ref m_OriginalExtension, value); }

        [JsonPropertyName("filetime")]
        public DateTime FileTime { get => m_FileTime; set => Set(ref m_FileTime, value); }
    }
}
