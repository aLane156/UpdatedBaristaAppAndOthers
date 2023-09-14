using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WcMonaldsSelfService.ViewModel;

namespace WcMonaldsSelfService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainWindowVM vm = new();

        public MainWindow()
        {
            InitializeComponent();
            InitialiseMenu();
            vm.mw = this;
            SetTotal();
        }

        /// <summary>
        /// Adds all items to menu from vm items
        /// </summary>
        private void InitialiseMenu()
        {
            try
            {
                List<string> menuNames = vm.GetFullMenu();
                foreach (string name in menuNames)
                {
                    Menu_List.Items.Add(name);
                }
            } catch { }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Menu_List.SelectedItem == null) { return; }
            try
            {
                ChangeItemDisplayFormat(true);
                vm.SetItemFromList(Menu_List.SelectedIndex, out string itemName, out float itemPrice);
                Item_Name.Text = itemName;
                Item_Price.Text = itemPrice.ToString();
            } 
            catch { }
        }

        private void Add_To_Order_Click(object sender, RoutedEventArgs e)
        {

            vm.AddItemToBasket();
        }

        public void AddItemToBasketList(string name)
        {
            Basket_List.Items.Add(name);
        }

        public void RemoveItemFromBasketList()
        {
            Basket_List.Items.RemoveAt(Basket_List.SelectedIndex);
        }

        private void Basket_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Basket_List.SelectedItem == null) { return; }
            try
            {
                ChangeItemDisplayFormat(false);
                vm.SetItemFromList(Basket_List.SelectedIndex, out string itemName, out float itemPrice, true);
                Item_Name.Text = itemName;
                Item_Price.Text = itemPrice.ToString();
            }
            catch { }
        }

        /// <summary>
        /// Changes the centeral display between menu mode and basket mode
        /// </summary>
        /// <param name="toUnadded">Should the menu be set to menu mode</param>
        public void ChangeItemDisplayFormat(bool toUnadded)
        {
            if (toUnadded)
            {
                Add_To_Order.Visibility = Visibility.Visible;
                Duplicate_Button.Visibility = Visibility.Collapsed;
                Remove_Button.Visibility = Visibility.Collapsed;
                Basket_List.SelectedItem = null;
            }
            else
            {
                Add_To_Order.Visibility = Visibility.Collapsed;
                Duplicate_Button.Visibility = Visibility.Visible;
                Remove_Button.Visibility = Visibility.Visible;
                Menu_List.SelectedItem = null;
            }
        }

        /// <summary>
        /// Sets the total price of the basket
        /// </summary>
        /// <param name="total">The calculated total amount</param>
        public void SetTotal(float total = 0)
        {
            if (total == 0) 
            {
                Total_Cost.Text = "";
                return; 
            }
            Total_Cost.Text = $"Total: £{total}";
        }

        private void To_CheckOut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            vm.RemoveFromBasket();
        }

        private void Add_Another_Button_Click(object sender, RoutedEventArgs e)
        {
            vm.CopyAdditionalToBasket();
        }
    }
}
