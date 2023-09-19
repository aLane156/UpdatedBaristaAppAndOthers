using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
    /// Interaction logic for NewItemWindow.xaml
    /// </summary>
    public partial class NewItemWindow : Window
    {
        public NewItemWindow()
        {
            InitializeComponent();

            //
            ItemType.ItemsSource = new List<string> {"Cold Drinks", "Hot Drinks", "Cold Food", "Hot Food", "Cakes and Pastries"};
        }

        /// <summary>
        /// Checks if the item already exists, and returns false if it does.
        /// </summary>
        /// <returns></returns>
        private bool CheckIfItemExists(Menu PurchaseableItems)
        {
            for (int i = 1; i < PurchaseableItems.Count; i++)
            {
                // Converting both strings to lowercase makes sure that no duplicates will be created due to different cases.
                if (PurchaseableItems[i.ToString()].Name.ToLower() == NameText.Text.ToLower())
                {
                    MessageBox.Show("That item already exists.");
                    EnterButton.IsEnabled = false;
                    return false;
                }
            }
            return true;
        }

        static string NewItemID(Menu PurchaseableItems)
        {
            string NewIDString = "";

            for (int i = 1; i < PurchaseableItems.Count + 2; i++)
            {
                if (!PurchaseableItems.ContainsKey(i))
                {
                    NewIDString = i.ToString();
                }
            }

            return NewIDString;
        }

        private string ConvertPrice()
        {
            int PriceInt = int.Parse(PriceText.Text);
            float NewPriceInt = PriceInt / 100;

            return NewPriceInt.ToString();
        }
        private bool ValidateName()
        {
            if (NameText.Text != "")
            {
                foreach (char c in NameText.Text)
                {
                    if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ValidatePrice()
        {
            if (PriceText.Text != "")
            {
                foreach (char c in PriceText.Text)
                {
                    if (!char.IsDigit(c))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Every time the NameText field is changed, this procedure is called. The name must only contain letters for it to be valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!ValidateName())
            {
                EnterButton.IsEnabled = false;
                ErrorText.Text = "The name can only contain letters and white spaces.";
            }
            else
            {
                if (ErrorText.Text != "") { ErrorText.Text = ""; }
                if (!EnterButton.IsEnabled) { EnterButton.IsEnabled = true; }
            }
        }

        private void PriceText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!ValidatePrice())
            {
                EnterButton.IsEnabled = false;
                ErrorText.Text = "The price can only contain numbers.";
            }
            else
            {
                if (ErrorText.Text != "") { ErrorText.Text = ""; }
                if (!EnterButton.IsEnabled) { EnterButton.IsEnabled = true; }
            }
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            using StreamReader streamReader = new("Menu.json");
            string json = streamReader.ReadToEnd();
            Menu PurchaseableItems = JsonConvert.DeserializeObject<Menu>(json);

            streamReader.Close();

            if (CheckIfItemExists(PurchaseableItems))
            {
                Item newItem = new()
                {
                    Name = NameText.Text,
                    Type = ItemType.SelectedItem.ToString(),
                    ItemID = NewItemID(PurchaseableItems),
                    Price = ConvertPrice(),
                };

                PurchaseableItems.Add((1 + PurchaseableItems.Count).ToString(), newItem);
                var UpdatesItems = JsonConvert.SerializeObject(PurchaseableItems);
                File.WriteAllText("Menu.json", UpdatesItems);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new();
            mainMenuWindow.Show();
            this.Close();
        }
    }
}
