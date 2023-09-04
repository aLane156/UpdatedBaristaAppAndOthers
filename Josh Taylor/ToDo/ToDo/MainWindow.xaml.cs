using Newtonsoft.Json;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.AccessControl;

namespace ToDo
{
    public partial class MainWindow : Window
    {
        // Declare a global variable to store the TaskList object
        TaskList tasks;

        public MainWindow()
        {
            InitializeComponent();

            InitializeTasks();

            AddUI();
        }


        // Read the file and deserialize its content into a TaskList object when the program starts
        private void InitializeTasks()
        {
            string json = File.ReadAllText("myFile.json");
            tasks = JsonConvert.DeserializeObject<TaskList>(json);
        }

        private void SaveTasks()
        {
            // Change the TaskList back to a JSON string
            string updatedJson = JsonConvert.SerializeObject(tasks);

            // Write the updated JSON string back to the file
            File.WriteAllText("myFile.json", updatedJson);
        }


        private void AddButton_clicked(object sender, RoutedEventArgs e)
        {
            InitializeTasks();

            // Add empty object
            Task newTask = new Task();
            newTask.description = "";
            newTask.complete = false;
            newTask.date = "00/00/0000";

            // Add to end of tasks
            tasks.Add((1+tasks.Count).ToString(), newTask);
            
            SaveTasks();

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
                    if (tb.Background == Brushes.Crimson)
                    {

                        // Get textBox id by removing start
                        string taskId = tb.Name.Replace("textBoxDescription", "");

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

                            SaveTasks();

                            AddUI();
                        }
                    }
                }
            }
        }
        private void DeleteFinished_clicked(object sender, RoutedEventArgs e)
        {
            // Use a for loop and iterate in reverse order
            for (int i = tasks.Count; i > 0; i--)
            {
                // Get the task at the current index
                Task task = tasks[i.ToString()];

                // For the task with this id
                if (task.complete == true)
                {
                    // Remove the task from the collection
                    tasks.Remove(i.ToString());

                    // Update the taskId of all tasks with a higher taskId than the deleted one
                    int deletedTaskId = i;
                    var keysToUpdate = tasks.Keys.Where(k => int.Parse(k) > deletedTaskId).ToList();
                    foreach (var key in keysToUpdate)
                    {
                        var value = tasks[key];
                        tasks.Remove(key);
                        int newKey = int.Parse(key) - 1;
                        //value = new Task { Id = newKey.ToString() }; // Update the taskId of the task
                        tasks[newKey.ToString()] = value;
                    }
                }
            }

            SaveTasks();


            AddUI();
        }

        // When textbox updated save changes
        private void TextBox_TaskChange(object sender, TextChangedEventArgs e, string property)
        {
            // Select the updated textbox
            TextBox currentTextBox = sender as TextBox;
            if (currentTextBox != null)
            {

                string taskId = currentTextBox.Name.Replace("textBox"+property, "");

                // For the task with this id
                if (tasks.ContainsKey(taskId))
                {
                    Task task = tasks[taskId];

                    // New value
                    string newValue = currentTextBox.Text;

                    // Change the property based on the parameter
                    if (property == "Description")
                    {
                        task.description = newValue;
                    }
                    else if (property == "Date")
                    {
                        task.date = newValue;
                        UpdateDate(sender);
                    }
                    else if (property == "Tag")
                    {
                        task.tag = newValue;
                    }
                    /*
                    switch (property)
                    {
                        case "Description":
                            task.description = newValue;
                            break;
                        case "Date":
                            task.date = newValue;
                            UpdateDate(sender);
                            break;
                        case "Tag":
                            task.tag = newValue;
                            UpdateDate(sender);
                            break;
                        default:
                            break;
                    }
                    */

                    SaveTasks();
                }
            }
            //AddUI();
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
                        if (tb.Width == 500) { tb.Background = Brushes.MediumAquamarine; } 
                    }
                }
                currentTextBox.Background = Brushes.Crimson;
            }
            //AddUI();
        }
        private void CheckBox_TaskChange(object sender, RoutedEventArgs e)
        {
            // Select the updated textbox
            CheckBox currentCheckBoxBox = sender as CheckBox;
            if (currentCheckBoxBox != null)
            {

                // Get textBox id by removing start
                string taskId = currentCheckBoxBox.Name.Replace("checkBox", "");

                // For the task with this id
                if (tasks.ContainsKey(taskId))
                {
                    Task task = tasks[taskId];

                    // New description
                    task.complete = Convert.ToBoolean(currentCheckBoxBox.IsChecked);

                    SaveTasks();
                }

            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            object selectedItem = comboBox.SelectedItem;
            string selectedValue = selectedItem.ToString();

            InitializeTasks();

            // Filter tasks to only contain Task objects with the same tag as selectedValue
            var filteredTasks = tasks.Where(kvp => kvp.Value.tag == selectedValue)
                                     .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Create a new instance of TaskList using the filtered tasks
            tasks = new TaskList();
            foreach (var kvp in filteredTasks)
            {
                tasks.Add(kvp.Key, kvp.Value);
            }

            AddUI();
        }

        private void UpdateDate(object box)
        {
            TextBox textbox = box as TextBox;

            if (textbox.Width == 100)
            {
                try
                {
                    string dateString = textbox.Text.ToString();
                    DateTime date = DateTime.Parse(dateString);

                    int year = date.Year;
                    int month = date.Month;
                    int day = date.Day;

                    DateTime date1 = DateTime.Now;
                    DateTime date2 = new DateTime(year, month, day);

                    TimeSpan difference = date2 - date1;
                    int days = (int)difference.TotalDays;

                    if (days < 0) { textbox.Background = Brushes.Crimson; }
                    else if (days < 2) { textbox.Background = Brushes.DarkOrange; }
                    else { textbox.Background = Brushes.MediumAquamarine; }

                }
                catch (Exception d)
                {
                    Console.WriteLine("Wrong format for date");
                    textbox.Background = Brushes.MediumAquamarine;
                }
            }
        }

        private void AddUI()
        {
            // Create a ScrollViewer
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            // Create a Grid
            Grid grid = new Grid();
            grid.Margin = new Thickness(10);

            // Define two ColumnDefinitions
            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(60);
            grid.ColumnDefinitions.Add(col1);

            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(600);
            grid.ColumnDefinitions.Add(col2);

            ColumnDefinition col3 = new ColumnDefinition();
            col3.Width = new GridLength(100);
            grid.ColumnDefinitions.Add(col3);


            // Add button
            Button addButton = new Button();
            addButton.Content = "Add";
            addButton.Click += AddButton_clicked;
            addButton.Height = 30;
            addButton.Background = Brushes.Aquamarine;
            Grid.SetRow(addButton, 0);
            Grid.SetColumn(addButton, 0);
            grid.Children.Add(addButton);

            // Delete button
            Button deleteButton = new Button();
            deleteButton.Content = "Delete";
            deleteButton.Click += DeleteButton_clicked;
            deleteButton.Height = 30;
            deleteButton.Width = 60;
            deleteButton.HorizontalAlignment = HorizontalAlignment.Left;
            deleteButton.Background = Brushes.Aquamarine;
            Grid.SetRow(deleteButton, 0);
            Grid.SetColumn(deleteButton, 1);
            grid.Children.Add(deleteButton);

            // Delete Finished button
            Button deleteFinished = new Button();
            deleteFinished.Content = "Delete Finished";
            deleteFinished.Click += DeleteFinished_clicked;
            deleteFinished.Height = 30;
            deleteFinished.Background = Brushes.Aquamarine;
            Grid.SetRow(deleteFinished, 0);
            Grid.SetColumn(deleteFinished, 2);
            grid.Children.Add(deleteFinished);

            // Combo box for tags
            ComboBox comboBox = new ComboBox();

            string json = File.ReadAllText("myFile.json");
            TaskList tempTasks = JsonConvert.DeserializeObject<TaskList>(json);
            for (int i = 0; i < tempTasks.Count; i++)
            {
                Task task = tempTasks[(i + 1).ToString()];
                    
                if(!comboBox.Items.Contains(task.tag))
                {
                    comboBox.Items.Add(task.tag);
                }
            }

            comboBox.Height = 30;
            comboBox.Background = Brushes.Aquamarine;
            comboBox.Width = 100;
            comboBox.HorizontalAlignment = HorizontalAlignment.Right;
            comboBox.SelectionChanged += ComboBox_SelectionChanged;
            Grid.SetRow(comboBox, 0);
            Grid.SetColumn(comboBox, 1);
            grid.Children.Add(comboBox);

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

            // Loop though and create a textbox for each task
            for (int i = 0; i < tasks.Count; i++)
            {
                Task task = tasks[(i + 1).ToString()];

                CheckBox checkBox = new CheckBox();
                checkBox.IsChecked = task.complete;
                checkBox.Name = "checkBox" + (i + 1).ToString();
                checkBox.Height = 25;
                checkBox.Margin = new Thickness(1);
                checkBox.Background = Brushes.Aquamarine;

                TextBox textBox = new TextBox();
                textBox.Text = task.description;
                textBox.Name = "textBoxDescription" + (i + 1).ToString();
                textBox.Height = 25;
                textBox.Width = 500;
                textBox.Margin = new Thickness(1);
                textBox.Background = Brushes.MediumAquamarine;
                textBox.HorizontalAlignment = HorizontalAlignment.Left;

                TextBox textBox2 = new TextBox();
                textBox2.Text = task.date.ToString();
                textBox2.Name = "textBoxDate" + (i + 1).ToString();
                textBox2.Height = 25;
                textBox2.Width = 100;
                textBox2.Margin = new Thickness(1);
                textBox2.Background = Brushes.MediumAquamarine;

                TextBox textBox3 = new TextBox();
                textBox3.Text = task.tag;
                textBox3.Name = "textBoxTag" + (i + 1).ToString();
                textBox3.Height = 25;
                textBox3.Width = 100;
                textBox3.Margin = new Thickness(1);
                textBox3.Background = Brushes.MediumAquamarine;
                textBox3.HorizontalAlignment = HorizontalAlignment.Right;

                // Attach the TextChanged event handler
                textBox.TextChanged += (sender, e) => TextBox_TaskChange(sender, e, "Description");
                textBox2.TextChanged += (sender, e) => TextBox_TaskChange(sender, e, "Date");
                textBox3.TextChanged += (sender, e) => TextBox_TaskChange(sender, e, "Tag");
                textBox.GotFocus += (s, e) => TextBox_Clicked(s, null);
                checkBox.Click += CheckBox_TaskChange;

                // Set the Grid.Row and Grid.Column attached properties
                Grid.SetRow(checkBox, i + 1);
                Grid.SetColumn(checkBox, 0);
                Grid.SetRow(textBox, i + 1);
                Grid.SetColumn(textBox, 1);
                Grid.SetRow(textBox2, i + 1);
                Grid.SetColumn(textBox2, 2);
                Grid.SetRow(textBox3, i + 1);
                Grid.SetColumn(textBox3, 1);

                // Add the CheckBox and TextBox to the Grid
                grid.Children.Add(checkBox);
                grid.Children.Add(textBox);
                grid.Children.Add(textBox2);
                grid.Children.Add(textBox3);

                try
                {
                    string dateString = textBox2.Text.ToString();
                    DateTime date = DateTime.Parse(dateString);

                    int year = date.Year;
                    int month = date.Month;
                    int day = date.Day;

                    DateTime date1 = DateTime.Now;
                    DateTime date2 = new DateTime(year, month, day);

                    TimeSpan difference = date2 - date1;
                    int days = (int)difference.TotalDays;

                    if (days < 0) { textBox2.Background = Brushes.Crimson; }
                    else if (days < 2) { textBox2.Background = Brushes.DarkOrange; }
                    else { textBox2.Background = Brushes.MediumAquamarine; }

                }
                catch (Exception d)
                {
                    Console.WriteLine("Wrong format for date");
                    textBox2.Background = Brushes.MediumAquamarine;
                }
            }
            // Add the Grid to the ScrollViewer
            scrollViewer.Content = grid;

            scrollViewer.Background = Brushes.Azure;

            // Add the container to the window
            this.Content = scrollViewer;
 
        }


    }
}