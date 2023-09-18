using Microsoft.Windows.Themes;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using TestWithWindow;

namespace TestWithWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public LoginWindow()
        {
            InitializeComponent();
        }

        private bool ValidateID()
        {
            bool Valid = true;

            // All IDs are 6 characters long.
            if (EmployeeIDText.Text.Length != 6)
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

            return Valid;
        }


        /// <summary>
        /// Reads and deserializes json file to c#. Checks Employees list for ID. If it exists, it compares the password attached to the ID to the one entered to see if it is correct.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckDetails()
        {
            // Checks if json.json exists, and creates it if it does not.
            if (File.Exists("json.json"))
            {
                try
                {
                    using (StreamReader Streamreader = new("json.json"))
                    {
                        // Opens the json.json file and deserializes it into c# objects.
                        string json = Streamreader.ReadToEnd();
                        EmployeeList Employees = JsonConvert.DeserializeObject<EmployeeList>(json);

                        // These flags can cause certain error messages to be returned at the end of the procedure if something is entered incorrectly.
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

                            LoggedInWindow LoggedInWindow = new();
                            LoggedInWindow.Show();
                            this.Close();
                        }
                    }    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message.ToString());
                }
            }
            else
            {
                File.Create("json.json");
            }
        }

        private string HashPassword()
        {
            // Uses the hash key SHA1 in order to hash the entered password to compare it with the hashed password in the json.json file.
            using SHA1 sha1 = SHA1.Create();
            return Convert.ToHexString(sha1.ComputeHash(Encoding.UTF8.GetBytes(PasswordText.Text)));
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            // Returns in error message if the ID is not valid. Otherwise, it allows the ID to be checked.
            if (ValidateID() == true)
            {
                CheckDetails();
            }
            else
            {
                MessageBox.Show("EmployeeID is invalid.");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Closes the application.

            Application.Current.Shutdown();
        }

        private void HashButton_Click(object sender, RoutedEventArgs e)
        {
            /* using StreamReader Streamreader = new("json.json");
            string json = Streamreader.ReadToEnd();
            EmployeeList Employees = JsonConvert.DeserializeObject<EmployeeList>(json);

            for (int i = 1; 1 <= Employees.Count; i++)
            {
                using (SHA1 sha1 = SHA1.Create())
                {
                    var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(Employees[i.ToString()].HashedPassword));
                    var stringBuilder = new StringBuilder(hash.Length * 2);

                    foreach (byte b in hash)
                    {
                        stringBuilder.Append(b.ToString("X2"));
                    }

                    Employees[i.ToString()].HashedPassword = stringBuilder.ToString();
                    var modifiedJsonString = JsonConvert.SerializeObject(Employees[i.ToString()].HashedPassword);
                    MessageBox.Show(modifiedJsonString);
                    // File.WriteAllText("json.json", modifiedJsonString);
                }  
            } */
        } 
    }
}
