using LeseEulenBibliothek.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Input;

namespace LeseEulenBibliothek.Data
{
    public class FolderData : ObservableBase, IIndexedEntry
    {
        private string m_FolderName = "";
        private int m_Index = -1;
        private string m_FolderPath = "";
        private ObservableCollection<FolderDataEntry> m_Collection = new ObservableCollection<FolderDataEntry>();

        [JsonIgnore]
        public ICommand ReOrderCommand { get; }

        [JsonIgnore]
        public int LowerLimit => 1;
        [JsonIgnore]
        public int UpperLimit => 99;

        [JsonPropertyName("index")]
        public int IndexNumber
        {
            get => m_Index;
            set => Set(ref m_Index, value);
        }

        [JsonPropertyName("name")]
        public string FolderName 
        { 
            get => m_FolderName;
            set
            {
                if (Set(ref m_FolderName, value))
                    NotifyPropertyChanged(nameof(DisplayFolderPath));
            }
        }

        [JsonPropertyName("path")]
        public string FolderPath
        {
            get => m_FolderPath;
            set 
            {
                if (Set(ref m_FolderPath, value))
                    NotifyPropertyChanged(nameof(DisplayFolderPath));
            }
        }

        [JsonIgnore]
        public string DisplayFolderPath
        {
            get => ResolveDisplayFolderPath();
        }

        [JsonPropertyName("entries")]
        public ObservableCollection<FolderDataEntry> FileEntries
        {
            get => m_Collection;
            set => Set(ref m_Collection, value);
        }

        [JsonIgnore]
        public ObservableCollection<FolderDataEntry> EntryCollection => m_Collection;

        public FolderData()
        {
            ReOrderCommand = new CommandHandler(ReOrderList);
        }
        private void ReOrderList(object obj)
        {
            var list = m_Collection.ToList();
            list.Sort(CompareEntries);
            int index = 1;
            foreach (var entry in list)
                entry.IndexNumber = index++;
        }

        private int CompareEntries(FolderDataEntry x, FolderDataEntry y)
        {
            return x.IndexNumber.CompareTo(y.IndexNumber);
        }

        private string ResolveDisplayFolderPath()
        {
            if (FolderPath == FolderName)
                return "";
            return $"({FolderPath})";
        }
    }
}
