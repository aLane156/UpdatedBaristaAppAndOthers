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
            CheckFileIntegrity(pathwayToSaveFile);
            Search_In.Text = " All";
        }

        /// <summary>
        /// Updates the UI to reflect the outcome of the corruption detectors
        /// </summary>
        public void CheckFileIntegrity(string pathway)
        {
            if (File.Exists(pathway) && AreFileEditDatesEqual(pathway) && IsIntactSimple(pathway))
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
            UpdateCopyFile(pathway);
        }

        /// <summary>
        /// Compares the file to it's copy and checks for differences
        /// </summary>
        /// <param name="pathway">The pathway to the file to be checked</param>
        /// <returns>True if intact, false for corrupt - defaults true if missing</returns>
        public static bool IsIntactSimple(string pathway)
        {
            string copyPathway = pathway[..^4] + "COPY.txt";
            if (File.Exists(copyPathway))
            {
                //check similarity
                string[] LinesCopy = File.ReadAllLines(copyPathway);
                string totalLinesCopy = "";
                string[] LinesMain = File.ReadAllLines(pathway);
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

        /// <summary>
        /// Compares the edit dates for security reasons
        /// </summary>
        /// <param name="pathway">The pathway to the file used</param>
        /// <returns>True is last edit dates are equal, otherwise false</returns>
        public static bool AreFileEditDatesEqual(string pathway)
        {
            return File.GetLastWriteTime(pathway) == File.GetLastWriteTime(pathway[..^4] + "COPY.txt");
        }

        /// <summary>
        /// Makes the copy file the same as the master file
        /// </summary>
        /// <param name="pathway">The pathway to the master file</param>
        public static void UpdateCopyFile(string pathway)
        {
            string copyPathway = pathway[..^4] + "COPY.txt";
            if (File.Exists(copyPathway))
            {
                File.Delete(copyPathway);
            }
            File.Copy(pathway, copyPathway);
        }

        /// <summary>
        /// Reads the entirety of a line in a file
        /// </summary>
        /// <param name="lineNo">The line of a file to read, 0 based</param>
        /// <returns>The whole line</returns>
        public string ReadLineOfFile(int lineNo)
        {
            string line = File.ReadLines(pathwayToSaveFile).ElementAt(lineNo);
            return line;
        }

        /// <summary>
        /// To be used a by a button click
        /// </summary>
        public void SaveTaskName_Click(object sender, RoutedEventArgs e)
        {
            CreateNewTask(Task_Name_Input.Text, Task_Time_Input.Text);
            UpdateDropDown();
        }

        /// <summary>
        /// Adds a new task entry to the master file with standard formatting, the updates the copy
        /// </summary>
        /// <param name="name">The name of the task</param>
        /// <param name="time">The time of the task</param>
        public void CreateNewTask(string name, string time)
        {
            if (string.IsNullOrWhiteSpace(name) || (time.Length > 5 && time[..5] == "Event")) { return; }
            List<string> lines = File.ReadLines(pathwayToSaveFile).ToList();
            lines.AddRange(new List<string> { "Event - " + name, time, "Description, notes, etc..."}); // CHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGE
            File.WriteAllLines(pathwayToSaveFile, lines);
            Task_Name_Input.Text = "";
            Task_Time_Input.Text = "";
            Task_Date_Input.Text = "";
            UpdateCopyFile(pathwayToSaveFile);
        }

        /// <summary>
        /// Updates the task ui to show the details of the selected task
        /// </summary>
        /// <param name="lineNo">The line in the master file from which the task starts</param>
        public void ReadEvent(int lineNo)
        {
            string lineContents = ReadLineOfFile(lineNo);
            if (lineContents.Length > 8 && lineContents[..5] == "Event")
            {
                TaskName.Text = lineContents[8..];
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

        /// <summary>
        /// Finds the first occurance of a line in the master file
        /// </summary>
        /// <param name="line">The exact line to search for</param>
        /// <returns>The index of the first occurance of that line, 0 based, -1 if the line is absent</returns>
        public int FindLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) { return -1; }
            int lineNo = -1;
            StreamReader sr = new(pathwayToSaveFile);
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

        /// <summary>
        /// Finds every task with proper formatting in a file
        /// </summary>
        /// <returns>A list containing the starting index of every file</returns>
        public int[] FindTasks()
        {
            List<int> linesWithTaskOn = new();
            StreamReader sr = new(pathwayToSaveFile);
            string line;
            int lineNo = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Length > 8 && line[..5] == "Event")
                {
                    linesWithTaskOn.Add(lineNo);
                }
                lineNo++;
            }
            sr.Close();
            return linesWithTaskOn.ToArray();
        }

        /// <summary>
        /// Finds every task with proper formatting in a file which contains some text
        /// </summary>
        /// <param name="mustContain">The exact text to be searched for</param>
        /// <returns>A list containing the starting index of every file which contains the text</returns>
        public int[] FindTasks(string mustContain)
        {
            List<int> linesWithTaskOnNAME = new();
            List<int> linesWithTaskOnTIME = new();
            List<int> linesWithTaskOnDESC = new();

            StreamReader sr = new(pathwayToSaveFile);
            string line;
            int lineNo = 0;
            int lastTaskLine = 0;
            bool usedThisTask = false;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Length > 8 && line[..5] == "Event")
                {
                    lastTaskLine = lineNo;
                    usedThisTask = false;
                    line = line[8..];
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

        /// <summary>
        /// Fully updates the task list on the right
        /// </summary>
        /// <param name="selectLast">Should the last item in the list be automatically selected? Set to true when creating a new task, which will become the last upon creation</param>
        public void UpdateDropDown(bool selectLast = true)
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
                lbi.Content = nameInList[8..];
                Task_List.Items.Add(lbi);
            }
            if (selectLast) { Task_List.SelectedItem = Task_List.Items[^1]; SelectTask(); }
        }

        /// <summary>
        /// Fully updates the task list on the right with only tasks which contains some text
        /// </summary>
        /// <param name="selectLast">Should the last item in the list be automatically selected? Set to true when creating a new task, which will become the last upon creation</param>
        /// <param name="mustContain">The exact text to be searched for</param>
        public void UpdateDropDown(bool selectLast, string mustContain)
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
                lbi.Content = nameInList[8..];
                Task_List.Items.Add(lbi);
            }
            if (selectLast) { Task_List.SelectedItem = Task_List.Items[^1]; SelectTask(); }
        }

        /// <summary>
        /// To be used a by a button click
        /// </summary>
        public void ListBoxClick(object sender, MouseButtonEventArgs e)
        {
            SelectTask();
        }

        /// <summary>
        /// Fully finds, loads and displays a task from the selected index of the task list on the right
        /// </summary>
        public void SelectTask()
        {
            if (Task_List.SelectedItem == null) { return; }
            string taskNameToFind = Task_List.SelectedItem.ToString();
            taskNameToFind = "Event - " + taskNameToFind[37..];
            ReadEvent(FindLine(taskNameToFind));
            Save_Edits.Background = Brushes.LightGray;
        }

        /// <summary>
        /// To be used a by a button click
        /// </summary>
        public void SaveEdits(object sender, RoutedEventArgs e)
        {
            if (Task_List.SelectedItem == null) { Save_Edits.Background = Brushes.LightGray; return; } // not sure why this is here
            if (string.IsNullOrWhiteSpace(TaskName.Text) || (Task_Time.Text.Length > 5 && Task_Time.Text[..5] == "Event") || 
                (Task_Desc.Text.Length > 5 && Task_Desc.Text[..5] == "Event")) { return; }

            string taskNameToFind = Task_List.SelectedItem.ToString();
            taskNameToFind = "Event - " + taskNameToFind[37..];
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
            UpdateCopyFile(pathwayToSaveFile);
        }

        /// <summary>
        /// To be used a by a button click
        /// </summary>
        public void DeleteTask(object sender, RoutedEventArgs e)
        {
            if (Task_List.SelectedItem == null) { return; } // same code as select task
            string taskNameToFind = Task_List.SelectedItem.ToString();
            taskNameToFind = "Event - " + taskNameToFind[37..];
            List<string> lines = File.ReadLines(pathwayToSaveFile).ToList();
            lines.RemoveRange(FindLine(taskNameToFind), 3);// CHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGECHANGECHANGECHNAGE
            File.WriteAllLines(pathwayToSaveFile, lines);
            UpdateDropDown();
            UpdateCopyFile(pathwayToSaveFile);
        }

        /// <summary>
        /// To be used a by a button click
        /// </summary>
        public void TaskName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save_Edits.Background = Brushes.LightSteelBlue;
        }

        /// <summary>
        /// To be used a by a button click
        /// </summary>
        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save_Edits.Background = Brushes.LightSteelBlue;
        }

        /// <summary>
        /// To be used a by a button click
        /// </summary>
        public void Task_Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            Save_Edits.Background = Brushes.LightSteelBlue;
        }

        /// <summary>
        /// To be used a by a button click
        /// </summary>
        public void Button_Click(object sender, RoutedEventArgs e) //update file pathway
        {
            if (Save_File_Pathway.Text[0] == '"' && Save_File_Pathway.Text[^1] == '"'){
                Save_File_Pathway.Text = Save_File_Pathway.Text[1..^1];
            }
            if (!File.Exists(Save_File_Pathway.Text)) { return; }
            pathwayToSaveFile = Save_File_Pathway.Text;
            UpdateDropDown();
            CheckFileIntegrity(pathwayToSaveFile);
        }

        /// <summary>
        /// To be used a by a button click
        /// </summary>
        public void Duplicate_Task_Click(object sender, RoutedEventArgs e)
        {
            if (Task_List.SelectedItem == null) { return; }

            string taskNameToFind = Task_List.SelectedItem.ToString();
            taskNameToFind = "Event - " + taskNameToFind[37..];
            int startOfSelectedTask = FindLine(taskNameToFind);
            if (startOfSelectedTask == -1) { return; }

            List<string> lines = File.ReadLines(pathwayToSaveFile).ToList();
            lines.AddRange(new List<string> { lines[startOfSelectedTask] + " copy", lines[startOfSelectedTask+1], lines[startOfSelectedTask+2] });// CHANGECHANGECHNAGECHANGECHANGECHNAGE
            File.WriteAllLines(pathwayToSaveFile, lines);

            UpdateDropDown();
            UpdateCopyFile(pathwayToSaveFile);
        }

        /// <summary>
        /// To be used a by a button click
        /// </summary>
        public void Search_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Search_Box.Text))
            {
                UpdateDropDown(false);
            } else
            {
                UpdateDropDown(true, Search_Box.Text);
            }
        }

        /// <summary>
        /// To be used a by a button click
        /// </summary>
        public void Task_Date_Input_Changed(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Task_Date_Input.Text)) { return; }
            DateTime date = (DateTime)Task_Date_Input.SelectedDate;
            string dateText;
            TimeSpan dif = date - DateTime.Today;

            switch (dif.Days)
            {
                case 0:
                    dateText = "Later Today";
                    break;
                case < 7:
                    dateText = date.DayOfWeek.ToString();
                    break;
                default:
                    dateText = $"{date.DayOfWeek} the {date.Day}";
                    break;
            }

            Task_Time_Input.Text = dateText;
        }

        /// <summary>
        /// To be used a by a button click
        /// </summary>
        public void Search_In_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
