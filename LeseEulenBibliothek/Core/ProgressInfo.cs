using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Data;

namespace LeseEulenBibliothek.Core
{
    public class ProgressInfo : ObservableBase
    {
        Visibility m_ProgressVisibility = Visibility.Hidden;
        private List<string> m_ProgressTexts = new List<string>();
        private ObservableCollection<string> m_ViewList = new ObservableCollection<string>();
        private string m_TaskText = "";
        private Timer m_SyncTimer = new Timer();
        private object m_LockObj = new object();
        private object m_ListLock = new object();

        public ProgressInfo()
        {
            BindingOperations.EnableCollectionSynchronization(m_ViewList, m_ListLock);
            m_SyncTimer.Enabled = false;
            m_SyncTimer.Interval = 50;
            m_SyncTimer.Elapsed += SyncTimer_Elapsed;
        }

        private void SyncTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (m_LockObj)
            {
                var currentList = m_ViewList.ToList();
                var newList = m_ProgressTexts.ToList();

                foreach (var entry in currentList)
                {
                    if(!newList.Contains(entry))
                        m_ViewList.Remove(entry);                
                }
                foreach (var entry in newList)
                {
                    if (!currentList.Contains(entry))
                        m_ViewList.Add(entry);
                }
            }
        }

        public Visibility ProgressVisibility
        {
            get => m_ProgressVisibility;
            set => Set(ref m_ProgressVisibility, value);
        }

        public string TaskText
        {
            get => m_TaskText;
            set => Set(ref m_TaskText, value);
        }

        public ObservableCollection<string> ProgressTexts
        {
            get => m_ViewList;
        }

        public void AddStep(string step)
        {
            lock (m_LockObj)
            {
                m_ProgressTexts.Add(step);
            }
        }

        public void RemoveStep(string step)
        {
            lock (m_LockObj)
            {
                m_ProgressTexts.Remove(step);
            }
        }

        public void StartTask(string taskName)
        {
            ClearSteps();
            TaskText = taskName;
            ProgressVisibility = Visibility.Visible;
            m_SyncTimer.Enabled = true;
        }
        public void EndTask()
        {
            ProgressVisibility = Visibility.Hidden;
            m_SyncTimer.Enabled = false;
            ClearSteps();
        }

        public void ClearSteps()
        {
            lock (m_LockObj)
            {
                m_ProgressTexts.Clear();
                m_ViewList.Clear();
            }
        }
    }
}
