using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace Barista_App
{
    /// <summary>
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        // This window will mainly serve as a gateway to other windows.
        public MainMenuWindow()
        {
            InitializeComponent();

            using StreamReader streamReader = new("CurrentUser.json");
            string json = streamReader.ReadToEnd();
            CurrentUser user = JsonConvert.DeserializeObject<CurrentUser>(json);

            TitleText.Text = "Currently signed in as: " + user.UFirstName + " " + user.USurname + ".";

            int LevelOfAccessAscii = (int)Convert.ToInt32(user.UID[0]);

            // Ascii code 49 represents the character 1. If an employee's ID has a number greater than 1, their level of access is not high enough to add or delete employees.
            if (LevelOfAccessAscii >  49)
            {
                AddEmployeeButton.IsEnabled = false;
                AddEmployeeButton.Visibility = Visibility.Hidden;
                DeleteEmployeeButton.IsEnabled = false;
                DeleteEmployeeButton.Visibility = Visibility.Hidden;
                AddItemButton.IsEnabled = false;
                AddItemButton.Visibility = Visibility.Hidden;
                DeleteItemButton.IsEnabled = false;
                DeleteItemButton.Visibility = Visibility.Hidden;
            }
        }    

        private void NewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderMainWindow orderMainWindow = new();
            orderMainWindow.Show();
            this.Close();
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            NewEmployeeWindow addEmployeeWindow = new();
            addEmployeeWindow.Show();
            this.Close();
        }

        private void EditEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            EditEmployeeSelectWindow editEmployeeSelectWindow = new();
            editEmployeeSelectWindow.Show();
            this.Close();
        }

        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            // Delete employee
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            NewItemWindow newItemWindow = new();
            newItemWindow.Show();
            this.Close();
        }

        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            // Delete item
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            this.Close();
        }
    }
}
