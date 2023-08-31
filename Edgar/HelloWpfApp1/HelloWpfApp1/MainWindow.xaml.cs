using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Markup;
using System.Xml.Linq;
//using Newtonsoft.json;

namespace HelloWpfApp1
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<TaskItem> tasks = new ObservableCollection<TaskItem>();

        public MainWindow()
        {
            InitializeComponent();
            taskList.ItemsSource = tasks;
            taskDescription.ItemsSource = tasks;
            taskDate.ItemsSource = tasks;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string date = txtDate.Text.Trim();

            if (!string.IsNullOrEmpty(name))
            {
                if (!taskList.Items.Contains(txtName.Text))
                {
                    tasks.Add(new TaskItem { Name = name, Description = description, Date = date });
                    txtName.Clear();
                    txtDescription.Clear();
                    txtDate.Clear();
                }
                else throw new InvalidOperationException("Name already used");
            }
            else throw new InvalidOperationException("Name not filled");
        }
    

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (taskList.SelectedIndex >= 0)
            {
                tasks.RemoveAt(taskList.SelectedIndex);
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string date = txtDate.Text.Trim();
            if (taskList.SelectedIndex >= 0)
            {
                tasks.RemoveAt(taskList.SelectedIndex);
                tasks.Add(new TaskItem { Name = name, Description = description, Date = date});
            }
        }

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void TaskList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (taskList.SelectedItem is TaskItem selectedTask)
            {
                txtName.Text = selectedTask.Name;
                txtDescription.Text = selectedTask.Description;
                txtDate.Text = selectedTask.Date;
            }
        }
    }

    public class TaskItem
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Date { get; set; }
        public bool IsCompleted { get; set; }
    }
}
