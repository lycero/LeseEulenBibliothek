using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LeseEulenBibliothek.Core
{
    public class CollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList list)
            {
                var view = CollectionViewSource.GetDefaultView(list);
                view.SortDescriptions.Add(new System.ComponentModel.SortDescription() { PropertyName = "IndexNumber" });
                if (view is ListCollectionView listView)
                {
                    listView.LiveSortingProperties.Add("IndexNumber");
                    listView.IsLiveSorting = true;
                }
                return view;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
