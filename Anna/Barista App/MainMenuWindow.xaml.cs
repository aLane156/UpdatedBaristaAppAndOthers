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
            AddEmployeeWindow addEmployeeWindow = new();
            addEmployeeWindow.Show();
            this.Close();
        }

        private void EditEmployeeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            this.Close();
        }
    }
}
