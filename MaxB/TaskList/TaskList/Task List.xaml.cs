using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;

namespace TaskList
{
    public partial class MainWindow : Window
    {
        public string pathwayToSaveFile = "C:\\Users\\mb153367\\Documents\\testing-file.txt";

        public MainWindow()
        {
            InitializeComponent();
            UpdateDropDown();
            CheckFileIntegrity();
            Search_In.Text = " All";
        }

        private void CheckFileIntegrity()
        {
            if (File.Exists(pathwayToSaveFile) && AreFileEditDatesEqual(pathwayToSaveFile) && IsIntactSimple())
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

        private bool AreFileEditDatesEqual(string pathway)
        {
            return File.GetLastWriteTime(pathway) == File.GetLastWriteTime(pathway.Substring(0, pathway.Length - 4) + "COPY.txt");
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

        private string ReadLineOfFile(int lineNo)
        {
            string line = File.ReadLines(pathwayToSaveFile).ElementAt(lineNo);
            return line;
        }

        private void SaveTaskName_Click(object sender, RoutedEventArgs e)
        {
            CreateNewTask(Task_Name_Input.Text, Task_Time_Input.Text);
            UpdateDropDown();
        }

        private void CreateNewTask(string name, string time)
        {
            if (string.IsNullOrWhiteSpace(name) || (time.Length > 5 && time.Substring(0, 5) == "Event")) { return; }
            List<string> lines = File.ReadLines(pathwayToSaveFile).ToList();
            lines.AddRange(new List<string> { "Event - " + name, time, "Description, notes, etc..."}); // CHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGE
            File.WriteAllLines(pathwayToSaveFile, lines);
            Task_Name_Input.Text = "";
            Task_Time_Input.Text = "";
            Task_Date_Input.Text = "";
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
                Task_Desc.Text = ReadLineOfFile(lineNo + 2);// CHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGE
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
            return linesWithTaskOn.ToArray();
        }
        private int[] FindTasks(string mustContain)
        {
            List<int> linesWithTaskOnNAME = new List<int>();
            List<int> linesWithTaskOnTIME = new List<int>();
            List<int> linesWithTaskOnDESC = new List<int>();

            StreamReader sr = new StreamReader(pathwayToSaveFile);
            string line;
            int lineNo = 0;
            int lastTaskLine = 0;
            bool usedThisTask = false;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Length > 8 && line.Substring(0, 5) == "Event")
                {
                    lastTaskLine = lineNo;
                    usedThisTask = false;
                    line = line.Substring(8);
                }
                if (line.ToUpper().Contains(mustContain.ToUpper()) && !usedThisTask)
                {
                    usedThisTask= true;
                    switch (lineNo - lastTaskLine)
                    {
                        case 0:
                            linesWithTaskOnNAME.Remove(lastTaskLine);
                            linesWithTaskOnNAME.Add(lastTaskLine);
                            break;
                        case 1:
                            linesWithTaskOnTIME.Remove(lastTaskLine);
                            linesWithTaskOnTIME.Add(lastTaskLine);
                            break;
                        case 2:
                            linesWithTaskOnDESC.Remove(lastTaskLine);
                            linesWithTaskOnDESC.Add(lastTaskLine);
                            break;
                    }
                    
                }
                lineNo++;
            }
            sr.Close();

            switch (Search_In.SelectedIndex)
            {
                case 1:
                    return linesWithTaskOnNAME.ToArray();
                case 2:
                    return linesWithTaskOnTIME.ToArray();
                case 3:
                    return linesWithTaskOnDESC.ToArray();
                default:
                    linesWithTaskOnTIME.AddRange(linesWithTaskOnDESC);
                    linesWithTaskOnNAME.AddRange(linesWithTaskOnTIME);
                    return linesWithTaskOnNAME.ToArray();
            }
        }

        private void UpdateDropDown(bool selectLast = true)
        {
            int[] TaskLines = FindTasks();

            if (TaskLines.Length == 0) 
            {
                TaskName.Text = "Nothing to do yet...";
                Task_Time.Text = "";
                Task_Time_Label.Text = "";
                Task_Desc.Text = "Why not add something?";// CHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGE
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
        private void UpdateDropDown(bool selectLast, string mustContain)
        {
            int[] TaskLines = FindTasks(mustContain);

            if (TaskLines.Length == 0)
            {
                TaskName.Text = "No tasks found containing " + mustContain;
                Task_Time.Text = "";
                Task_Time_Label.Text = "";
                Task_Desc.Text = "";// CHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGE
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
            if (Task_List.SelectedItem == null) { Save_Edits.Background = Brushes.LightGray; return; } // not sure why this is here
            if (string.IsNullOrWhiteSpace(TaskName.Text) || (Task_Time.Text.Length > 5 && Task_Time.Text.Substring(0, 5) == "Event") || 
                (Task_Desc.Text.Length > 5 && Task_Desc.Text.Substring(0, 5) == "Event")) { return; }

            string taskNameToFind = Task_List.SelectedItem.ToString();
            taskNameToFind = "Event - " + taskNameToFind.Substring(37, taskNameToFind.Length - 37);
            string[] lines = File.ReadAllLines(pathwayToSaveFile);
            int taskStartingLine = FindLine(taskNameToFind);
            lines[taskStartingLine] = "Event - " + TaskName.Text;
            lines[taskStartingLine + 1] = Task_Time.Text;
            lines[taskStartingLine + 2] = Task_Desc.Text;// CHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGE
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
            lines.RemoveRange(FindLine(taskNameToFind), 3);// CHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGE
            File.WriteAllLines(pathwayToSaveFile, lines);
            UpdateDropDown();
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

        private void Button_Click(object sender, RoutedEventArgs e) //update file pathway
        {
            if (Save_File_Pathway.Text[0] == '"' && Save_File_Pathway.Text[Save_File_Pathway.Text.Length-1] == '"'){
                Save_File_Pathway.Text = Save_File_Pathway.Text.Substring(1, Save_File_Pathway.Text.Length - 2);
            }
            if (!File.Exists(Save_File_Pathway.Text)) { return; }
            pathwayToSaveFile = Save_File_Pathway.Text;
            UpdateDropDown();
            CheckFileIntegrity();
        }

        private void Duplicate_Task_Click(object sender, RoutedEventArgs e)
        {
            if (Task_List.SelectedItem == null) { return; }

            string taskNameToFind = Task_List.SelectedItem.ToString();
            taskNameToFind = "Event - " + taskNameToFind.Substring(37, taskNameToFind.Length - 37);
            int startOfSelectedTask = FindLine(taskNameToFind);
            if (startOfSelectedTask == -1) { return; }

            List<string> lines = File.ReadLines(pathwayToSaveFile).ToList();
            lines.AddRange(new List<string> { lines[startOfSelectedTask] + " copy", lines[startOfSelectedTask+1], lines[startOfSelectedTask+2] });// CHANGECHANGECHNAGECHANGECHANGECHNAGE
            File.WriteAllLines(pathwayToSaveFile, lines);

            UpdateDropDown();
            UpdateCopyFile();
        }

        private void Search_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Search_Box.Text))
            {
                UpdateDropDown(false);
            } else
            {
                UpdateDropDown(true, Search_Box.Text);
            }
        }

        private void Task_Date_Input_Changed(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Task_Date_Input.Text)) { return; }
            DateTime date = (DateTime)Task_Date_Input.SelectedDate;
            string dateText = Task_Date_Input.Text;
            TimeSpan dif = date - DateTime.Today;

            switch (dif.Days)
            {
                case 0:
                    dateText = "Later Today";
                    break;
                case < 7:
                    dateText = date.DayOfWeek.ToString();
                    break;
                case < 14:
                    dateText = "Next " + date.DayOfWeek.ToString();
                    break;
            }

            Task_Time_Input.Text = dateText;
        }

        private void Search_In_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Search_Box.Text))
            {
                UpdateDropDown(false);
            }
            else
            {
                UpdateDropDown(true, Search_Box.Text);
            }
        }
    }
}
