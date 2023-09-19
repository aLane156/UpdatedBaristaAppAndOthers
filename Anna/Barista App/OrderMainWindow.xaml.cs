using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace Barista_App
{
    /// <summary>
    /// Interaction logic for OrderMainWindow.xaml
    /// </summary>
    public partial class OrderMainWindow : Window
    {
        //
        // Future feature: could hacve only one screen which displays items to add to the order, and each time it is loaded depending on the button clicked it shows different items.
        //

        public OrderMainWindow()
        {
            InitializeComponent();

            DisplayOrder();
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
                else
                {
                    FinishOrderButton.IsEnabled = false;
                }

                OrderDisplayText.Text = "Current order: " + Environment.NewLine + ItemList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred with loading the current order: " + ex.Message.ToString());
            }
        }

        private void LoadAddItemWindow(string itemType)
        {
            File.WriteAllText("ItemType.txt", itemType);

            AddItemWindow addItemWindow = new();
            addItemWindow.Show();
            this.Close();
        }

        private void ColdDrinksButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAddItemWindow("Cold Drinks");
        }

        private void HotDrinksButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAddItemWindow("Hot Drinks");
        }

        private void ColdFoodButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAddItemWindow("Cold Food");
        }

        private void HotFoodButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAddItemWindow("Hot Food");
        }

        private void CakesAndPastriesButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAddItemWindow("Cakes and Pastries");
            /* CakesAndPastriesWindow cakesAndPastriesWindow = new();
            cakesAndPastriesWindow.Show();
            this.Close(); */
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchItemWindow searchWindow = new();
            searchWindow.Show();
            this.Close();
        }

        private void FinishOrderButton_Click(object sender, RoutedEventArgs e)
        {
            FinishOrderWindow finishOrderWindow = new();
            finishOrderWindow.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("CurrentOrder.json", string.Empty);

            MainMenuWindow mainMenuWindow = new();
            mainMenuWindow.Show();
            this.Close();
        }
    }
}
