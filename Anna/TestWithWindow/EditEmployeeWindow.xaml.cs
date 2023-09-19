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

namespace TestWithWindow
{
    /// <summary>
    /// Interaction logic for EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : Window
    {
        public EditEmployeeWindow()
        {
            InitializeComponent();


            // Inserts employee data into fields so that they can be "edited". If the employee is changing their own details, they also have the option to change their password.
            using (StreamReader Streamreader = new("EditingEmployee.json"))
            {
                string json = Streamreader.ReadToEnd();
                EditingEmployee editingEmployee = JsonConvert.DeserializeObject<EditingEmployee>(json);

                EditingEmployeeTitle.Text = "This data belongs to: " + editingEmployee.EditFirstName + " " + editingEmployee.EditSurname;
                EditFirstNameText.Text = editingEmployee.EditFirstName;
                EditSurnameText.Text = editingEmployee.EditSurname;
                EditIDText.Text = editingEmployee.EditEmployeeID;
                EditPositionText.Text = editingEmployee.EditPosition;

                using (StreamReader Streamreader2 = new("CurrentUser.json"))
                {
                    string json2 = Streamreader2.ReadToEnd();
                    CurrentUser currentUser = JsonConvert.DeserializeObject<CurrentUser>(json2);

                    if (currentUser.UID == editingEmployee.EditEmployeeID)
                    {
                        NewPasswordText.Visibility = Visibility.Visible;
                        NewConfirmPasswordText.Visibility = Visibility.Visible;
                        NewPasswordText.IsEnabled = true;
                        NewConfirmPasswordText.IsEnabled = true;

                        EditPasswordText.Visibility = Visibility.Visible;
                        EditConfirmPasswordText.Visibility = Visibility.Visible;
                        EditPasswordText.IsEnabled = true;
                        EditConfirmPasswordText.IsEnabled = true;
                    }
                    else
                    {
                        NewPasswordText.Visibility = Visibility.Hidden;
                        NewConfirmPasswordText.Visibility = Visibility.Hidden;
                        NewPasswordText.IsEnabled = false;
                        NewConfirmPasswordText.IsEnabled = false;

                        EditPasswordText.Visibility = Visibility.Hidden;
                        EditConfirmPasswordText.Visibility = Visibility.Hidden;
                        EditPasswordText.IsEnabled = false;
                        EditConfirmPasswordText.IsEnabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Opens the json.json file and deserialises it to Employee objects, then finds which object is the one to be changed. All other objects have their data copied to be rewritten to the cleared json.json file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitChangesButton_Click(object sender, RoutedEventArgs e)
        {
            using (StreamReader Streamreader = new("json.json"))
            {
                string json = Streamreader.ReadToEnd();
                EmployeeList Employees = JsonConvert.DeserializeObject<EmployeeList>(json);
                // EmployeesCopy will be used to rebuild the Employees dictionary while making sure the changes are included.
                EmployeeList EmployeesCopy = new();

                Streamreader.Close();

                // Clears the json.json file so data can be rewritten to it without risk of it being duplicated.
                File.WriteAllText("json.json", string.Empty);

                int u = 0;

                for (int i = 1; i < Employees.Count; i++)
                {
                    if (EditIDText.Text == Employees[i.ToString()].EmployeeID)
                    {
                        u = i;
                        break;
                    }
                }

                if (EditPasswordText.Text != "")
                {
                    if (EditPasswordText.Text == EditConfirmPasswordText.Text)
                    {
                        for (int j = 1; j < Employees.Count + 1; j++)
                        {
                            // If the selected employee object does not have the same ID as the user being changed, its details are not changed.
                            if (u != j)
                            {
                                Employee employee = new()
                                {
                                    FirstName = Employees[j.ToString()].FirstName,
                                    Surname = Employees[j.ToString()].Surname,
                                    EmployeeID = Employees[j.ToString()].EmployeeID,
                                    Position = Employees[j.ToString()].Position,
                                    HashedPassword = Employees[j.ToString()].HashedPassword,
                                };

                                EmployeesCopy.Add((1 + EmployeesCopy.Count).ToString(), employee);
                                var UpdatedEmployees = JsonConvert.SerializeObject(EmployeesCopy);
                                File.WriteAllText("json.json", UpdatedEmployees);
                            }
                            else
                            {
                                Employee employee = new()
                                {
                                    FirstName = EditFirstNameText.Text,
                                    Surname = EditSurnameText.Text,
                                    EmployeeID = EditIDText.Text,
                                    Position = EditPositionText.Text,
                                    HashedPassword = HashPassword(),
                                };

                                EmployeesCopy.Add((1 + EmployeesCopy.Count).ToString(), employee);
                                var UpdatedEmployees = JsonConvert.SerializeObject(EmployeesCopy);
                                File.WriteAllText("json.json", UpdatedEmployees);

                                MessageBox.Show("Change was successful!");
                            }
                        }
                    }
                    else
                    {
                        // No data will be changed if the passwords are not the same.
                        MessageBox.Show("The new password and confirm password fields are not the same.");
                    }
                }
                else
                {
                    // The password will not be changed, but the rest of the process is similar to before.

                    for (int j = 1; j < Employees.Count + 1; j++)
                    {
                        if (u != j)
                        {
                            Employee employee = new()
                            {
                                FirstName = Employees[j.ToString()].FirstName,
                                Surname = Employees[j.ToString()].Surname,
                                EmployeeID = Employees[j.ToString()].EmployeeID,
                                Position = Employees[j.ToString()].Position,
                                HashedPassword = Employees[j.ToString()].HashedPassword,
                            };

                            EmployeesCopy.Add((1 + EmployeesCopy.Count).ToString(), employee);
                            var UpdatedEmployees = JsonConvert.SerializeObject(EmployeesCopy);
                            File.WriteAllText("json.json", UpdatedEmployees);
                        }
                        else
                        {
                            Employee employee = new()
                            {
                                FirstName = EditFirstNameText.Text,
                                Surname = EditSurnameText.Text,
                                EmployeeID = EditIDText.Text,
                                Position = EditPositionText.Text,
                                HashedPassword = Employees[u.ToString()].HashedPassword,
                            };

                            EmployeesCopy.Add((1 + EmployeesCopy.Count).ToString(), employee);
                            var UpdatedEmployees = JsonConvert.SerializeObject(EmployeesCopy);
                            File.WriteAllText("json.json", UpdatedEmployees);

                            MessageBox.Show("Change was successful!");
                        }
                    }
                }
            }
        }

        private string HashPassword()
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(EditPasswordText.Text));
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
            //CLears the EditingEmployee.json file.
            File.WriteAllText("EditingEmployee.json", string.Empty);

            LoggedInWindow LoggedInWindow = new();
            LoggedInWindow.Show();
            this.Close();
        }
    }
}
