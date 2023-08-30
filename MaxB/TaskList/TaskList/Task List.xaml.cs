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

namespace TaskList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReadEvent(2);
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
            CreateNewEvent(Task_Name_Input.Text, Task_Time_Input.Text);
        }

        private void CreateNewEvent(string name, string time)
        {
            List<string> lines = File.ReadLines("C:\\Users\\mb153367\\Documents\\testing-file.txt").ToList();
            lines.AddRange(new List<string> { "Event - " + name, time });
            File.WriteAllLines("C:\\Users\\mb153367\\Documents\\testing-file.txt", lines);
        }

        private void ReadEvent(int lineNo)
        {
            string lineContents = ReadLineOfFile(lineNo);
            if (lineContents.Length > 8 && lineContents.Substring(0, 5) == "Event")
            {
                TaskName.Text = lineContents.Substring(8, lineContents.Length - 8);
                Task_Time.Text = "Time: " + ReadLineOfFile(lineNo + 1);
            }
            else
            {
                TaskName.Text = "No Event Starts at " + lineNo.ToString();
            }
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int[] EventLines = FindTasks();
            Task_Time.Text = "";
            foreach (int line in EventLines)
            {
                Task_Time.Text += line.ToString() + ", ";
            }
        }
    }
}
