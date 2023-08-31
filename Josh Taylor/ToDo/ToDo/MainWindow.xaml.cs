using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Controls.Primitives;
using static ToDo.MainWindow;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Windows.Themes;

namespace ToDo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AddUI();
        }

        private void AddButton_clicked(object sender, RoutedEventArgs e)
        {
            // Read file
            string json = File.ReadAllText("myFile.json");
            TaskList tasks = JsonConvert.DeserializeObject<TaskList>(json);

            // Add empty object
            Task newTask = new Task();
            newTask.description = "";
            newTask.complete = false;
            newTask.date = 00000000;

            // Add to end of tasks
            tasks.Add((1+tasks.Count).ToString(), newTask);

            // Change the TaskList back to a JSON string
            string updatedJson = JsonConvert.SerializeObject(tasks);

            // Write the updated JSON string back to the file
            File.WriteAllText("myFile.json", updatedJson);

            AddUI();
        }
        private void DeleteButton_clicked(object sender,  RoutedEventArgs e)
        {
            Button button = sender as Button;
            var panel = button.Parent as Panel;
            if (panel != null)
            {
                foreach (var tb in panel.Children.OfType<TextBox>())
                {
                    if (tb.Background == Brushes.Red)
                    {
                        // Read file
                        string json = File.ReadAllText("myFile.json");
                        TaskList tasks = JsonConvert.DeserializeObject<TaskList>(json);

                        // Get textBox id by removing start
                        string taskId = tb.Name.Replace("textBox", "");

                        // For the task with this id
                        if (tasks.ContainsKey(taskId))
                        {
                            tasks.Remove(taskId);

                            // Update taskId of all tasks with a higher taskId than the deleted one
                            int deletedTaskId = int.Parse(taskId);
                            var keysToUpdate = tasks.Keys.Where(k => int.Parse(k) > deletedTaskId).ToList();
                            foreach (var key in keysToUpdate)
                            {
                                var value = tasks[key];
                                tasks.Remove(key);
                                int newKey = int.Parse(key) - 1;
                                tasks[newKey.ToString()] = value;
                            }

                            // Change the TaskList back to a JSON string
                            string updatedJson = JsonConvert.SerializeObject(tasks);

                            // Write the updated JSON string back to the file
                            File.WriteAllText("myFile.json", updatedJson);

                            AddUI();
                        }
                    }
                }
            }
        }

        // When textbox updated save changes
        private void TextBox_TaskChange(object sender, TextChangedEventArgs e)
        {
            // Select the updated textbox
            TextBox currentTextBox = sender as TextBox;
            if (currentTextBox != null)
            {
                // Read file
                string json = File.ReadAllText("myFile.json");
                TaskList tasks = JsonConvert.DeserializeObject<TaskList>(json);

                // Get textBox id by removing start
                string taskId = currentTextBox.Name.Replace("textBox", "");

                // For the task with this id
                if (tasks.ContainsKey(taskId))
                {
                    Task task = tasks[taskId];

                    // New description
                    task.description = currentTextBox.Text;

                    // Change the TaskList back to a JSON string
                    string updatedJson = JsonConvert.SerializeObject(tasks);

                    // Write the updated JSON string back to the file
                    File.WriteAllText("myFile.json", updatedJson);
                }
            }
        }
        private void TextBox_Clicked(object sender, MouseButtonEventArgs e)
        {
            // Select the updated textbox
            TextBox currentTextBox = sender as TextBox;

            if (currentTextBox != null)
            {
                var panel = currentTextBox.Parent as Panel;
                if (panel != null)
                {
                    foreach (var tb in panel.Children.OfType<TextBox>())
                    {
                        tb.Background = Brushes.White;
                    }
                }
                currentTextBox.Background = Brushes.Red;
            }
        }
        private void CheckBox_TaskChange(object sender, RoutedEventArgs e)
        {
            // Select the updated textbox
            CheckBox currentCheckBoxBox = sender as CheckBox;
            if (currentCheckBoxBox != null)
            {
                // Read file
                string json = File.ReadAllText("myFile.json");
                TaskList tasks = JsonConvert.DeserializeObject<TaskList>(json);

                // Get textBox id by removing start
                string taskId = currentCheckBoxBox.Name.Replace("checkBox", "");

                // For the task with this id
                if (tasks.ContainsKey(taskId))
                {
                    Task task = tasks[taskId];

                    // New description
                    task.complete = Convert.ToBoolean(currentCheckBoxBox.IsChecked);

                    // Change the TaskList back to a JSON string
                    string updatedJson = JsonConvert.SerializeObject(tasks);

                    // Write the updated JSON string back to the file
                    File.WriteAllText("myFile.json", updatedJson);
                }
            }
        }

        private void AddUI()
        {
            // Read from the Json file
            string json = File.ReadAllText("myFile.json");
            TaskList tasks = JsonConvert.DeserializeObject<TaskList>(json);

            // Create a ScrollViewer
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            // Create a Grid
            Grid grid = new Grid();
            grid.Margin = new Thickness(10);

            // topGrid 

            // Define two ColumnDefinitions
            //grid.ColumnDefinitions.Add(new ColumnDefinition());
            //grid.ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(100);
            grid.ColumnDefinitions.Add(col1);

            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(100);
            grid.ColumnDefinitions.Add(col2);

            // Define RowDefinitions for each task

            int count;
            if (tasks.Count < 13)
            {
                count = 13;
            }
            else
            {
                count = tasks.Count;
            }
            for (int i = 0; i < count + 1; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            // Add button
            Button addButton = new Button();
            addButton.Content = "Add";
            addButton.Click += AddButton_clicked;
            addButton.Height = 30;
            Grid.SetRow(addButton, 0);
            Grid.SetColumn(addButton, 0);
            grid.Children.Add(addButton);

            // Delete button
            Button deleteButton = new Button();
            deleteButton.Content = "Delete";
            deleteButton.Click += DeleteButton_clicked;
            deleteButton.Height = 30;
            Grid.SetRow(deleteButton, 0);
            Grid.SetColumn(deleteButton, 1);
            grid.Children.Add(deleteButton);

            // Loop though and create a textbox for each task
            for (int i = 0; i < tasks.Count; i++)
            {
                Task task = tasks[(i + 1).ToString()];

                CheckBox checkBox = new CheckBox();
                checkBox.IsChecked = task.complete;
                checkBox.Name = "checkBox" + (i + 1).ToString();
                checkBox.Height = 25;
                checkBox.Margin = new Thickness(1);

                TextBox textBox = new TextBox();
                textBox.Text = task.description;
                textBox.Name = "textBox" + (i + 1).ToString();
                textBox.Height = 25;
                textBox.Margin = new Thickness(1);

                // Attach the TextChanged event handler
                textBox.TextChanged += TextBox_TaskChange;
                textBox.GotFocus += (s, e) => TextBox_Clicked(s, null);
                checkBox.Click += CheckBox_TaskChange;

                // Set the Grid.Row and Grid.Column attached properties
                Grid.SetRow(checkBox, i + 1);
                Grid.SetColumn(checkBox, 0);
                Grid.SetRow(textBox, i + 1);
                Grid.SetColumn(textBox, 1);

                // Add the CheckBox and TextBox to the Grid
                grid.Children.Add(checkBox);
                grid.Children.Add(textBox);
            }

            // Add the Grid to the ScrollViewer
            scrollViewer.Content = grid;

            // Add the ScrollViewer to the Window
            this.Content = scrollViewer;
        }


    }
}
/*
Old code 
if (tasks.ContainsKey("1"))
{
    Task task = tasks["1"];
    Console.WriteLine("Task ID: 1");
    Console.WriteLine("Description: " + task.description);
    Console.WriteLine("Complete: " + task.complete);
    Console.WriteLine("Date: " + task.date);

}
else
{
    Console.WriteLine("No task found with ID");
}
*/