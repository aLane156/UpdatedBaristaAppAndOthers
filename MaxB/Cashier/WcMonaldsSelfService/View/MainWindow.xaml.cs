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
        }

        private void InitialiseMenu()
        {
            List<string> menuNames = vm.GetFullMenu();
            foreach (string name in menuNames)
            {
                Menu_List.Items.Add(name);
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string itemName;
            float itemPrice;
            vm.SetItemFromList(Menu_List.SelectedIndex, out itemName, out itemPrice);
            Item_Name.Text = itemName;
            Item_Price.Text = itemPrice.ToString();
        }

        private void Add_To_Order_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
