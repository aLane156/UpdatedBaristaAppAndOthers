using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System;

namespace Barista_App
{
    /// <summary>
    /// Interaction logic for CakesAndPastriesWindow.xaml
    /// </summary>
    public partial class CakesAndPastriesWindow : Window
    {
        public CakesAndPastriesWindow()
        {
            InitializeComponent();
        }

        private void BattenbergCakeButton_Click(object sender, RoutedEventArgs e)
        {
            // Item ID 001

            using StreamReader streamReader = new("Menu.json");
            string json = streamReader.ReadToEnd();
            Menu PurchaseableItems = JsonConvert.DeserializeObject<Menu>(json);

            streamReader.Close();

            string TargetID = "1";
            string u = "";

            for (int i = 1; i < PurchaseableItems.Count; i++)
            {
                if (i.ToString() == TargetID)
                {
                    u = i.ToString();
                    break;
                }
            }

            Item BattenbergCake = new()
            {
                Name = PurchaseableItems[u].Name,
                ItemID = PurchaseableItems[u].ItemID,
                Price = PurchaseableItems[u].Price
            };

            AddNextItem(BattenbergCake);
        }

        private void CroissantButton_Click(object sender, RoutedEventArgs e)
        {
            // Item ID 2
            using StreamReader streamReader = new("Menu.json");
            string json = streamReader.ReadToEnd();
            Menu PurchaseableItems = JsonConvert.DeserializeObject<Menu>(json);

            streamReader.Close();

            string TargetID = "2";
            string u = "";

            for (int i = 1; i < PurchaseableItems.Count; i++)
            {
                if (i.ToString() == TargetID)
                {
                    u = i.ToString();
                    break;
                }
            }

            Item Croissant = new()
            {
                Name = PurchaseableItems[u].Name,
                ItemID = PurchaseableItems[u].ItemID,
                Price = PurchaseableItems[u].Price
            };

            AddNextItem(Croissant);
        }

        static void AddNextItem(Item ItemToAdd)
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
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OrderMainWindow orderMainWindow = new();
            orderMainWindow.Show();
            this.Close();
        }
    }
}
