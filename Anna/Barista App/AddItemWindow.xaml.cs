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

            LoadWindow(ItemTypeText);
        }

        /// <summary>
        /// Creates a grid of buttons, one for each item in the menu of the chosen type.
        /// </summary>
        private void LoadWindow(string ItemType)
        {
            using StreamReader streamReader = new("Menu.json");
            string json = streamReader.ReadToEnd();
            Menu PurchaseableItems = JsonConvert.DeserializeObject<Menu>(json);

            streamReader.Close();

            // Initialises a grid for the buttons to appear on.
            Grid grid = new()
            {
                Height = 1000,
                Width = 6000,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };

            // ItemsToDisplay contains all of the items of the chosen type.
            var ItemsToDisplay = new List<Item>();

            for (int j = 1; j < PurchaseableItems.Count; j++)
            {
                if (PurchaseableItems[j.ToString()].Type == ItemType)
                {
                    ItemsToDisplay.Add(PurchaseableItems[j.ToString()]);
                }
            }

            // NumOfRows is defined before the loop so the function doesn't have to be ran several times.
            int NumOfRows = CalculateRows(ItemsToDisplay);

            int i = 1;
            for ( int r = 0; r <= NumOfRows; r++)
            {
                // Creates a new row.
                RowDefinition rowDefinition = new()
                {
                    Name = "Row" + r.ToString()
                };
                grid.RowDefinitions.Add(rowDefinition);

                for (int c = 0; c < 3; c++)
                {
                    //Creates 3 columns for each row, since 3 buttons will be displayed on each row.
                    ColumnDefinition columnDefinition = new()
                    {
                        Name = "Column" + c.ToString()
                    };
                    grid.ColumnDefinitions.Add(columnDefinition);

                    // As there can be more rows and columns than items, some spaces will be empty.
                    if (ItemsToDisplay.Count >= i)
                    {
                        // Creates a new button.
                        Button button = new()
                        {
                            Name = "Button" + ItemsToDisplay[i - 1].ItemID,
                            Content = ItemsToDisplay[i - 1].Name,
                            Height = 200,
                            Width = 400,
                            FontSize = 40,
                            Margin = new Thickness(20, 20, 20, 20),
                        };
                        button.Click += ButtonClicked;

                        // Set's the button to a specific part of the grid.
                        Grid.SetRow(button, r);
                        Grid.SetColumn(button, c);
                        grid.Children.Add(button);
                        i++;
                    } 
                }   
            }
            ButtonsGrid.Children.Add(grid);
        }

        static int CalculateRows(List<Item> ItemsToDisplay)
        {
            // There are 3 buttons per row, so the num of rows should be the number of buttons to be created / 3 + 1.
            if (ItemsToDisplay.Count % 3 == 0)
            {
                return ItemsToDisplay.Count / 3;
            }
            else
            {
                return (ItemsToDisplay.Count / 3) + 1;
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

            for (int i = 1; i < PurchaseableItems.Count + 1; i++)
            {
                if (ButtonID == PurchaseableItems[i.ToString()].ItemID)
                {
                    string u = i.ToString();
                    Item newItem = new()
                    {
                        Name = PurchaseableItems[u].Name,
                        Type = PurchaseableItems[u].Type,
                        ItemID = PurchaseableItems[u].ItemID,
                        Price = PurchaseableItems[u].Price
                    };

                    AddItem(newItem);
                    break;
                }
            }
        }

        static void AddItem(Item ItemToAdd)
        {
            using StreamReader streamReader = new("CurrentOrder.json");
            string json = streamReader.ReadToEnd();
            Order Items = JsonConvert.DeserializeObject<Order>(json);

            streamReader.Close();

            int key = 0;
            if (Items == null)
            {
                key = 1;

                Order ItemsIsNull = new()
                {
                    { key.ToString(), ItemToAdd }
                };
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
