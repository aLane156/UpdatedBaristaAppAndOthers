using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for FinishOrderWindow.xaml
    /// </summary>
    public partial class FinishOrderWindow : Window
    {
        public FinishOrderWindow()
        {
            InitializeComponent();

            // A payment option must be selected before the transaction is complete.
            TransactionButton.IsEnabled = false;

            DisplayOrder();
            DisplayPrice();
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

        private void DisplayPrice()
        {

            string TotalPriceString = CalculatePrice().ToString();

            if (TotalPriceString.Length == 3)
            {
                TotalPriceString += "0";
            }

            TotalPriceText.Text = "Total: £" + TotalPriceString;
        }

        /// <summary>
        /// Calculates the total price of the order by parseing the string Price for each item in CurrentOrder.json into double and then adding them to a total.
        /// </summary>
        /// <returns></returns>
        static double CalculatePrice()
        {
            using StreamReader streamReader = new("CurrentOrder.json");
            string json = streamReader.ReadToEnd();
            Order Items = JsonConvert.DeserializeObject<Order>(json);

            streamReader.Close();

            double TotalPrice = 0;

            for (int i = 1; i < Items.Count + 1; i++)
            {
                TotalPrice += double.Parse(Items[i.ToString()].Price, System.Globalization.CultureInfo.InvariantCulture);
            }

            return TotalPrice;
        }


        // Using cash and using card cannot be ticked at the same time.
        private void UsingCashRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UsingCardRadioButton.IsChecked = false;
            TransactionButton.IsEnabled = true;
        }

        private void UsingCardRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UsingCashRadioButton.IsChecked = false;
            TransactionButton.IsEnabled = true;
        }

        private void TransactionButton_Click(object sender, RoutedEventArgs e)
        {
            // Money.txt is a file containing all of the money gained throughout the program.
            using StreamReader streamReader = new("Money.txt");
            string CurrentMoneyString = File.ReadAllText("Money.txt");
            if (CurrentMoneyString == "")
            {
                CurrentMoneyString = "0";
            }
            double CurrentMoney = double.Parse(CurrentMoneyString, System.Globalization.CultureInfo.InvariantCulture);

            CurrentMoney += CalculatePrice();

            streamReader.Close();

            File.WriteAllText("Money.txt", CurrentMoney.ToString());


            // Clears the order once it is complete
            File.WriteAllText("CurrentOrder.json", string.Empty);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OrderMainWindow orderMainWindow = new();
            orderMainWindow.Show();
            this.Close();
        }
    }
}
