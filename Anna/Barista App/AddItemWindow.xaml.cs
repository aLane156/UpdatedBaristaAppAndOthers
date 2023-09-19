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
    /// Interaction logic for AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        public AddItemWindow()
        {
            InitializeComponent();

            // Changes the title text to the contents of ItemType.txt.
            using StreamReader streamReader = new("ItemType.txt");
            string ItemTypeText = streamReader.ReadToEnd();

            ItemTypeTitle.Text = ItemTypeText;

            streamReader.Close();

            LoadWindow();
        }

        /// <summary>
        /// Creates a grid of buttons, one for each item in the menu. In future, change code to only show buttons for foods of a certain type.
        /// </summary>
        private void LoadWindow()
        {
            using StreamReader streamReader = new("Menu.json");
            string json = streamReader.ReadToEnd();
            Menu PurchaseableItems = JsonConvert.DeserializeObject<Menu>(json);

            streamReader.Close();

            Grid grid = new()
            {
                Height = 800,
                Width = 6000,
                VerticalAlignment = VerticalAlignment.Bottom,
            };

            int NumOfRows = CalculateRows(PurchaseableItems);

            int i = 1;
            for ( int r = 0; r <= NumOfRows; r++)
            {
                RowDefinition rowDefinition = new();
                rowDefinition.Name = "Row" + r.ToString();
                grid.RowDefinitions.Add(rowDefinition);

                for (int c = 0; c < 3; c++)
                {

                    ColumnDefinition columnDefinition = new();
                    columnDefinition.Name = "Column" + c.ToString();
                    grid.ColumnDefinitions.Add(columnDefinition);

                    if (PurchaseableItems.Count >= i)
                    {
                        Button button = new()
                        {
                            Name = "Button" + PurchaseableItems[i.ToString()].ItemID,
                            Content = PurchaseableItems[i.ToString()].Name,
                            Height = 200,
                            Width = 600,
                            FontSize = 40,
                            Margin = new Thickness(10, 10, 10, 10)
                        };
                        button.Click += ButtonClicked;

                        Grid.SetRow(button, r);
                        Grid.SetColumn(button, c);
                        grid.Children.Add(button);
                        i++;
                    } 
                }   
            }
            ButtonsGrid.Children.Add(grid);
            //this.Content = grid;
        }

        private int CalculateRows(Menu PurchaseableItems)
        {
            // There are 3 buttons per row, so the num of rows should be the number of buttons to be created / 3 + 1.
            if (PurchaseableItems.Count % 3 == 0)
            {
                return  PurchaseableItems.Count / 3;
            }
            else
            {
                return (PurchaseableItems.Count / 3) + 1;
            }
        }

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            // Retrieve the button's name, then remove the "Button" part to get the ID.
            string ButtonName = ((Button)sender).Name;
            string ButtonID = ButtonName.Replace("Button", "");

            using StreamReader streamReader = new("Menu.json");
            string json = streamReader.ReadToEnd();
            Menu PurchaseableItems = JsonConvert.DeserializeObject<Menu>(json);

            streamReader.Close();

            for (int i = 1; i < PurchaseableItems.Count() + 1; i++)
            {
                if (ButtonID == PurchaseableItems[i.ToString()].ItemID)
                {
                    string u = i.ToString();
                    Item newItem = new()
                    {
                        Name = PurchaseableItems[u].Name,
                        ItemID = PurchaseableItems[u].ItemID,
                        Price = PurchaseableItems[u].Price
                    };

                    AddItem(newItem);
                    break;
                }
            }
        }

        private void AddItem(Item ItemToAdd)
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
            // Clears ItemType.txt.
            File.WriteAllText("ItemType.txt", string.Empty);

            OrderMainWindow orderMainWindow = new();
            orderMainWindow.Show();
            this.Close();
        }
    }
}
