using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CashierApp.Model
{
    public class CustomDataGrid : DataGrid
    {
        /// <summary>
        /// A customised data grid with the ability to select multiple items.
        /// </summary>
        public CustomDataGrid() 
        {
            this.SelectionChanged += CustomDataGrid_SelectionChangedHandler;
        }

        void CustomDataGrid_SelectionChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList.Clear();
            foreach (var item in this.Items)
            {
                this.SelectedItemsList.Add(item);
            }
        }

        public IList SelectedItemsList
        {
            get { return (IList) GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
            DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(CustomDataGrid), new PropertyMetadata(null));
    }
}
