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
        public string pathwayToSaveFile = "C:\\Users\\mb153367\\Documents\\testing-file.txt";

        public MainWindow()
        {
            InitializeComponent();
            UpdateDropDown(true);
            CheckFileIntegrity();
        }

        private void CheckFileIntegrity()
        {
            if (File.Exists(pathwayToSaveFile) && IsIntactSimple())
            {
                Corruptometer.Content = "File is intact";
                Corruptometer.Foreground = Brushes.Gray;
            }
            else
            {
                Corruptometer.Content = "File may be corrupted!";
                Corruptometer.Foreground = Brushes.Red;
                //if (File.Exists(pathwayToSaveFile)) { SetHash(false); }
            }
            UpdateCopyFile();
        }

        private bool IsIntactSimple()
        {
            string copyPathway = pathwayToSaveFile.Substring(0, pathwayToSaveFile.Length - 4) + "COPY.txt";
            if (File.Exists(copyPathway))
            {
                //check similarity
                string[] LinesCopy = File.ReadAllLines(copyPathway);
                string totalLinesCopy = "";
                string[] LinesMain = File.ReadAllLines(pathwayToSaveFile);
                string totalLinesMain = "";
                if (LinesCopy.Length != LinesMain.Length) { return false; }
                for (int i = 0; i < LinesCopy.Length; i++)
                {
                    totalLinesMain += LinesMain[i];
                    totalLinesCopy += LinesCopy[i];
                }

                return totalLinesCopy == totalLinesMain;
            } else
            {
                return true;
            }
        }

        private void UpdateCopyFile()
        {
            string copyPathway = pathwayToSaveFile.Substring(0, pathwayToSaveFile.Length - 4) + "COPY.txt";
            if (File.Exists(copyPathway))
            {
                File.Delete(copyPathway);
                
            }
            File.Copy(pathwayToSaveFile, copyPathway);
        }

        /*
        private bool IsCorrupted(string filePathway)
        {
            if (!File.Exists(filePathway)) { return false; }
            if (File.ReadAllLines(pathwayToSaveFile).ElementAt(0).Length > 4)
            {
                string firstLine = File.ReadAllLines(pathwayToSaveFile).ElementAt(0);
                if (firstLine.Substring(0, 4) == "HASH")
                {
                    List<string> _lines = File.ReadAllLines(pathwayToSaveFile).ToList();
                    _lines.RemoveAt(0);
                    string _totLines = "";
                    foreach (string line in _lines) { _totLines += line; }
                    DEBUGhashLog.Text = "Hash in file: " + firstLine + " Hash attempts " + _totLines.GetHashCode().ToString() + _totLines.GetHashCode().ToString() + _totLines.GetHashCode().ToString();
                    return firstLine != "HASH" + _totLines.GetHashCode().ToString();
                }
            }
            SetHash(true);
            return false;
        }

        private void SetHash(bool createNew)
        {
            List<string> lines = File.ReadAllLines(pathwayToSaveFile).ToList();
            if (!createNew)
            {
                lines.RemoveAt(0);
            }
            string totLines = "";
            foreach (string line in lines) { totLines += line; }
            lines.Insert(0, "HASH" + totLines.GetHashCode());
            File.WriteAllLines(pathwayToSaveFile, lines);
        }*/

        private string ReadLineOfFile(int lineNo)
        {
            string line = File.ReadLines(pathwayToSaveFile).ElementAt(lineNo);
            return line;
        }

        private void SaveTaskName_Click(object sender, RoutedEventArgs e)
        {
            CreateNewEvent(Task_Name_Input.Text, Task_Time_Input.Text);
            UpdateDropDown(true);
        }

        private void CreateNewEvent(string name, string time)
        {
            List<string> lines = File.ReadLines(pathwayToSaveFile).ToList();
            lines.AddRange(new List<string> { "Event - " + name, time, "Description, notes, etc..." });
            File.WriteAllLines(pathwayToSaveFile, lines);
            Task_Name_Input.Text = "";
            Task_Time_Input.Text = "";
            UpdateCopyFile();
        }

        private void ReadEvent(int lineNo)
        {
            string lineContents = ReadLineOfFile(lineNo);
            if (lineContents.Length > 8 && lineContents.Substring(0, 5) == "Event")
            {
                TaskName.Text = lineContents.Substring(8, lineContents.Length - 8);
                Task_Time.Text = ReadLineOfFile(lineNo + 1);
                if (string.IsNullOrWhiteSpace(Task_Time.Text))
                {
                    Task_Time_Label.Text = "";
                } else
                {
                    Task_Time_Label.Text = "Time:";
                }
                Task_Desc.Text = ReadLineOfFile(lineNo + 2);
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
            StreamReader sr = new StreamReader(pathwayToSaveFile);
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
            StreamReader sr = new StreamReader(pathwayToSaveFile);
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

        private void UpdateDropDown(bool selectLast)
        {
            int[] TaskLines = FindTasks();

            if (TaskLines.Length == 0) 
            {
                TaskName.Text = "Nothing to do yet...";
                Task_Time.Text = "";
                Task_Time_Label.Text = "";
                Task_Desc.Text = "Why not add something?";
                Task_List.Items.Clear();
                Save_Edits.Background = Brushes.LightGray;
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
            if (selectLast) { Task_List.SelectedItem = Task_List.Items[Task_List.Items.Count - 1]; SelectTask(); }
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
            if (Task_List.SelectedItem == null) { Save_Edits.Background = Brushes.LightGray; return; } // same code as select task
            string taskNameToFind = Task_List.SelectedItem.ToString();
            taskNameToFind = "Event - " + taskNameToFind.Substring(37, taskNameToFind.Length - 37);
            string[] lines = File.ReadAllLines(pathwayToSaveFile);
            int taskStartingLine = FindLine(taskNameToFind);
            lines[taskStartingLine] = "Event - " + TaskName.Text;
            lines[taskStartingLine + 1] = Task_Time.Text;
            lines[taskStartingLine + 2] = Task_Desc.Text;
            File.WriteAllLines(pathwayToSaveFile, lines);
            UpdateDropDown(false);
            if (string.IsNullOrWhiteSpace(Task_Time.Text))
            {
                Task_Time_Label.Text = "";
            }
            else
            {
                Task_Time_Label.Text = "Time:";
            }
            UpdateCopyFile();
        }

        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            if (Task_List.SelectedItem == null) { return; } // same code as select task
            string taskNameToFind = Task_List.SelectedItem.ToString();
            taskNameToFind = "Event - " + taskNameToFind.Substring(37, taskNameToFind.Length - 37);
            List<string> lines = File.ReadLines(pathwayToSaveFile).ToList();
            lines.RemoveRange(FindLine(taskNameToFind), 3);
            File.WriteAllLines(pathwayToSaveFile, lines);
            UpdateDropDown(true);
            UpdateCopyFile();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pathwayToSaveFile = Save_File_Pathway.Text;
            UpdateDropDown(true);
            CheckFileIntegrity();
        }
    }
}
