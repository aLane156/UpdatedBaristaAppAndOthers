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
            newTask.date = "00/00/0000";

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
                    if (tb.Background == Brushes.Crimson)
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
        private void DeleteFinished_clicked(object sender, RoutedEventArgs e)
        {
            // Read file
            string json = File.ReadAllText("myFile.json");
            TaskList tasks = JsonConvert.DeserializeObject<TaskList>(json);

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

            // Change the TaskList back to a JSON string
            string updatedJson = JsonConvert.SerializeObject(tasks);

            // Write the updated JSON string back to the file
            File.WriteAllText("myFile.json", updatedJson);


            AddUI();
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
            //AddUI();
        }
        private void TextBox2_TaskChange(object sender, TextChangedEventArgs e)
        {
            // Select the updated textbox
            TextBox currentTextBox = sender as TextBox;
            if (currentTextBox != null)
            {
                // Read file
                string json = File.ReadAllText("myFile.json");
                TaskList tasks = JsonConvert.DeserializeObject<TaskList>(json);

                // Get textBox id by removing start
                string taskId = currentTextBox.Name.Replace("_textBox2", "");

                // For the task with this id
                if (tasks.ContainsKey(taskId))
                {
                    Task task = tasks[taskId];

                    // New description
                    task.date = currentTextBox.Text;

                    // Change the TaskList back to a JSON string
                    string updatedJson = JsonConvert.SerializeObject(tasks);

                    // Write the updated JSON string back to the file
                    File.WriteAllText("myFile.json", updatedJson);
                }

                UpdateDate(sender);
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
                        if (tb.Width == 600) { tb.Background = Brushes.MediumAquamarine; } 
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

                AddUI();
            }
        }

        private void UpdateDate(object box)
        {
            string json = File.ReadAllText("myFile.json");
            TaskList tasks = JsonConvert.DeserializeObject<TaskList>(json);

            TextBox textbox = box as TextBox;

            var panel = textbox.Parent as Panel;

            for (int i = 0; i < tasks.Count; i++)
            {
                Task task = tasks[(i + 1).ToString()];

                // Use FindVisualChildren() to get all the text boxes within the panel
                foreach (var tb in FindVisualChildren<TextBox>(panel))
                {
                    if (tb.Width == 100)
                    {
                        try
                        {
                            string dateString = task.date.ToString();
                            DateTime date = DateTime.Parse(dateString);

                            int year = date.Year;
                            int month = date.Month;
                            int day = date.Day;

                            DateTime date1 = DateTime.Now;
                            DateTime date2 = new DateTime(year, month, day);

                            TimeSpan difference = date2 - date1;
                            int days = (int)difference.TotalDays;

                            if (days < 0) { tb.Background = Brushes.Crimson; }
                            else if (days < 2) { tb.Background = Brushes.DarkOrange; }
                            else { tb.Background = Brushes.MediumAquamarine; }

                        }
                        catch (Exception d)
                        {
                            Console.WriteLine("Wrong format for date");
                            tb.Background = Brushes.MediumAquamarine;
                        }
                    }
                }
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }




        private void AddUI()
        {
            // Read from the Json file
            try
            {
                string json = File.ReadAllText("myFile.json");
                TaskList tasks = JsonConvert.DeserializeObject<TaskList>(json);

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
                    textBox.Name = "textBox" + (i + 1).ToString();
                    textBox.Height = 25;
                    textBox.Width = 600;
                    textBox.Margin = new Thickness(1);
                    textBox.Background = Brushes.MediumAquamarine;

                    TextBox textBox2 = new TextBox();
                    textBox2.Text = task.date.ToString();
                    textBox2.Name = "_textBox2" + (i + 1).ToString();
                    textBox2.Height = 25;
                    textBox2.Width = 100;
                    textBox2.Margin = new Thickness(1);
                    textBox2.Background = Brushes.MediumAquamarine;

                    // Attach the TextChanged event handler
                    textBox.TextChanged += TextBox_TaskChange;
                    textBox.GotFocus += (s, e) => TextBox_Clicked(s, null);
                    //textBox2.GotFocus += (s, e) => TextBox_Clicked(s, null);
                    textBox2.TextChanged += TextBox2_TaskChange;
                    checkBox.Click += CheckBox_TaskChange;


                    // Set the Grid.Row and Grid.Column attached properties
                    Grid.SetRow(checkBox, i + 1);
                    Grid.SetColumn(checkBox, 0);
                    Grid.SetRow(textBox, i + 1);
                    Grid.SetColumn(textBox, 1);
                    Grid.SetRow(textBox2, i + 1);
                    Grid.SetColumn(textBox2, 2);

                    // Add the CheckBox and TextBox to the Grid
                    grid.Children.Add(checkBox);
                    grid.Children.Add(textBox);
                    grid.Children.Add(textBox2);

                    UpdateDate(textBox2);
                }

                // Add the Grid to the ScrollViewer
                scrollViewer.Content = grid;

                scrollViewer.Background = Brushes.Azure;

                // Add the container to the window
                this.Content = scrollViewer;
            }
            catch (Exception e)
            {
                Console.WriteLine("file doesn't work");
            }
        }


    }
}