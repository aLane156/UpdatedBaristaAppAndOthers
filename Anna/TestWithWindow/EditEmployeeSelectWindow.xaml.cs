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
    /// Interaction logic for EditEmployeeSelectWindow.xaml
    /// </summary>
    public partial class EditEmployeeSelectWindow : Window
    {
        public EditEmployeeSelectWindow()
        {
            InitializeComponent();

            using (StreamReader Streamreader = new("CurrentUser.json"))
            {
                string json = Streamreader.ReadToEnd();
                CurrentUser currentuser = JsonConvert.DeserializeObject<CurrentUser>(json);

                int LevelOfAccessAscii = (int)Convert.ToInt32(currentuser.UID[0]);

                if (LevelOfAccessAscii > 52)
                {
                    // Users of access level below 1 are not allowed to edit other users' data.
                    EditOtherDetailsRadioButton.IsEnabled = false;
                }
            }
        }

        private void EditOwnDetailsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            IDTextBoxText.IsEnabled = false;
            IDErrorText.IsEnabled = false;
            EditEmployeeIDText.IsEnabled = false;

            EditOtherDetailsRadioButton.IsChecked = false;

            EditDetailsButton.IsEnabled = true;
        }

        private void EditOtherDetailsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            IDTextBoxText.IsEnabled = true;
            IDErrorText.IsEnabled = true;
            EditEmployeeIDText.IsEnabled = true;

            EditOwnDetailsRadioButton.IsChecked = false;
        } 

        private void EditEmployeeIDText_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                using (StreamReader Streamreader = new("CurrentUser.json"))
                {
                    string json = Streamreader.ReadToEnd();
                    CurrentUser currentuser = JsonConvert.DeserializeObject<CurrentUser>(json);

                    if (EditEmployeeIDText.Text != "")
                    {
                        int LevelOfAccessAscii = (int)Convert.ToInt32(currentuser.UID[0]);
                        // Level of access of the employee whose details the user is trying to change.
                        int EditEmployeeAccessAscii = (int)Convert.ToInt32(EditEmployeeIDText.Text[0]);

                        int DifferenceInAccess = EditEmployeeAccessAscii - LevelOfAccessAscii;

                        foreach (char c in EditEmployeeIDText.Text)
                        {
                            if ((char.IsLetter(c)) || (char.IsSymbol(c)) || (char.IsPunctuation(c)) || (char.IsWhiteSpace(c)))
                            {
                                IDErrorText.Text = "Invalid ID.";
                                IDErrorText.Foreground = Brushes.DarkRed;
                                EditDetailsButton.IsEnabled = false;
                                break;
                            }
                            else if (EditEmployeeIDText.Text.Length != 6)
                            {
                                IDErrorText.Text = "Invalid ID.";
                                IDErrorText.Foreground = Brushes.DarkRed;
                                EditDetailsButton.IsEnabled = false;
                            }
                            else if (currentuser.UID == EditEmployeeIDText.Text)
                            {
                                IDErrorText.Text = "That is your own ID.";
                                IDErrorText.Foreground = Brushes.Green;
                                EditDetailsButton.IsEnabled = true;
                            }
                            else if ((DifferenceInAccess < 2) || EditEmployeeAccessAscii == 48)
                            {
                                IDErrorText.Text = "You do not have permission to edit that employee's details.";
                                IDErrorText.Foreground = Brushes.DarkRed;
                                EditDetailsButton.IsEnabled = false;
                            }
                            else
                            {
                                IDErrorText.Text = "";
                                EditDetailsButton.IsEnabled = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.ToString);
            }
        }

        /// <summary>
        /// First checks which radiobutton is active.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditOwnDetailsRadioButton.IsChecked == true)
            {
                using (StreamReader Streamreader = new("CurrentUser.json"))
                {
                    string json = Streamreader.ReadToEnd();
                    CurrentUser currentUser = JsonConvert.DeserializeObject<CurrentUser>(json);

                    Streamreader.Close();

                    if (!File.Exists("EditingEmployee.json"))
                    {
                        File.Create("EditingEmployeejson");
                    }

                    using (StreamReader Streamreader2 = new("EditingEmployee.json"))
                    {
                        EditingEmployee newEditingEmployee = new()
                        {
                            EditFirstName = currentUser.UFirstName,
                            EditSurname = currentUser.USurname,
                            EditEmployeeID = currentUser.UID,
                            EditPosition = currentUser.UPosition,
                            EditHashedPassword = currentUser.UHashedPassword,
                        };

                        Streamreader2.Close();

                        var UpdatedEditingEmployee = JsonConvert.SerializeObject(newEditingEmployee);
                        File.WriteAllText("EditingEmployee.json", UpdatedEditingEmployee);

                        EditEmployeeWindow EditEmployeeWindow = new();
                        EditEmployeeWindow.Show();
                        this.Close();
                    }
                }
            }

            else
            {
                using (StreamReader Streamreader = new("json.json"))
                {
                    // Opens the json.json file and deserializes it into c# objects.
                    string json = Streamreader.ReadToEnd();
                    EmployeeList Employees = JsonConvert.DeserializeObject<EmployeeList>(json);

                    Streamreader.Close();

                    bool IDExists = false;
                    int u = 0;

                    for (int i = 1; i < Employees.Count; i++)
                    {

                        if (EditEmployeeIDText.Text == Employees[i.ToString()].EmployeeID)
                        {
                            IDExists = true;
                            u = i;
                            break;
                        }
                    }

                    if (IDExists)
                    {
                        if (!File.Exists("EditingEmployee.json"))
                        {
                            File.Create("EditingEmployeejson");
                        }

                        using (StreamReader Streamreader2 = new("EditingEmployee.json"))
                        {
                            // Makes a copy of the employee object to be changed into a json file so that it can be carried onto the next window
                            EditingEmployee newEditingEmployee = new()
                            {
                                EditFirstName = Employees[u.ToString()].FirstName,
                                EditSurname = Employees[u.ToString()].Surname,
                                EditEmployeeID = Employees[u.ToString()].EmployeeID,
                                EditPosition = Employees[u.ToString()].Position,
                                EditHashedPassword = Employees[u.ToString()].HashedPassword,
                            };

                            Streamreader2.Close();

                            var UpdatedEditingEmployee = JsonConvert.SerializeObject(newEditingEmployee);
                            File.WriteAllText("EditingEmployee.json", UpdatedEditingEmployee);
                        }

                        EditEmployeeWindow EditEmployeeWindow = new();
                        EditEmployeeWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("That ID does not exist.");
                    }
                }
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
