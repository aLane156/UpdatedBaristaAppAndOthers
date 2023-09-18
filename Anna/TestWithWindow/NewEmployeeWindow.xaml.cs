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
using static TestWithWindow.LoginWindow;

namespace TestWithWindow
{
    /// <summary>
    /// Interaction logic for NewEmployeeWindow.xaml
    /// </summary>
    public partial class NewEmployeeWindow : Window
    {
        public string[] Levels { get; set; }

        public NewEmployeeWindow()
        {
            InitializeComponent();

            using (StreamReader Streamreader = new("CurrentUser.json"))
            {
                string json = Streamreader.ReadToEnd();
                CurrentUser currentuser = JsonConvert.DeserializeObject<CurrentUser>(json);

                int LevelOfAccessAscii = (int)Convert.ToInt32(currentuser.UID[0]);

                if (LevelOfAccessAscii >= 49)
                {
                    // An employee cannot give another employee the same level of access as them.
                    Levels = new string[] { "2", "3" };

                    DataContext = this;
                }
                else if (LevelOfAccessAscii == 48)
                {
                    Levels = new string[] { "1", "2", "3" };

                    DataContext = this;
                }
            }
        }

        /// <summary>
        /// Creates a new employee object and adds it to the json.json file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewUser(object sender, RoutedEventArgs e)
        {
            bool Valid = Validations();

            if (Valid == true)
            {
                try
                {
                    using (StreamReader Streamreader = new("json.json"))
                    {
                        // Opens the json.json file and deserializes it into c# objects.
                        string json = Streamreader.ReadToEnd();
                        EmployeeList Employees = JsonConvert.DeserializeObject<EmployeeList>(json);

                        Streamreader.Close();


                        string newID = GenerateID();

                        Employee newEmployee = new()
                        {
                            FirstName = FirstnameText.Text,
                            Surname = SurnameText.Text,
                            EmployeeID = newID,
                            Position = "",
                            HashedPassword = HashPassword(),
                        };

                        Employees.Add((1 + Employees.Count).ToString(), newEmployee);
                        var UpdatedEmployees = JsonConvert.SerializeObject(Employees);
                        File.WriteAllText("json.json", UpdatedEmployees);

                        MessageBox.Show("Employee Added. Their ID is: " + newID);

                        LoggedInWindow LoggedInWindow = new();
                        LoggedInWindow.Show();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message.ToString());
                }
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

            if ((FirstnameText.Text == string.Empty) || (SurnameText.Text == string.Empty) || (PasswordText.Text == string.Empty) || (ConfirmPasswordText.Text == string.Empty))
            {
                Valid = false;
                ErrorMessage = "All fields must have a value.";
            }

            string FullName = FirstnameText.Text + SurnameText.Text;
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
            using (StreamReader Streamreader = new("json.json"))
            {
                string json = Streamreader.ReadToEnd();
                EmployeeList Employees = JsonConvert.DeserializeObject<EmployeeList>(json);

                Random rnd = new();
                string newID = "";
                bool uniqueID = false;
                string IDStart;

                if (LevelOfAccessNumber.SelectedValue == "1")
                {
                    //An ID starting with 1-3 is the highest, except for the owner's.
                    IDStart = rnd.Next(1, 4).ToString();
                }
                else if (LevelOfAccessNumber.SelectedValue == "2")
                {
                    //An iD starting with 4-6 is higher than 7-9, but lower than 1-3
                     IDStart = rnd.Next(4,7).ToString();
                }
                else
                {
                    // The lowest level of authority. If no level is selected, the new employee automatically gets this level. Otherwise, select level 3.
                    IDStart = rnd.Next(7,10).ToString();
                }

                // If the ID is not unique, this repeats until it is.
                while (uniqueID == false)
                {
                    newID = IDStart + rnd.Next(0, 99999).ToString();
                    uniqueID = true;

                    for (int i = 1; i < Employees.Count + 1; i++)
                    {
                        if (newID == Employees[i.ToString()].EmployeeID)
                        {
                            uniqueID = false;
                            break;
                        }
                    }
                }

                return newID;
            }
            
        }

        /// <summary>
        /// Uses hashing algorithm SHA1 in order to hash the password, then returns that value.
        /// </summary>
        /// <returns> PasswordHash </returns>
        private string HashPassword()
        {
            // (Could change to SHA256 to reduce chance of collision)
            using (SHA1 sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(PasswordText.Text));
                var PasswordHash = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    PasswordHash.Append(b.ToString("X2"));
                }
                return PasswordHash.ToString();
            }  
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            LoggedInWindow LoggedInWindow = new();
            LoggedInWindow.Show();
            this.Close();
        }
    }
}
