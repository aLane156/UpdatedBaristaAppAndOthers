using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

namespace Barista_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string HashPassword()
        {
            // Uses the hash key SHA1 in order to hash the entered password to compare it with the hashed password in the EmployeeData.json file.
            using SHA1 sha1 = SHA1.Create();
            return Convert.ToHexString(sha1.ComputeHash(Encoding.UTF8.GetBytes(PasswordText.Text)));
        }

        /// <summary>
        /// Does not allow the user to click the enter button if the ID is invalid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmployeeIDText_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool Valid = true;

            // All IDs are 6 characters long.
            if (EmployeeIDText.Text.Length != 6 || EmployeeIDText.Text == "")
            {
                Valid = false;
            }

            // All IDs only contain integers.
            foreach (char c in EmployeeIDText.Text)
            {
                if (char.IsDigit(c) != true)
                {
                    Valid = false;
                }
            }
            if (!Valid)
            {
                ValidText.Text = "That ID is not valid.";
                ValidText.Foreground = Brushes.DarkRed;
                EnterButton.IsEnabled = false;
            }
            else
            {
                ValidText.Text = string.Empty;
                EnterButton.IsEnabled = true;
            }
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("EmployeeData.json"))
            {
                File.Create("EmployeeData.json");
            }

            try
            {
                using StreamReader streamReader = new("EmployeeData.json");
                // Opens the EmployeeData.json file and deserializes it into c# Employee objects.
                string json = streamReader.ReadToEnd();
                EmployeeList Employees = JsonConvert.DeserializeObject<EmployeeList>(json);

                bool EmployeeExists = false;
                bool CorrectPassword = false;
                int u = 0;

                // Loops for how many employee objects exist. Starts at 1 because the first key is 1.
                for (int i = 1; i < Employees.Count + 1; i++)
                {
                    if (EmployeeIDText.Text == Employees[i.ToString()].EmployeeID.ToString())
                    {
                        EmployeeExists = true;

                        string HashedUserInput = HashPassword();

                        if (HashedUserInput == Employees[i.ToString()].HashedPassword.ToString())
                        {
                            CorrectPassword = true;
                            u = i;
                        }
                        break;
                    }
                }
                if (!EmployeeExists)
                {
                    MessageBox.Show("EmployeeID is incorrect.");
                }

                else if ((EmployeeExists) && (!CorrectPassword))
                {
                    MessageBox.Show("Password is incorrect.");
                }

                else
                {
                    using (StreamReader Streamreader2 = new("CurrentUser.json"))
                    {
                        CurrentUser currentUser = new()
                        {
                            UFirstName = Employees[u.ToString()].FirstName,
                            USurname = Employees[u.ToString()].Surname,
                            UID = Employees[u.ToString()].EmployeeID,
                            UPosition = Employees[u.ToString()].Position,
                            UHashedPassword = Employees[u.ToString()].HashedPassword,
                        };

                        Streamreader2.Close();

                        var UpdatedUser = JsonConvert.SerializeObject(currentUser);
                        File.WriteAllText("CurrentUser.json", UpdatedUser);
                    }

                    MessageBox.Show("Signing you in as " + Employees[u.ToString()].FirstName + " " + Employees[u.ToString()].Surname);

                    MainMenuWindow mainMenuWindow = new();
                    mainMenuWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred with signing you in: " + ex.Message.ToString());
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Closes the application.

            Application.Current.Shutdown();
        }

        private void HashButton_Click(object sender, RoutedEventArgs e)
        {
            /* using StreamReader streamReader = new("EmployeeData.json");
            string json = streamReader.ReadToEnd();
            EmployeeList Employees = JsonConvert.DeserializeObject<EmployeeList>(json);
            EmployeeList EmployeesTemp = new();

            streamReader.Close();

            File.WriteAllText("EmployeeData.json", string.Empty);

            for (int i = 1; 1 <= Employees.Count - 1; i++)
            {
                using SHA1 sha1 = SHA1.Create();
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(Employees[i.ToString()].HashedPassword));
                var stringBuilder = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    stringBuilder.Append(b.ToString("X2"));
                }

                Employee rewritePassword = new Employee()
                {
                    FirstName = Employees[i.ToString()].FirstName,
                    Surname = Employees[i.ToString()].Surname,
                    EmployeeID = Employees[i.ToString()].EmployeeID,
                    Position = Employees[i.ToString()].Position,
                    HashedPassword = stringBuilder.ToString(),
                };

                EmployeesTemp.Add((1 + EmployeesTemp.Count).ToString(), rewritePassword);

                var modifiedJsonString = JsonConvert.SerializeObject(EmployeesTemp);
                File.WriteAllText("EmployeeData.json", modifiedJsonString);
            } */
        }
    }
}
