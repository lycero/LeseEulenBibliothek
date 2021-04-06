using System;
using System.Collections.Generic;
using System.Text;

namespace LeseEulenBibliothek.Data
{
    public interface IIndexedEntry
    {
        public int IndexNumber { get; set; }
        public int LowerLimit { get; }
        public int UpperLimit { get; }
        public bool IsValidIndex() => IndexNumber >= LowerLimit && IndexNumber <= UpperLimit;
    }
}
