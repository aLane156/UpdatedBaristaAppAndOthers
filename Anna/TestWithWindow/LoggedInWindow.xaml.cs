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
using static TestWithWindow.LoginWindow;

namespace TestWithWindow
{
    /// <summary>
    /// Interaction logic for LoggedInWindow.xaml
    /// </summary>
    public partial class LoggedInWindow : Window
    {
        public LoggedInWindow()
        {
            InitializeComponent();

            using (StreamReader Streamreader = new("CurrentUser.json"))
            {
                string json = Streamreader.ReadToEnd();
                CurrentUser currentuser = JsonConvert.DeserializeObject<CurrentUser>(json);

                LoggedInText.Text = "Current user: " + currentuser.UFirstName + " " + currentuser.USurname;

                int LevelOfAccessAscii = (int)Convert.ToInt32(currentuser.UID[0]);
                if (LevelOfAccessAscii >= 52)
                {
                    // 52 is the ascii code for 4, so if the employee ID's first digit is greater than or equal to 4, they cannot use the following procedures.
                    NewEmployeeButton.IsEnabled = false;
                    DeleteEmployeeButton.IsEnabled = false;
                }
            }
        }

        private void NewEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Access granted");

            NewEmployeeWindow newEmployeeWindow = new();
            newEmployeeWindow.Show();
            this.Close();
        }

        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Access granted");

            // DeleteEmployeeWindow deleteEmployeeWindow = new();
            // deleteEmployeeWindow.Show();
            // this.Close();
        }

        private void EditEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            EditEmployeeSelectWindow editEmployeeSelectWindow = new();
            editEmployeeSelectWindow.Show();
            this.Close();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            // Deletes all of the data in the CUrrentUser file in order to sign them out.
            File.WriteAllText("CurrentUser.json", string.Empty);

            LoginWindow LoginWindow = new();
            LoginWindow.Show();
            this.Close();
        }
    }
}
