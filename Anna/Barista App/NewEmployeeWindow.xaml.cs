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
using System.Windows.Shapes;

namespace Barista_App
{
    /// <summary>
    /// Interaction logic for AddEmployeeWindow.xaml
    /// </summary>
    public partial class NewEmployeeWindow : Window
    {
        public NewEmployeeWindow()
        {
            InitializeComponent();

            using StreamReader streamReader = new("CurrentUser.json");
            string json = streamReader.ReadToEnd();
            CurrentUser currentUser = JsonConvert.DeserializeObject<CurrentUser>(json);

            int LevelOfAccessInt = (int)Char.GetNumericValue(currentUser.UID[0]);

            if (LevelOfAccessInt == 1)
            {
                // An employee cannot give another employee the same level of access as them.
                AccessLevelNumber.ItemsSource = new List<string> { "2", "3"};
            }
            else
            {
                AccessLevelNumber.ItemsSource = new List<string> {"1", "2", "3" };
            }
        }

        /// <summary>
        /// Checks the input data to see if it valid. Gives an error message and returns false if it is not.
        /// </summary>
        /// <returns> Valid </returns>
        private bool Validations()
        {
            bool Valid = true;
            string ErrorMessage = "";

            if ((FirstNameText.Text == string.Empty) || (SurnameText.Text == string.Empty) || (PasswordText.Text == string.Empty) || (ConfirmPasswordText.Text == string.Empty))
            {
                Valid = false;
                ErrorMessage = "All fields must have a value.";
            }

            string FullName = FirstNameText.Text + SurnameText.Text;
            foreach (char c in FullName)
            {
                // Checks each character in the string to see if it is not a letter or a white space.
                if (char.IsDigit(c) || char.IsSymbol(c))
                {
                    Valid = false;
                    ErrorMessage += Environment.NewLine + "First name and surname fields must only contain letters.";
                    break;
                }
            }

            // COmpares the password and password confirmation to see if they are the same.
            if (PasswordText.Text != ConfirmPasswordText.Text)
            {
                ErrorMessage += Environment.NewLine + "The two passwords are not the same.";
            }

            if (!Valid)
            {
                MessageBox.Show(ErrorMessage);
            }
            return Valid;
        }

        /// <summary>
        /// Generates a 6 digit number, then checks to see if it already exists.
        /// </summary>
        /// <returns> newID </returns>
        private string GenerateID()
        {
            using StreamReader streamReader = new("EmployeeData.json");
            string json = streamReader.ReadToEnd();
            EmployeeList Employees = JsonConvert.DeserializeObject<EmployeeList>(json);

            Random rnd = new();
            int newIDint;
            string newIDstring = "";
            bool uniqueID = false;

            // If the ID is not unique, this repeats until it is.
            while (uniqueID == false)
            {
                // newID must first be calculated as an integer to prevent a digit from being lost in the case that the first digit of rnd.Next is a 0.
                newIDint = (int.Parse(AccessLevelNumber.SelectedItem.ToString()) * 100000) + rnd.Next(0, 99999);
                newIDstring = Convert.ToString(newIDint);
                uniqueID = true;

                for (int i = 1; i < Employees.Count + 1; i++)
                {
                    if (newIDstring == Employees[i.ToString()].EmployeeID)
                    {
                        uniqueID = false;
                        break;
                    }
                }
            }

            return newIDstring;

        }

        /// <summary>
        /// Uses hashing algorithm SHA1 in order to hash the password, then returns that value.
        /// </summary>
        /// <returns> PasswordHash </returns>
        private string HashPassword()
        {
            // (Could change to SHA256 to reduce chance of collision)
            using SHA1 sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(PasswordText.Text));
            var PasswordHash = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                PasswordHash.Append(b.ToString("X2"));
            }
            return PasswordHash.ToString();
        }

        /// <summary>
        /// Initialises a new Employee object, then updates the EmployeeData.json file to include the new employee.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewUser(object sender, RoutedEventArgs e)
        {
            if (Validations())
            {
                try
                {
                    using StreamReader streamReader = new("EmployeeData.json");
                    // Opens the json.json file and deserializes it into c# objects.
                    string json = streamReader.ReadToEnd();
                    EmployeeList Employees = JsonConvert.DeserializeObject<EmployeeList>(json);

                    streamReader.Close();

                    string newID = GenerateID();

                    Employee newEmployee = new()
                    {
                        FirstName = FirstNameText.Text,
                        Surname = SurnameText.Text,
                        EmployeeID = newID,
                        Position = "",
                        HashedPassword = HashPassword(),
                    };

                    Employees.Add((1 + Employees.Count).ToString(), newEmployee);
                    var UpdatedEmployees = JsonConvert.SerializeObject(Employees);
                    File.WriteAllText("EmployeeData.json", UpdatedEmployees);

                    MessageBox.Show("Employee Added. Their ID is: " + newID);

                    MainMenuWindow mainMenuWindow = new();
                    mainMenuWindow.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message.ToString());
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new();
            mainMenuWindow.Show();
            this.Close();
        }
    }
}
