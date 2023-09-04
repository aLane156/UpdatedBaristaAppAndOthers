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
using Task_Manager;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        public string[] Hours { get; set; }
        public string[] Minutes { get; set; }

        public AddTaskWindow()
        {
            InitializeComponent();

            Hours = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23"};

            Minutes = new string[] {"00", "05", "10", "15", "20", "25", "30", "35", "40", "45", "50", "55"};

            DataContext = this;
        }

        private void ToggleDateChecked(object sender, RoutedEventArgs e)
        {
            DueDateCalendar.IsEnabled = true;
            ToggleTime.IsEnabled = true;
            DueTimeHours.IsEnabled = true;
            DueTimeMinutes.IsEnabled = true;
        }

        private void ToggleDateUnchecked(object sender, RoutedEventArgs e)
        {
            DueDateCalendar.IsEnabled = false;
            ToggleTime.IsEnabled = false;
            DueTimeHours.IsEnabled = false;
            DueTimeMinutes.IsEnabled = false;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new();
            MainWindow.Show();
            this.Close();
        }

        private void EnterButtonClicked(object sender, RoutedEventArgs e)
        {
            // Must check if title field has a value. The description field does not need a value in case the task does not need any further information.

            bool ValidCredentials = true;

            if (TitleText.Text == "")
            {
                ValidCredentials = false;

                MessageBox.Show("A task must have a title");
            }

            if (ValidCredentials == true)
            {
                //Serializes a new task with the title, desc, date, and time if using.
                // Create copy of tasks list, change it, then write it to Tasks.json

                // Reads file to create a copy of its contents as a TaskList
                try
                {
                    using (StreamReader r = new("Tasks.json"))
                    {
                        string json = r.ReadToEnd();
                        TaskList taskList = JsonConvert.DeserializeObject<TaskList>(json);

                        r.Close();

                        if ((ToggleDate.IsChecked == true) && (ToggleTime.IsChecked == false))
                        {
                            // Gives a value for the date, but leaves the time as null as it is unchecked.

                            Task newTask = new()
                            {
                                Title = TitleText.Text,
                                Description = DescriptionText.Text,
                                Date = DueDateCalendar.SelectedDate.Value.ToShortDateString(),
                                Time = ""
                            };

                            taskList.Add((1 + taskList.Count).ToString(), newTask);

                            var UpdatedTasks = JsonConvert.SerializeObject(taskList);
                            File.WriteAllText("Tasks.json", UpdatedTasks);
                        }

                        else if (ToggleTime.IsChecked == true)
                        {
                            // As ToggleTime can only be checked when ToggleDate is checked, we don't need to have the program see if it is checked.

                            var newTask = new List<Task>
                            {
                                new Task
                                {
                                    Title = TitleText.Text,
                                    Description = DescriptionText.Text,
                                    Date = DueDateCalendar.SelectedDate.Value.ToShortDateString(),
                                    Time = DueTimeHours.SelectedValue + ":" + DueTimeMinutes.SelectedValue
                                },
                            };

                            var TaskJson = JsonConvert.SerializeObject(newTask);
                            var UpdatedTasks = taskList + TaskJson;
                            File.WriteAllText(@"Tasks.json", UpdatedTasks);
                        }

                        else
                        {
                            // Neither  a date nor time has been provided, so they are both null.
                            var newTask = new List<Task>
                            {
                                new Task
                                {
                                    Title = TitleText.Text,
                                    Description = DescriptionText.Text,
                                    Date = "",
                                    Time = ""
                                },
                            };

                            var TaskJson = JsonConvert.SerializeObject(newTask);
                            var UpdatedTasks = taskList + TaskJson;
                            File.WriteAllText(@"Tasks.json", UpdatedTasks);

                        }

                        MessageBox.Show("Task Added");

                        MainWindow MainWindow = new();
                        MainWindow.Show();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message.ToString());
                }

            }   
        }      
    }
}
