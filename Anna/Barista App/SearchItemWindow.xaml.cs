using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Barista_App
{
    /// <summary>
    /// Interaction logic for SearchItemWindow.xaml
    /// </summary>
    public partial class SearchItemWindow : Window
    {
        public SearchItemWindow()
        {
            InitializeComponent();

            DisplayOrder();
            CreateListBox();
        }

        private void DisplayOrder()
        {
            try
            {
                using StreamReader streamReader = new("CurrentOrder.json");
                string json = streamReader.ReadToEnd();
                Order Items = JsonConvert.DeserializeObject<Order>(json);

                streamReader.Close();

                string ItemList = "";

                if (Items != null)
                {
                    for (int i = 1; i < Items.Count + 1; i++)
                    {
                        ItemList = ItemList + Environment.NewLine + Items[i.ToString()].Name;
                    }
                }

                OrderDisplayText.Text = "Current order: " + Environment.NewLine + ItemList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred with loading the current order: " + ex.Message.ToString());
            }
        }

        private void CreateListBox()
        {
            using StreamReader streamReader = new("Menu.json");
            string json = streamReader.ReadToEnd();
            Menu PurchaseableItems = JsonConvert.DeserializeObject<Menu>(json);

            streamReader.Close();

            for (int i = 1; i < PurchaseableItems.Count + 1; i++)
            {
                SearchListBox.Items.Add(PurchaseableItems[i.ToString()].Name);
            }
        }

        private void AddNextItem(Item ItemToAdd)
        {
            using StreamReader streamReader = new("CurrentOrder.json");
            string json = streamReader.ReadToEnd();
            Order Items = JsonConvert.DeserializeObject<Order>(json);

            streamReader.Close();

            int key = 0;
            if (Items == null)
            {
                key = 1;

                Order ItemsIsNull = new();

                ItemsIsNull.Add(key.ToString(), ItemToAdd);
                var UpdatedOrder1 = JsonConvert.SerializeObject(ItemsIsNull);
                File.WriteAllText("CurrentOrder.json", UpdatedOrder1);
            }
            else
            {
                key = 1 + Items.Count;

                Items.Add(key.ToString(), ItemToAdd);
                var UpdatedOrder2 = JsonConvert.SerializeObject(Items);
                File.WriteAllText("CurrentOrder.json", UpdatedOrder2);
            }

            DisplayOrder();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchListBox.Items.Clear();

            using StreamReader streamReader = new("Menu.json");
            string json = streamReader.ReadToEnd();
            Menu PurchaseableItems = JsonConvert.DeserializeObject<Menu>(json);

            streamReader.Close();

            for (int i = 1; i < PurchaseableItems.Count + 1; i++)
            {
                if (SearchBar.Text == "")
                {
                    SearchListBox.Items.Add(PurchaseableItems[i.ToString()].Name);
                }
                else
                {
                    if (Regex.IsMatch(PurchaseableItems[i.ToString()].Name.ToLower(), SearchBar.Text.ToLower()))
                    {
                        SearchListBox.Items.Add(PurchaseableItems[i.ToString()].Name);
                    }
                }
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            // If the SelectedIndex attribute equals -1, it means no item has been selected, therefore nothing can be added to the order.
            if (SearchListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Select an item to add it to the order.");
            }
            else
            {
                using StreamReader streamReader = new("Menu.json");
                string json = streamReader.ReadToEnd();
                Menu PurchaseableItems = JsonConvert.DeserializeObject<Menu>(json);

                streamReader.Close();

                // MessageBox.Show(SearchListBox.SelectedValue.ToString());

                for (int i = 1; i < PurchaseableItems.Count + 1; i++)
                {
                    if (SearchListBox.SelectedValue.ToString() == PurchaseableItems[i.ToString()].Name)
                    {
                        string j = i.ToString();

                        Item newItem = new()
                        {
                            Name = PurchaseableItems[j].Name,
                            ItemID = PurchaseableItems[j].ItemID,
                            Price = PurchaseableItems[j].Price,
                        };

                        AddNextItem(newItem);
                    }
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OrderMainWindow orderMainWindow = new();
            orderMainWindow.Show();
            this.Close();
        }
    }
}
