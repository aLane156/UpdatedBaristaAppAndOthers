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
using System.Collections.Immutable;
using System.Xml.Linq;

namespace TaskList
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateDropDown();
        }

        private string ReadLineOfFile(int lineNo)
        {
            string line = File.ReadLines("C:\\Users\\mb153367\\Documents\\testing-file.txt").ElementAt(lineNo);
            return line;
        }

        private void WriteLineOfFile(int lineNo, string changeTo)
        {
            string[] lines = File.ReadAllLines("C:\\Users\\mb153367\\Documents\\testing-file.txt");
            lines[lineNo] = changeTo;
            File.WriteAllLines("C:\\Users\\mb153367\\Documents\\testing-file.txt", lines);
        }

        private void SaveTaskName_Click(object sender, RoutedEventArgs e)
        {
            CreateNewEvent(Task_Name_Input.Text, Task_Time_Input.Text);//                                                                <------------ affected by desc add
            UpdateDropDown();
        }

        private void CreateNewEvent(string name, string time)
        {
            List<string> lines = File.ReadLines("C:\\Users\\mb153367\\Documents\\testing-file.txt").ToList();
            lines.AddRange(new List<string> { "Event - " + name, time });//                                                                <------------ affected by desc add
            File.WriteAllLines("C:\\Users\\mb153367\\Documents\\testing-file.txt", lines);
            Task_Name_Input.Text = "";
            Task_Time_Input.Text = "";
        }

        private void ReadEvent(int lineNo)
        {
            string lineContents = ReadLineOfFile(lineNo);
            if (lineContents.Length > 8 && lineContents.Substring(0, 5) == "Event")
            {
                TaskName.Text = lineContents.Substring(8, lineContents.Length - 8);
                Task_Time.Text = ReadLineOfFile(lineNo + 1);//                                                                <------------ affected by desc add
            }
            else
            {
                TaskName.Text = "No Event Starts at " + lineNo.ToString();
            }
        }

        private int FindLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) { return -1; }
            int lineNo = -1;
            StreamReader sr = new StreamReader("C:\\Users\\mb153367\\Documents\\testing-file.txt");
            string CurLine;
            while ((CurLine = sr.ReadLine()) != null)
            {
                lineNo++;
                if (CurLine == line)
                {
                    sr.Close();
                    return lineNo;
                }
            }
            sr.Close();
            return lineNo;
        }

        private int[] FindTasks()
        {
            List<int> linesWithTaskOn = new List<int>();
            StreamReader sr = new StreamReader("C:\\Users\\mb153367\\Documents\\testing-file.txt");
            string line;
            int lineNo = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Length > 8 && line.Substring(0, 5) == "Event")
                {
                    linesWithTaskOn.Add(lineNo);
                }
                lineNo++;
            }
            sr.Close();
            return ListToArray(linesWithTaskOn);
        }

        private int[] ListToArray(List<int> list)
        {
            int[] array = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                array[i] = list[i];
            }
            return array;
        }

        private void UpdateDropDown()
        {
            int[] TaskLines = FindTasks();

            if (TaskLines.Length == 0) 
            {
                TaskName.Text = "Nothing to do yet...";
                Task_Time.Text = "Why not add something?";
                Task_List.Items.Clear();
                return;
            }

            Task_List.Items.Clear();
            ListBoxItem lbi;
            string nameInList;
            for (int i = 0; i < TaskLines.Length; i++)
            {
                lbi = new ListBoxItem();
                nameInList = ReadLineOfFile(TaskLines[i]);
                lbi.Content = nameInList.Substring(8, nameInList.Length - 8);
                Task_List.Items.Add(lbi);
            }
            Task_List.SelectedItem = Task_List.Items[Task_List.Items.Count - 1];
            SelectTask();
        }

        private void ListBoxClick(object sender, MouseButtonEventArgs e)
        {
            SelectTask();
        }

        private void SelectTask()
        {
            if (Task_List.SelectedItem == null) { return; }
            string taskNameToFind = Task_List.SelectedItem.ToString();
            taskNameToFind = "Event - " + taskNameToFind.Substring(37, taskNameToFind.Length - 37);
            ReadEvent(FindLine(taskNameToFind));
            Save_Edits.Background = Brushes.LightGray;
        }

        private void SaveEdits(object sender, RoutedEventArgs e)
        {
            if (Task_List.SelectedItem == null) { return; } // same code as select task
            string taskNameToFind = Task_List.SelectedItem.ToString();
            taskNameToFind = "Event - " + taskNameToFind.Substring(37, taskNameToFind.Length - 37);
            string[] lines = File.ReadAllLines("C:\\Users\\mb153367\\Documents\\testing-file.txt");
            int taskStartingLine = FindLine(taskNameToFind);
            lines[taskStartingLine] = "Event - " + TaskName.Text;
            lines[taskStartingLine + 1] = Task_Time.Text;//                                                                <------------ affected by desc add
            File.WriteAllLines("C:\\Users\\mb153367\\Documents\\testing-file.txt", lines);
            UpdateDropDown();
        }

        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            if (Task_List.SelectedItem == null) { return; } // same code as select task
            string taskNameToFind = Task_List.SelectedItem.ToString();
            taskNameToFind = "Event - " + taskNameToFind.Substring(37, taskNameToFind.Length - 37);
            List<string> lines = File.ReadLines("C:\\Users\\mb153367\\Documents\\testing-file.txt").ToList();
            lines.RemoveRange(FindLine(taskNameToFind), 2); //                                                                <------------ affected by desc add
            File.WriteAllLines("C:\\Users\\mb153367\\Documents\\testing-file.txt", lines);
            UpdateDropDown();
        }

        private void TaskName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save_Edits.Background = Brushes.LightSteelBlue;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save_Edits.Background = Brushes.LightSteelBlue;
        }

        private void Task_Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save_Edits.Background = Brushes.LightSteelBlue;
        }
    }
}
