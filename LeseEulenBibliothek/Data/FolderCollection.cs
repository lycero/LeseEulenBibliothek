using LeseEulenBibliothek.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace LeseEulenBibliothek.Data
{
    public class FolderCollection : ObservableBase
    {
        private ObservableCollection<FolderData> m_Collection = new ObservableCollection<FolderData>();

        [JsonPropertyName("folders")]
        public ObservableCollection<FolderData> FolderEntries
        {
            get => m_Collection;
            set => Set(ref m_Collection, value);
        }

        [JsonIgnore]
        public ObservableCollection<FolderData> Collection => m_Collection;
    }
}
